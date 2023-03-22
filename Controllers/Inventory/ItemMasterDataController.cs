using iSOL_Enterprise.Dal;
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
            AdministratorDal addal = new AdministratorDal();
            
            ViewData["properties"] = dal.GetProperties();
            ViewData["Series"] = addal.GetSeries(4);
            ViewData["ItemGroup"] = dal.GetItemsGroup();
            ViewData["ListName"] = dal.GetListName();
            ViewData["Manufacturer"] = dal.GetManufacturer();
            ViewData["Shiptype"] = dal.GetShipType();
            ViewData["CustomsGroup"] = dal.GetCustomsGroup();
            ViewData["TaxGroup"] = dal.GetTaxGroup();


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
