using iSOL_Enterprise.Dal;
using iSOL_Enterprise.Dal.Purchase;
using iSOL_Enterprise.Dal.Sale;
using iSOL_Enterprise.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SqlHelperExtensions;
using System.Data;

namespace iSOL_Enterprise.Controllers
{
    [Authorize]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class GoodsReturnController : Controller
    {
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GoodsReturnMaster(string DocId = "", int BaseType = 0)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetInt32("UserId").ToString()) || string.IsNullOrEmpty(HttpContext.Request.Query["Source"]) || !CommonDal.CheckPage(HttpContext.Request.Query["Source"], HttpContext.Session.GetInt32("UserId")))
            {
                return RedirectToAction("index", "Home");
            }
            SalesQuotationDal dal = new SalesQuotationDal();
            AdministratorDal Adal = new AdministratorDal();
            ViewBag.GetSeries = Adal.GetSeries(21);
            if (DocId != "" && BaseType != 0)
            {
                ViewBag.DocId = DocId;
                ViewBag.BaseType = BaseType;
            }
            else
            {
                ViewBag.DocId = 0;
                ViewBag.BaseType = 0;
            }
            ViewBag.SalesEmployee = new SelectList(dal.GetSalesEmployee(), "SlpCode", "SlpName",-1);
            //ViewBag.SalesEmployee = new SelectList(dal.GetSalesEmployee(), "SlpCode", "SlpName",-1);
            return View();
        }

        public IActionResult EditGoodsReturnMaster(int id, int aprv1ghas = 0)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetInt32("UserId").ToString()) || string.IsNullOrEmpty(HttpContext.Request.Query["Source"]) || !CommonDal.CheckPage(HttpContext.Request.Query["Source"], HttpContext.Session.GetInt32("UserId")))
            {
                return RedirectToAction("index", "Home");
            }
            GoodsReturnDal dal1 = new GoodsReturnDal();
            SalesQuotationDal dal = new SalesQuotationDal();
            CommonDal cdal = new CommonDal();
            DeliveryDal Ddal = new DeliveryDal();
            AdministratorDal Adal = new AdministratorDal();
            ViewBag.GetSeries = Adal.GetSeries(21);
            ViewBag.Warehouse = Ddal.GetWareHouseData();
            ViewBag.SalesEmployee = dal.GetSalesEmployee();
            ViewBag.Taxes = dal.GetVatGroupData("P");
            ViewBag.Countries = cdal.GetCountries();
            ViewBag.Payments = dal.GetPaymentTerms();
            ViewBag.Currency = cdal.GetCurrencydata();
            ViewBag.SaleOrderList = cdal.GetSaleOrders();
            ViewBag.ApprovalView = aprv1ghas;
            bool flag = CommonDal.Check_IsNotEditable("RPD1", id);
            ViewBag.Status = flag == false && aprv1ghas == 0 ? "Open" : "Closed";
            //ViewBag.Status = "Open" ;
            return View(dal1.GetGoodsReturnDetails(id));
        }
       
        public string getUpdatedDocumentNumberOnLoad()
        {
            return SqlHelper.getUpdatedDocumentNumberOnLoad("ORPD", "GRT");
        }
        public IActionResult GetGoodsReturnData()
        {
            ResponseModels response = new ResponseModels();
            try
            {

                GoodsReturnDal dal = new GoodsReturnDal();
                response.Data = dal.GetGoodsReturnData();
            }
            catch (Exception ex)
            {

                return Json(response);
            }


            return Json(response);
        }
        [HttpPost]
        public IActionResult AddGoodsReturn(string formData)
        {
            try
            {
                GoodsReturnDal dal = new GoodsReturnDal();
                return formData == null ? Json(new { isInserted = false, message = "Data can't be null !" }) : dal.AddGoodsReturn(formData) == true ? Json(new { isInserted = true, message = "Goods Return Added Successfully !" }) : Json(new { isInserted = false, message = "An Error occured !" });

            }
            catch (Exception)
            {

                throw;
            }

        }
        [HttpPost]
        public IActionResult EditGoodsReturn(string formData)
        {
            try
            {


                GoodsReturnDal dal = new GoodsReturnDal();
                return formData == null ? Json(new { isInserted = false, message = "Data can't be null !" }) : dal.EditGoodsReturn(formData) == true ? Json(new { isInserted = true, message = "Goods Return Updated Successfully !" }) : Json(new { isInserted = false, message = "An Error occured !" });

            }
            catch (Exception)
            {

                throw;
            }

        }

    }
}
