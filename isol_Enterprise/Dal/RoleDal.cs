using iSOL_Enterprise.Common;
using iSOL_Enterprise.Models;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using SAPbobsCOM;
using SqlHelperExtensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Permissions;
using System.Threading.Tasks;

namespace iSOL_Enterprise.Dal
{
    public class RoleDal
    {
        public List<TreeModel> GetPages(string RoleCode = null)
        {

            CommonDal cdal = new ();

            List<TreeModel> Pages = new List<TreeModel>();

            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, "select Id,PageId, PageName from Pages where IsActive=1 and RowStatus=1"))
            {
                while (rdr.Read())
                {
                    TreeModel model = new TreeModel();

                    model.id = rdr["PageId"].ToString();
                    model.text = rdr["PageName"].ToString();
                    model.@checked = RoleCode == null ? true : cdal.CheckPageOnRole(RoleCode, rdr["PageId"].ToString());
                    model.population -= null;
                    model.flagUrl = null;
                    model.children = GetPagesActivities(rdr["PageId"].ToString(), RoleCode);
                    Pages.Add(model);
                }
            }
            return Pages;
        }

        private List<TreeModel> GetPagesActivities(string PageID, string RoleCode = null)
        {
            CommonDal cdal = new();

            string DetailQuery = @"select p.Id, p.PageId, r.RoleActivityTypeCode, r.RoleActivityTypeName,p.icon,p.class from RoleActivityTypes r
                                    inner join PageActivity p on p.RoleActivityTypeCode = r.RoleActivityTypeCode and p.isActive=1
                                    where p.PageId='"+PageID+"' and r.IsActive=1";
            List<TreeModel> Pages = new List<TreeModel>();

            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, DetailQuery))
            {
                while (rdr.Read())
                {
                    TreeModel model = new TreeModel();

                    model.id = rdr["Id"].ToString();
                    model.text = "<i class='" + rdr["icon"].ToString() + " text-"+ rdr["class"].ToString() + " mr-2 ml-2 icon-nm'></i>" +  rdr["RoleActivityTypeName"].ToString();
                    model.@checked = RoleCode == null ? true : cdal.CheckPageActivityOnRole(RoleCode, PageID, rdr["RoleActivityTypeCode"].ToString());
                    model.population -= null;
                    model.flagUrl = null;
                    model.children = null;
                    Pages.Add(model);
                }
            }
            return Pages;
        }
        public List<RoleModels> GetAllRole()
        {
            List<RoleModels> lstModel = new List<RoleModels>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, @"select  r.Id,r.RoleName,r.RoleCode,u.FirstName,r.CreatedOn,r.IsActive,r.guid  from Roles r
                                                                                            left join Users u on u.Id = r.CreatedBy where  r.RowStatus=1"))
            {
                while (rdr.Read())
                {
                    RoleModels model = new ()
                    {
                        Id = rdr["Id"].ToInt(),
                        RoleName = rdr["RoleName"].ToString(),
                        RoleCode = rdr["RoleCode"].ToString(),
                        CreatedBy = rdr["FirstName"].ToString(),
                        Guid = rdr["guid"].ToString(),
                        CreatedOn = rdr["CreatedOn"].ToDateTime(),
                        IsActive = (bool) rdr["IsActive"],

                    
                    };

                    lstModel.Add(model);
                }
            }
            return lstModel;
        }

        public ResponseModels Add(string formData,string Name,int? UserId)
        {
            ResponseModels response = new ();
            var model = JsonConvert.DeserializeObject<dynamic>(formData);

            bool RoleNameCheck = CommonDal.Count("Roles", "RoleName", model.RoleName.ToString());
            if (RoleNameCheck)
            {
                response.isSuccess = false;
                response.Message = "Role already exits try different !";
                return response;
            }

            string query = @"insert into Roles(Id,Guid,RoleCode,RoleName,IsActive,
                            RowStatus,CreatedBy,CreatedOn) 
                            values(@Id,@Guid,@RoleCode,@RoleName,@IsActive,
                            @RowStatus,@CreatedBy,@CreatedOn)";

            

            SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
            conn.Open();
            SqlTransaction tran = conn.BeginTransaction();
            int res = 0;
            try
            {
                int RoleId = CommonDal.getPrimaryKey(tran, "Roles");
                string RoleCode = "RL" + RoleId;


                List<SqlParameter> param = new List<SqlParameter>
                {
                    new SqlParameter("@Id",RoleId),
                    new SqlParameter("@Guid", CommonDal.generatedGuid()),
                    new SqlParameter("@RoleCode",RoleCode),
                    new SqlParameter("@RoleName",model.RoleName.ToString()),
                    new SqlParameter("@IsActive",(bool) model.IsActive),
                    new SqlParameter("@RowStatus",true),
                    new SqlParameter("@CreatedBy",UserId),
                    new SqlParameter("@CreatedOn",DateTime.Now)
                };

                res = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, query, param.ToArray()).ToInt();
                if (res <= 0)
                {
                    tran.Rollback();
                    response.isSuccess = false;
                    response.Message = "An Error occured !";
                    return response;
                }
                foreach (string PageActivityID in model.Permissions)
                {
                   

                    int count = SqlHelper.ExecuteScalar(tran, CommandType.Text, @"select count(*) from Pages where PageId=@PageId", new SqlParameter("@PageId", PageActivityID)).ToInt();
                    if (count > 0)
                    {
                        int URid = CommonDal.getPrimaryKey(tran, "UserRoles");
                        string UserRolesQuery = @"insert into UserRoles (id,RoleCode,PageId,IsActive)
                                                                values(@id,@RoleCode,@PageId,@IsActive)";

                        List<SqlParameter> param3 = new List<SqlParameter>
                                {
                                    new SqlParameter("@id",URid),
                                    new SqlParameter("@RoleCode",RoleCode),
                                    new SqlParameter("@PageId",PageActivityID),
                                    new SqlParameter("@IsActive",(bool) model.IsActive),
                                };
                        res = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, UserRolesQuery, param3.ToArray()).ToInt();
                        if (res <= 0)
                        {
                            tran.Rollback();
                            response.isSuccess = false;
                            response.Message = "An Error occured !";
                            return response;
                        }

                    }
                    else 
                    {
                        int ActivityID = PageActivityID.ToInt();
                        int id = CommonDal.getPrimaryKey(tran, "UserRolePageActivity");
                        using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, "select * from PageActivity where IsActive=1 and Id=@Id",new SqlParameter("@Id", ActivityID)))
                        {
                            while (rdr.Read())
                            {

                            

                                
                                string UserRolePageActivity = @"insert into UserRolePageActivity (id,RoleCode,RoleActivityCode,PageId,Status)
                                                                                          values(@id,@RoleCode,@RoleActivityCode,@PageId,@Status)";

                                List<SqlParameter> param2 = new List<SqlParameter>
                                {
                                    new SqlParameter("@id",id),
                                    new SqlParameter("@RoleCode",RoleCode),
                                    new SqlParameter("@RoleActivityCode",rdr["RoleActivityTypeCode"].ToString()),
                                    new SqlParameter("@PageId",rdr["PageId"].ToString()),
                                    new SqlParameter("@Status",(bool) model.IsActive),
                                };
                                res = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, UserRolePageActivity, param2.ToArray()).ToInt();
                                if (res <= 0)
                                {
                                    
                                    tran.Rollback();
                                    response.isSuccess = false;
                                    response.Message = "An Error occured !";
                                    return response;
                                }
                                id += 1;
                            }
                        }
                    }
                }

                if (res > 0)
                {
                    tran.Commit();                    
                    response.isSuccess = true;
                    response.Message = "Role Added Successfully !";
                    return response;
                }
            }
            catch (Exception ex)
            {
                
                tran.Rollback();
                response.isSuccess = false;
                response.Message = "An Error occured !";
                return response;
            }
            // return result;
            return response;


        }

        public ResponseModels Edit(string formData, string Name, int? UserId)
        {
            ResponseModels response = new();
            var model = JsonConvert.DeserializeObject<dynamic>(formData);

            
            bool RoleNameCheck = CommonDal.CountOnEdit("Roles", "RoleName", model.RoleName.ToString(), model.Guid.ToString());
            if (RoleNameCheck)
            {
                response.isSuccess = false;
                response.Message = "Role already exits try different !";
                return response;
            }

            
            string query = @"update Roles set RoleName=@RoleName , isActive = @isActive , ModifiedBy=@ModifiedBy , ModifiedOn=@ModifiedOn where Guid =@Guid";

            SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
            conn.Open();
            SqlTransaction tran = conn.BeginTransaction();
            int res = 0;
            try
            {
                int RoleId = CommonDal.getPrimaryKey(tran, "Roles");
                


                List<SqlParameter> param = new List<SqlParameter>
                {
                    
                    new SqlParameter("@Guid", model.Guid.ToString()),                    
                    new SqlParameter("@RoleName",model.RoleName.ToString()),
                    new SqlParameter("@IsActive",(bool) model.IsActive),                    
                    new SqlParameter("@ModifiedBy",UserId),
                    new SqlParameter("@ModifiedOn",DateTime.Now)
                };

                res = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, query, param.ToArray()).ToInt();
                if (res <= 0)
                {
                    tran.Rollback();
                    response.isSuccess = false;
                    response.Message = "An Error occured !";
                    return response;
                }

                string RoleCode = SqlHelper.ExecuteScalar(tran,CommandType.Text, @"select RoleCode from Roles where Guid =@Guid", new SqlParameter("@Guid",model.Guid.ToString())).ToString();

                int count = 0;
                int UserRolesCount = SqlHelper.ExecuteScalar(tran, CommandType.Text, @"select count(*) from UserRoles where RoleCode = '" + RoleCode + "'").ToInt();
                int UserRolePageActivityCount = SqlHelper.ExecuteScalar(tran, CommandType.Text, @"select count(*) from UserRolePageActivity where RoleCode = '" + RoleCode + "'").ToInt();
                string DeleteQuery = "";
                if (UserRolesCount > 0)
                {
                      DeleteQuery = @"delete from UserRoles where RoleCode = '" + RoleCode + "'";
                      count = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, DeleteQuery).ToInt();
                      if (count <= 0)
                      {
                          tran.Rollback();
                          response.isSuccess = false;
                          response.Message = "An Error occured !";
                          return response;
                      }
                }
                if (UserRolePageActivityCount > 0)
                {
                      DeleteQuery = @" delete from UserRolePageActivity where RoleCode = '" + RoleCode + "'";
                      count = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, DeleteQuery).ToInt();
                      if (count <= 0)
                      {
                          tran.Rollback();
                          response.isSuccess = false;
                          response.Message = "An Error occured !";
                          return response;
                      }
                }
                
                foreach (string PageActivityID in model.Permissions)
                {


                     count = SqlHelper.ExecuteScalar(tran, CommandType.Text, @"select count(*) from Pages where PageId=@PageId", new SqlParameter("@PageId", PageActivityID)).ToInt();
                    if (count > 0)
                    {
                        int URid = CommonDal.getPrimaryKey(tran, "UserRoles");
                        string UserRolesQuery = @"insert into UserRoles (id,RoleCode,PageId,IsActive)
                                                                values(@id,@RoleCode,@PageId,@IsActive)";

                        List<SqlParameter> param3 = new List<SqlParameter>
                                {
                                    new SqlParameter("@id",URid),
                                    new SqlParameter("@RoleCode",RoleCode),
                                    new SqlParameter("@PageId",PageActivityID),
                                    new SqlParameter("@IsActive",(bool) model.IsActive),
                                };
                        res = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, UserRolesQuery, param3.ToArray()).ToInt();
                        if (res <= 0)
                        {
                            tran.Rollback();
                            response.isSuccess = false;
                            response.Message = "An Error occured !";
                            return response;
                        }

                    }
                    else
                    {
                        int ActivityID = PageActivityID.ToInt();
                        int id = CommonDal.getPrimaryKey(tran, "UserRolePageActivity");
                        using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, "select * from PageActivity where IsActive=1 and Id=@Id", new SqlParameter("@Id", ActivityID)))
                        {
                            while (rdr.Read())
                            {




                                string UserRolePageActivity = @"insert into UserRolePageActivity (id,RoleCode,RoleActivityCode,PageId,Status)
                                                                                          values(@id,@RoleCode,@RoleActivityCode,@PageId,@Status)";

                                List<SqlParameter> param2 = new List<SqlParameter>
                                {
                                    new SqlParameter("@id",id),
                                    new SqlParameter("@RoleCode",RoleCode),
                                    new SqlParameter("@RoleActivityCode",rdr["RoleActivityTypeCode"].ToString()),
                                    new SqlParameter("@PageId",rdr["PageId"].ToString()),
                                    new SqlParameter("@Status",(bool) model.IsActive),
                                };
                                res = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, UserRolePageActivity, param2.ToArray()).ToInt();
                                if (res <= 0)
                                {

                                    tran.Rollback();
                                    response.isSuccess = false;
                                    response.Message = "An Error occured !";
                                    return response;
                                }
                                id += 1;
                            }
                        }
                    }
                }
                
                if (res > 0)
                {
                    tran.Commit();
                    response.isSuccess = true;
                    response.Message = "Role Updated Successfully !";
                    return response;
                }
            }
            catch (Exception ex)
            {

                tran.Rollback();
                response.isSuccess = false;
                response.Message = "An Exception occured !";
                return response;
            }
            // return result;
            return response;


        }

        public RoleModels Get(string Guid)
        {
            string qeury = "select * From Roles where Guid=@Guid";
            
            RoleModels model = new RoleModels();

            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, qeury, new SqlParameter("@Guid", Guid)))
            {
                while (rdr.Read())
                {
                    model.RoleName = rdr["RoleName"].ToString();
                    model.RoleCode = rdr["RoleCode"].ToString();
                    model.IsActive = rdr["IsActive"].ToBool();
                    
                    //roleModels.ListPages.Add(PageModel);
                }

            }
            return model;

        }
    }
}
