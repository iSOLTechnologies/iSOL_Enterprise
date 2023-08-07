using iSOL_Enterprise.Dal;
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
    public class DeliveryController : Controller
    {
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult DeliveryMaster(string DocId = "", int BaseType = 0)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetInt32("UserId").ToString()) || string.IsNullOrEmpty(HttpContext.Request.Query["Source"]) || !CommonDal.CheckPage(HttpContext.Request.Query["Source"], HttpContext.Session.GetInt32("UserId")))
            {
                return RedirectToAction("index", "Home");
            }
            SalesQuotationDal dal = new SalesQuotationDal();
			AdministratorDal Adal = new AdministratorDal();
			ViewBag.GetSeries = Adal.GetSeries(15);
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

        public IActionResult EditDeliveryMaster(int id, int aprv1ghas = 0)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetInt32("UserId").ToString()) || string.IsNullOrEmpty(HttpContext.Request.Query["Source"]) || !CommonDal.CheckPage(HttpContext.Request.Query["Source"], HttpContext.Session.GetInt32("UserId")))
            {
                return RedirectToAction("index", "Home");
            }
            DeliveryDal dal1 = new DeliveryDal();
            SalesQuotationDal dal = new SalesQuotationDal();
            CommonDal cdal = new CommonDal();
            AdministratorDal Adal = new AdministratorDal();
            ViewBag.GetSeries = Adal.GetSeries(15);
            ViewBag.Warehouse = dal1.GetWareHouseData();
            ViewBag.SalesEmployee = dal.GetSalesEmployee();
            ViewBag.Taxes = dal.GetVatGroupData("S");
            ViewBag.Countries = cdal.GetCountries();
            ViewBag.Payments = dal.GetPaymentTerms();
            ViewBag.Currency = cdal.GetCurrencydata();
            ViewBag.SaleOrderList = cdal.GetSaleOrders();
            ViewBag.ApprovalView = aprv1ghas;
            bool flag = CommonDal.Check_IsNotEditable("DLN1", id);
            ViewBag.Status = flag == false && aprv1ghas == 0 ? "Open" : "Closed";
            return View(dal1.GetDeliveryDetails(id));
        }
        public IActionResult GetBaseDocData(string cardcode,int BaseType)
        {
            ResponseModels response = new ResponseModels();
            try
            {

                CommonDal dal = new CommonDal();
                response.Data = dal.GetBaseDocData(cardcode,BaseType);
            }
            catch (Exception ex)
            {

                return Json(response);
            }


            return Json(response);
        }
        //[HttpGet]

        //public IActionResult GetBaseDocItemService(int DocId,int BaseType)
        //{
        //    try
        //    {
        //        CommonDal dal = new CommonDal();

        //        return Json(new { baseDoc = dal.GetBaseDocType(DocId,BaseType), list = dal.GetBaseDocItemServiceList(DocId,BaseType) });
        //    }
        //    catch (Exception)
        //    {
        //        return Json("");
        //        throw;
        //    }

        //}
        [HttpGet]
        public IActionResult GetBatchList(string itemcode  , string warehouse)
        {
            try
            {
                DeliveryDal dal = new DeliveryDal();

                return Json(new { data = dal.GetBatchList(itemcode , warehouse) });
            }
            catch (Exception)
            {
                return Json("");
                throw;
            }
        }
        public IActionResult GetWareHouseData()
        {
            try
            {

                DeliveryDal dal = new DeliveryDal();

                return Json(dal.GetWareHouseData());
            }
            catch (Exception ex)
            {

                return Json(ex.Message);
            }
        }
        //public string getUpdatedDocumentNumberOnLoad()
        //{
        //    DataTable dt = SqlHelper.GetData("select top 1 DocNum From ODLN  order by Id desc");
        //    if (dt.Rows.Count <= 0)
        //    {
        //        return "DV-0001";
        //    }
        //    string UpdateCode = SqlHelper.ExecuteScalar(SqlHelper.defaultDB, CommandType.Text, @"select top 1 DocNum From ODLN  order by Id desc").ToString();
        //    // DataTable dt1 = SqlHelper.GetData("select top 1 ItemCode From[dbo].[Item_Master]  order by Id desc");
        //    //   string data = dt.Rows[1]["ItemCode"].ToString();
        //    string[] str = UpdateCode.Split('-');
        //    string lastItem = str[str.Length - 1];
        //    int no = int.Parse(lastItem);
        //    no = no + 1;
        //    string code = str[0];
        //    string a = string.Format("{0:D" + lastItem.Length + "}", no);
        //    return code + "-" + a;
        //}
        public string getUpdatedDocumentNumberOnLoad()
        {
            return SqlHelper.getUpdatedDocumentNumberOnLoad("ODLN", "DV");
        }
        public IActionResult GetDeliveryData()
        {
            ResponseModels response = new ResponseModels();
            try
            {

                DeliveryDal dal = new DeliveryDal();
                response.Data = dal.GetDeliveryData();
            }
            catch (Exception ex)
            {

                return Json(response);
            }


            return Json(response);
        }
        [HttpPost]
        public IActionResult AddDelivery(string formData)
        {
            try
            { 
                DeliveryDal dal = new DeliveryDal();
                return formData == null ? Json(new { isInserted = false, message = "Data can't be null !" }) : dal.AddDelivery(formData) == true ? Json(new { isInserted = true, message = "Delivery Added Successfully !" }) : Json(new { isInserted = false, message = "An Error occured !" });

            }
            catch (Exception)
            {

                throw;
            }

        }
        [HttpPost]
        public IActionResult EditDelivery(string formData)
        {
            try
            {


                DeliveryDal dal = new DeliveryDal();
                return formData == null ? Json(new { isInserted = false, message = "Data can't be null !" }) : dal.EditDelivery(formData) == true ? Json(new { isInserted = true, message = "Delivery Updated Successfully !" }) : Json(new { isInserted = false, message = "An Error occured !" });

            }
            catch (Exception)
            {

                throw;
            }

        }

    }
}
