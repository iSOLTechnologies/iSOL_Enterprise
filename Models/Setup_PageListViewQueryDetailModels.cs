using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iSOL_Enterprise.Models
{
    public class Setup_PageListViewQueryDetailModels
    {
        public int? Id { get; set; }
        public int? MasterId { get; set; }
        public string ColumnName { get; set; }
        public string DataType { get; set; }
        public bool? Status { get; set; }
    }
}
