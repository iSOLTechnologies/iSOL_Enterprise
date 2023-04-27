using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iSOL_Enterprise.Models
{
    public class UserRolePageActivityModels
    {
        public int Id { get; set; }
        public string PageId { get; set; }
        public string RoleActivityCode { get; set; }
        public string Guid { get; set; }
        public bool? Status { get; set; }
    }
}
