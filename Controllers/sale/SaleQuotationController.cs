using Microsoft.AspNetCore.Mvc;

namespace iSOL_Enterprise.Controllers.Sales
{
    public class SaleQuotationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult SaleQuotationMaster()
        {
            return View();
        }
    }
}
