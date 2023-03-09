namespace iSOL_Enterprise.Models.sale
{
    public class planningSheetModel
    {

        public int docEntry { get; set; }

        public DateTime ? u_PlanDate { get; set; }
        public DateTime ? u_SODate { get; set; }
        public DateTime ? u_ShipDate { get; set; }
        public string ? u_SOnum { get; set; }
        public string ? u_ItemCode { get; set; }
        public string ? status { get; set; }
        public string? IsPosted { get; set; }
    }
}
