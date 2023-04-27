using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace iSOL_Enterprise.Models
{
    public class Setup_CompanyProfileModels
    {
        [ScaffoldColumn(false)]
        [DataObjectFieldAttribute(true, true, false)]
        public int id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Currency { get; set; }
        public string Contact { get; set; }
        public string NTN { get; set; }
        public string Region { get; set; }
        public string Legalname { get; set; }
        public string Website { get; set; }


    }
}
