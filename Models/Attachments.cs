using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iSOL_Enterprise.Models
{
    public class Attachments
    {
        public int id { get; set; }

        public string RefType { get; set; }
        public string RefTypeNo { get; set; }

        public string Path { get; set; }

        public string FileBase64 { get; set; }

        public string FileName { get; set; }
    }
}
