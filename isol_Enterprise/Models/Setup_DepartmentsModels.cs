using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iSOL_Enterprise.Models
{
    public class Setup_DepartmentsModels : BaseModels
    {
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public bool? Status { get; set; }
        public bool? RowStatus { get; set; }

    }
}
