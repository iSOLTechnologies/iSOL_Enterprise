using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReportManager.Models
{
    public class Document_MasterModel
    {
        public int DocEntry { get; set; }
        public int DocNum { get; set; }
        public string Path { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string DocType { get; set; } = "PDF";
       
    }
}