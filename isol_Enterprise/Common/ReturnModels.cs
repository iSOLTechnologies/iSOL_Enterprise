using System;
using System.Collections.Generic;
using System.Text;

namespace iSOL_Enterprise.Common
{
    public class ReturnModels
    {
        public Status status { get; set; }
        public string Messages { get; set; }
        public string Value { get; set; }
    }
    public class roles
    {
        public bool IsCreate { get; set; }
        public bool IsEdit { get; set; }
        public bool IsDelete { get; set; }
    }
    public class update_HelperModel
    {
        public string Query { get; set; }
        public Dictionary<string, object> parameters { get; set; }
    }


    public enum Status
    {
        Success, Failed
    }
}
