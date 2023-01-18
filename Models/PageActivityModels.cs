using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iSOL_Enterprise.Models
{
    public class PageActivityModels
    {
        public int Id { get; set; }
        public string PageId { get; set; }
        public string RoleActivityCode { get; set; }
        public string RoleActivityTypeName { get; set; }
        public string Icon { get; set; }
        public string Class { get; set; }
        public bool? IsActive { get; set; }
    }
}
