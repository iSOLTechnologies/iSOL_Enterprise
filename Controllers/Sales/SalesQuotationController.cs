using Microsoft.AspNetCore.Mvc;

namespace iSOL_Enterprise.Controllers.Sales
{
    public class SalesQuotationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult SalesQuotationMaster()
        {
            return View();
        }
    }
}
