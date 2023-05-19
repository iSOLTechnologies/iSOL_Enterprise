namespace iSOL_Enterprise.Models
{
    public class ApprovalModel
    {

        public int Id { get; set; }
        public string? Guid { get; set; }
        public int ObjectCode { get; set; }
        public int DocEntry { get; set; }
        public string? DocNum { get; set; }
        public string? RequestedBy { get; set; }
        public string? PageName { get; set; }
        public DateTime? Date { get; set; }
        public bool Status { get; set; }
        public bool Seen { get; set; }

    }
}
