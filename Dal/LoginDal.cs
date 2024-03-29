﻿using iSOL_Enterprise.Common;
using iSOL_Enterprise.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SqlHelperExtensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace iSOL_Enterprise.Dal
{
    public class LoginDal
    {
        public UsersModels Get(UsersModels input)
        {
            SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
            conn.Open();
            SqlTransaction tran = conn.BeginTransaction();
            UsersModels model = new UsersModels();
            try
            {
                string LoginQuery = @"select u.id,u.FirstName,u.LastName,u.Email,u.Username,u.ContactNumber,u.Password,u.IsLoggedIn,u.DateOfBirth,u.Gender,u.UserPic,u.RoleCode from users u
 where u.email=@Username and u.Password=@Password and IsActive=1";

                SqlParameter[] param = new SqlParameter[]
                {
                new SqlParameter("@Username",input.Username),
                new SqlParameter("@Password",input.Password)
                };


                using (var rdr = SqlHelper.ExecuteReader(tran, CommandType.Text, LoginQuery, param))
                {
                    while (rdr.Read())
                    {
                        model.Id = rdr["Id"].ToInt();
                        model.FirstName = rdr["FirstName"].ToString();
                        model.LastName = rdr["LastName"].ToString();
                        model.Email = rdr["Email"].ToString();
                        model.ContactNumber = rdr["ContactNumber"].ToString();
                        model.Password = rdr["Password"].ToString();
                        model.IsLoggedIn = rdr["IsLoggedIn"].ToBool();
                        model.DateOfBirth = rdr["DateOfBirth"].ToDateTime();
                        if (rdr["Gender"].ToString() == "M")
                            model.Gender = "Male";
                        else if (rdr["Gender"].ToString() == "F")
                            model.Gender = "Female";
                        else
                            model.Gender = String.Empty;
                        model.UserPic = rdr["UserPic"].ToString();
                        model.RoleCode = rdr["RoleCode"].ToString();
                        model.Guid = input.Guid;
                    }
                    rdr.Close();
                    bool result = new LoginDal().GenerateSession(tran, model);
                    bool GenerateLogs = new LoginDal().GenerateSessionLogs(tran, model);
                    if (result == true && GenerateLogs == true)
                    {
                        // CommonDal.SessionGUID = input.Guid;
                        model.IsSession = true;
                    }
                    else
                    {
                        model.IsSession = false;
                    }
                 

                }
                return model;
            }
            catch (Exception ex)
            {

                tran.Rollback();
                return model;
            }

        }

        public UsersModels GetForAPI(_usersModels input)
        {
            SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
            conn.Open();
            SqlTransaction tran = conn.BeginTransaction();
            _usersModels model = new _usersModels();
            try
            {
                string LoginQuery = @"select u.id,u.FirstName,u.LastName,u.Email,u.Username,u.ContactNumber,u.Password,u.IsLoggedIn,u.DateOfBirth,u.Gender,u.UserPic,u.RoleCode from users u
 where u.email=@Username and u.Password=@Password and IsActive=1";

                SqlParameter[] param = new SqlParameter[]
                {
                new SqlParameter("@Username",input.Email),
                new SqlParameter("@Password",input.Password)
                };


                using (var rdr = SqlHelper.ExecuteReader(tran, CommandType.Text, LoginQuery, param))
                {
                    while (rdr.Read())
                    {
                        model.Id = rdr["Id"].ToInt();
                        model.FirstName = rdr["FirstName"].ToString();
                        model.LastName = rdr["LastName"].ToString();
                        model.Email = rdr["Email"].ToString();
                        model.ContactNumber = rdr["ContactNumber"].ToString();
                        model.Password = rdr["Password"].ToString();
                        model.IsLoggedIn = rdr["IsLoggedIn"].ToBool();
                        model.DateOfBirth = rdr["DateOfBirth"].ToDateTime();
                        if (rdr["Gender"].ToString() == "M")
                            model.Gender = "Male";
                        else if (rdr["Gender"].ToString() == "F")
                            model.Gender = "Female";
                        else
                            model.Gender = String.Empty;
                        model.UserPic = rdr["UserPic"].ToString();
                        model.RoleCode = rdr["RoleCode"].ToString();
                        model.Guid = input.Guid;
                    }
                    rdr.Close();
                    bool result = new LoginDal().GenerateSession(tran, model);
                    bool GenerateLogs = new LoginDal().GenerateSessionLogs(tran, model);
                    model.ListPages = new NavDal().getMenu(model.RoleCode);
                    if (result == true && GenerateLogs == true)
                    {
                        // CommonDal.SessionGUID = input.Guid;
                        model.IsSession = true;
                    }
                    else
                    {
                        model.IsSession = false;
                    }


                }
                return model;
            }
            catch (Exception ex)
            {

                tran.Rollback();
                return model;
            }

        }
        public bool GenerateSession(SqlTransaction tran, UsersModels input)
        {

            bool result;
            List<SqlParameter> param = new List<SqlParameter>
            {
                new SqlParameter("@id",input.Id),
                new SqlParameter("@IpAddress",input.IpAddress),
                new SqlParameter("@MachineName",input.MachineName),
                new SqlParameter("@SessionId",input.Guid),
                new SqlParameter("@LogType","Login"),
            };
            result = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, @"update Users set SessionId=@SessionId,IpAddress=@IpAddress, 
MachineName=@MachineName, IsLoggedIn=1 where id=@id", param.ToArray()).ToBoolean();

            return true;
        }

        public bool GenerateSessionLogs(SqlTransaction tran, UsersModels input)
        {
            bool result;
            List<SqlParameter> param = new List<SqlParameter>
            {
                new SqlParameter("@id",input.Id),
                new SqlParameter("@USerId",input.Id),
                new SqlParameter("@IpAddress",input.IpAddress),
                new SqlParameter("@MachineName",input.MachineName),
                new SqlParameter("@SessionId",input.Guid),
                new SqlParameter("@LogType","Login"),
            };

            result = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, @"insert into UsersSessionLogs (UserId,SessionId,IpAddress,MachineName,LogType)
                values(@UserId,@SessionId,@IpAddress,@MachineName,@LogType)", param.ToArray()).ToBoolean();
            if (result)
            {
                tran.Commit();
                return true;

            }
            tran.Rollback();
            return false;
        }

        public bool GenerateJson(string Guid, int id, string Root)
        {
            try
            {
                string Username = "";
                UsersModels model = new UsersModels();
                DataTable dt = SqlHelper.GetData("select * from users where id=@id and IsActive=1 and RowStatus=1", new SqlParameter("@id", id));
                foreach (DataRow rdr in dt.Rows)
                {
                    model.Id = rdr["Id"].ToInt();
                    model.FirstName = rdr["FirstName"].ToString();
                    model.LastName = rdr["LastName"].ToString();
                    model.Username = rdr["Username"].ToString();
                    Username = rdr["Username"].ToString();
                    model.Email = rdr["Email"].ToString();
                    model.ContactNumber = rdr["ContactNumber"].ToString();
                    model.Password = rdr["Password"].ToString();
                    model.IsLoggedIn = rdr["IsLoggedIn"].ToBool();
                    model.DateOfBirth = rdr["DateOfBirth"].ToDateTime();
                    if (rdr["Gender"].ToString() == "M")
                        model.Gender = "Male";
                    else if (rdr["Gender"].ToString() == "F")
                        model.Gender = "Female";
                    else
                        model.Gender = String.Empty;
                    model.UserPic = rdr["UserPic"].ToString();
                    model.RoleCode = rdr["RoleCode"].ToString();
                    model.Guid = Guid;
                }

                string json = JsonConvert.SerializeObject(model);

                string path = Path.Combine(Root, "UserJson\\" + Username + ".json");
                System.IO.File.WriteAllText(path, json);
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }

        }

        public bool ReomveSession(UsersModels input)
        {

            bool result;
            List<SqlParameter> param = new List<SqlParameter>
            {
                 new SqlParameter("@Username",input.Username),
                new SqlParameter("@Password",input.Password)
            };
            result = SqlHelper.ExecuteNonQuery(@"update Users set SessionId='',IpAddress='', 
MachineName='', IsLoggedIn=0 where Username=@Username and Password=@Password", param.ToArray()).ToBoolean();

            return true;
        }

        public ResponseModels ChangePassword(UsersModels input)
        {
            ResponseModels models = new ResponseModels();
            int result = 0;
            SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
            conn.Open();
            SqlTransaction tran = conn.BeginTransaction();
            try
            {

                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@Id",input.Id),
                    new SqlParameter("@Password",input.CurrentPassword),
                    new SqlParameter("@NewPassword",input.Password)
                };

                string password = SqlHelper.ExecuteScalar(tran, CommandType.Text, "select password from users where Id=@Id", param.ToArray()).ToString();
                if (password == input.CurrentPassword)
                {
                    //           models.Data = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, "update users set Password=@NewPassword where Id=@Id", param.ToArray()).ToBool();
                    int i = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, "update users set Password=@NewPassword where Id=@Id", param.ToArray()).ToInt();
                    if (i > 0)
                    {
                        models.isInserted = true;
                        models.Data = true;
                    }
                    else
                    {
                        models.isError = false;
                        models.Data = false;
                    }
                    //        models.isInserted = true;
                    tran.Commit();
                    return models;

                }
                else
                {
                    models.isInserted = false;
                    tran.Commit();
                    return models;
                }
            }
            catch (Exception)
            {

                tran.Rollback();
                return models;
            }


        }

    }
}
