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


                List<ModulesModels> list = new List<ModulesModels>();


                DataTable dt = SqlHelper.GetData(@"select m.name,m.icon,m.ModuleId from modules m
inner join SubModules sm on sm.ModuleId=m.ModuleId
inner join Pages p on p.SubModuleid=sm.SubModuleId
inner join UserRoles ur on ur.PageId=p.PageId and ur.RoleCode='" + RoleCode + @"'
 where m.isActive=1 
 group by m.name,m.ModuleId,m.Icon,m.lineindex order by m.lineindex");

                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ModulesModels modules = new ModulesModels();
                        modules.Name = dt.Rows[i]["Name"].ToString();
                        modules.ModuleId = dt.Rows[i]["Moduleid"].ToString();
                        modules.Icon = dt.Rows[i]["Icon"].ToString();

                        modules.ListSubModules = new List<SubModulesModels>();

                        DataTable dtSubModule = SqlHelper.GetData(@"select sm.name,sm.icon,sm.SubModuleId from Modules m  
inner join SubModules sm on m.ModuleId = sm.ModuleId
inner join Pages p on p.SubModuleid=sm.SubModuleId
inner join UserRoles ur on ur.pageid=p.PageId and ur.RoleCode='" + RoleCode + @"'
  where sm.isActive=1 and m.isActive=1 and m.Moduleid='" + modules.ModuleId +
      "'group by sm.name,sm.icon,sm.SubModuleId,m.lineindex order by m.lineindex");
                        for (int x = 0; x < dtSubModule.Rows.Count; x++)
                        {
                            SubModulesModels _SubModulesModel = new SubModulesModels();
                            //pages.indexid = Convert.ToInt32(dtpages.Rows[k]["indexid"]);
                            _SubModulesModel.Name = dtSubModule.Rows[x]["Name"].ToString();
                            _SubModulesModel.Icon = dtSubModule.Rows[x]["Icon"].ToString();
                            _SubModulesModel.SubModuleId = dtSubModule.Rows[x]["SubModuleId"].ToString();
                            _SubModulesModel.ListPages = new List<PagesModels>();

                            DataTable dtpages = SqlHelper.GetData(@"select p.PageId,p.Guid,p.PageName,p.Controller,p.ActionName,p.SubModuleid,p.Icon from pages p
                                                                    inner join SubModules sm on p.SubModuleid = sm.SubModuleId
																	inner join UserRoles ur on ur.PageId=p.PageId and ur.IsActive=1
																	inner join Users u on u.RoleCode=ur.RoleCode and u.IsActive=1 and u.RowStatus=1 and ur.RoleCode=u.RoleCode
                                                                    where p.isActive=1 and p.Rowstatus=1 and u.RoleCode='" + RoleCode + "' and sm.SubModuleId='" + _SubModulesModel.SubModuleId + @"'
                                                                    group by p.PageId,p.Guid,p.PageName,p.Controller,p.ActionName,p.SubModuleid,p.Icon,p.lineIndex order by p.lineindex");
                            for (int k = 0; k < dtpages.Rows.Count; k++)
                            {
                                PagesModels pages = new PagesModels();
                                pages.PageName = dtpages.Rows[k]["PageName"].ToString();
                                pages.Controller = dtpages.Rows[k]["Controller"].ToString();
                                pages.ActionName = dtpages.Rows[k]["ActionName"].ToString();
                                pages.SubModuleid = dtpages.Rows[k]["SubModuleid"].ToString();
                                pages.Icon = dtpages.Rows[k]["Icon"].ToString();
                                pages.PageId = dtpages.Rows[k]["PageId"].ToString();
                                pages.Guid = dtpages.Rows[k]["Guid"].ToString();
                                _SubModulesModel.ListPages.Add(pages);
                            }

                            modules.ListSubModules.Add(_SubModulesModel);

                        }

                        list.Add(modules);
                    }

                }


                //sdfl kjfghdfisu fgdfyufguyds fgdiusfhsiufghdfiug

                return list;
            }
            catch (Exception ex)
            {

                return null;

            }
        }
    }
}
