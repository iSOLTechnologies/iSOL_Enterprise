using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iSOL_Enterprise.Models
{
    public class PagesModels :BaseModels
    {
        public string PageId { get; set; }
        public string Guid { get; set; }
        public string SubModuleid { get; set; }
        public string PageName { get; set; }
        public string Description { get; set; }
        public string Controller { get; set; }
        public string Icon { get; set; }
        public string ActionName { get; set; }
        public int? ParentId { get; set; }
        public byte? LevelType { get; set; }
        public bool? IsActive { get; set; }

        public List<PageActivityModels> ListPageActivity { get; set; }
        public List<UserRolePageActivityModels> ListUserRolePageActivity { get; set; }


    }
}
