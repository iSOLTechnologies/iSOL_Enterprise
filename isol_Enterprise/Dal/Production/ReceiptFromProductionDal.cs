
using iSOL_Enterprise.Common;
using iSOL_Enterprise.Models;
using iSOL_Enterprise.Models.Logs;
using Newtonsoft.Json;
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
            string GetQuery = "select Id,Guid,DocNum,DocDate,Comments,JrnlMemo,isPosted,is_Edited from OIGN where BaseType = '102'  order by id DESC";


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
                    list.Add(models);
                }
            }
            return list;
        }

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

        public int GetId(string guid)
        {
            guid = HttpUtility.UrlDecode(guid);
            return Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.defaultDB, CommandType.Text, "select Id from OIGN where GUID ='" + guid.ToString() + "'"));

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
                string headerQuery = @"select Id,LineNum,BaseRef,BaseType,ItemCode,Dscription,WhsCode,Quantity,TranType,Price,LineTotal,AcctCode,SaleOrderCode From IGN1 where Id =" + id;
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

                    string HeadQuery = @"insert into OIGN (Id,Guid,DocEntry,MySeries,DocNum,Series,DocDate,Ref2,Comments,JrnlMemo,DocTotal,BaseType) 
                                        values(@Id,@Guid,@DocEntry,@MySeries,@DocNum,@Series,@DocDate,@Ref2,@Comments,@JrnlMemo,@DocTotal,@BaseType)";



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
                            string Tabitem = "Id,LineNum,BaseRef,BaseType,ItemCode,Dscription,WhsCode,Quantity,TranType,Price,LineTotal,AcctCode,UomEntry,UomCode,OpenQty,SaleOrderCode";
                            string TabitemP = "@Id,@LineNum,@BaseRef,@BaseType,@ItemCode,@Dscription,@WhsCode,@Quantity,@TranType,@Price,@LineTotal,@AcctCode,@UomEntry,@UomCode,@OpenQty,@SaleOrderCode";
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
                    string TabHeader = @"DocDate =@DocDate,Ref2=@Ref2,Comments=@Comments,JrnlMemo=@JrnlMemo,DocTotal=@DocTotal,is_Edited=1";

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
                        foreach (var item in model.ListItems)
                        {
                            param.Clear();
                            string ITT1_Query = "";
                            if (item.LineNum != null && item.LineNum != "")
                            {

                                string Tabitem = @"BaseRef=@BaseRef,BaseType=@BaseType,ItemCode=@ItemCode,Dscription=@Dscription,WhsCode=@WhsCode,Quantity=@Quantity,
                                                   TranType=@TranType,Price=@Price,LineTotal=@LineTotal,AcctCode=@AcctCode,UomEntry=@UomEntry,UomCode=@UomCode,OpenQty=@OpenQty,SaleOrderCode=@SaleOrderCode";

                                ITT1_Query = @"update IGN1 set " + Tabitem + " where id=" + Id + " and LineNum=" + item.LineNum;

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
                                                OITLModel.BaseType = "NULL";
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
                                string Tabitem = "Id,LineNum,BaseRef,BaseType,ItemCode,Dscription,WhsCode,Quantity,TranType,Price,LineTotal,AcctCode,UomEntry,UomCode,OpenQty,SaleOrderCode";
                                string TabitemP = "@Id,@LineNum,@BaseRef,@BaseType,@ItemCode,@Dscription,@WhsCode,@Quantity,@TranType,@Price,@LineTotal,@AcctCode,@UomEntry,@UomCode,@OpenQty,@SaleOrderCode";
                                ITT1_Query = @"insert into IGN1 (" + Tabitem + ") " +
                                                     "values(" + TabitemP + ")";
                                LineNum = CommonDal.getLineNumber(tran, "IGN1", Id.ToString());

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
                                OITLModel.BaseType = "NULL";
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

                            #endregion

                            res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, ITT1_Query, param1.ToArray()).ToInt();
                            if (res1 <= 0)
                            {
                                tran.Rollback();
                                response.isSuccess = false;
                                response.Message = "An Error Occured";
                                return response;

                            }
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
