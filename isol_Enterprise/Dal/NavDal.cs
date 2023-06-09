using iSOL_Enterprise.Common;
using iSOL_Enterprise.Models;
using SqlHelperExtensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;


namespace iSOL_Enterprise.Dal
{
    public class NavDal
    {
        public List<ModulesModels> getMenu(string RoleCode)
        {
            try
            {
                string query = @"select m.name,m.icon,m.ModuleId,m.Icon from modules m
                                inner join Pages p on p.Moduleid=m.ModuleId
                                inner join UserRoles ur on ur.PageId=p.PageId and ur.RoleCode='" + RoleCode + @"'
                                 where m.isActive=1 
                                 group by m.name,m.ModuleId,m.Icon,m.lineindex order by m.lineindex";
                List<ModulesModels> modules = new List<ModulesModels>();
                

                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, query))
                {
                    while (rdr.Read())
                    {

                        ModulesModels model = new ModulesModels();
                        model.Name = rdr["name"].ToString();
                        model.ModuleId = rdr["ModuleId"].ToString();
                        model.Icon = rdr["Icon"].ToString();
                        model.ListModules = new List<ModulesModels>();


                        string PAgeQuery = @"select p.PageName,p.Controller,p.ActionName,p.icon,p.PageId,p.Guid from Pages p
                                        inner join UserRoles ur on ur.PageId=p.PageId and ur.RoleCode='" + RoleCode + @"' and p.ModuleId=@ModuleId
                                         where p.isActive=1 
                                        order by p.lineindex";
                        using (var rdr2 = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, PAgeQuery, new System.Data.SqlClient.SqlParameter("@ModuleId", model.ModuleId)))
                        {
							List<PagesModels> pages = new List<PagesModels>();
							while (rdr2.Read())
                            {

                                PagesModels page = new PagesModels();
                                page.PageName = rdr2["PageName"].ToString();
                                page.Controller = rdr2["Controller"].ToString();
                                page.ActionName = rdr2["ActionName"].ToString();
                                page.Icon = rdr2["Icon"].ToString();
                                page.PageId = rdr2["PageId"].ToString();
                                page.Guid = rdr2["Guid"].ToString();
                                pages.Add(page);
                            }
                                model.ListPages = pages;

                        }
                        modules.Add(model);
                    }
                }
                return modules;
               
            }
            catch (Exception ex)
            {

                return null;

            }
        }
    }
}
