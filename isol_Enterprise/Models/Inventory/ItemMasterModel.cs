namespace iSOL_Enterprise.Models.Inventory
{
    public class ItemMasterModel : BaseModels
    {
        public string? Guid { get; set; }
        public string? ItemCode { get; set; }
        public string? ItemName { get; set; }
        public string? InventoryUom { get; set; }
        public string? WhsCode { get; set; }
        public string? WarehouseName { get; set; }
        public string ? SalesItem { get;set ;}
        public string ? PurchaseItem { get;set ;}
        public string ? InventoryItem { get; set;  }
        public string? DocStatus { get; set; }
        public string? IsPosted { get; set; }
        public string? IsEdited { get; set; }
        public bool isApproved { get; set; }
        public bool apprSeen { get; set; }

        public decimal onHand { get;set; }
        public decimal isCommitted { get;set; }
        public decimal OnOrder { get;set; }
        public decimal Available { get;set; }
        public decimal LastPurPrc { get;set; }
        public decimal Total { get;set; }
    }
}
