using Microsoft.AspNetCore.Mvc;

namespace iSOL_Enterprise.Controllers.Reports
{
    public class PurchaseTrackingReportV1Controller : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
