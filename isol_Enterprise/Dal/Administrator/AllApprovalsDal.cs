using iSOL_Enterprise.Common;
using iSOL_Enterprise.Models;
using iSOL_Enterprise.Models.sale;
using SqlHelperExtensions;
using System.Data;
using System.Data.SqlClient;

namespace iSOL_Enterprise.Dal.Administrator
{
    public class AllApprovalsDal
    {

        public List<ApprovalModel> GetAllApprovals()
        {
            string GetQuery = @"select Top(1000) da.Id,da.DocEntry,da.ObjectCode,da.RequestedBy,da.DocNum,da.Status,da.Guid,da.Date,Pages.PageName,da.seen as Seen from tbl_DocumentsApprovals da
                                inner join Pages on da.ObjectCode = Pages.ObjectCode order by da.Id desc";


            List<ApprovalModel> list = new List<ApprovalModel>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {

                    ApprovalModel models = new ()
                    {
                        Id = rdr["Id"].ToInt(),
                        ObjectCode = rdr["ObjectCode"].ToInt(),
                        DocEntry = rdr["DocEntry"].ToInt(),
                        DocNum = rdr["DocNum"].ToString(),
                        Guid = rdr["Guid"].ToString(),
                        RequestedBy = rdr["RequestedBy"].ToString(),
                        PageName = rdr["PageName"].ToString(),
                        Date = rdr["Date"].ToDateTime(),
                        Status = rdr["Status"].ToBool(),
                        Seen = rdr["Seen"].ToBool(),

                    };
                    
                    list.Add(models);
                }
            }
            return list;
        }
        public ResponseModels RejectAcceptDoc(int DocEntry, int ObjectType, int Status , int id)
        {
            ResponseModels response = new ResponseModels();
            SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
            conn.Open();
            SqlTransaction tran = conn.BeginTransaction();
            CommonDal dal = new CommonDal();
            try
            {

                int res = 0;
                #region Update Table Document Approvals
                string Query = @"update tbl_DocumentsApprovals set seen = 1 , Status = "+Status + " where id="+id;
                res = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, Query).ToInt();
                if (res <= 0)
                {
                    tran.Rollback();
                    response.isSuccess = false;
                    response.isError = true;
                    response.Message = "An Error Occured !";
                    
                    return response;
                }
                string DocTabe = dal.GetMasterTable(ObjectType);
                string UpdateDocStQuery = @"update " + DocTabe + " set isApproved =" + Status + ", apprSeen = 1 where id = " + DocEntry;
                res = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, UpdateDocStQuery).ToInt();
                if (res <= 0)
                {
                    tran.Rollback();
                    response.isSuccess = false;
                    response.isError = true;
                    response.Message = "An Error Occured !";
                    
                   
                }
                #endregion

                if (Status == 0)    //Check if document was rejected then reverse transaction 
                {
                   bool resp =  OnRejectReverseTransactions(tran,DocEntry, ObjectType);
                    if (!resp)
                        res = 0;
                }
                if (res != 0)
                {
                    response.isSuccess = true;
                    response.Message = Status == 1 ? "Document Accepted Successfully !" : "Document Rejected Successfully !";
                    tran.Commit();
                    
                }
                else
                {
                    response.isSuccess = false;
                    response.isError = true;
                    response.Message = "An Error Occured !";
                }
                    return response;
                
            }
            catch (Exception)
            {
                tran.Rollback();
                throw;
            }
        }

        public bool OnRejectReverseTransactions(SqlTransaction tran, int DocEntry, int ObjectType)
        {
            //ResponseModels response = new ResponseModels();
            bool response = true;
            CommonDal dal = new();
            try
            {
                #region If its a transaction Document
                if (ObjectType == 15 || ObjectType == 16 || ObjectType == 14 ||
                    ObjectType == 20 || ObjectType == 21 || ObjectType == 19 )
                {

                    List<SalesQuotation_DetailsModels> rows = GetDocRows(tran,DocEntry, ObjectType);
                    foreach (var row in rows)
                    {
                        response = dal.ReverseOutTransaction(tran, DocEntry, row.LineNum, ObjectType);                        
                        if (!response)
                            return false;
                        if (ObjectType == 16 || ObjectType == 21)
                            if (row.BaseType != -1 && row.BaseType != null)
                                response = ReverseReturns(tran, row,ObjectType);
                                 if (!response)
                                    return false;

                    }

                }
                #endregion
                
                return response;

            }
            catch (Exception)
            {

                throw;
            }

        }
        public bool ReverseReturns(SqlTransaction tran, SalesQuotation_DetailsModels row,int ObjectType)
        {
            try
            {
                CommonDal dal = new();
                int res = 1;
                string table = dal.GetRowTable(row.BaseType);

                string GetFromBaseQuery = "select BaseEntry,BaseLine,ItemCode from " + table + " where Id =" + row.BaseEntry + "and LineNum =" +row.BaseLine + "and ItemCode = '" + row.ItemCode + "'";

                  tbl_docRow docRowModel = new tbl_docRow();
                    using (var rdr = SqlHelper.ExecuteReader(tran, CommandType.Text, GetFromBaseQuery))
                    {
                        while (rdr.Read())
                        {


                            docRowModel.BaseEntry = Convert.ToInt32(rdr["BaseEntry"]);
                            docRowModel.BaseLine = Convert.ToInt32(rdr["BaseLine"]);
                            docRowModel.ItemCode = rdr["ItemCode"].ToString();
                            string UpdateDLQuery = @"Update " + table + " set Quantity =Quantity - " + row.Quantity + " , OpenQty = OpenQty - " + row.Quantity + " where Id =" + row.BaseEntry + "and LineNum =" + row.BaseLine + "and ItemCode = '" + row.ItemCode + "'";
                            res = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, UpdateDLQuery).ToInt();
                            if (res <= 0)
                            {
                                tran.Rollback();
                                return false;
                            }

                            if (docRowModel.BaseEntry != null && docRowModel.BaseLine !=null)
                            {
                            string tableR = "RDR1";
                            if (ObjectType == 21)
                                tableR = "POR1";

                                string UpdateSOQuery = @"Update "+ tableR + " set OpenQty =OpenQty + " + row.Quantity + " where Id =" + docRowModel.BaseEntry + "and LineNum =" + docRowModel.BaseLine + "and ItemCode = '" + docRowModel.ItemCode + "'";
                                res = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, UpdateSOQuery).ToInt();
                                if (res <= 0)
                                {
                                    tran.Rollback();
                                    return false;
                                }
                            }
                        }
                    }

               return res > 0 ?  true : false;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<SalesQuotation_DetailsModels> GetDocRows(SqlTransaction tran, int DocEntry, int ObjectType)
        {
            CommonDal dal = new();
            string RowTable = dal.GetRowTable(ObjectType);

            string GetQuery = "select Id,LineNum,ItemCode,BaseEntry,BaseLine,BaseType,Quantity from "+ RowTable + " where Id = "+ DocEntry + " order by LineNum";


            List<SalesQuotation_DetailsModels> list = new List<SalesQuotation_DetailsModels>();
            using (var rdr = SqlHelper.ExecuteReader(tran, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {

                    SalesQuotation_DetailsModels models = new ()
                    {
                        Id = rdr["Id"].ToInt(),
                        LineNum = rdr["LineNum"].ToInt(),
                        ItemCode = rdr["ItemCode"].ToString(),
                        BaseEntry = rdr["BaseEntry"].ToInt(),
                        BaseLine = rdr["BaseLine"].ToInt(),
                        BaseType = rdr["BaseType"].ToInt(),
                        Quantity = rdr["Quantity"].ToDouble(),
                    };
                   
                    list.Add(models);
                }
            }
            return list;

        }
        
    }
}
