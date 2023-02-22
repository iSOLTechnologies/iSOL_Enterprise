namespace iSOL_Enterprise.Models.Sale
{
    public class tbl_pages
    {
        public int SerialNo { get; set; }
        public int ObjectCode { get; set; }
        public string? PageName { get; set; }
        public List<tbl_series>? Series { get; set; }
    }
}
