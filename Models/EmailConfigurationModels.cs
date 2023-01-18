using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iSOL_Enterprise.Models
{
    public class EmailConfigurationModels : BaseModels
    {
        public string EmailFrom { get; set; }
        public string Password { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string SMTP { get; set; }
        public int Port { get; set; }
        public bool SSL { get; set; }
        public string BaseURL { get; set; }
        public string Type { get; set; }
        public bool? IsActice { get; set; }
        public bool? RowStatus { get; set; }
    }
}
