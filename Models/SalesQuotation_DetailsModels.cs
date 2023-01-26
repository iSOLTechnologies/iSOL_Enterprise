﻿namespace iSOL_Enterprise.Models
{
    public class SalesQuotation_DetailsModels : BaseModels
    {
        public int MasterId { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string WhsCode { get; set; }
        public string WhsName { get; set; }
        public double? Quantity { get; set; }
        public double? Price { get; set; }
        public double? LineTotal { get; set; }
    }
}
