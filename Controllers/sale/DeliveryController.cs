using Microsoft.AspNetCore.Mvc;

namespace iSOL_Enterprise.Controllers
{
    public class DeliveryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult DeliveryMaster()
        {
            return View();
        }
    }
}
