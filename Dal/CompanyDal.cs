using iSOL_Enterprise.Common;
using iSOL_Enterprise.Models;
using Microsoft.CodeAnalysis.Differencing;
using NuGet.Packaging.Signing;
using SqlHelperExtensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace iSOL_Enterprise.Dal
{
    public class CompanyDal
    {
        public bool Add(CompanyModels input)
        {
            string query = @"insert into Company(Id,Guid,CompanyCode,CompanyName,ContactPerson,ContactNo,InterestRate,IsActive,RowStatus,CreatedBy,CreatedOn) 
values(@Id,@Guid,@CompanyCode,@CompanyName,@ContactPerson,@ContactNo,@InterestRate,@IsActive,@RowStatus,@CreatedBy,@CreatedOn)";

            SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
            conn.Open();
            SqlTransaction tran = conn.BeginTransaction();
            int res = 0;
            try
            {
                input.Id = CommonDal.getPrimaryKey(tran, "Company");

                List<SqlParameter> param = new List<SqlParameter>
                {
                    new SqlParameter("@id",input.Id),
                    new SqlParameter("@Guid",input.Guid = CommonDal.generatedGuid()),
                    new SqlParameter("@CompanyCode","CO-"+input.Id),
                    new SqlParameter("@CompanyName",input.CompanyName),
                    new SqlParameter("@ContactPerson",input.ContactPerson),
                    new SqlParameter("@ContactNo",input.ContactNo),
                    new SqlParameter("@IsActive",input.IsActive),
                    new SqlParameter("@InterestRate",input.InterestRate),
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
        public bool Edit(CompanyModels input)
        {
            string query = @"update Company set CompanyName=@CompanyName,ContactNo=@ContactNo,ContactPerson=@ContactPerson,InterestRate=@InterestRate,IsActive=@IsActive,ModifiedBy=@ModifiedBy,ModifiedOn=@ModifiedOn where Guid=@Guid";

            SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
            conn.Open();
            SqlTransaction tran = conn.BeginTransaction();
            int res = 0;
            try
            {

                List<SqlParameter> param = new List<SqlParameter>
                {
                    new SqlParameter("@Guid",input.Guid),
                    new SqlParameter("@CompanyName",input.CompanyName),
                    new SqlParameter("@ContactNo",input.ContactNo),
                    new SqlParameter("@ContactPerson",input.ContactPerson),
                    new SqlParameter("@InterestRate",input.InterestRate),
                    new SqlParameter("@IsActive",input.IsActive),
                    new SqlParameter("@RowStatus",input.RowStatus=true),
                    new SqlParameter("@ModifiedBy",input.ModifiedBy),
                    new SqlParameter("@ModifiedOn",input.ModifiedOn)
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

        public CompanyModels Get(string Guid)
        {

            CompanyModels models = new CompanyModels();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, "select * from Company where Guid=@Guid", new SqlParameter("@Guid", Guid)))
            {
                while (rdr.Read())
                {


                    models.Id = rdr["Id"].ToInt();
                    models.CompanyName = rdr["CompanyName"].ToString();
                    models.CompanyCode = rdr["CompanyCode"].ToString();
                    models.ContactNo = rdr["ContactNo"].ToString();
                    models.ContactPerson = rdr["ContactPerson"].ToString();
                    models.InterestRate = rdr["InterestRate"].ToDecimal();
                    models.CustomerCollection = rdr["CustomerCollection"].ToDecimal();
                    models.IsActive = rdr["IsActive"].ToBool();
                    models.Guid = rdr["Guid"].ToString();

                }
            }


            return models;
        }

        public List<CompanyModels> GetAllCompany()
        {
            List<CompanyModels> lstModel = new List<CompanyModels>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, "select CompanyCode, CompanyName from Company where IsActive=1 and RowStatus=1"))
            {
                while (rdr.Read())
                {
                    CompanyModels model = new CompanyModels();
                    model.CompanyCode = rdr["CompanyCode"].ToString();
                    model.CompanyName = rdr["CompanyName"].ToString();
                    lstModel.Add(model);
                }
            }
            return lstModel;
        }
        


    }
}
