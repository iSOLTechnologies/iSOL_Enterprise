using Microsoft.AspNetCore.Mvc.Rendering;

namespace iSOL_Enterprise.Models.Sale
{
    public class tbl_pages
    {
        public int SerialNo { get; set; }
        public int ObjectCode { get; set; }
        public int PageSeries { get; set; }
        public string? PageName { get; set; }
        public bool? Approve { get; set; }
        public List<tbl_series>? Series { get; set; }

        public List<ListModel> ApprovalTypes { get; } = new() {
            new()
            {
                Value = 0,
                Text = "No",

            },
            new()
            {
                Value = 1,
                Text = "Yes",

            }
        } ; 
    }
}
