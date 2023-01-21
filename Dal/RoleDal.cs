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
    public class RoleDal
    {
        public List<PagesModels> GetPages()
        {
            string DetailQuery = @"select p.PageId, r.RoleActivityTypeCode, r.RoleActivityTypeName,p.icon,p.class from RoleActivityTypes r
inner join PageActivity p on p.RoleActivityTypeCode = r.RoleActivityTypeCode and p.isActive=1
where p.PageId=@PageId and r.IsActive=1";


            List<PagesModels> lstModel = new List<PagesModels>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, "select PageId, PageName from Pages where IsActive=1 and RowStatus=1"))
            {
                while (rdr.Read())
                {
                    PagesModels model = new PagesModels();
                    model.PageName = rdr["PageName"].ToString();
                    model.PageId = rdr["PageId"].ToString();
                    model.ListPageActivity = new List<PageActivityModels>();

                    using (var rdr2 = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, DetailQuery, new SqlParameter("@PageId", model.PageId)))
                    {
                        while (rdr2.Read())
                        {
                            PageActivityModels SubModel = new PageActivityModels();
                            SubModel.RoleActivityCode = rdr2["RoleActivityTypeCode"].ToString();
                            SubModel.RoleActivityTypeName = rdr2["RoleActivityTypeName"].ToString();
                            SubModel.PageId = rdr2["PageId"].ToString();
                            SubModel.Icon = rdr2["Icon"].ToString();
                            SubModel.Class = rdr2["Class"].ToString();
                            model.ListPageActivity.Add(SubModel);
                        }
                    }

                    lstModel.Add(model);

                }
            }
            return lstModel;
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

        public bool Add(RoleModels input)
        {
            string query = @"insert into Roles(Id,Guid,RoleCode,RoleName,IsActive,
RowStatus,CreatedBy,CreatedOn) 
values(@Id,@Guid,@RoleCode,@RoleName,@IsActive,
@RowStatus,@CreatedBy,@CreatedOn)";

            string ActivityQuery = @"insert into UserRolePageActivity(Id,RoleActivityCode,PageId,Status,RoleCode)
values(@Id,@RoleActivityCode,@PageId,@Status,@RoleCode)";

            SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
            conn.Open();
            SqlTransaction tran = conn.BeginTransaction();
            int res = 0;
            try
            {
                input.Id = CommonDal.getPrimaryKey(tran, "Roles");
                input.RoleCode = "RL" + input.Id;


                List<SqlParameter> param = new List<SqlParameter>
                {
                    new SqlParameter("@id",input.Id),
                    new SqlParameter("@Guid",input.Guid = CommonDal.generatedGuid()),
                    new SqlParameter("@RoleCode",input.RoleCode),
                    new SqlParameter("@RoleName",input.RoleName),
                    new SqlParameter("@IsActive",input.IsActive),
                    new SqlParameter("@RowStatus",input.RowStatus=true),
                    new SqlParameter("@CreatedBy",input.CreatedBy),
                    new SqlParameter("@CreatedOn",input.CreatedOn)
                };

                res = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, query, param.ToArray()).ToInt();

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
                        return false;
                    }
                }

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
