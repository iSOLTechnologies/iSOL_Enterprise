namespace iSOL_Enterprise.Models.Approval
{
    public class ApprovalStatusModel
    {
        public bool isApproved { get; set; }
        public bool apprSeen { get; set; }
        public string? IsEdited { get; set; }
        public string? Sap_Ref_No { get; set; }
    }
}
