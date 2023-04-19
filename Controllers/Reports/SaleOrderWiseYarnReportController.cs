using Microsoft.AspNetCore.Mvc;

namespace iSOL_Enterprise.Controllers.Reports
{
    public class SaleOrderWiseYarnReportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
