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
    public class UsersDal
    {
        public List<_usersModels> GetAllUsers()
        {
            List<_usersModels> lstModel = new List<_usersModels>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, "select Id,ISNULL(FirstName,'') + ' ' + ISNULL(LastName,'') as Name from users where RowStatus=1 and IsActive=1"))
            {
                while (rdr.Read())
                {
                    _usersModels model = new _usersModels();
                    model.Name = rdr["Name"].ToString();
                    model.Id = rdr["Id"].ToInt();
                    lstModel.Add(model);
                }
            }
            return lstModel;
        }

         public List<_usersModels> GetAllUsersByRole(string RoleCode)
        {
            List<_usersModels> lstModel = new List<_usersModels>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, "select Id,ISNULL(FirstName,'') + ' ' + ISNULL(LastName,'') as Name from users where RoleCode=@RoleCode and RowStatus=1 and IsActive=1",new SqlParameter("@RoleCode", RoleCode)))
            {
                while (rdr.Read())
                {
                    _usersModels model = new _usersModels();
                    model.Name = rdr["Name"].ToString();
                    model.Id = rdr["Id"].ToInt();
                    lstModel.Add(model);
                }
            }
            return lstModel;
        }



        public List<RoleModels> ListRoles()
        {
            List<RoleModels> lstModel = new List<RoleModels>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, "select * from Roles where RowStatus=1 and IsActive=1"))
            {
                while (rdr.Read())
                {
                    RoleModels model = new RoleModels();
                    model.RoleName = rdr["RoleName"].ToString();
                    model.RoleCode = rdr["RoleCode"].ToString();
                    lstModel.Add(model);
                }
            }
            return lstModel;
        }
        public List<RegionModels> ListRegions()
        {
            List<RegionModels> lstModel = new List<RegionModels>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, "select * from Regions where RowStatus=1 and IsActive=1"))
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

        public List<_usersModels> ListSuperiors()
        {
            List<_usersModels> lstModel = new List<_usersModels>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, "select id,ISNULL(FirstName,'') + ' ' + ISNULL(LastName,'') as Name from Users where RowStatus=1 and IsActive=1"))
            {
                while (rdr.Read())
                {
                    _usersModels model = new _usersModels();
                    model.Name = rdr["Name"].ToString();
                    model.Id = rdr["Id"].ToInt();
                    lstModel.Add(model);
                }
            }
            return lstModel;
        }

        public _usersModels GetData(int? UserId)
        {

            _usersModels models = new _usersModels();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, "select * from users where id=@Id", new SqlParameter("@Id", UserId)))
            {
                while (rdr.Read())
                {


                    models.Id = rdr["Id"].ToInt();
                    models.FirstName = rdr["FirstName"].ToString();
                    models.LastName = rdr["LastName"].ToString();

                }
            }


            return models;
        }

        public _usersModels GetDataByUser(int UserId)
        {

            _usersModels models = new _usersModels();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, "select * from users where id=@Id", new SqlParameter("@Id", UserId)))
            {
                while (rdr.Read())
                {


                    models.Id = rdr["Id"].ToInt();
                    models.FirstName = rdr["FirstName"].ToString();
                    models.LastName = rdr["LastName"].ToString();
                    models.ContactNumber = rdr["ContactNumber"].ToString();
                    models.Email = rdr["Email"].ToString();
                    models.Password = rdr["Password"].ToString();
                    models.RoleCode = rdr["RoleCode"].ToString();
                    models.SuperiorId = rdr["SuperiorId"].ToInt();
                    models.RegionCode = rdr["RegionCode"].ToString();
                    models.UserPic = rdr["UserPic"].ToString();
                    models.IsActive = rdr["IsActive"].ToBool();
                }
            }


            return models;
        }

        public bool Add(_usersModels input)
        {
            string query = @"insert into Users(Id,Guid,FirstName,LastName,Password,ContactNumber,Email,RoleCode,UserPic,SuperiorId,RegionCode,IsActive,
RowStatus,CreatedBy,CreatedOn) 
values(@Id,@Guid,@FirstName,@LastName,@Password,@ContactNumber,@Email,@RoleCode,@UserPic,@SuperiorId,@RegionCode,@IsActive,
@RowStatus,@CreatedBy,@CreatedOn)";

            SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
            conn.Open();
            SqlTransaction tran = conn.BeginTransaction();
            int res = 0;
            try
            {
                input.Id = CommonDal.getPrimaryKey(tran, "Users");


                string Path = "";
                #region GenerateFiles

                if (!string.IsNullOrEmpty(input.UserPic))
                {
                    int attachId = CommonDal.getPrimaryKey(tran, "Attachments");
                    string name = input.Username + DateTime.Now.ToString("ddmmmyyyyhhmmss");
                    name += "." + CommonDal.getExtensionFromFile(input.UserPic);
                    Path = CommonDal.SaveFileFromBase64(input.UserPic, input.WebRootPath, name);
                  
                    Path += "." + CommonDal.getExtensionFromFile(input.UserPic);
                    string AttQry = @"insert into [Attachments] (Id,Reftype,ReftypeNo,Name,Path ,IsActive,RowStatus,CreatedOn,CreatedBy) 
                                            Values(@Id,@Reftype,@ReftypeNo,@Name,@Path ,1,1,@CreatedOn,@CreatedBy)";

                    List<SqlParameter> param2 = new List<SqlParameter>
                        {
                            new SqlParameter("@Id",attachId),
                            new SqlParameter("@Reftype","Users"),
                            new SqlParameter("@ReftypeNo",input.Id),
                            new SqlParameter("@Name",name),
                            new SqlParameter("@Path",Path),
                            new SqlParameter("@CreatedBy",input.CreatedBy),
                            new SqlParameter("@CreatedOn",input.CreatedOn)
                        };
                    SqlHelper.ExecuteNonQuery(tran, CommandType.Text, AttQry, param2.ToArray());
                }

                #endregion

                List<SqlParameter> param = new List<SqlParameter>
                {
                    new SqlParameter("@id",input.Id),
                    new SqlParameter("@Guid",input.Guid = CommonDal.generatedGuid()),
                    new SqlParameter("@FirstName",input.FirstName),
                    new SqlParameter("@LastName",input.LastName),
                    new SqlParameter("@ContactNumber",input.ContactNumber),
                    new SqlParameter("@Email",input.Email),
                    new SqlParameter("@Password",input.Password),
                    new SqlParameter("@DepartmentCode",input.DepartmentCode),
                    new SqlParameter("@RoleCode",input.RoleCode),
                    new SqlParameter("@EmployeeOf",input.EmployeeOf),
                    new SqlParameter("@UserPic",Path),
                    new SqlParameter("@SuperiorId",input.SuperiorId),
                    new SqlParameter("@RegionCode",input.RegionCode),
                    new SqlParameter("@IsActive",input.IsActive),
                    new SqlParameter("@RowStatus",input.RowStatus=true),
                    new SqlParameter("@CreatedBy",input.CreatedBy),
                    new SqlParameter("@CreatedOn",input.CreatedOn)
                };

                res = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, query, param.ToArray()).ToInt();
                //            string email = SqlHelper.ExecuteScalar(tran, CommandType.Text, "select email from Users where id=@UserId", param.ToArray()).ToString();

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
        public bool Edit(_usersModels input)
        {
            string query = @"update Users set FirstName=@FirstName,LastName=@LastName,Password=@Password,ContactNumber=@ContactNumber,Email=@Email,RoleCode=@RoleCode,SuperiorId=@SuperiorId,RegionCode=@RegionCode,IsActive=@IsActive,
ModifiedBy=@ModifiedBy,ModifiedOn=@ModifiedOn where Id=@Id";

            SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
            conn.Open();
            SqlTransaction tran = conn.BeginTransaction();
            int res = 0;
            try
            {

                List<SqlParameter> param = new List<SqlParameter>
                {
                    new SqlParameter("@id",input.Id),
                    new SqlParameter("@FirstName",input.FirstName),
                    new SqlParameter("@LastName",input.LastName),
                    new SqlParameter("@ContactNumber",input.ContactNumber),
                    new SqlParameter("@Email",input.Email),
                    new SqlParameter("@Password",input.Password),
                    new SqlParameter("@RoleCode",input.RoleCode),
                    new SqlParameter("@IsActive",input.IsActive),
                    new SqlParameter("@SuperiorId",input.SuperiorId),
                    new SqlParameter("@RegionCode",input.RegionCode),
                    new SqlParameter("@ModifiedOn",input.ModifiedOn),
                    new SqlParameter("@ModifiedBy",input.ModifiedBy)
                };

                res = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, query, param.ToArray()).ToInt();
                //            string email = SqlHelper.ExecuteScalar(tran, CommandType.Text, "select email from Users where id=@UserId", param.ToArray()).ToString();

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
