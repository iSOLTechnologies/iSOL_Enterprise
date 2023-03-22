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
        public IActionResult GetNewItemCode(int Series)
        {
            try
            {
                ItemMasterDataDal dal = new ItemMasterDataDal();
                ResponseModels response = new ResponseModels();
                return Json(new { Success = true, ItemCode = dal.GetNewItemCode(Series) });

            }
            catch (Exception)
            {

                throw;
            }
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
        [HttpPost]
        public IActionResult AddItemMasterData(string formData)
        {
            try
            {
                ItemMasterDataDal dal = new ItemMasterDataDal();
                return formData == null ? Json(new { isInserted = false, message = "Data can't be null !" }) : dal.AddItemMasterData(formData) == true ? Json(new { isInserted = true, message = "Item Added Successfully !" }) : Json(new { isInserted = false, message = "An Error occured !" });
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
