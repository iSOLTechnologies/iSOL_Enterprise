using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iSOL_Enterprise.Models
{
    public class RoleModels : BaseModels
    {
        public int Id { get; set; }
        public string Guid { get; set; }
        public string RoleCode { get; set; }
        public string CreatedBy { get; set; }
        public string RoleName { get; set; }
        public bool? IsActive { get; set; }
        public bool? RowStatus { get; set; }
        public dynamic Permissions { get; set; }
        public List<PagesModels> ListPages { get; set; }
        public List<_usersModels> ListUsers { get; set; }
    }
}
