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
    public class DepartmentsDal
    {
        public List<Setup_DepartmentsModels> GetAllSetup_Departments()
        {
            List<Setup_DepartmentsModels> lstModel = new List<Setup_DepartmentsModels>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, "select DepartmentCode, DepartmentName from Setup_Departments where RowStatus=1 and Status=1"))
            {
                while (rdr.Read())
                {
                    Setup_DepartmentsModels model = new Setup_DepartmentsModels();
                    model.DepartmentCode = rdr["DepartmentCode"].ToString();
                    model.DepartmentName = rdr["DepartmentName"].ToString();
                    lstModel.Add(model);
                }
            }
            return lstModel;
        }

        public string GetDepartmentCodeByUserId(string UserId)
        {
            string DepartmentCode = SqlHelper.ExecuteScalar(SqlHelper.defaultDB, CommandType.Text, "select DepartmentCode from Users where Id=@UserId",new SqlParameter("@UserId", UserId)).ToString();
           
            return DepartmentCode;
        }
    }
}
