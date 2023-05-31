namespace iSOL_Enterprise.Models.Production
{
	public class ProductionOrderRowMaterialModel
	{

		public string? DocNum { get; set; }
		public string? ProductionOrderType { get; set; }
		public string? OriginNum { get; set; }
		public decimal ExtraQty { get; set; }
		public string? ItemCode { get; set; }
		public string? ItemName { get; set; }
		public string? wareHouse { get; set; }
		public int? Sap_Ref_No { get; set; }
		public decimal PlannedQty { get; set; }
		public decimal IssuedQty { get; set; }
		public int RowNum { get; set; }
	}
}
