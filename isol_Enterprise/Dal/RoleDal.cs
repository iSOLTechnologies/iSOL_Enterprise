using iSOL_Enterprise.Common;
using iSOL_Enterprise.Models;
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
        public List<TreeModel> GetPages()
        {
            


            List<TreeModel> Pages = new List<TreeModel>();

            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, "select Id,PageId, PageName from Pages where IsActive=1 and RowStatus=1"))
            {
                while (rdr.Read())
                {
                    TreeModel model = new TreeModel();

                    model.id = rdr["PageId"].ToString();
                    model.text = rdr["PageName"].ToString();
                    model.@checked = true;
                    model.population -= null;
                    model.flagUrl = null;
                    model.children = GetPagesActivities(rdr["PageId"].ToString());
                    Pages.Add(model);
                }
            }
            return Pages;
        }

        private List<TreeModel> GetPagesActivities(string PageID)
        {
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
                    model.@checked = true;
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
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, "select RoleCode, RoleName from Roles where IsActive=1 and RowStatus=1"))
            {
                while (rdr.Read())
                {
                    RoleModels model = new RoleModels();
                    model.RoleCode = rdr["RoleCode"].ToString();
                    model.RoleName = rdr["RoleName"].ToString();
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
                    new SqlParameter("@id",RoleId),
                    new SqlParameter("@Guid", CommonDal.generatedGuid()),
                    new SqlParameter("@RoleCode",RoleCode),
                    new SqlParameter("@RoleName",model.RoleName.ToString()),
                    new SqlParameter("@IsActive",model.IsActive),
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
                foreach (var PageActivityID in model.Permissions)
                {

                    using (var rdr = SqlHelper.ExecuteReader(tran, CommandType.Text, "select * from PageActivity where IsActive=1 and Id='@Id'",new SqlParameter("@Id",PageActivityID)))
                    {
                        while (rdr.Read())
                        {

                            int count = SqlHelper.ExecuteScalar(tran, CommandType.Text, @"select count(*) from Pages where PageId='@PageId'", new SqlParameter("@Id", PageActivityID)).ToInt();
                            if (count > 0)
                            {
                                int URid = CommonDal.getPrimaryKey(tran, "UserRoles");
                                string UserRolesQuery = @"insert into UserRoles id,RoleCode,PageId,Status 
                                                                                      values(@id,@RoleCode,@PageId,@Status)";

                                List<SqlParameter> param3 = new List<SqlParameter>
                                {
                                    new SqlParameter("@id",URid),
                                    new SqlParameter("@RoleCode",RoleCode),                                
                                    new SqlParameter("@PageId",PageActivityID),
                                    new SqlParameter("@Status",model.IsActive),
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

                            int id = CommonDal.getPrimaryKey(tran, "UserRolePageActivity");
                            string UserRolePageActivity = @"insert into UserRolePageActivity id,RoleCode,RoleActivityCode,PageId,Status 
                                                                                      values(@id,@RoleCode,@RoleActivityCode,@PageId,@Status)";

                            List<SqlParameter> param2 = new List<SqlParameter>
                            {
                                new SqlParameter("@id",id),
                                new SqlParameter("@RoleCode",RoleCode),
                                new SqlParameter("@RoleActivityCode",rdr["RoleActivityTypeCode"].ToString()),
                                new SqlParameter("@PageId",rdr["PageId"].ToString()),
                                new SqlParameter("@Status",model.IsActive),
                            };
                            res = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, UserRolePageActivity, param2.ToArray()).ToInt();
                            if (res <= 0)
                            {
                                tran.Rollback();
                                tran.Rollback();
                                response.isSuccess = false;
                                response.Message = "An Error occured !";
                                return response;
                            }
                        }
                    }

                }

                if (res > 0)
                {
                    tran.Commit();
                    tran.Rollback();
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

        public bool Edit(RoleModels input)
        {
            string query = @"update Roles set RoleName=@RoleName,IsActive=@IsActive,ModifiedBy=@ModifiedBy,ModifiedOn=@ModifiedOn where Guid=@Guid";
            string ActivityQuery = @"insert into UserRolePageActivity(Id,RoleActivityCode,PageId,Status,RoleCode)
values(@Id,@RoleActivityCode,@PageId,@Status,@RoleCode)";

            SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
            conn.Open();
            SqlTransaction tran = conn.BeginTransaction();
            int res = 0;
            try
            {

                List<SqlParameter> param = new List<SqlParameter>
                {

                    new SqlParameter("@Guid",input.Guid),
                    new SqlParameter("@RoleName",input.RoleName),
                    new SqlParameter("@IsActive",input.IsActive),
                    new SqlParameter("@ModifiedBy",input.ModifiedBy),
                    new SqlParameter("@ModifiedOn",input.ModifiedOn)
                };

                res = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, query, param.ToArray()).ToInt();

                string deleteQuery = "Delete from UserRoles where RoleCode=@RoleCode";
                int Delres = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, deleteQuery, new SqlParameter("@RoleCode", input.RoleCode)).ToInt();

                string deleteAct = "Delete from UserRolePageActivity where RoleCode=@RoleCode";
                int DelAct = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, deleteAct, new SqlParameter("@RoleCode", input.RoleCode)).ToInt();

                string detailQuery = @"insert into UserRoles(Id,RoleCode,PageId,IsActive)
values(@Id,@RoleCode,@PageId,@IsActive)";

                foreach (var item in input.ListPages)
                {
                    item.Id = CommonDal.getPrimaryKey(tran, "UserRoles");
                    List<SqlParameter> param2 = new List<SqlParameter>
                    {
                        new SqlParameter("@id",item.Id),
                        new SqlParameter("@RoleCode",input.RoleCode),
                        new SqlParameter("@PageId",item.PageId),
                        new SqlParameter("@IsActive",input.IsActive=true),

                    };

                    foreach (var item2 in item.ListUserRolePageActivity)
                    {
                        item2.Id = CommonDal.getPrimaryKey(tran, "UserRolePageActivity");

                        List<SqlParameter> param3 = new List<SqlParameter>
                        {
                            new SqlParameter("@id",item2.Id),
                            new SqlParameter("@RoleActivityCode",item2.RoleActivityCode),
                            new SqlParameter("@RoleCode",input.RoleCode),
                            new SqlParameter("@PageId",item.PageId),
                            new SqlParameter("@Status",item2.Status),

                        };
                        int res3 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, ActivityQuery, param3.ToArray()).ToInt();
                        if (res3 <= 0)
                        {
                            tran.Rollback();
                            return false;

                        }
                    }

                    int res2 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, detailQuery, param2.ToArray()).ToInt();
                    if (res2 <= 0)
                    {
                        tran.Rollback();
                    }
                }

                if (res > 0 &&  DelAct > 0 && Delres > 0)
                {
                    tran.Commit();
                }
                else
                {
                    tran.Rollback();
                    return false;
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

        public RoleModels Get(string Guid)
        {
            string qeury = "select *From Roles where Guid=@Guid";
            string detailQuery = "select *from UserRoles where RoleCode=@RoleCode";
            string SubQuery = "select *from UserRolePageActivity where RoleCode=@RoleCode and PageId=@PageId";

            RoleModels model = new RoleModels();

            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, qeury, new SqlParameter("@Guid", Guid)))
            {
                while (rdr.Read())
                {
                    model.RoleName = rdr["RoleName"].ToString();
                    model.RoleCode = rdr["RoleCode"].ToString();
                    model.IsActive = rdr["IsActive"].ToBool();
                    model.ListPages = new List<PagesModels>();

                    using (var rdr2 = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, detailQuery, new SqlParameter("@RoleCode", model.RoleCode)))
                    {
                        while (rdr2.Read())
                        {
                            PagesModels PageModel = new PagesModels();
                            PageModel.PageId = rdr2["PageId"].ToString();
                            PageModel.ListUserRolePageActivity = new List<UserRolePageActivityModels>();

                            using (var rdr3 = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, SubQuery, new SqlParameter("@RoleCode", model.RoleCode), new SqlParameter("PageId", PageModel.PageId)))
                            {
                                while (rdr3.Read())
                                {
                                    UserRolePageActivityModels ActivityModel = new UserRolePageActivityModels();
                                    ActivityModel.PageId = rdr3["PageId"].ToString();
                                    ActivityModel.RoleActivityCode = rdr3["RoleActivityCode"].ToString();
                                    ActivityModel.Status = rdr3["Status"].ToBool();
                                    PageModel.ListUserRolePageActivity.Add(ActivityModel);
                                }
                            }
                            model.ListPages.Add(PageModel);
                        }
                    }
                    //roleModels.ListPages.Add(PageModel);
                }

            }
            return model;

        }
    }
}
