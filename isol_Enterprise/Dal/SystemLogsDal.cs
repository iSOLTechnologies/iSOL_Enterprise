using iSOL_Enterprise.Common;
using iSOL_Enterprise.Models;
using SqlHelperExtensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace iSOL_Enterprise.Dal
{
    public class SystemLogsDal
    {
        public bool Add(SystemLogsModels input)
        {
            string query = @"insert into UserLogs(Id,Guid,UserId,ControllerName,MsgLog,IpAddress,CountryCode,CountryName,Location,
RowStatus,CreatedBy,CreatedOn) 
values(@Id,@Guid,@UserId,@ControllerName,@MsgLog,@IpAddress,@CountryCode,@CountryName,@Location,
@RowStatus,@CreatedBy,@CreatedOn)";

            SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
            conn.Open();
            SqlTransaction tran = conn.BeginTransaction();
            int res = 0;
            try
            {
                input.Id = CommonDal.getPrimaryKey(tran, "UserLogs");

                List<SqlParameter> param = new List<SqlParameter>
                {
                    new SqlParameter("@id",input.Id),
                    new SqlParameter("@Guid",input.Guid = CommonDal.generatedGuid()),
                    new SqlParameter("@UserId",input.UserId),
                    new SqlParameter("@ControllerName",input.ControllerName),
                    new SqlParameter("@MsgLog",input.MsgLog),
                    new SqlParameter("@IpAddress",input.IpAddress),
                    new SqlParameter("@CountryCode",input.CountryCode),
                    new SqlParameter("@CountryName",input.CountryName),
                    new SqlParameter("@Location",input.Location),

                    new SqlParameter("@RowStatus",input.RowStatus=true),
                    new SqlParameter("@CreatedBy",input.CreatedBy),
                    new SqlParameter("@CreatedOn",input.CreatedOn)
                };

                res = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, query, param.ToArray()).ToInt();
                if (res > 0)
                {
                    tran.Commit();
                }

            }
            catch (Exception ex)
            {
                tran.Rollback();
                return false;
            }
            // return result;
            return res > 0 ? true : false;


        }
    }
}
