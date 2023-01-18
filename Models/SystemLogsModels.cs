using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iSOL_Enterprise.Models
{
    public class SystemLogsModels : BaseModels
    {
        public string Guid { get; set; }
        public int? UserId { get; set; }
        public string ControllerName { get; set; }
        public string MsgLog { get; set; }
        public string IpAddress { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string Location { get; set; }
        public bool? RowStatus { get; set; }

    }
}
