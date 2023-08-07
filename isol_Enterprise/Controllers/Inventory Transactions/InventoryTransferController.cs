using iSOL_Enterprise.Dal.Inventory;
using iSOL_Enterprise.Dal.Inventory_Transactions;
using iSOL_Enterprise.Dal;
using iSOL_Enterprise.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using iSOL_Enterprise.Dal.Sale;

namespace iSOL_Enterprise.Controllers.Inventory_Transactions
{
    public class InventoryTransferController : Controller
    {
        IConfiguration _configuration;
        public InventoryTransferController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            ViewBag.Url = _configuration["ReportUrl"].ToString();
            return View();
        }

        public IActionResult GetData()
        {
            ResponseModels response = new ResponseModels();
            try
            {

                InventoryTransferDal dal = new InventoryTransferDal();
                response.Data = dal.GetData();
            }
            catch (Exception ex)
            {

                return Json(response);
            }


            return Json(response);
        }
        public IActionResult InventoryTransferMaster(string id = "")
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetInt32("UserId").ToString()) || string.IsNullOrEmpty(HttpContext.Request.Query["Source"]) || !CommonDal.CheckPage(HttpContext.Request.Query["Source"], HttpContext.Session.GetInt32("UserId")))
            {
                return RedirectToAction("index", "Home");
            }
            ItemMasterDataDal Idal = new ItemMasterDataDal();
            AdministratorDal dal = new AdministratorDal();
            SalesQuotationDal Sdal = new SalesQuotationDal();
            ViewData["Series"] = dal.GetSeries(67);
            ViewData["MySeries"] = dal.GetMySeries(67);
            ViewData["SalesEmployee"] = new SelectList(Sdal.GetSalesEmployee(), "SlpCode", "SlpName");
            ViewData["GroupNum"] = new SelectList(Idal.GetListName(), "Value", "Text");
            if (id != "")
            {
                ViewBag.OldId = id;
            }
            else
                ViewBag.OldId = 0;
            return View();
        }
        public IActionResult GetOldData(string ItemID)
        {
            try
            {
                InventoryTransferDal dal = new InventoryTransferDal();
                int id = dal.GetId(ItemID);
                return Json(new
                {
                    success = true,
                    HeaderData = dal.GetHeaderOldData(id),
                    RowData = dal.GetRowOldData(id)
                });

            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpPost]
        public IActionResult AddInventoryTransfer(string formData)
        {
            try
            {
                InventoryTransferDal dal = new InventoryTransferDal();
                if (formData != null)
                {
                    ResponseModels response = dal.AddInventoryTransfer(formData);
                    return Json(new { isInserted = response.isSuccess, Message = response.Message });
                }
                else
                {
                    return Json(new { isInserted = false, Message = "Data can't be null" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
