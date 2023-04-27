using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace iSOL_Enterprise.Models
{
    public class ModulesModels : _ModulesModels
    {
        [ScaffoldColumn(false)]
        [DataObjectFieldAttribute(true, true, false)]
        public int? id { get; set; }
        public string ModuleId { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public int lineIndex { get; set; }
        public bool isactive { get; set; }

    }
    public class _ModulesModels
    {
        public List<ModulesModels> ListModules { get; set; }
        public List<SubModulesModels> ListSubModules { get; set; }
    }
}
