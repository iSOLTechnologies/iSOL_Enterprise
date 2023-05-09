using iSOL_Enterprise.Dal;
using iSOL_Enterprise.Dal.Business;
using iSOL_Enterprise.Dal.Inventory;
using iSOL_Enterprise.Dal.Production;
using iSOL_Enterprise.Dal.Sale;
using iSOL_Enterprise.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace iSOL_Enterprise.Controllers.Production
{
    public class ReceiptFromProductionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ReceiptFromProductionMaster(string id = "")
        {
            ItemMasterDataDal Idal = new ItemMasterDataDal();
            AdministratorDal dal = new AdministratorDal();
            ViewData["Series"] = dal.GetSeries(59);
            ViewData["MySeries"] = dal.GetMySeries(59);
            ViewData["GroupNum"] = new SelectList(Idal.GetListName(), "Value", "Text");
            if (id != "")
            {
                ViewBag.OldId = id;
            }
            else
                ViewBag.OldId = 0;

            return View();
        }
        [HttpGet]
        public IActionResult GetData()
        {
            ResponseModels response = new ResponseModels();
            try
            {

                ReceiptFromProductionDal dal = new ReceiptFromProductionDal();
                response.Data = dal.GetData();
            }
            catch (Exception ex)
            {

                return Json(response);
            }


            return Json(response);
        }
        public IActionResult GetProductionOrders()
        {
            ResponseModels response = new ResponseModels();
            try
            {

                ReceiptFromProductionDal dal = new ReceiptFromProductionDal();
                response.Data = dal.GetProductionOrdersData();
            }
            catch (Exception ex)
            {

                return Json(response);
            }


            return Json(response);
        }

        public IActionResult GetOldData(string guid)
        {
            try
            {
                ReceiptFromProductionDal dal = new ReceiptFromProductionDal();
                int id = dal.GetId(guid);

                return Json(new
                {
                    success = true,
                    HeaderData = dal.GetOldHeaderData(id),
                    RowData = dal.GetOldItemsData(id)
                });

            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpPost]
        public IActionResult AddUpdateReceiptFromProduction(string formData)
        {
            try
            {
                ReceiptFromProductionDal dal = new ReceiptFromProductionDal();
                if (formData != null)
                {

                    ResponseModels response = dal.AddUpdateReceiptFromProduction(formData);
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
