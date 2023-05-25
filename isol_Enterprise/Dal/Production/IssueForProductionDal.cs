using iSOL_Enterprise.Common;
using iSOL_Enterprise.Models;
using iSOL_Enterprise.Models.Logs;
using Newtonsoft.Json;
using SqlHelperExtensions;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace iSOL_Enterprise.Dal.Production
{
    public class IssueForProductionDal
    {

        public List<SalesQuotation_MasterModels> GetData()
        {
            string GetQuery = "select Id,Guid,DocNum,DocDate,Comments,JrnlMemo,isPosted,is_Edited from OIGE where BaseType = '202'  order by id DESC";


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

        

        public int GetId(string guid)
        {
            guid = HttpUtility.UrlDecode(guid);
            return Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.defaultDB, CommandType.Text, "select Id from OIGE where GUID ='" + guid.ToString() + "'"));

        }
        public dynamic GetOldHeaderData(int id)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
                string headerQuery = @"select MySeries,DocNum,Series,DocDate,Ref2,Comments,JrnlMemo,DocTotal From OIGE where Id =" + id;
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
                string headerQuery = @"select Id,LineNum,BaseRef,BaseType,ItemCode,Dscription,WhsCode,Quantity,TranType,Price,LineTotal,AcctCode,SaleOrderCode From IGE1 where Id =" + id;
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

        public ResponseModels AddUpdateIssueForProduction(string formData)
        {
            ResponseModels response = new ResponseModels();

            try
            {


                var model = JsonConvert.DeserializeObject<dynamic>(formData);
                if (model.OldId != null)
                {
                    response = EditIssueForProduction(model);
                }
                else
                {
                    response = AddIssueForProduction(model);
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
        public ResponseModels AddIssueForProduction(dynamic model)
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
                    int Id = CommonDal.getPrimaryKey(tran, "OIGE");

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
                        int count = SqlHelper.ExecuteScalar(tran, CommandType.Text, "select Count(*) from OIGE where DocNum ='" + model.HeaderData.DocNum.ToString() + "'");
                        if (count > 0)
                        {
                            tran.Rollback();
                            response.isSuccess = false;
                            response.Message = "Duplicate Document Number !";
                            return response;
                        }
                    }
                    #endregion

                    string HeadQuery = @"insert into OIGE (Id,Guid,DocEntry,MySeries,DocNum,Series,DocDate,Ref2,Comments,JrnlMemo,DocTotal,BaseType) 
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

                    param.Add(cdal.GetParameter("@BaseType", 202, typeof(int)));


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
                            string ITT1_Query = @"insert into IGE1 (" + Tabitem + ") " +
                                                 "values(" + TabitemP + ")";


                            #region sqlparam
                            List<SqlParameter> param1 = new List<SqlParameter>();
                            param1.Add(cdal.GetParameter("@Id", Id, typeof(int)));
                            param1.Add(cdal.GetParameter("@LineNum", LineNum, typeof(int)));
                            param1.Add(cdal.GetParameter("@BaseRef", item.BaseType, typeof(string)));
                            param1.Add(cdal.GetParameter("@BaseType", 202, typeof(int)));
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
                    response.Message = "Issue For Production Added Successfully !";

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
        public ResponseModels EditIssueForProduction(dynamic model)
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

                    string HeadQuery = @"Update OIGE set " + TabHeader + " where GUID = '" + model.OldId + "'";

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

                    param.Add(cdal.GetParameter("@BaseType", 202, typeof(int)));


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
                            param.Clear();
                            string ITT1_Query = "";
                            if (item.LineNum != null && item.LineNum != "")
                            {

                                string Tabitem = @"BaseRef=@BaseRef,BaseType=@BaseType,ItemCode=@ItemCode,Dscription=@Dscription,WhsCode=@WhsCode,Quantity=@Quantity,
                                                   TranType=@TranType,Price=@Price,LineTotal=@LineTotal,AcctCode=@AcctCode,UomEntry=@UomEntry,UomCode=@UomCode,OpenQty=@OpenQty,SaleOrderCode=@SaleOrderCode";

                                ITT1_Query = @"update IGE1 set " + Tabitem + " where id=" + Id + " and LineNum=" + item.LineNum;
                            }
                            else
                            {
                                string Tabitem = "Id,LineNum,BaseRef,BaseType,ItemCode,Dscription,WhsCode,Quantity,TranType,Price,LineTotal,AcctCode,UomEntry,UomCode,OpenQty,SaleOrderCode";
                                string TabitemP = "@Id,@LineNum,@BaseRef,@BaseType,@ItemCode,@Dscription,@WhsCode,@Quantity,@TranType,@Price,@LineTotal,@AcctCode,@UomEntry,@UomCode,@OpenQty,@SaleOrderCode";
                                ITT1_Query = @"insert into IGE1 (" + Tabitem + ") " +
                                                     "values(" + TabitemP + ")";
                                LineNum = CommonDal.getLineNumber(tran, "IGN1", Id.ToString());
                            }


                            #region sqlparam
                            List<SqlParameter> param1 = new List<SqlParameter>();
                            param1.Add(cdal.GetParameter("@Id", Id, typeof(int)));
                            param1.Add(cdal.GetParameter("@LineNum", LineNum, typeof(int)));
                            param1.Add(cdal.GetParameter("@BaseRef", item.BaseType, typeof(string)));
                            param1.Add(cdal.GetParameter("@BaseType", 202, typeof(int)));
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

                        }
                    }


                }
                if (res1 > 0)
                {
                    tran.Commit();
                    response.isSuccess = true;
                    response.Message = "Issue For Production Updated Successfully !";

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
