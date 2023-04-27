using Microsoft.AspNetCore.Mvc;

namespace iSOL_Enterprise.Controllers.Reports
{
    public class PurchaseTrackingReportV1Controller : Controller
    {
        IConfiguration _configuration;
        public PurchaseTrackingReportV1Controller(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            ViewBag.Url = _configuration["ReportUrl"].ToString();
            return View();
        }
    }
}
