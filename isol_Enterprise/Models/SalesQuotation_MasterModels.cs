﻿namespace iSOL_Enterprise.Models
{
    public class SalesQuotation_MasterModels : BaseModels
    {

        public string? Guid { get; set; }
        public string? DocNum { get; set; }
        public string? DocType { get; set; }
        public DateTime? DocDate { get; set; }
        public DateTime? PostingDate { get; set; }
        public string? CardCode { get; set; }
        public string? CardName { get; set; }
        public string? Warehouse { get; set; }
        public string? DocStatus { get; set; }
        public string? IsPosted { get; set; }
        public string? IsEdited { get; set; }
        public decimal? Quanity { get; set; }
        public List<SalesQuotation_DetailsModels> ListSalesQuotationDetails { get; set; }
    }
}