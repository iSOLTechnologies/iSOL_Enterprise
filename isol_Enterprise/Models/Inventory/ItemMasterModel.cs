namespace iSOL_Enterprise.Models.Inventory
{
    public class ItemMasterModel : BaseModels
    {
        public string? Guid { get; set; }
        public string? ItemCode { get; set; }
        public string? ItemName { get; set; }
        public string ? SalesItem { get;set ;}
        public string ? PurchaseItem { get;set ;}
        public string ? InventoryItem { get; set;  }
        public string? DocStatus { get; set; }
        public string? IsPosted { get; set; }
        public string? IsEdited { get; set; }
        public bool isApproved { get; set; }
        public bool apprSeen { get; set; }
    }
}
