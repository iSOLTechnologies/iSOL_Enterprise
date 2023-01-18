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
        public List<PagesModels> getMenu(string RoleCode)
        {
            try
            {


                List<PagesModels> list = new List<PagesModels>();


                DataTable dt = SqlHelper.GetData(@"select p.PageName,p.Controller,p.ActionName,p.icon,p.PageId,p.Guid from Pages p
inner join UserRoles ur on ur.PageId=p.PageId and ur.RoleCode='" + RoleCode + @"'
 where p.isActive=1 
order by p.lineindex");

                if (dt != null)
                {
                    for (int k = 0; k < dt.Rows.Count; k++)
                    {
                        PagesModels pages = new PagesModels();
                        pages.PageName = dt.Rows[k]["PageName"].ToString();
                        pages.Controller = dt.Rows[k]["Controller"].ToString();
                        pages.ActionName = dt.Rows[k]["ActionName"].ToString();
                        pages.Icon = dt.Rows[k]["Icon"].ToString();
                        pages.PageId = dt.Rows[k]["PageId"].ToString();
                        pages.Guid = dt.Rows[k]["Guid"].ToString();
                        list.Add(pages);
                    }

                }

                return list;
            }
            catch (Exception ex)
            {

                return null;

            }
        }
    }
}
