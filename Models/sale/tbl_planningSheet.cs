using iSOL_Enterprise.Common;
using Newtonsoft.Json;
using System;

namespace iSOL_Enterprise.Models.Sale
{
    public class tbl_planningSheet
    {
        public string? Key;
        public dynamic? Value;
        
        public tbl_planningSheet(string Key, DateTime? Value) 
        { 
            this.Key= Key;
            this.Value = Value?.ToString("yyyy-MM-dd"); 
                JsonConvert.SerializeObject(Value);
            
        }
        public tbl_planningSheet(string Key, int? Value)
        {
            this.Key = Key;
            this.Value = Value;

        }
        public tbl_planningSheet(string Key, string? Value)
        {
            this.Key = Key;
            this.Value = Value;

        }
    }
}
