namespace iSOL_Enterprise.Models.Logs
{
    public class OITL
    {

        public int LogEntry { get; set; }
        public int ID { get; set; }
        public int DocLine { get; set; }
        public int DocType { get; set; }
        public dynamic BaseType { get; set; }
        public decimal Quantity { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public DateTime DocDate { get; set; }
    }
}
