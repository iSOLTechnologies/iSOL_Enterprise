using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iSOL_Enterprise.Models
{
    public class RegionModels :BaseModels
    {
        public string Guid { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; }
        public bool? RowStatus { get; set; }
    }
}
