using iSOL_Enterprise.Dal.Inventory;
using iSOL_Enterprise.Models;
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
            ItemMasterDataDal dal = new ItemMasterDataDal();
            ViewData["properties"] = dal.GetProperties();
            return View();
        }
        public IActionResult GetWareHouseList()
        {

            ResponseModels response = new ResponseModels();
            try
            {

                ItemMasterDataDal dal = new ItemMasterDataDal();
                response.Data = dal.GetWareHouseList();
            }
            catch (Exception ex)
            {

                return Json(response);
            }


            return Json(response);
        }
    }
}
