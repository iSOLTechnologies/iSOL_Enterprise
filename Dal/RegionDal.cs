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
    public class RegionDal
    {
        public List<RegionModels> GetAllRegions()
        {
            List<RegionModels> lstModel = new List<RegionModels>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, "select Code, Name from Regions where IsActive=1 and RowStatus=1"))
            {
                while (rdr.Read())
                {
                    RegionModels model = new RegionModels();
                    model.Name = rdr["Name"].ToString();
                    model.Code = rdr["Code"].ToString();
                    lstModel.Add(model);
                }
            }
            return lstModel;
        }
        public bool Add(RegionModels input)
        {
            string query = @"insert into Regions(Id,Guid,Code,Name,IsActive,RowStatus,CreatedBy,CreatedOn) 
values(@Id,@Guid,@Code,@Name,@IsActive,@RowStatus,@CreatedBy,@CreatedOn)";

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
                    new SqlParameter("@Code","RG-"+input.Id),
                    new SqlParameter("@Name",input.Name),
                    new SqlParameter("@IsActive",input.IsActive),    
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
        public bool Edit(RegionModels input)
        {
            string query = @"update Regions set Name=@Name,IsActive=@IsActive,ModifiedBy=@ModifiedBy,ModifiedOn=@ModifiedOn where Guid=@Guid";

            SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
            conn.Open();
            SqlTransaction tran = conn.BeginTransaction();
            int res = 0;
            try
            {

                List<SqlParameter> param = new List<SqlParameter>
                {
                    new SqlParameter("@Guid",input.Guid),
                    new SqlParameter("@Name",input.Name),
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

        public RegionModels Get(string Guid)
        {

            RegionModels models = new RegionModels();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, "select * from Regions where Guid=@Guid", new SqlParameter("@Guid", Guid)))
            {
                while (rdr.Read())
                {


                    models.Id = rdr["Id"].ToInt();
                    models.Code = rdr["Code"].ToString();
                    models.Name = rdr["Name"].ToString();
                    models.IsActive = rdr["IsActive"].ToBool();
                    models.Guid = rdr["Guid"].ToString();

                }
            }


            return models;
        }
    }
}
