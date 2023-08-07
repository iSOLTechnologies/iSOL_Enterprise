using iSOL_Enterprise.Dal;
using iSOL_Enterprise.Dal.Business;
using iSOL_Enterprise.Dal.Inventory;
using iSOL_Enterprise.Dal.Production;
using iSOL_Enterprise.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace iSOL_Enterprise.Controllers.Production
{
    public class ProductionOrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetData()
        {
            ResponseModels response = new ResponseModels();
            try
            {

                ProductionOrderDal dal = new ProductionOrderDal();
                response.Data = dal.GetData();
            }
            catch (Exception ex)
            {

                return Json(response);
            }


            return Json(response);
        }
        public IActionResult ProductionOrderMaster(string id = "", int aprv1ghas = 0)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetInt32("UserId").ToString()) || string.IsNullOrEmpty(HttpContext.Request.Query["Source"]) || !CommonDal.CheckPage(HttpContext.Request.Query["Source"], HttpContext.Session.GetInt32("UserId")))
            {
                return RedirectToAction("index", "Home");
            }

            AdministratorDal addal = new AdministratorDal();
            ItemMasterDataDal Idal = new ItemMasterDataDal();
            BusinessPartnerMasterDataDal Bdal = new BusinessPartnerMasterDataDal();
            ViewData["GroupNum"] = new SelectList(Idal.GetListName(), "Value", "Text");
            ViewData["ProjectCode"] = new SelectList(Bdal.GetProjectCodes(), "Value", "Text");
            ViewData["Series"] = addal.GetSeries(202);
            ViewData["MySeries"] = addal.GetMySeries(202);
            ViewBag.ApprovalView = aprv1ghas;
            if (id != "")
            {
                ViewBag.OldId = id;
            }
            else
                ViewBag.OldId = 0;

            return View();
        }
        public IActionResult GetOldData(string guid)
        {
            try
            {
                ProductionOrderDal dal = new ProductionOrderDal();
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
        public IActionResult AddUpdateProductionOrder(string formData)
        {
            try
            {
                ProductionOrderDal dal = new ProductionOrderDal();
                if (formData != null)
                {

                    ResponseModels response = dal.AddUpdateProductionOrder(formData);
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
