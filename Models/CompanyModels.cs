using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iSOL_Enterprise.Models
{
    public class CompanyModels :BaseModels
    {
        public string Guid { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string ContactPerson { get; set; }
        public string ContactNo { get; set; }
        public string Source { get; set; }
        public decimal? InterestRate { get; set; }
        public decimal? OutStandingPayments { get; set; }
        public decimal? CustomerCollection { get; set; }
        public bool? IsActive { get; set; }
        public bool? RowStatus { get; set; }
    }
}
