using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iSOL_Enterprise.Models
{
    public class SubModulesModels
    {
        public int? id { get; set; }
        public string ModuleId { get; set; }
        public string SubModuleId { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public int lineIndex { get; set; }
        public bool isactive { get; set; }
        public List<PagesModels> ListPages { get; set; }

    }
}
