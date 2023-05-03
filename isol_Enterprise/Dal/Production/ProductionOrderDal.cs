using iSOL_Enterprise.Models;
using Newtonsoft.Json;
using SqlHelperExtensions;
using System.Data.SqlClient;
using System.Data;
using iSOL_Enterprise.Common;
using System.Reflection;
using SAPbobsCOM;

namespace iSOL_Enterprise.Dal.Production
{
    public class ProductionOrderDal
    {

        public List<SalesQuotation_MasterModels> GetData()
        {
            string GetQuery = "select Id,Guid,DocNum,ItemCode,PostDate,PlannedQty,Warehouse,isPosted,is_Edited from OWOR order by id DESC";


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
                    list.Add(models);
                }
            }
            return list;
        }
        public int GetId(string guid)
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.defaultDB, CommandType.Text, "select Id from OWOR where GUID ='" + guid.ToString() + "'"));

        }
        public dynamic GetOldHeaderData(int id)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
                string headerQuery = @"select Id,Guid,DocEntry,Type,Series,MySeries,DocNum,Status,PostDate, ItemCode,StartDate,ProdName,
                                         DueDate,PlannedQty,Warehouse,LinkToObj,OriginNum,CardCode,Project From OWOR where Id =" + id;
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
                   response = EditBillOfMaterial(model);
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
            SqlTransaction tran = conn.BeginTransaction();
            int res1 = 0;
            try
            {

                if (model.HeaderData != null)
                {
                    List<SqlParameter> param = new List<SqlParameter>();
                    int Id = CommonDal.getPrimaryKey(tran, "OWOR");

                    param.Add(cdal.GetParameter("@Id", Id, typeof(int)));
                    param.Add(cdal.GetParameter("@Guid", CommonDal.generatedGuid(), typeof(string)));
                    param.Add(cdal.GetParameter("@DocEntry", Id, typeof(int)));

                    string TabHeader = @"Id,Guid,DocEntry,Type,Series,MySeries,DocNum,Status,PostDate, ItemCode,StartDate,ProdName,
                                         DueDate,PlannedQty,Warehouse,LinkToObj,OriginNum,CardCode,Project";
                    string TabHeaderP = @"@Id,@Guid,@DocEntry,@Type,@Series,@MySeries,@DocNum,@Status,@PostDate,@ItemCode,@StartDate,@ProdName,
                                         @DueDate,@PlannedQty,@Warehouse,@LinkToObj,@OriginNum,@CardCode,@Project";
                    
                    string HeadQuery = @"insert into OWOR (" + TabHeader + ") " +
                                        "values("+TabHeaderP+")";



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
                    param.Add(cdal.GetParameter("@LinkToObj", model.HeaderData.LinkToObj, typeof(string)));
                    param.Add(cdal.GetParameter("@OriginNum", model.HeaderData.OriginNum, typeof(int)));
                    param.Add(cdal.GetParameter("@CardCode", model.HeaderData.CardCode, typeof(int)));
                    param.Add(cdal.GetParameter("@Project", model.HeaderData.Project, typeof(string)));
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
                            

                            string Tabitem = "Id,DocEntry,LineNum,VisOrder,ItemCode,ItemName,BaseQty,PlannedQty,wareHouse,IssueType";
                            string TabitemP = "@Id,@DocEntry,@LineNum,@VisOrder,@ItemCode,@ItemName,@BaseQty,@PlannedQty,@wareHouse,@IssueType";
                            string ITT1_Query = @"insert into WOR1 ("+Tabitem+") "+
                                                 "values("+TabitemP+")";

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


        public ResponseModels EditBillOfMaterial(dynamic model)
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
                    
                    string TabHeader = "Qauntity= @Quantity,ToWH =@ToWH,PriceList=@PriceList,TreeType=@TreeType,OcrCode=@OcrCode,Project=@Project,PlAvgSize=@PlAvgSize";
                    
                    string HeadQuery = @"Update OITT set " + TabHeader + " where guid = '" + model.OldId + "'";

                    #region SqlParameters

                    #region Header data                    
                    param.Add(cdal.GetParameter("@Quantity", model.HeaderData.Qauntity, typeof(decimal)));
                    param.Add(cdal.GetParameter("@ToWH", model.HeaderData.ToWH, typeof(string)));                    
                    param.Add(cdal.GetParameter("@PriceList", model.HeaderData.PriceList, typeof(string)));
                    param.Add(cdal.GetParameter("@TreeType", model.HeaderData.TreeType, typeof(char)));
                    param.Add(cdal.GetParameter("@OcrCode", model.HeaderData.OcrCode, typeof(string)));
                    param.Add(cdal.GetParameter("@Project", model.HeaderData.Project, typeof(string)));
                    param.Add(cdal.GetParameter("@PlAvgSize", model.HeaderData.PlAvgSize, typeof(decimal)));                    
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

                               string  Tabitem = "Father=@Father,VisOrder=@VisOrder,Code=@Code,ItemName=@ItemName,Quantity=@Quantity,Uom=@Uom," +
                                                 "Warehouse=@Warehouse,IssueMthd=@IssueMthd,PriceList=@PriceList,Price=@Price,LineTotal=@LineTotal,Comment=@Comment";
                               
                               ITT1_Query = @"update ITT1 set " + Tabitem + " where id=" + Id + " and ChildNum=" + item.LineNum;
                            }
                            else
                            {
                                string Tabitem =  "Id,Father,ChildNum,VisOrder,Type,Code,ItemName,Quantity,Uom,Warehouse,IssueMthd,PriceList,Price,LineTotal,Comment";
                                string TabitemP = "@Id,@Father,@ChildNum,@VisOrder,@Type,@Code,@ItemName,@Quantity,@Uom,@Warehouse,@IssueMthd,@PriceList,@Price,@LineTotal,@Comment";
                                ITT1_Query = @"insert into ITT1 (" + Tabitem + ") " +
                                                     "values(" + TabitemP + ")";
                                ChildNum = SqlHelper.ExecuteScalar(tran,CommandType.Text, @"select MAX (ChildNum) from ITT1 where id =" +Id).ToInt() + 1;
                            }


                            #region ListItems data
                            param.Add(cdal.GetParameter("@id", Id, typeof(int)));
                            param.Add(cdal.GetParameter("@Father", model.HeaderData.ItemmCode, typeof(string)));
                            param.Add(cdal.GetParameter("@ChildNum", ChildNum, typeof(int)));
                            param.Add(cdal.GetParameter("@VisOrder", ChildNum, typeof(int)));
                            param.Add(cdal.GetParameter("@Type", item.MaterialType, typeof(int)));
                            param.Add(cdal.GetParameter("@Code", item.ItemCode, typeof(string)));
                            param.Add(cdal.GetParameter("@ItemName", item.ItemName, typeof(string)));
                            param.Add(cdal.GetParameter("@Quantity", item.QTY, typeof(decimal)));
                            param.Add(cdal.GetParameter("@Uom", item.BuyUnitMsr, typeof(string)));
                            param.Add(cdal.GetParameter("@Warehouse", item.Warehouse, typeof(string)));
                            param.Add(cdal.GetParameter("@IssueMthd", item.IssueMthd, typeof(char)));
                            param.Add(cdal.GetParameter("@PriceList", item.PriceList1, typeof(int)));
                            param.Add(cdal.GetParameter("@Price", item.UPrc, typeof(decimal)));
                            param.Add(cdal.GetParameter("@LineTotal", item.TtlPrc, typeof(decimal)));
                            param.Add(cdal.GetParameter("@Comment", item.Comment, typeof(string)));
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
                    response.Message = "Bill Of Material Updated Successfully !";

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
