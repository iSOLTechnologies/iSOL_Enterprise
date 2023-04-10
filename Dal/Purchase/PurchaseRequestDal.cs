using iSOL_Enterprise.Common;
using iSOL_Enterprise.Models;
using iSOL_Enterprise.Models.sale;
using Newtonsoft.Json;
using SqlHelperExtensions;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace iSOL_Enterprise.Dal.Purchase
{
    public class PurchaseRequestDal
    {



        public dynamic GetPurchaseRequestEditDetails(int id)
        {
            DataSet ds = new DataSet();
            SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
            SqlDataAdapter sda = new SqlDataAdapter("select * from OPRQ where id = " + id + ";select * from PRQ1 where id = " + id + "", conn);
            sda.Fill(ds);
            return ds;
        }





        public List<SalesQuotation_MasterModels> GetData()
        {
            string GetQuery = "select * from OPRQ order by id DESC";


            List<SalesQuotation_MasterModels> list = new List<SalesQuotation_MasterModels>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {
                    list.Add(
                        new SalesQuotation_MasterModels()
                        {
                            //DocStatus = CommonDal.Check_IsNotEditable("PQT1", rdr["Id"].ToInt()) == false ? "Open" : "Closed",
                            DocStatus = "Open",
                            Id = rdr["Id"].ToInt(),
                            DocNum = rdr["DocNum"].ToString(),
                            DocDate = rdr["DocDueDate"].ToDateTime(),
                            CardName = rdr["Requester"].ToString(),
                        }
                        );
                }
            }
            return list;
        }
        public List<UsersModels> GetUsers()
        {
            string GetQuery = "select U_NAME,Department,Branch,USER_CODE from OUSR";


            List<UsersModels> list = new List<UsersModels>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {
                    list.Add(new UsersModels()
                    {
                        Username = rdr["U_NAME"].ToString(),
                        UserCode = rdr["USER_CODE"].ToString(),
                        Department = rdr["Department"].ToInt(),
                        Branch = rdr["Branch"].ToInt()
                    });
                }
            }
            return list;
        }
        public List<UsersModels> GetEmployes()
        {
            string GetQuery = "select firstName,lastName,Code from OHEM";


            List<UsersModels> list = new List<UsersModels>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {
                    list.Add(new UsersModels()
                    {
                        FirstName = rdr["firstName"].ToString(),
                        LastName = rdr["lastName"].ToString(),
                        UserCode = rdr["Code"].ToString()
                    });
                }
            }
            return list;
        }
        public List<tbl_OSLP> GetBranch()
        {
            string GetQuery = "select Code,Name from OUBR";


            List<tbl_OSLP> list = new List<tbl_OSLP>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {
                    list.Add(new tbl_OSLP()
                    {
                        SlpCode = rdr["Code"].ToInt(),
                        SlpName = rdr["Name"].ToString()
                    });
                }
            }
            return list;
        }
        public List<tbl_OSLP> GetDepartment()
        {
            string GetQuery = "select Code,Name from OUDP";


            List<tbl_OSLP> list = new List<tbl_OSLP>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {
                    list.Add(new tbl_OSLP()
                    {
                        SlpCode = rdr["Code"].ToInt(),
                        SlpName = rdr["Name"].ToString()
                    });
                }
            }
            return list;
        }
        public ResponseModels AddPurchaseRequest(string formData)
        {
            var model = JsonConvert.DeserializeObject<dynamic>(formData);
            string DocType = model.ListItems == null ? "S" : "I";

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
                    int Id = CommonDal.getPrimaryKey(tran, "OPRQ");

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

                    string HeadQuery = @"insert into OPRQ (Id,Guid,DocType,ReqType,Requester,MySeries,DocNum,Series,ReqName,Branch,Department,DocDate,DocDueDate,Notify,Email,TaxDate,ReqDate,OwnerCode,Comments,DocTotal) 
                                        values(@Id,@Guid,@DocType,@ReqType,@Requester,@MySeries,@DocNum,@Series,@ReqName,@Branch,@Department,@DocDate,@DocDueDate,@Notify,@Email,@TaxDate,@ReqDate,@OwnerCode,@Comments,@DocTotal)";



                    #region SqlParameters

                    #region Header data
                    param.Add(cdal.GetParameter("@DocType", DocType, typeof(char)));
                    param.Add(cdal.GetParameter("@ReqType", model.HeaderData.ReqType, typeof(int)));
                    param.Add(cdal.GetParameter("@Requester", model.HeaderData.Requester, typeof(string)));
                    param.Add(cdal.GetParameter("@MySeries", model.HeaderData.MySeries, typeof(int)));
                    param.Add(cdal.GetParameter("@DocNum", model.HeaderData.DocNum, typeof(string)));
                    param.Add(cdal.GetParameter("@Series", model.HeaderData.Series, typeof(int)));
                    param.Add(cdal.GetParameter("@ReqName", model.HeaderData.ReqName, typeof(string)));
                    param.Add(cdal.GetParameter("@Branch", model.HeaderData.Branch, typeof(short)));
                    param.Add(cdal.GetParameter("@Department", model.HeaderData.Department, typeof(short)));
                    param.Add(cdal.GetParameter("@DocDate", model.HeaderData.DocDate, typeof(DateTime)));
                    param.Add(cdal.GetParameter("@DocDueDate", model.HeaderData.DocDueDate, typeof(DateTime)));
                    param.Add(cdal.GetParameter("@Notify", model.HeaderData.Notify, typeof(char)));
                    param.Add(cdal.GetParameter("@TaxDate", model.HeaderData.TaxDate, typeof(DateTime)));
                    param.Add(cdal.GetParameter("@ReqDate", model.HeaderData.ReqDate, typeof(DateTime)));
                    if (model.HeaderData.Notify == "Y")
                    {
                        param.Add(cdal.GetParameter("@Email", model.HeaderData.Email, typeof(string)));
                    }
                    else
                    {
                        param.Add(cdal.GetParameter("@Email", "", typeof(string)));
                    }
                    #endregion

                    #region Footer Data
                    param.Add(cdal.GetParameter("@OwnerCode", model.FooterData.OwnerCode, typeof(string)));
                    param.Add(cdal.GetParameter("@Comments", model.FooterData.Comments, typeof(string)));
                    param.Add(cdal.GetParameter("@DocTotal", model.FooterData.Total, typeof(string)));
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

                            string RowQueryItem1 = @"insert into PRQ1
                                (Id,LineNum,ItemCode,LineVendor,PQTReqDate,Quantity,OpenQty,WhsCode,DiscPrcnt,Price,VatGroup,UomEntry,UomCode,LineTotal,CountryOrg)
                          values(@Id,@LineNum,@ItemCode,@LineVendor,@PQTReqDate,@Quantity,@OpenQty,@WhsCode,@DiscPrcnt,@Price,@VatGroup,@UomEntry,@UomCode,@LineTotal,@CountryOrg)";
                            var BaseRef = item.BaseRef;
                            #region sqlparam
                            List<SqlParameter> param1 = new List<SqlParameter>();
                            param1.Add(cdal.GetParameter("@Id", Id, typeof(int)));
                            param1.Add(cdal.GetParameter("@LineNum", LineNum, typeof(int)));
                            param1.Add(cdal.GetParameter("@ItemCode", item.ItemCode, typeof(string)));
                            param1.Add(cdal.GetParameter("@LineVendor", item.LineVendor, typeof(string)));
                            param1.Add(cdal.GetParameter("@PQTReqDate", item.PQTReqDate, typeof(DateTime)));
                            param1.Add(cdal.GetParameter("@Quantity", item.QTY, typeof(decimal)));
                            param1.Add(cdal.GetParameter("@OpenQty", item.QTY, typeof(decimal)));
                            param1.Add(cdal.GetParameter("@WhsCode", item.WhsCode, typeof(string)));
                            param1.Add(cdal.GetParameter("@Price", item.Price, typeof(decimal)));
                            param1.Add(cdal.GetParameter("@DiscPrcnt", item.DicPrc, typeof(decimal)));
                            param1.Add(cdal.GetParameter("@VatGroup", item.VatGroup, typeof(string)));
                            param1.Add(cdal.GetParameter("@UomEntry", item.UomEntry, typeof(int)));
                            param1.Add(cdal.GetParameter("@UomCode", item.UomCode, typeof(string)));
                            param1.Add(cdal.GetParameter("@LineTotal", item.TtlPrc, typeof(decimal)));
                            param1.Add(cdal.GetParameter("@CountryOrg", item.CountryOrg, typeof(string)));

                            #endregion

                            res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, RowQueryItem1, param1.ToArray()).ToInt();
                            if (res1 <= 0)
                            {
                                tran.Rollback();
                                response.isSuccess = false;
                                response.Message = "An Error Occured";
                                return response;
                            }
                            LineNum += 1;

                        }
                    }
                    else if (model.ListService != null)
                    {
                        int LineNum = 0;

                        foreach (var item in model.ListService)
                        {
                            string RowQueryItem1 = @"insert into PRQ1
                                (Id,LineNum,Dscription,LineVendor,PQTReqDate,AcctCode,VatGroup,LineTotal)
                          values(@Id,@LineNum,@Dscription,@LineVendor,@PQTReqDate,@AcctCode,@VatGroup,@LineTotal)";

                            #region sqlparam
                            List<SqlParameter> param1 = new List<SqlParameter>();
                            param1.Add(cdal.GetParameter("@Id", Id, typeof(int)));
                            param1.Add(cdal.GetParameter("@LineNum", LineNum, typeof(int)));
                            param1.Add(cdal.GetParameter("@Dscription", item.Dscription, typeof(string)));
                            param1.Add(cdal.GetParameter("@PQTReqDate", item.PQTReqDate, typeof(DateTime)));
                            param1.Add(cdal.GetParameter("@LineVendor", item.LineVendor, typeof(string)));
                            param1.Add(cdal.GetParameter("@AcctCode", item.AcctCode, typeof(string)));
                            param1.Add(cdal.GetParameter("@LineTotal", item.TotalLC, typeof(decimal)));
                            param1.Add(cdal.GetParameter("@VatGroup", item.VatGroup, typeof(string)));
                            #endregion

                            res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, RowQueryItem1, param1.ToArray()).ToInt();
                            if (res1 <= 0)
                            {
                                tran.Rollback();
                                response.isSuccess = false;
                                response.Message = "An Error Occured";
                                return response;
                            }
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
                    response.Message = "Purchase Request Added Successfully !";

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


        public ResponseModels EditPurchaseRequest(string formData)
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

                    param.Add(cdal.GetParameter("@Id", model.HeaderData.MyId, typeof(int)));

                    string HeadQuery = @"Update OPRQ set ReqType = @ReqType,Requester = @Requester,ReqName = @ReqName,Branch = @Branch,Department = @Department,DocDate = @DocDate,DocDueDate = @DocDueDate,Notify = @Notify,Email = @Email,TaxDate = @TaxDate,ReqDate = @ReqDate,OwnerCode = @OwnerCode,Comments = @Comments,DocTotal = @DocTotal Where Id = @Id";


                    #region SqlParameters

                    #region Header data
                    //param.Add(cdal.GetParameter("@DocType", DocType, typeof(char)));
                    param.Add(cdal.GetParameter("@ReqType", model.HeaderData.ReqType, typeof(int)));
                    param.Add(cdal.GetParameter("@Requester", model.HeaderData.Requester, typeof(string)));
                    param.Add(cdal.GetParameter("@ReqName", model.HeaderData.ReqName, typeof(string)));
                    param.Add(cdal.GetParameter("@Branch", model.HeaderData.Branch, typeof(short)));
                    param.Add(cdal.GetParameter("@Department", model.HeaderData.Department, typeof(short)));
                    param.Add(cdal.GetParameter("@DocDate", model.HeaderData.DocDate, typeof(DateTime)));
                    param.Add(cdal.GetParameter("@DocDueDate", model.HeaderData.DocDueDate, typeof(DateTime)));
                    param.Add(cdal.GetParameter("@Notify", model.HeaderData.Notify, typeof(char)));
                    param.Add(cdal.GetParameter("@TaxDate", model.HeaderData.TaxDate, typeof(DateTime)));
                    param.Add(cdal.GetParameter("@ReqDate", model.HeaderData.ReqDate, typeof(DateTime)));
                    if (model.HeaderData.Notify == "Y")
                    {
                        param.Add(cdal.GetParameter("@Email", model.HeaderData.Email, typeof(string)));
                    }
                    else
                    {
                        param.Add(cdal.GetParameter("@Email", "", typeof(string)));
                    }
                    #endregion

                    #region Footer Data
                    param.Add(cdal.GetParameter("@OwnerCode", model.FooterData.OwnerCode, typeof(string)));
                    param.Add(cdal.GetParameter("@Comments", model.FooterData.Comments, typeof(string)));
                    param.Add(cdal.GetParameter("@DocTotal", model.FooterData.Total, typeof(string)));
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
                        foreach (var item in model.ListItems)
                        {

                            if (item.LineNum != "" && item.LineNum != null)
                            {
                                if (item.LineNum != "" && item.LineNum != null)
                                {
                                    decimal OpenQty = Convert.ToDecimal(SqlHelper.ExecuteScalar(SqlHelper.defaultDB, CommandType.Text, "select OpenQty from PRQ1 where Id=" + model.HeaderData.MyId + " and LineNum=" + item.LineNum + ""));
                                    if (OpenQty > 0)
                                    {

                                        string RowQueryItem1 = @"Update PRQ1 set
                                ItemCode = @ItemCode,LineVendor = @LineVendor,PQTReqDate = @PQTReqDate,Quantity = @Quantity,WhsCode = @WhsCode,DiscPrcnt = @DiscPrcnt,Price = @Price,VatGroup = @VatGroup,UomEntry = @UomEntry,UomCode = @UomCode,LineTotal = @LineTotal,CountryOrg = @CountryOrg Where Id = @Id ";
                                        var BaseRef = item.BaseRef;
                                        #region sqlparam
                                        List<SqlParameter> param1 = new List<SqlParameter>();
                                        param1.Add(cdal.GetParameter("@Id", model.HeaderData.MyId, typeof(int)));
                                        param1.Add(cdal.GetParameter("@LineNum", item.LineNum, typeof(int)));
                                        param1.Add(cdal.GetParameter("@ItemCode", item.ItemCode, typeof(string)));
                                        param1.Add(cdal.GetParameter("@LineVendor", item.LineVendor, typeof(string)));
                                        param1.Add(cdal.GetParameter("@PQTReqDate", item.PQTReqDate, typeof(DateTime)));
                                        param1.Add(cdal.GetParameter("@Quantity", item.QTY, typeof(decimal)));
                                        param1.Add(cdal.GetParameter("@WhsCode", item.WhsCode, typeof(string)));
                                        param1.Add(cdal.GetParameter("@Price", item.UPrc, typeof(decimal)));
                                        param1.Add(cdal.GetParameter("@DiscPrcnt", item.DicPrc, typeof(decimal)));
                                        param1.Add(cdal.GetParameter("@VatGroup", item.VatGroup, typeof(string)));
                                        param1.Add(cdal.GetParameter("@UomEntry", item.UomEntry, typeof(int)));
                                        param1.Add(cdal.GetParameter("@UomCode", item.UomCode, typeof(string)));
                                        param1.Add(cdal.GetParameter("@LineTotal", item.TtlPrc, typeof(decimal)));
                                        param1.Add(cdal.GetParameter("@CountryOrg", item.CountryOrg, typeof(string)));

                                        #endregion

                                        res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, RowQueryItem1, param1.ToArray()).ToInt();
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
                            else
                            {
                                int LineNo = CommonDal.getLineNumber(tran, "QUT1", model.HeaderData.MyId.ToString());
                                string RowQueryItem1 = @"insert into PRQ1
                                (Id,LineNum,ItemCode,LineVendor,PQTReqDate,Quantity,OpenQty,WhsCode,DiscPrcnt,Price,VatGroup,UomEntry,UomCode,LineTotal,CountryOrg)
                          values(@Id,@LineNum,@ItemCode,@LineVendor,@PQTReqDate,@Quantity,@OpenQty,@WhsCode,@DiscPrcnt,@Price,@VatGroup,@UomEntry,@UomCode,@LineTotal,@CountryOrg)";
                                var BaseRef = item.BaseRef;
                                #region sqlparam
                                List<SqlParameter> param1 = new List<SqlParameter>();
                                param1.Add(cdal.GetParameter("@Id", model.HeaderData.MyId, typeof(int)));
                                param1.Add(cdal.GetParameter("@LineNum", LineNo, typeof(int)));
                                param1.Add(cdal.GetParameter("@ItemCode", item.ItemCode, typeof(string)));
                                param1.Add(cdal.GetParameter("@LineVendor", item.LineVendor, typeof(string)));
                                param1.Add(cdal.GetParameter("@PQTReqDate", item.PQTReqDate, typeof(DateTime)));
                                param1.Add(cdal.GetParameter("@Quantity", item.QTY, typeof(decimal)));
                                param1.Add(cdal.GetParameter("@OpenQty", item.QTY, typeof(decimal)));
                                param1.Add(cdal.GetParameter("@WhsCode", item.WhsCode, typeof(string)));
                                param1.Add(cdal.GetParameter("@Price", item.Price, typeof(decimal)));
                                param1.Add(cdal.GetParameter("@DiscPrcnt", item.DicPrc, typeof(decimal)));
                                param1.Add(cdal.GetParameter("@VatGroup", item.VatGroup, typeof(string)));
                                param1.Add(cdal.GetParameter("@UomEntry", item.UomEntry, typeof(int)));
                                param1.Add(cdal.GetParameter("@UomCode", item.UomCode, typeof(string)));
                                param1.Add(cdal.GetParameter("@LineTotal", item.TtlPrc, typeof(decimal)));
                                param1.Add(cdal.GetParameter("@CountryOrg", item.CountryOrg, typeof(string)));

                                #endregion

                                res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, RowQueryItem1, param1.ToArray()).ToInt();
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
                    else if (model.ListService != null)
                    {
                        int LineNum = 0;

                        foreach (var item in model.ListService)
                        {


                            string RowQueryItem1 = @"Update PRQ1 Dscription=@Dscription ,LineVendor=@LineVendor ,PQTReqDate=@PQTReqDate ,AcctCode=@AcctCode ,VatGroup=@VatGroup ,LineTotal =@LineTotal  Where Id = @Id and LineNum = @LineNum";

                            #region sqlparam
                            List<SqlParameter> param1 = new List<SqlParameter>();
                            param1.Add(cdal.GetParameter("@Id", model.HeaderData.MyId, typeof(int)));
                            param1.Add(cdal.GetParameter("@LineNum", LineNum, typeof(int)));
                            param1.Add(cdal.GetParameter("@Dscription", item.Dscription, typeof(string)));
                            param1.Add(cdal.GetParameter("@PQTReqDate", item.PQTReqDate, typeof(DateTime)));
                            param1.Add(cdal.GetParameter("@LineVendor", item.LineVendor, typeof(string)));
                            param1.Add(cdal.GetParameter("@AcctCode", item.AcctCode, typeof(string)));
                            param1.Add(cdal.GetParameter("@LineTotal", item.TotalLC, typeof(decimal)));
                            param1.Add(cdal.GetParameter("@VatGroup", item.VatGroup, typeof(string)));
                            #endregion

                            res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, RowQueryItem1, param1.ToArray()).ToInt();
                            if (res1 <= 0)
                            {
                                tran.Rollback();
                                response.isSuccess = false;
                                response.Message = "An Error Occured";
                                return response;
                            }
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
                    response.Message = "Purchase Request Added Successfully !";
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
