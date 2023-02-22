using iSOL_Enterprise.Dal;
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
        public IActionResult DeliveryMaster()
        {
            SalesQuotationDal dal = new SalesQuotationDal();

            ViewBag.SalesEmployee = new SelectList(dal.GetSalesEmployee(), "SlpCode", "SlpName");
            //ViewBag.SalesEmployee = new SelectList(dal.GetSalesEmployee(), "SlpCode", "SlpName");
            return View();
        }

        public IActionResult EditDeliveryMaster(int id)
        {
            DeliveryDal dal1 = new DeliveryDal();
            SalesQuotationDal dal = new SalesQuotationDal();
            ViewBag.SalesEmployee = dal.GetSalesEmployee();
            ViewBag.Taxes = dal.GetVatGroupData();
            ViewBag.Countries = dal.GetCountries();
            ViewBag.Payments = dal.GetPaymentTerms();
            bool flag = CommonDal.Check_IsEditable("INV1", id);
            ViewBag.Status = flag == false ? "Open" : "Closed";
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
