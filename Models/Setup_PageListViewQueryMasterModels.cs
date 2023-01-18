using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace iSOL_Enterprise.Models
{
    public class Setup_PageListViewQueryMasterModels
    {
        public int Id { get; set; }
        public string PageId { get; set; }
        public string Query { get; set; }
        public bool? Status { get; set; }
    }
    
    public class Setup_PageListViewQueryMasterViewModels : Setup_PageListViewQueryMasterModels
    {
        public DataTable Data { get; set; }
    }
       
}
