﻿using iSOL_Enterprise.Models.sale;
using SqlHelperExtensions;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;
using iSOL_Enterprise.Common;
using iSOL_Enterprise.Models;
using System.Reflection.Emit;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Logging.Abstractions;
using SAPbobsCOM;
using iSOL_Enterprise.Models.Logs;

namespace iSOL_Enterprise.Dal.Inventory_Transactions
{
    public class GoodsIssueDal
    {
        public List<SalesQuotation_MasterModels> GetData()
        {
            string GetQuery = "select * from OIGE order by id DESC";
            List<SalesQuotation_MasterModels> list = new List<SalesQuotation_MasterModels>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {

                    SalesQuotation_MasterModels models = new SalesQuotation_MasterModels();
                   //models.DocStatus = CommonDal.Check_IsNotEditable("PDN1", rdr["Id"].ToInt()) == false ? "Open" : "Closed";
                    models.DocStatus =  "Open" ;
                    models.Id = rdr["Id"].ToInt(); 
                    models.DocDate = rdr["DocDate"].ToDateTime();
                    models.DocNum = rdr["DocNum"].ToString(); 
                    models.Guid = rdr["Guid"].ToString(); 
                    models.IsPosted = rdr["isPosted"].ToString();
                    models.IsEdited = rdr["is_Edited"].ToString();
                    list.Add(models);                
                }
            }
            return list;
        }

        public dynamic GetHeaderOldData(int ItemID)
        {
            try
            {

                DataSet ds = new DataSet();
                SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
                string headerQuery = @"select MySeries,DocNum,Series,DocDate,TaxDate,GroupNum,Ref2,Comments,JrnlMemo,DocTotal From OIGE where Id =" + ItemID;
                SqlDataAdapter sda = new SqlDataAdapter(headerQuery, conn);
                sda.Fill(ds);
                string JSONString = string.Empty;
                JSONString = Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables);
                return JSONString;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public dynamic GetRowOldData(int ItemID)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
                string headerQuery = @"select ItemCode,Dscription,WhsCode,Quantity,Price,LineTotal,AcctCode,UomEntry,UomCode From IGE1 where Id =" + ItemID;
                SqlDataAdapter sda = new SqlDataAdapter(headerQuery, conn);
                sda.Fill(ds);
                string JSONString = string.Empty;
                JSONString = Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables);
                return JSONString;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public ResponseModels AddGoodsIssue(string formData)
        {
            var model = JsonConvert.DeserializeObject<dynamic>(formData);
            ResponseModels response = new ResponseModels();
            CommonDal cdal = new CommonDal();
            SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
            conn.Open();
            int MySeries = Convert.ToInt32(model.HeaderData.MySeries);
            SqlTransaction tran = conn.BeginTransaction();
            int res1 = 0;
            try
            {
                //int Id = CommonDal.getPrimaryKey(tran, "OITM");
                //string DocNum = SqlHelper.getUpdatedDocumentNumberOnLoad(tran, "OQUT", "SQ");
                if (model.HeaderData != null)
                {
                    List<SqlParameter> param = new List<SqlParameter>();
                    int Id = CommonDal.getPrimaryKey(tran, "OIGE");

                    param.Add(cdal.GetParameter("@Id", Id, typeof(int)));
                    param.Add(cdal.GetParameter("@Guid", CommonDal.generatedGuid(), typeof(string)));

                    #region BackendCheck For Series
                    if (MySeries != -1)
                    {
                        string? DocNum = SqlHelper.MySeriesUpdate_GetItemCode(MySeries, tran);
                        if (DocNum == null)
                        {
                            tran.Rollback();
                            response.isSuccess = false;
                            response.Message = "An Error Occured";
                            return response;
                        }
                        model.HeaderData.DocNum = DocNum;
                    }
                    #endregion
                    else
                    {
                        int count = SqlHelper.ExecuteScalar(tran, CommandType.Text, "select Count(*) from OIGE where DocNum ='" + model.HeaderData.DocNum.ToString() + "'");
                        if (count > 0)
                        {
                            tran.Rollback();
                            response.isSuccess = false;
                            response.Message = "Duplicate Document Number !";
                            return response;
                        }
                    }
                    string HeadQuery = @"insert into OIGE (Id,Guid,MySeries,DocNum,Series,DocDate,GroupNum,TaxDate,Ref2,Comments,JrnlMemo,DocTotal) 
                                        values(@Id,@Guid,@MySeries,@DocNum,@Series,@DocDate,@GroupNum,@TaxDate,@Ref2,@Comments,@JrnlMemo,@DocTotal)";



                    #region SqlParameters

                    #region Header data
                    param.Add(cdal.GetParameter("@MySeries", model.HeaderData.MySeries, typeof(int)));
                    param.Add(cdal.GetParameter("@DocNum", model.HeaderData.DocNum, typeof(string)));
                    param.Add(cdal.GetParameter("@Series", model.HeaderData.Series, typeof(int)));
                    param.Add(cdal.GetParameter("@DocDate", model.HeaderData.DocDate, typeof(DateTime)));
                    param.Add(cdal.GetParameter("@GroupNum", model.HeaderData.GroupNum, typeof(Int16)));
                    param.Add(cdal.GetParameter("@TaxDate", model.HeaderData.TaxDate, typeof(DateTime)));
                    param.Add(cdal.GetParameter("@Ref2", model.HeaderData.Ref2, typeof(string)));
                    #endregion

                    #region Footer Data
                    param.Add(cdal.GetParameter("@Comments", model.FooterData.Comments, typeof(string)));
                    param.Add(cdal.GetParameter("@JrnlMemo", model.FooterData.JrnlMemo, typeof(string)));
                    param.Add(cdal.GetParameter("@DocTotal", model.FooterData.TotalBeforeDiscount, typeof(decimal)));
                    #endregion




                    #endregion

                    res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, HeadQuery, param.ToArray()).ToInt();
                    if (res1 <= 0)
                    {
                        tran.Rollback();
                        response.isSuccess = false;
                        response.Message = "An Error Occured";
                        return response;
                    }

                    if (model.ListItems != null)
                    {
                        int LineNum = 0;
                        foreach (var item in model.ListItems)
                        {

                            string RowQueryItem1 = @"insert into IGE1
                                (Id,LineNum,BaseRef,BaseEntry,BaseLine,ItemCode,Dscription,WhsCode,Quantity,Price,LineTotal,AcctCode,UomEntry,UomCode,BaseQty,OpenQty)
                          values(@Id,@LineNum,@BaseRef,@BaseEntry,@BaseLine,@ItemCode,@Dscription,@WhsCode,@Quantity,@Price,@LineTotal,@AcctCode,@UomEntry,@UomCode,@BaseQty,@OpenQty)";
                            var BaseRef = item.BaseRef;
                            #region sqlparam
                            List<SqlParameter> param1 = new List<SqlParameter>();
                            param1.Add(cdal.GetParameter("@Id", Id, typeof(int)));
                            param1.Add(cdal.GetParameter("@LineNum", LineNum, typeof(int)));
                            param1.Add(cdal.GetParameter("@BaseRef", item.BaseRef, typeof(string)));
                            param1.Add(cdal.GetParameter("@BaseEntry", item.BaseEntry, typeof(int)));
                            param1.Add(cdal.GetParameter("@BaseLine", item.BaseLine, typeof(int)));
                            param1.Add(cdal.GetParameter("@ItemCode", item.ItemCode, typeof(string)));
                            param1.Add(cdal.GetParameter("@Dscription", item.ItemName, typeof(string)));
                            param1.Add(cdal.GetParameter("@WhsCode", item.Warehouse, typeof(string)));
                            param1.Add(cdal.GetParameter("@Quantity", item.QTY, typeof(decimal)));
                            param1.Add(cdal.GetParameter("@Price", item.UPrc, typeof(decimal)));
                            param1.Add(cdal.GetParameter("@LineTotal", item.TtlPrc, typeof(decimal)));
                            param1.Add(cdal.GetParameter("@AcctCode", item.AcctCode, typeof(string)));
                            //param1.Add(cdal.GetParameter("@ItemCost", item.ItemCost, typeof(string)));
                            param1.Add(cdal.GetParameter("@UomEntry", item.UomEntry, typeof(int)));
                            param1.Add(cdal.GetParameter("@UomCode", item.UomCode, typeof(string)));
                            param1.Add(cdal.GetParameter("@BaseQty", item.BaseQty, typeof(string)));
                            param1.Add(cdal.GetParameter("@OpenQty", item.QTY, typeof(decimal)));

                            #endregion

                            res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, RowQueryItem1, param1.ToArray()).ToInt();
                            if (res1 <= 0)
                            {
                                tran.Rollback();
                                response.isSuccess = false;
                                response.Message = "An Error Occured";
                                return response;
                            }
                            int LogEntry = CommonDal.getPrimaryKey(tran, "LogEntry", "OITL");
                            //int QUT1Id = CommonDal.getPrimaryKey(tran, "DLN1");
                            #region UpdateWarehouse&GenerateLog

                                #region OITLLog
                                item.BaseType = item.BaseType == "" ? "NULL" : Convert.ToInt32(item.BaseType);

                                OITL OITLModel = new OITL();
                                OITLModel.LogEntry = LogEntry;
                                OITLModel.CardCode = "NULL";
                                OITLModel.CardName = "NULL";
                                OITLModel.ItemCode = item.ItemCode.ToString();
                                OITLModel.ItemName = item.ItemName.ToString();
                                OITLModel.ID = Id;
                                OITLModel.DocLine = LineNum;
                                OITLModel.DocType = 60;
                                OITLModel.BaseType = item.BaseType;
                                OITLModel.Quantity = -1 * (decimal)item.QTY;
                                OITLModel.DocDate = Convert.ToDateTime(model.HeaderData.DocDate);

                                if (!cdal.OITLLog(tran, OITLModel))
                                {                                
                                    response.isSuccess = false;
                                    response.Message = "An Error Occured";
                                    return response;
                                }

                                #endregion

                                #region Bataches & Logs working

                                if (model.Batches != null)
                                {
                                    bool batchresponse = cdal.OutBatches(tran, model.Batches, item.ItemCode.ToString(), LogEntry, item.Warehouse.ToString(), LineNum);
                                    if (!batchresponse)
                                    {
                                        tran.Rollback();
                                        response.isSuccess = false;
                                        response.Message = "An Error Occured";
                                        return response;
                                    }
                                
                                }
                                #endregion

                            #endregion

                            #region Update OITW If Sap Integration is OFF

                            if (!SqlHelper.SAPIntegration)
                            {
                                string UpdateOITWQuery = @"Update OITW set onHand = onHand - @Quantity where WhsCode = '" + item.Warehouse + "' and ItemCode = '" + item.ItemCode + "'";
                                List<SqlParameter> param2 = new List<SqlParameter>();
                                param2.Add(cdal.GetParameter("@Quantity", item.QTY, typeof(decimal)));
                                res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, UpdateOITWQuery, param2.ToArray()).ToInt();
                                if (res1 <= 0)
                                {
                                    tran.Rollback();
                                    response.isSuccess = false;
                                    response.Message = "An Error Occured";
                                    return response;
                                }
                            }

                            #endregion

                            LineNum += 1;
                        }
                    }


                        if (model.ListAttachment != null)
                        {


                            int LineNo = 0;
                            int ATC1Id = CommonDal.getPrimaryKey(tran, "AbsEntry", "ATC1");
                            foreach (var item in model.ListAttachment)
                            {
                                if (item.selectedFilePath != "" && item.selectedFileName != "" && item.selectedFileDate != "")
                                {


                                    string RowQueryAttachment = @"insert into ATC1(AbsEntry,Line,trgtPath,FileName,Date)
                                                  values(" + ATC1Id + ","
                                                            + LineNo + ",'"
                                                            + item.selectedFilePath + "','"
                                                            + item.selectedFileName + "','"
                                                            + Convert.ToDateTime(item.selectedFileDate) + "')";
                                    #region sqlparam
                                    //List<SqlParameter> param3 = new List<SqlParameter>
                                    //            {
                                    //                new SqlParameter("@AbsEntry",ATC1Id),
                                    //                new SqlParameter("@Line",ATC1Line),
                                    //                new SqlParameter("@trgtPath",item.trgtPath),
                                    //                new SqlParameter("@FileName",item.FileName),
                                    //                new SqlParameter("@Date",item.Date),
                                    //            };
                                    #endregion
                                    res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, RowQueryAttachment).ToInt();
                                    if (res1 <= 0)
                                    {
                                        tran.Rollback();
                                        response.isSuccess = false;
                                        response.Message = "An Error Occured";
                                        return response;

                                    }
                                    LineNo += 1;
                                }
                            }



                        }


                    }
                    if (res1 > 0)
                    {
                        tran.Commit();
                        response.isSuccess = true;
                        response.Message = "Goods Issue Added Successfully !";

                    }

                }
            
            catch (Exception e)
            {
                tran.Rollback();
                response.isSuccess = false;
                response.Message = e.Message;
                return response;
            }
            return response;

        }




    }
}
