using iSOL_Enterprise.Models.Approval;

namespace iSOL_Enterprise.Models.ChartOfAccount
{
    public class ChartOfAccountMasterModel : ApprovalStatusModel
    {
        public int DocEntry { get;set; }
        public string AcctCode { get;set; }
        public string AcctName { get;set; }
        public string FatherNum { get;set; }
        public string AccntntCod { get;set; }
        public string ActCurr { get;set; }
        public int Levels { get;set; }
        public string CurrTotal { get;set; }
    }
}
