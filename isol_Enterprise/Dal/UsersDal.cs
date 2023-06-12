using iSOL_Enterprise.Common;
using iSOL_Enterprise.Dal.Home;
using iSOL_Enterprise.Models;
using Newtonsoft.Json;
using SqlHelperExtensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace iSOL_Enterprise.Dal
{
    public class UsersDal
    {
        public List<_usersModels> GetAllUsers()
        {
            List<_usersModels> lstModel = new List<_usersModels>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, "select Id,ISNULL(FirstName,'') + ' ' + ISNULL(LastName,'') as Name,ContactNumber,Email,RoleCode,SuperiorId,RegionCode,IsActive,Guid,is_Edited,isApproved,apprSeen from users where RowStatus=1 and IsActive=1 order by Id desc"))
            {
                while (rdr.Read())
                {
                    _usersModels models = new _usersModels();
                    models.Id = rdr["Id"].ToInt();
                    models.Name = rdr["Name"].ToString();                    
                    models.ContactNumber = rdr["ContactNumber"].ToString();
                    models.Email = rdr["Email"].ToString();                    
                    models.RoleCode = rdr["RoleCode"].ToString();
                    models.SuperiorId = rdr["SuperiorId"].ToInt();
                    models.RegionCode = rdr["RegionCode"].ToString();                    
                    models.IsActive = rdr["IsActive"].ToBool();
                    models.Guid = rdr["Guid"].ToString();
                    models.IsEdited = rdr["is_Edited"].ToString();
                    models.isApproved = rdr["isApproved"].ToBool();
                    models.apprSeen = rdr["apprSeen"].ToBool();
                    lstModel.Add(models);
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

        public _usersModels GetDataByUser(string Guid)
        {

            _usersModels models = new _usersModels();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, "select * from users where Guid=@Guid", new SqlParameter("@Guid", Guid)))
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

        public ResponseModels Add(_usersModels input)
        {
            ResponseModels response = new();
            
            bool EmailCheck = CommonDal.Count("Users", "Email", input.Email);
            if (EmailCheck)
            {
                response.isSuccess = false;
                response.Message = "Email already registered try different";
                return (response);
            }
            Byte[] SecurityStamp = PasswordHelperDal.GenerateSalt();
            string PasswordHash = input.Password.HashPassword(SecurityStamp);
            if (PasswordHash != null)
            {
            
            string query = @"insert into Users(Id,Guid,FirstName,LastName,PasswordHash,SecurityStamp,ContactNumber,Email,RoleCode,UserPic,SuperiorId,RegionCode,IsActive,
                                RowStatus,CreatedBy,CreatedOn) 
                                values(@Id,@Guid,@FirstName,@LastName,@PasswordHash,@SecurityStamp,@ContactNumber,@Email,@RoleCode,@UserPic,@SuperiorId,@RegionCode,@IsActive,
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
                    res = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, AttQry, param2.ToArray());
                        if (res <= 0)
                        {
                            tran.Rollback();
                            response.isSuccess = false;
                            response.Message = "An Error Occured";
                            return response;
                        }
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
                    new SqlParameter("@PasswordHash",PasswordHash),
                    new SqlParameter("@SecurityStamp",SecurityStamp),
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
                if (res <= 0)
                {
                    tran.Rollback();
                    response.isSuccess = false;
                    response.Message = "An Error Occured";
                    return response;
                }

                else
                {
                    tran.Commit();
                    response.isSuccess = true;
                    response.Message = "User Added Successfully !";
                    return response;

                }

            }
            catch (Exception ex)
            {
                tran.Rollback();
                response.isSuccess = false;
                response.Message = "An exception occured";
                    return response;
            }
            // return result;
            
            }
            else
            {
                response.isSuccess = false;
                response.Message = "An Error Occured";
                return (response);
            }
        }
        public ResponseModels Edit(_usersModels input)
        {
            ResponseModels response = new();

            bool EmailCheck = CommonDal.CountOnEdit("Users", "Email", input.Email,input.Guid);
            if (EmailCheck)
            {

                response.isSuccess = false;
                response.Message = "Email already registered try different";
                return (response);
            }

            string query = @"update Users set FirstName=@FirstName,LastName=@LastName,ContactNumber=@ContactNumber,Email=@Email,RoleCode=@RoleCode,IsActive=@IsActive,
                            ModifiedBy=@ModifiedBy,ModifiedOn=@ModifiedOn where Guid=@Guid";

            SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
            conn.Open();
            SqlTransaction tran = conn.BeginTransaction();
            int res = 0;
            try
            {

                List<SqlParameter> param = new List<SqlParameter>
                {
                    new SqlParameter("@Guid",input.Guid),
                    new SqlParameter("@FirstName",input.FirstName),
                    new SqlParameter("@LastName",input.LastName),
                    new SqlParameter("@ContactNumber",input.ContactNumber),
                    new SqlParameter("@Email",input.Email),                   
                    new SqlParameter("@RoleCode",input.RoleCode),
                    new SqlParameter("@IsActive",input.IsActive),
                    new SqlParameter("@ModifiedOn",input.ModifiedOn),
                    new SqlParameter("@ModifiedBy",input.ModifiedBy)
                };

                res = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, query, param.ToArray()).ToInt();
                //            string email = SqlHelper.ExecuteScalar(tran, CommandType.Text, "select email from Users where id=@UserId", param.ToArray()).ToString();

                if (res <= 0)
                {
                    tran.Rollback();
                    response.isSuccess = false;
                    response.Message = "An Error Occured";
                    return response;
                }
                else
                {
                    tran.Commit();
                    response.isSuccess = true;
                    response.Message = "User Updated Successfully !";
                    return response;

                }

            }
            catch (Exception ex)
            {
                tran.Rollback();
                response.isSuccess = false;
                response.Message = "An exception occured";
                return response;
            }
            // return result;            

        }
        public ResponseModels ChangePassword(string formData)
        {
            ResponseModels response = new();

            SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
            conn.Open();            
            SqlTransaction tran = conn.BeginTransaction();
            int res = 0;


            try
            {
                UsersModels user = new();
                var model = JsonConvert.DeserializeObject<dynamic>(formData);

                using (var rdr = SqlHelper.ExecuteReader(tran, CommandType.Text, "select PasswordHash,SecurityStamp from Users where Guid=@Guid", new SqlParameter("@Guid", model.HeaderData.Guid.ToString())))
                {
                    while (rdr.Read())
                    {
                        
                        user.PasswordHash = rdr["PasswordHash"].ToString();
                        user.SecurityStamp = (byte[])rdr["SecurityStamp"];
                    }
                }
                if ( !string.IsNullOrEmpty(user.PasswordHash) && user.SecurityStamp.Count() > 0)
                        {

                            if ( PasswordHelperDal.VerifyPassword(model.HeaderData.OldPassword.ToString(),user.PasswordHash,user.SecurityStamp) )
                            {
                                Byte[] SecurityStamp = PasswordHelperDal.GenerateSalt();
                                string NewPassword = model.HeaderData.NewPassword.ToString();
                                string PasswordHash = NewPassword.HashPassword(SecurityStamp);

                                string UpQuery = @"update Users set SecurityStamp = @SecurityStamp , PasswordHash=@PasswordHash where Guid=@Guid";
                                List<SqlParameter> param = new List<SqlParameter>
                                {
                                    new SqlParameter("@SecurityStamp",SecurityStamp),
                                    new SqlParameter("@PasswordHash",PasswordHash),
                                    new SqlParameter("@Guid",model.HeaderData.Guid.ToString())
                                    
                                };
                                res = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, UpQuery,param.ToArray());
                                if (res <= 0)
                                {
                                    tran.Rollback();
                                    response.isSuccess = false;
                                    response.Message = "An Error Occured";
                                    return response;
                                }
                                else
                                {
                                    tran.Commit();
                                    response.isSuccess = true;
                                    response.Message = "Password Changed Sucessfull !";
                                    return response;

                                }

                            }
                            else
                            {
                                tran.Rollback();
                                response.isSuccess = false;
                                response.Message = "Old Password Doesn't Match !";
                                return response;
                            }


                        }
                        else
                        {
                            tran.Rollback();
                            response.isSuccess = false;
                            response.Message = "An error occured !";
                            return response;
                        }
                   
                
                tran.Rollback();
                response.isSuccess = false;
                response.Message = "User not Found !";
                return response;

            }
            catch (Exception)
            {

                throw;
            }


        }
    }
}
