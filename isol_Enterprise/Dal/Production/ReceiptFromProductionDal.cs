

using iSOL_Enterprise.Common;
using iSOL_Enterprise.Models;
using iSOL_Enterprise.Models.Logs;
using Newtonsoft.Json;
using SqlHelperExtensions;
using System.Data;
using System.Data.SqlClient;

namespace iSOL_Enterprise.Dal.Production
{
    public class ReceiptFromProductionDal
    {

        public List<SalesQuotation_MasterModels> GetProductionOrdersData()
        {
            List<SalesQuotation_MasterModels> list = new List<SalesQuotation_MasterModels>();
            string GetQuery = "";

            GetQuery = "select DocNum,OriginNum,ItemCode,ProdName,Warehouse,PlannedQty from OWOR order by DocEntry desc";


            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {

                    list.Add(
                        new SalesQuotation_MasterModels()
                        {
                            DocNum = rdr["DocNum"].ToString(),
                            OriginNum = rdr["OriginNum"].ToString(),
                            ItemCode = rdr["ItemCode"].ToString(),
                            ItemName = rdr["ProdName"].ToString(),
                            Warehouse = rdr["Warehouse"].ToString(),
                            PlannedQty = rdr["PlannedQty"].ToDecimal(),

                        });

                }
            }

            return list;
        }

        public ResponseModels AddUpdateReceiptFromProduction(string formData)
        {
            ResponseModels response = new ResponseModels();

            try
            {


                var model = JsonConvert.DeserializeObject<dynamic>(formData);
                if (model.OldId != null)
                {
                    response = EditReceiptFromProduction(model);
                }
                else
                {
                    response = AddReceiptFromProduction(model);
                }

                return response;


            }
            catch (Exception e)
            {

                response.isSuccess = false;
                response.Message = e.Message;
                return response;
            }

        }
        public ResponseModels AddReceiptFromProduction(dynamic model)
        {
            ResponseModels response = new ResponseModels();
            CommonDal cdal = new CommonDal();
            SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
            conn.Open();
            int MySeries = Convert.ToInt32(model.HeaderData.MySeries);
            SqlTransaction tran = conn.BeginTransaction();
            int res1 = 0;
            try
            {
                
                if (model.HeaderData != null)
                {

                    List<SqlParameter> param = new List<SqlParameter>();
                    int Id = CommonDal.getPrimaryKey(tran, "OIGN");

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
                    else
                    {
                        int count = SqlHelper.ExecuteScalar(tran, CommandType.Text, "select Count(*) from OIGN where DocNum ='" + model.HeaderData.DocNum.ToString() + "'");
                        if (count > 0)
                        {
                            tran.Rollback();
                            response.isSuccess = false;
                            response.Message = "Duplicate Document Number !";
                            return response;
                        }
                    }
                    #endregion

                    string HeadQuery = @"insert into OIGN (Id,Guid,MySeries,DocNum,Series,DocDate,Ref2,Comments,JrnlMemo,DocTotal,BaseType) 
                                        values(@Id,@Guid,@MySeries,@DocNum,@Series,@DocDate,@Ref2,@Comments,@JrnlMemo,@DocTotal,@BaseType)";



                    #region SqlParameters

                    #region Header data
                    param.Add(cdal.GetParameter("@MySeries", model.HeaderData.MySeries, typeof(int)));
                    param.Add(cdal.GetParameter("@DocNum", model.HeaderData.DocNum, typeof(string)));
                    param.Add(cdal.GetParameter("@Series", model.HeaderData.Series, typeof(int)));
                    param.Add(cdal.GetParameter("@DocDate", model.HeaderData.DocDate, typeof(DateTime)));                    
                    param.Add(cdal.GetParameter("@Ref2", model.HeaderData.Ref2, typeof(string)));
                    #endregion

                    #region Footer Data
                    param.Add(cdal.GetParameter("@Comments", model.FooterData.Comments, typeof(string)));
                    param.Add(cdal.GetParameter("@JrnlMemo", model.FooterData.JrnlMemo, typeof(string)));
                    param.Add(cdal.GetParameter("@DocTotal", model.FooterData.TotalBeforeDiscount, typeof(decimal)));
                    #endregion

                    param.Add(cdal.GetParameter("@BaseType", 102, typeof(int)));


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

                            string RowQueryItem1 = @"insert into IGN1
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

                            #region OITL Log
                            int LogEntry = CommonDal.getPrimaryKey(tran, "LogEntry", "OITL");   //Primary Key


                            item.BaseType = item.BaseType == "" ? "NULL" : Convert.ToInt32(item.BaseType);


                            OITL OITLModel = new OITL();
                            OITLModel.LogEntry = LogEntry;
                            OITLModel.CardCode = "NULL";
                            OITLModel.CardName = "NULL";
                            OITLModel.ItemCode = item.ItemCode.ToString();
                            OITLModel.ItemName = item.ItemName.ToString();
                            OITLModel.ID = Id;
                            OITLModel.DocLine = LineNum;
                            OITLModel.DocType = 59;
                            OITLModel.BaseType = item.BaseType;
                            OITLModel.Quantity = (decimal)item.QTY;
                            OITLModel.DocDate = Convert.ToDateTime(model.HeaderData.DocDate);

                            if (!cdal.OITLLog(tran, OITLModel))
                            {
                                response.isSuccess = false;
                                response.Message = "An Error Occured";
                                return response;
                            }

                            #endregion

                            if (model.Batches != null)
                            {
                                bool responseBatch = cdal.InBatches(tran, model.Batches, item.ItemCode.ToString(), LogEntry, item.Warehouse.ToString(), LineNum);
                                if (!responseBatch)
                                {
                                    tran.Rollback();
                                    response.isSuccess = false;
                                    response.Message = "An Error Occured";
                                    return response;
                                }
                            }


                            #region Update OITW If Sap Integration is OFF

                            if (!SqlHelper.SAPIntegration)
                            {
                                string UpdateOITWQuery = @"Update OITW set onHand = onHand + @Quantity where WhsCode = '" + item.Warehouse + "' and ItemCode = '" + item.ItemCode + "'";
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
                    response.Message = "Good Receipt Added Successfully !";

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
        public ResponseModels EditReceiptFromProduction(dynamic model)
        {
            ResponseModels response = new ResponseModels();
            return response;
        }
    }
}
