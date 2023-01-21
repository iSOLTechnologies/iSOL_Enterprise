using Microsoft.AspNetCore.Mvc;

namespace iSOL_Enterprise.Controllers
{
    public class SaleOrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult SaleOrderMaster()
        {
            return View();
        }
    }
}
