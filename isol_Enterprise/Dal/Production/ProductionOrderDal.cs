using iSOL_Enterprise.Models;
using Newtonsoft.Json;
using SqlHelperExtensions;
using System.Data.SqlClient;
using System.Data;
using iSOL_Enterprise.Common;
using System.Reflection;
using SAPbobsCOM;
using System.Web;
using System;

namespace iSOL_Enterprise.Dal.Production
{
    public class ProductionOrderDal
    {

        public List<SalesQuotation_MasterModels> GetData()
        {
            string GetQuery = "select Id,Guid,DocNum,ItemCode,PostDate,PlannedQty,Warehouse,isPosted,is_Edited,isApproved,apprSeen from OWOR order by id DESC";


            List<SalesQuotation_MasterModels> list = new List<SalesQuotation_MasterModels>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {

                    SalesQuotation_MasterModels models = new SalesQuotation_MasterModels();

                    models.DocStatus = "Open";
                    models.Id = rdr["Id"].ToInt();
                    models.CardCode = rdr["DocNum"].ToString();
                    models.CardName = rdr["ItemCode"].ToString();
                    models.DocDate = Convert.ToDateTime(rdr["PostDate"]);
                    models.Quanity = Convert.ToDecimal(rdr["PlannedQty"]);
                    models.Guid = rdr["Guid"].ToString();
                    models.Warehouse = rdr["Warehouse"].ToString();
                    models.IsPosted = rdr["isPosted"].ToString();
                    models.IsEdited = rdr["is_Edited"].ToString();
                    models.isApproved = rdr["isApproved"].ToBool();
                    models.apprSeen = rdr["apprSeen"].ToBool();
                    list.Add(models);
                }
            }
            return list;
        }
        public int GetId(string guid)
        {
            guid = HttpUtility.UrlDecode(guid);
            return Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.defaultDB, CommandType.Text, "select Id from OWOR where GUID ='" + guid.ToString() + "'"));

        }
        public dynamic GetOldHeaderData(int id)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
                string headerQuery = @"select Id,Guid,DocEntry,Type,Series,MySeries,DocNum,Status,PostDate, ItemCode,StartDate,ProdName,Priority,
                                         DueDate,PlannedQty,Warehouse,LinkToObj,OriginNum,CardCode,Project,Comments,PickRmrk,Sap_Ref_No From OWOR where Id =" + id;
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
                string headerQuery = @"select Id,DocEntry,LineNum,VisOrder,ItemCode,ItemName,BaseQty,PlannedQty,wareHouse,IssueType From WOR1 where Id =" + id;
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

        public ResponseModels AddUpdateProductionOrder(string formData)
        {
            ResponseModels response = new ResponseModels();

            try
            {


                var model = JsonConvert.DeserializeObject<dynamic>(formData);
                if (model.OldId != null)
                {
                    response = EditProductionOrder(model);
                }
                else
                {
                    response = AddProductionOrder(model);
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
        public ResponseModels AddProductionOrder(dynamic model)
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
                    int Id = CommonDal.getPrimaryKey(tran, "OWOR");
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
                    #endregion
                    else
                    {
                        int count = SqlHelper.ExecuteScalar(tran, CommandType.Text, "select Count(*) from OWOR where DocNum ='" + model.HeaderData.DocNum.ToString() + "'");
                        if (count > 0)
                        {
                            tran.Rollback();
                            response.isSuccess = false;
                            response.Message = "Duplicate Document Number !";
                            return response;
                        }
                    }


                    int ObjectCode = 202;
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
                            response.isSuccess = false;
                            response.Message = "An Error Occured";
                            return response;
                        }
                    }

                    #endregion

                    string TabHeader = @"Id,Guid,DocEntry,Type,Series,MySeries,DocNum,Status,PostDate, ItemCode,StartDate,ProdName,
                                         DueDate,PlannedQty,Warehouse,Priority,LinkToObj,OriginNum,CardCode,Project,Comments,PickRmrk,isApproved";
                    string TabHeaderP = @"@Id,@Guid,@DocEntry,@Type,@Series,@MySeries,@DocNum,@Status,@PostDate,@ItemCode,@StartDate,@ProdName,
                                         @DueDate,@PlannedQty,@Warehouse,@Priority,@LinkToObj,@OriginNum,@CardCode,@Project,@Comments,@PickRmrk,@isApproved";

                    string HeadQuery = @"insert into OWOR (" + TabHeader + ") " +
                                        "values(" + TabHeaderP + ")";



                    #region SqlParameters

                    #region Header data
                    param.Add(cdal.GetParameter("@Type", model.HeaderData.Type, typeof(char)));
                    param.Add(cdal.GetParameter("@Series", model.HeaderData.Series, typeof(int)));
                    param.Add(cdal.GetParameter("@MySeries", model.HeaderData.MySeries, typeof(int)));
                    param.Add(cdal.GetParameter("@DocNum", model.HeaderData.DocNum, typeof(string)));
                    param.Add(cdal.GetParameter("@Status", model.HeaderData.Status, typeof(char)));
                    param.Add(cdal.GetParameter("@PostDate", model.HeaderData.PostDate, typeof(DateTime)));
                    param.Add(cdal.GetParameter("@ItemCode", model.HeaderData.ItemCode, typeof(string)));
                    param.Add(cdal.GetParameter("@StartDate", model.HeaderData.StartDate, typeof(DateTime)));
                    param.Add(cdal.GetParameter("@ProdName", model.HeaderData.ProdName, typeof(string)));
                    param.Add(cdal.GetParameter("@DueDate", model.HeaderData.DueDate, typeof(DateTime)));
                    param.Add(cdal.GetParameter("@PlannedQty", model.HeaderData.PlannedQtyH, typeof(decimal)));
                    param.Add(cdal.GetParameter("@Warehouse", model.HeaderData.Warehouse, typeof(string)));
                    param.Add(cdal.GetParameter("@Priority", model.HeaderData.Priority, typeof(Int16)));
                    param.Add(cdal.GetParameter("@LinkToObj", model.HeaderData.LinkToObj, typeof(string)));
                    param.Add(cdal.GetParameter("@OriginNum", model.HeaderData.OriginNum, typeof(int)));
                    param.Add(cdal.GetParameter("@CardCode", model.HeaderData.CardCode, typeof(string)));
                    param.Add(cdal.GetParameter("@Project", model.HeaderData.Project, typeof(string)));
                    param.Add(cdal.GetParameter("@isApproved", isApproved, typeof(int)));
                    #endregion

                    #region Footer Data

                    param.Add(cdal.GetParameter("@Comments", model.HeaderData.Comments, typeof(string)));
                    param.Add(cdal.GetParameter("@PickRmrk", model.HeaderData.PickRmrk, typeof(string)));
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
                        int ChildNum = 0;
                        foreach (var item in model.ListItems)
                        {
                            param.Clear();


                            string Tabitem = "Id,DocEntry,LineNum,VisOrder,ItemCode,ItemName,BaseQty,PlannedQty,wareHouse,IssueType,IssuedQty";
                            string TabitemP = "@Id,@DocEntry,@LineNum,@VisOrder,@ItemCode,@ItemName,@BaseQty,@PlannedQty,@wareHouse,@IssueType,0";
                            string ITT1_Query = @"insert into WOR1 (" + Tabitem + ") " +
                                                 "values(" + TabitemP + ")";

                            #region ListItems data
                            param.Add(cdal.GetParameter("@id", Id, typeof(int)));
                            param.Add(cdal.GetParameter("@DocEntry", Id, typeof(int)));
                            param.Add(cdal.GetParameter("@LineNum", ChildNum, typeof(int)));
                            param.Add(cdal.GetParameter("@VisOrder", ChildNum, typeof(int)));
                            param.Add(cdal.GetParameter("@ItemCode", item.ItemCode, typeof(string)));
                            param.Add(cdal.GetParameter("@ItemName", item.ItemName, typeof(string)));
                            param.Add(cdal.GetParameter("@BaseQty", item.BaseQty, typeof(decimal)));
                            param.Add(cdal.GetParameter("@PlannedQty", item.PlannedQty, typeof(decimal)));
                            param.Add(cdal.GetParameter("@wareHouse", item.Warehouse, typeof(string)));
                            param.Add(cdal.GetParameter("@IssueType", item.IssueMthd, typeof(char)));

                            #endregion

                            res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, ITT1_Query, param.ToArray()).ToInt();
                            if (res1 <= 0)
                            {
                                tran.Rollback();
                                response.isSuccess = false;
                                response.Message = "An Error Occured";
                                return response;

                            }
                            ChildNum += 1;
                        }
                    }


                }
                if (res1 > 0)
                {
                    tran.Commit();
                    response.isSuccess = true;
                    response.Message = "Production Order Added Successfully !";

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


        public ResponseModels EditProductionOrder(dynamic model)
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

                    int ObjectCode = 202;
                    int isApproved = ObjectCode.GetApprovalStatus(tran);
                    #region Insert in Approval Table

                    if (isApproved == 0)
                    {
                        ApprovalModel approvalModel = new()
                        {
                            Id = CommonDal.getPrimaryKey(tran, "tbl_DocumentsApprovals"),
                            ObjectCode = ObjectCode,
                            DocEntry = Convert.ToInt32(SqlHelper.ExecuteScalar(tran, CommandType.Text, @"select Id from OWOR where guid='" + model.OldId + "'")),
                            DocNum = SqlHelper.ExecuteScalar(tran, CommandType.Text, @"select DocNum from OWOR where guid='" + model.OldId + "'").ToString(),
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

                    string TabHeader = @"Type=@Type,Status=@Status,PostDate=@PostDate,StartDate=@StartDate,ProdName=@ProdName,DueDate=@DueDate,
                                         PlannedQty=@PlannedQty,Warehouse=@Warehouse,Priority=@Priority,LinkToObj=@LinkToObj,OriginNum=@OriginNum,
                                         CardCode=@CardCode,Project=@Project,Comments=@Comments,PickRmrk=@PickRmrk,is_Edited=1,isApproved =@isApproved,apprSeen =0 ";



                    string HeadQuery = @"Update OWOR set " + TabHeader + " where guid = '" + model.OldId + "'";

                    #region SqlParameters

                    #region Header data
                    param.Add(cdal.GetParameter("@Type", model.HeaderData.Type, typeof(char)));
                    param.Add(cdal.GetParameter("@Series", model.HeaderData.Series, typeof(int)));
                    param.Add(cdal.GetParameter("@MySeries", model.HeaderData.MySeries, typeof(int)));
                    param.Add(cdal.GetParameter("@DocNum", model.HeaderData.DocNum, typeof(string)));
                    param.Add(cdal.GetParameter("@Status", model.HeaderData.Status, typeof(char)));
                    param.Add(cdal.GetParameter("@PostDate", model.HeaderData.PostDate, typeof(DateTime)));
                    param.Add(cdal.GetParameter("@ItemCode", model.HeaderData.ItemCode, typeof(string)));
                    param.Add(cdal.GetParameter("@StartDate", model.HeaderData.StartDate, typeof(DateTime)));
                    param.Add(cdal.GetParameter("@ProdName", model.HeaderData.ProdName, typeof(string)));
                    param.Add(cdal.GetParameter("@DueDate", model.HeaderData.DueDate, typeof(DateTime)));
                    param.Add(cdal.GetParameter("@PlannedQty", model.HeaderData.PlannedQtyH, typeof(decimal)));
                    param.Add(cdal.GetParameter("@Warehouse", model.HeaderData.Warehouse, typeof(string)));
                    param.Add(cdal.GetParameter("@Priority", model.HeaderData.Priority, typeof(Int16)));
                    param.Add(cdal.GetParameter("@LinkToObj", model.HeaderData.LinkToObj, typeof(string)));
                    param.Add(cdal.GetParameter("@OriginNum", model.HeaderData.OriginNum, typeof(int)));
                    param.Add(cdal.GetParameter("@CardCode", model.HeaderData.CardCode, typeof(string)));
                    param.Add(cdal.GetParameter("@Project", model.HeaderData.Project, typeof(string)));
                    param.Add(cdal.GetParameter("@isApproved", isApproved, typeof(int)));
                    #endregion

                    #region Footer Data

                    param.Add(cdal.GetParameter("@Comments", model.HeaderData.Comments, typeof(string)));
                    param.Add(cdal.GetParameter("@PickRmrk", model.HeaderData.PickRmrk, typeof(string)));
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
                        int ChildNum = 0;
                        foreach (var item in model.ListItems)
                        {
                            param.Clear();
                            string ITT1_Query = "";
                            if (item.LineNum != null && item.LineNum != "")
                            {

                                string Tabitem = @"VisOrder=@VisOrder,ItemCode=@ItemCode,ItemName=@ItemName,BaseQty=@BaseQty,PlannedQty=@PlannedQty,wareHouse=@wareHouse,IssueType=@IssueType";

                                ITT1_Query = @"update WOR1 set " + Tabitem + " where id=" + Id + " and LineNum=" + item.LineNum;
                            }
                            else
                            {
                                string Tabitem = "Id,DocEntry,LineNum,VisOrder,ItemCode,ItemName,BaseQty,PlannedQty,wareHouse,IssueType,IssuedQty";
                                string TabitemP = "@Id,@DocEntry,@LineNum,@VisOrder,@ItemCode,@ItemName,@BaseQty,@PlannedQty,@wareHouse,@IssueType,0";
                                ITT1_Query = @"insert into WOR1 (" + Tabitem + ") " +
                                                     "values(" + TabitemP + ")";
                                ChildNum = CommonDal.getLineNumber(tran, "WOR1", Id.ToString());
                            }


                            #region ListItems data
                            param.Add(cdal.GetParameter("@id", Id, typeof(int)));
                            param.Add(cdal.GetParameter("@DocEntry", Id, typeof(int)));
                            param.Add(cdal.GetParameter("@LineNum", ChildNum, typeof(int)));
                            param.Add(cdal.GetParameter("@VisOrder", ChildNum, typeof(int)));
                            param.Add(cdal.GetParameter("@ItemCode", item.ItemCode, typeof(string)));
                            param.Add(cdal.GetParameter("@ItemName", item.ItemName, typeof(string)));
                            param.Add(cdal.GetParameter("@BaseQty", item.BaseQty, typeof(decimal)));
                            param.Add(cdal.GetParameter("@PlannedQty", item.PlannedQty, typeof(decimal)));
                            param.Add(cdal.GetParameter("@wareHouse", item.Warehouse, typeof(string)));
                            param.Add(cdal.GetParameter("@IssueType", item.IssueMthd, typeof(char)));

                            #endregion

                            res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, ITT1_Query, param.ToArray()).ToInt();
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
                    response.Message = "Production Order Updated Successfully !";

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
