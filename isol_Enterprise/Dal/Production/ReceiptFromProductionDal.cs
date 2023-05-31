
using iSOL_Enterprise.Common;
using iSOL_Enterprise.Models;
using iSOL_Enterprise.Models.Logs;
using Newtonsoft.Json;
using SAPbobsCOM;
using SqlHelperExtensions;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace iSOL_Enterprise.Dal.Production
{
    public class ReceiptFromProductionDal
    {
        public List<SalesQuotation_MasterModels> GetData()
        {
            string GetQuery = "select Id,Guid,DocNum,DocDate,Comments,JrnlMemo,isPosted,is_Edited,isApproved,apprSeen from OIGN where BaseType = '102'  order by id DESC";


            List<SalesQuotation_MasterModels> list = new List<SalesQuotation_MasterModels>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {

                    SalesQuotation_MasterModels models = new SalesQuotation_MasterModels();

                    models.DocStatus = "Open";
                    models.Id = rdr["Id"].ToInt();
                    models.CardCode = rdr["DocNum"].ToString();                    
                    models.DocDate = Convert.ToDateTime(rdr["DocDate"]);
                    models.Comments = rdr["Comments"].ToString();
                    models.Guid = rdr["Guid"].ToString();                   
                    models.IsPosted = rdr["isPosted"].ToString();
                    models.IsEdited = rdr["is_Edited"].ToString();
                    models.isApproved = rdr["isApproved"].ToBool();
                    models.apprSeen = rdr["apprSeen"].ToBool();
                    list.Add(models);
                }
            }
            return list;
        }

        public List<SalesQuotation_MasterModels> GetProductionOrdersData()
        {
            List<SalesQuotation_MasterModels> list = new List<SalesQuotation_MasterModels>();
            string GetQuery = "";

            GetQuery = "select DocNum,OriginNum,ItemCode,ProdName,Warehouse,PlannedQty,CmpltQty,Sap_Ref_No,CmpltQty from OWOR where isPosted = 1 order by DocEntry desc";


            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, GetQuery))
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
                            Sap_Ref_No = rdr["Sap_Ref_No"].ToString(),
                            CmpltQty = rdr["CmpltQty"].ToDecimal()
                        });

                }
            }

            return list;
        }

        public int GetId(string guid)
        {
            guid = HttpUtility.UrlDecode(guid);
            return Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.defaultDB, CommandType.Text, "select Id from OIGN where GUID ='" + guid.ToString() + "'"));

        }

        public dynamic GetProdData(string DocEntry)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
                string headerQuery = @"select DocNum,OriginNum,ItemCode,ProdName,Warehouse,PlannedQty,CmpltQty,Sap_Ref_No from OWOR where isPosted = 1 and Sap_Ref_No = '" + DocEntry+"' order by DocEntry desc";
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
        public dynamic GetOldHeaderData(int id)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
                string headerQuery = @"select MySeries,DocNum,Series,DocDate,Ref2,Comments,JrnlMemo,DocTotal From OIGN where Id =" + id;
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
        public dynamic GetOldItemsData(int id)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
                string headerQuery = @"select IGN1.Id as Id,LineNum,BaseRef,BaseType,BaseQty,IGN1.ItemCode as ItemCode,Dscription,WhsCode,Quantity,TranType,Price,LineTotal,AcctCode,SaleOrderCode,CmpltQty,IGN1.SaleOrderDocNo as SaleOrderDocNo,isReturn,RowNum,OWOR.Sap_Ref_No From IGN1
                                    inner join OWOR on IGN1.BaseRef = OWOR.DocNum
                                    where IGN1.Id = " + id;
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
        public decimal GetExtraQty(string DocNum,string ItemCode,int RowNum, SqlTransaction? Cnstrn = null,decimal OldQty = 0)
        {            
            string ExtraQtyQuery = @"select  b.IssuedQty - ( ( a.CmpltQty - "+OldQty +")* b.BaseQty) as ExtraQty from OWOR a "+
                                                        "inner JOIN WOR1 b on a.id = b.id " + 
                                                        "where a.DocNum = '" + DocNum + "' and b.ItemCode = '" + ItemCode + "' and b.LineNum = " + RowNum + "-1";

            decimal ExtraQty = Cnstrn != null ? Convert.ToDecimal(SqlHelper.ExecuteScalar(Cnstrn, CommandType.Text, ExtraQtyQuery)) : Convert.ToDecimal(SqlHelper.ExecuteScalar(SqlHelper.defaultDB, CommandType.Text, ExtraQtyQuery));

            return ExtraQty;
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
                    string Guid = CommonDal.generatedGuid();

                    param.Add(cdal.GetParameter("@Id", Id, typeof(int)));
                    param.Add(cdal.GetParameter("@Guid", Guid, typeof(string)));
                    param.Add(cdal.GetParameter("@DocEntry", Id, typeof(int)));

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


                    


                    int ObjectCode = 102;
                    int isApproved = ObjectCode.GetApprovalStatus(tran);
                    #region Insert in Approval Table

                    if (isApproved == 0)
                    {
                        ApprovalModel approvalModel = new()
                        {
                            Id = CommonDal.getPrimaryKey(tran, "tbl_DocumentsApprovals"),
                            ObjectCode = ObjectCode,
                            DocEntry = Id,
                            DocNum = (model.HeaderData.DocNum).ToString(),
                            Guid = Guid

                        };
                        bool resp = cdal.AddApproval(tran, approvalModel);
                        if (!resp)
                        {
                            //tran.Rollback();
                            response.isSuccess = false;
                            response.Message = "An error occured !";
                            return response;
                        }
                    }

                    #endregion

                    string HeadQuery = @"insert into OIGN (Id,Guid,DocEntry,MySeries,DocNum,Series,DocDate,Ref2,Comments,JrnlMemo,DocTotal,BaseType,isApproved) 
                                        values(@Id,@Guid,@DocEntry,@MySeries,@DocNum,@Series,@DocDate,@Ref2,@Comments,@JrnlMemo,@DocTotal,@BaseType,@isApproved)";



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
                    param.Add(cdal.GetParameter("@isApproved", isApproved, typeof(int)));


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
                        int? RowNum = null;
                        foreach (var item in model.ListItems)
                        {
                            bool isReturn = false;
                            if (item.ExtraQty.ToString() != null && item.ExtraQty.ToString() != "")
                                isReturn = true;

                            #region BackEnd check for Qty 
                            if (!isReturn)      //If not q return Type
                            {
                                decimal Qty = Convert.ToDecimal(SqlHelper.ExecuteScalar(tran, CommandType.Text, @"select PlannedQty - CmpltQty as PlannedQty from OWOR where DocNum ='" + item.BaseType + "'"));
                                if (Convert.ToDecimal(item.QTY) > Qty)
                                {
                                    tran.Rollback();
                                    response.isSuccess = false;
                                    response.Message = "Quantity can't be greater then Remaining Quantity !";
                                    return response;
                                }
                            }
                            else
                            {

                                decimal ExtraQty = GetExtraQty(item.BaseType.ToString(), item.ItemCode.ToString(),Convert.ToInt32 (item.RowNum),tran);
                                if (Convert.ToDecimal(item.QTY) > ExtraQty)
                                {
                                    tran.Rollback();
                                    response.isSuccess = false;
                                    response.Message = "Quantity can't be greater then Issued Quantity !";
                                    return response;
                                }
                                RowNum = Convert.ToInt32(item.RowNum);
                            }
                            #endregion

                            string Tabitem = "Id,LineNum,BaseRef,BaseType,ItemCode,Dscription,WhsCode,Quantity,TranType,Price,LineTotal,AcctCode,UomEntry,UomCode,OpenQty,SaleOrderCode,BaseQty,SaleOrderDocNo,isReturn,RowNum";
                            string TabitemP = "@Id,@LineNum,@BaseRef,@BaseType,@ItemCode,@Dscription,@WhsCode,@Quantity,@TranType,@Price,@LineTotal,@AcctCode,@UomEntry,@UomCode,@OpenQty,@SaleOrderCode,@BaseQty,@SaleOrderDocNo,@isReturn,@RowNum";
                            string ITT1_Query = @"insert into IGN1 (" + Tabitem + ") " +
                                                 "values(" + TabitemP + ")";

                            
                            #region sqlparam
                            List<SqlParameter> param1 = new List<SqlParameter>();
                            param1.Add(cdal.GetParameter("@Id", Id, typeof(int)));
                            param1.Add(cdal.GetParameter("@LineNum", LineNum, typeof(int)));
                            param1.Add(cdal.GetParameter("@BaseRef", item.BaseType, typeof(string)));                            
                            param1.Add(cdal.GetParameter("@BaseType", 102, typeof(int)));                            
                            param1.Add(cdal.GetParameter("@ItemCode", item.ItemCode, typeof(string)));
                            param1.Add(cdal.GetParameter("@Dscription", item.ItemName, typeof(string)));
                            param1.Add(cdal.GetParameter("@WhsCode", item.Warehouse, typeof(string)));
                            param1.Add(cdal.GetParameter("@Quantity", item.QTY, typeof(decimal)));
                            param1.Add(cdal.GetParameter("@TranType", item.TranType, typeof(char)));
                            param1.Add(cdal.GetParameter("@Price", item.UPrc, typeof(decimal)));
                            param1.Add(cdal.GetParameter("@LineTotal", item.TtlPrc, typeof(decimal)));
                            param1.Add(cdal.GetParameter("@AcctCode", item.AcctCode, typeof(string)));
                            param1.Add(cdal.GetParameter("@UomEntry", item.UomEntry, typeof(int)));
                            param1.Add(cdal.GetParameter("@UomCode", item.UomCode, typeof(string)));                            
                            param1.Add(cdal.GetParameter("@OpenQty", item.QTY, typeof(decimal)));
                            param1.Add(cdal.GetParameter("@SaleOrderCode", item.SaleOrderCode, typeof(int)));
                            param1.Add(cdal.GetParameter("@BaseQty", item.PlannedQty, typeof(decimal)));
                            param1.Add(cdal.GetParameter("@SaleOrderDocNo", item.SaleOrderDocNo, typeof(string)));
                            param1.Add(cdal.GetParameter("@isReturn", isReturn, typeof(bool)));
                            param1.Add(cdal.GetParameter("@RowNum", RowNum, typeof(int)));

                            #endregion

                            res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, ITT1_Query, param1.ToArray()).ToInt();
                            if (res1 <= 0)
                            {
                                tran.Rollback();
                                response.isSuccess = false;
                                response.Message = "An Error Occured";
                                return response;
                            }

                            #region OITL Log
                            int LogEntry = CommonDal.getPrimaryKey(tran, "LogEntry", "OITL");   //Primary Key


                            //item.BaseType =  "NULL" ;


                            OITL OITLModel = new OITL();
                            OITLModel.LogEntry = LogEntry;
                            OITLModel.CardCode = "NULL";
                            OITLModel.CardName = "NULL";
                            OITLModel.ItemCode = item.ItemCode.ToString();
                            OITLModel.ItemName = item.ItemName.ToString();
                            OITLModel.ID = Id;
                            OITLModel.DocLine = LineNum;
                            OITLModel.DocType = 59;
                            OITLModel.BaseType = "";
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

                            #region Update Production Order 
                            if (!isReturn)  //Update Completed Qty
                            { 
                                string POquery = @"update OWOR set CmpltQty = CmpltQty +"+ item.QTY + " where DocNum = '" + item.BaseType +"'";
                                res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, POquery).ToInt();
                                if (res1 <= 0)
                                {
                                    tran.Rollback();
                                    response.isSuccess = false;
                                    response.Message = "An Error Occured";
                                    return response;
                                }
                            }
                            else
                            {
                                int OWORID = SqlHelper.ExecuteScalar(tran, CommandType.Text, @"select Id from OWOR where DocNum ='" + item.BaseType + "'");

                                string UPWOR1 = @"update WOR1 set IssuedQty = IssuedQty -" + item.QTY + "where Id=" + OWORID + "and ItemCode = '" + item.ItemCode + "' and LineNum = " + item.RowNum + "-1";
                                res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, UPWOR1).ToInt();
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
                    response.Message = "Receipt From Production Added Successfully !";

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
            CommonDal cdal = new CommonDal();
            SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
            conn.Open();
            SqlTransaction tran = conn.BeginTransaction();
            int res1 = 0;
            try
            {
                int Id = GetId(model.OldId.ToString());

                if (model.HeaderData != null)
                {
                    List<SqlParameter> param = new List<SqlParameter>();

                    int ObjectCode = 102;
                    int isApproved = ObjectCode.GetApprovalStatus(tran);

                    #region Insert in Approval Table

                    if (isApproved == 0)
                    {
                        ApprovalModel approvalModel = new()
                        {
                            Id = CommonDal.getPrimaryKey(tran, "tbl_DocumentsApprovals"),
                            ObjectCode = ObjectCode,
                            DocEntry = Convert.ToInt32(SqlHelper.ExecuteScalar(tran, CommandType.Text, @"select Id from OIGN where guid='" + model.OldId + "'")),
                            DocNum = SqlHelper.ExecuteScalar(tran, CommandType.Text, @"select DocNum from OIGN where guid='" + model.OldId + "'").ToString(),
                            Guid = (model.OldId).ToString()
                        };
                        bool resp = cdal.AddApproval(tran, approvalModel);
                        if (!resp)
                        {
                            response.isSuccess = false;
                            response.Message = "An Error Occured";
                            return response;
                        }

                    }

                    #endregion

                    string TabHeader = @"DocDate =@DocDate,Ref2=@Ref2,Comments=@Comments,JrnlMemo=@JrnlMemo,DocTotal=@DocTotal,is_Edited=1,isApproved =@isApproved,apprSeen =0";

                    string HeadQuery = @"Update OIGN set " + TabHeader + " where GUID = '" + model.OldId + "'";

                    #region SqlParameters

                    #region Header data                    
                    param.Add(cdal.GetParameter("@DocDate", model.HeaderData.DocDate, typeof(DateTime)));
                    param.Add(cdal.GetParameter("@Ref2", model.HeaderData.Ref2, typeof(string)));
                    #endregion

                    #region Footer Data
                    param.Add(cdal.GetParameter("@Comments", model.FooterData.Comments, typeof(string)));
                    param.Add(cdal.GetParameter("@JrnlMemo", model.FooterData.JrnlMemo, typeof(string)));
                    param.Add(cdal.GetParameter("@DocTotal", model.FooterData.TotalBeforeDiscount, typeof(decimal)));
                    #endregion

                    param.Add(cdal.GetParameter("@BaseType", 102, typeof(int)));
                    param.Add(cdal.GetParameter("@isApproved", isApproved, typeof(int)));

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
                        int index = 0;
                        int? RowNum = null;
                        foreach (var item in model.ListItems)
                        {

                            bool isReturn = false;
                            decimal CmpltQty = 0;
                            if (item.ExtraQty.ToString() != null && item.ExtraQty.ToString() != "")
                                isReturn = true;

                            if(!isReturn)   //If Row is not Return Type
                             CmpltQty = Convert.ToDecimal( SqlHelper.ExecuteScalar(tran, CommandType.Text, @"select PlannedQty - CmpltQty as PlannedQty from OWOR where DocNum ='" + item.BaseType + "'"));
                            
                            decimal qty = 0;

                            param.Clear();
                            string ITT1_Query = "";
                            if (item.LineNum != null && item.LineNum != "")
                            {

                                string Tabitem = @"BaseRef=@BaseRef,BaseType=@BaseType,ItemCode=@ItemCode,Dscription=@Dscription,WhsCode=@WhsCode,Quantity=@Quantity,
                                                   TranType=@TranType,Price=@Price,LineTotal=@LineTotal,AcctCode=@AcctCode,UomEntry=@UomEntry,UomCode=@UomCode,OpenQty=@OpenQty,SaleOrderCode=@SaleOrderCode,SaleOrderDocNo=@SaleOrderDocNo";

                                ITT1_Query = @"update IGN1 set " + Tabitem + " where id=" + Id + " and LineNum=" + item.LineNum;
                                
                                qty = Convert.ToDecimal(SqlHelper.ExecuteScalar(tran, CommandType.Text, @"select Quantity from IGN1 where id=" + Id + " and LineNum=" + item.LineNum));

                                #region BackEnd check for Qty
                                if (!isReturn)      //If not q return Type
                                {
                                    CmpltQty = Convert.ToDecimal(SqlHelper.ExecuteScalar(tran, CommandType.Text, @"select PlannedQty - (CmpltQty -" + qty + ") as PlannedQty from OWOR where DocNum ='" + item.BaseType + "'"));

                                    if (Convert.ToDecimal(item.QTY) != qty)
                                    {


                                        if (Math.Abs(Convert.ToDecimal(item.QTY) - qty) > CmpltQty)
                                        {
                                            tran.Rollback();
                                            response.isSuccess = false;
                                            response.Message = "Quantity can't be greater then Remaining Quantity !";
                                            return response;
                                        }
                                    }
                                }    
                                
                                else
                                {
                                    decimal ExtraQty = GetExtraQty(item.BaseType.ToString(), item.ItemCode.ToString(), Convert.ToInt32(item.RowNum), tran , qty);
                                    if (Convert.ToDecimal(item.QTY) > ExtraQty)
                                    {
                                        tran.Rollback();
                                        response.isSuccess = false;
                                        response.Message = "Quantity can't be greater then Issued Quantity !";
                                        return response;
                                    }
                                    RowNum = Convert.ToInt32(item.RowNum);
                                }
                                #endregion
                                
                                #region If Item is Batch Type Generate Log

                                if (Convert.ToDecimal(item.QTY) != Convert.ToDecimal(item.OldQty))
                                    {
                                        ResponseModels ItemData = cdal.GetItemData(item.ItemCode.ToString(), "PR");
                                        if (ItemData.Data.ManBtchNum == "Y")
                                        {


                                            if (cdal.ReverseOutTransaction(tran, Convert.ToInt32(model.ID), Convert.ToInt32(item.LineNum), 59))
                                            {

                                                int LogEntry1 = CommonDal.getPrimaryKey(tran, "LogEntry", "OITL");

                                                #region OITLLog
                                                OITL OITLModel = new OITL();
                                                OITLModel.LogEntry = LogEntry1;
                                                OITLModel.CardCode = model.HeaderData.CardCode.ToString();
                                                OITLModel.CardName = model.HeaderData.CardName.ToString();
                                                OITLModel.ItemCode = item.ItemCode.ToString();
                                                OITLModel.ItemName = item.ItemName.ToString();
                                                OITLModel.ID = Id;
                                                OITLModel.DocLine = Convert.ToInt32(item.LineNum);
                                                OITLModel.DocType = 59;
                                                OITLModel.BaseType = "";
                                                OITLModel.Quantity = (decimal)item.QTY;
                                                OITLModel.DocDate = Convert.ToDateTime(model.HeaderData.DocDate);

                                                if (!cdal.OITLLog(tran, OITLModel))
                                                {
                                                    tran.Rollback();
                                                    response.isSuccess = false;
                                                    response.Message = "An Error Occured";
                                                    return response;
                                                }

                                                if (!cdal.InBatches(tran, model.Batches, item.ItemCode.ToString(), LogEntry1, item.Warehouse.ToString(), index))
                                                {
                                                    tran.Rollback();
                                                    response.isSuccess = false;
                                                    response.Message = "An Error Occured";
                                                    return response;
                                                }
                                                #endregion


                                            }

                                        

                                        }

                                    }
                                #endregion
                            }
                            else
                            {
                                string Tabitem = "Id,LineNum,BaseRef,BaseType,ItemCode,Dscription,WhsCode,Quantity,TranType,Price,LineTotal,AcctCode,UomEntry,UomCode,OpenQty,SaleOrderCode,BaseQty,SaleOrderDocNo,isReturn,RowNum";
                                string TabitemP = "@Id,@LineNum,@BaseRef,@BaseType,@ItemCode,@Dscription,@WhsCode,@Quantity,@TranType,@Price,@LineTotal,@AcctCode,@UomEntry,@UomCode,@OpenQty,@SaleOrderCode,@BaseQty,@SaleOrderDocNo,@isReturn,@RowNum";
                                ITT1_Query = @"insert into IGN1 (" + Tabitem + ") " +
                                                     "values(" + TabitemP + ")";
                                
                                LineNum = CommonDal.getLineNumber(tran, "IGN1", Id.ToString());
                                
                                
                                #region BackEnd check for Qty 
                                if (!isReturn)      //If not q return Type
                                {
                                    
                                    if (Convert.ToDecimal(item.QTY) > CmpltQty)
                                    {
                                        tran.Rollback();
                                        response.isSuccess = false;
                                        response.Message = "Quantity can't be greater then Remaining Quantity !";
                                        return response;
                                    }
                                }
                                else
                                {

                                    decimal ExtraQty = GetExtraQty(item.BaseType.ToString(), item.ItemCode.ToString(), Convert.ToInt32(item.RowNum), tran);
                                    if (Convert.ToDecimal(item.QTY) > ExtraQty)
                                    {
                                        tran.Rollback();
                                        response.isSuccess = false;
                                        response.Message = "Quantity can't be greater then Issued Quantity !";
                                        return response;
                                    }
                                    RowNum = Convert.ToInt32(item.RowNum);
                                }
                                #endregion

                                #region OITL Log
                                int LogEntry = CommonDal.getPrimaryKey(tran, "LogEntry", "OITL");   //Primary Key

                                OITL OITLModel = new OITL();
                                OITLModel.LogEntry = LogEntry;
                                OITLModel.CardCode = model.HeaderData.CardCode.ToString();
                                OITLModel.CardName = model.HeaderData.CardName.ToString();
                                OITLModel.ItemCode = item.ItemCode.ToString();
                                OITLModel.ItemName = item.ItemName.ToString();
                                OITLModel.ID = Convert.ToInt32(model.ID);
                                OITLModel.DocLine = LineNum;
                                OITLModel.DocType = 59;
                                OITLModel.BaseType = "";
                                OITLModel.Quantity = (decimal)item.QTY;
                                OITLModel.DocDate = Convert.ToDateTime(model.HeaderData.DocDate);

                                if (!cdal.OITLLog(tran, OITLModel))
                                {
                                    tran.Rollback();
                                    response.isSuccess = false;
                                    response.Message = "An Error Occured";
                                    return response;
                                }

                                    #region Batches & Log Working

                                    if (model.Batches != null)
                                    {
                                        bool resp = cdal.InBatches(tran, model.Batches, item.ItemCode.ToString(), LogEntry, item.Warehouse.ToString(), index);
                                        if (!resp)
                                        {                                        
                                                tran.Rollback();
                                                response.isSuccess = false;
                                                response.Message = "An Error Occured";
                                                return response;
                                        
                                        }

                                    }
                                    #endregion
                                #endregion
                            }

                            #region sqlparam
                            List<SqlParameter> param1 = new List<SqlParameter>();
                            param1.Add(cdal.GetParameter("@Id", Id, typeof(int)));
                            param1.Add(cdal.GetParameter("@LineNum", LineNum, typeof(int)));
                            param1.Add(cdal.GetParameter("@BaseRef", item.BaseType, typeof(string)));
                            param1.Add(cdal.GetParameter("@BaseType", 102, typeof(int)));
                            param1.Add(cdal.GetParameter("@ItemCode", item.ItemCode, typeof(string)));
                            param1.Add(cdal.GetParameter("@Dscription", item.ItemName, typeof(string)));
                            param1.Add(cdal.GetParameter("@WhsCode", item.Warehouse, typeof(string)));
                            param1.Add(cdal.GetParameter("@Quantity", item.QTY, typeof(decimal)));
                            param1.Add(cdal.GetParameter("@TranType", item.TranType, typeof(char)));
                            param1.Add(cdal.GetParameter("@Price", item.UPrc, typeof(decimal)));
                            param1.Add(cdal.GetParameter("@LineTotal", item.TtlPrc, typeof(decimal)));
                            param1.Add(cdal.GetParameter("@AcctCode", item.AcctCode, typeof(string)));
                            param1.Add(cdal.GetParameter("@UomEntry", item.UomEntry, typeof(int)));
                            param1.Add(cdal.GetParameter("@UomCode", item.UomCode, typeof(string)));
                            param1.Add(cdal.GetParameter("@OpenQty", item.QTY, typeof(decimal)));
                            param1.Add(cdal.GetParameter("@SaleOrderCode", item.SaleOrderCode, typeof(int)));
                            param1.Add(cdal.GetParameter("@BaseQty", item.PlannedQty, typeof(decimal)));
                            param1.Add(cdal.GetParameter("@SaleOrderDocNo", item.SaleOrderDocNo, typeof(string)));
                            param1.Add(cdal.GetParameter("@isReturn", isReturn, typeof(bool)));
                            param1.Add(cdal.GetParameter("@RowNum", RowNum, typeof(int)));
                            #endregion

                            res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, ITT1_Query, param1.ToArray()).ToInt();
                            if (res1 <= 0)
                            {
                                tran.Rollback();
                                response.isSuccess = false;
                                response.Message = "An Error Occured";
                                return response;

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



                            #region Update Production Order 
                            if (!isReturn)  //Update Completed Qty
                            {                           
                                string POquery = @"update OWOR set CmpltQty = (CmpltQty - " + qty + ") +" + item.QTY + " where DocNum = '" + item.BaseType + "'";
                                res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, POquery).ToInt();
                                if (res1 <= 0)
                                {
                                    tran.Rollback();
                                    response.isSuccess = false;
                                    response.Message = "An Error Occured";
                                    return response;
                                }
                            }
                            else
                            {
                                int OWORID = SqlHelper.ExecuteScalar(tran, CommandType.Text, @"select Id from OWOR where DocNum ='" + item.BaseType + "'");

                                string UPWOR1 = @"update WOR1 set IssuedQty = (IssuedQty + "+qty+" ) -" + item.QTY + "where Id=" + OWORID + "and ItemCode = '" + item.ItemCode + "' and LineNum = " + item.RowNum + "-1";
                                res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, UPWOR1).ToInt();
                                if (res1 <= 0)
                                {
                                    tran.Rollback();
                                    response.isSuccess = false;
                                    response.Message = "An Error Occured";
                                    return response;
                                }
                            }
                            #endregion

                            ++index;
                        }
                    }


                }
                if (res1 > 0)
                {
                    tran.Commit();
                    response.isSuccess = true;
                    response.Message = "Receipt From Production Updated Successfully !";

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
