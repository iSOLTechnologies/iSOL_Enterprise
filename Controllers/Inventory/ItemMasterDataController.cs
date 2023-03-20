using Microsoft.AspNetCore.Mvc;

namespace iSOL_Enterprise.Controllers.Inventory
{
    public class ItemMasterDataController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ItemMasterDataMaster()
        {
            return View();
        }
    }
}
