using iSOL_Enterprise.Common;
using iSOL_Enterprise.Models;
using SqlHelperExtensions;
using System.Data;

namespace iSOL_Enterprise.Dal.Administrator
{
    public class AllApprovalsDal
    {

        public List<ApprovalModel> GetAllApprovals()
        {
            string GetQuery = @"select da.Id,da.DocId,da.ObjectCode,da.RequestedBy,da.Status,da.Date,Pages.PageName,da.seen as Seen from tbl_DocumentsApprovals da
                                inner join Pages on da.ObjectCode = Pages.ObjectCode";


            List<ApprovalModel> list = new List<ApprovalModel>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {

                    ApprovalModel models = new ()
                    {
                        Id = rdr["Id"].ToInt(),
                        ObjectCode = rdr["ObjectCode"].ToInt(),
                        DocId = rdr["Id"].ToInt(),
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
    }
}
