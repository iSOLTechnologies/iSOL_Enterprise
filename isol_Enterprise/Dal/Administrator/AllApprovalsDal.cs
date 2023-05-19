using iSOL_Enterprise.Common;
using iSOL_Enterprise.Models;
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
                    
                    return response;
                }

                #endregion
                response.isSuccess = true;
                response.Message = Status == 1 ? "Document Accepted Successfully !" : "Document Rejected Successfully !";
                tran.Commit();
                return response;
            }
            catch (Exception)
            {
                tran.Rollback();
                throw;
            }
        }
    }
}
