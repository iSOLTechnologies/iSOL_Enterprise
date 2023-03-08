using iSOL_Enterprise.Common;
using iSOL_Enterprise.Dal;
using iSOL_Enterprise.Models;
using iSOL_Enterprise.Models.sale;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SAPbobsCOM;
using SqlHelperExtensions;
using System.Data;
using System.Data.SqlClient;

namespace iSOL_Enterprise.Controllers.Sales
{
    [Authorize]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class GoodReceiptController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GoodReceiptMaster()
        {
            SalesQuotationDal dal = new SalesQuotationDal();
            AdministratorDal Adal = new AdministratorDal();
            ViewBag.GetSeries = Adal.GetSeries(20);
            ViewBag.SalesEmployee = new SelectList(dal.GetSalesEmployee(), "SlpCode", "SlpName");
            return View();
        }

        public IActionResult EditGoodReceiptMaster(int id)
        {
            SalesQuotationDal dal = new SalesQuotationDal();
            CommonDal cdal = new CommonDal();
            GoodReceiptDal dal1 = new GoodReceiptDal();
            ViewBag.SalesEmployee = dal.GetSalesEmployee();
            ViewBag.Taxes = dal.GetVatGroupData();
            ViewBag.Countries = cdal.GetCountries();
            ViewBag.Payments = dal.GetPaymentTerms();
            ViewBag.Currency = cdal.GetCurrencydata();
            bool flag = CommonDal.Check_IsEditable("PCH1", id);
            ViewBag.Status = flag == false ? "Open" : "Closed";
            return View(dal1.GetGoodReceiptEditDetails(id));
        }
        public IActionResult GetPurchaseOrderData(string cardcode)
        {
            ResponseModels response = new ResponseModels();
            try
            {

                GoodReceiptDal dal = new GoodReceiptDal();
                response.Data = dal.GetPurchaseOrderData(cardcode);
            }
            catch (Exception ex)
            {

                return Json(response);
            }


            return Json(response);
        }
        [HttpGet]

        public IActionResult GetOrderItemService(int DocId)
        {
            try
            {
                GoodReceiptDal dal = new GoodReceiptDal();

                return Json(new { baseDoc = dal.GetOrderType(DocId), list = dal.GetOrderItemServiceList(DocId) });
            }
            catch (Exception)
            {
                return Json("");
                throw;
            }

        }
        //public string getUpdatedDocumentNumberOnLoad()
        //{
        //    DataTable dt = SqlHelper.GetData("select top 1 DocNum From OPDN  order by Id desc");
        //    if (dt.Rows.Count <= 0)
        //    {
        //        return "GR-0001";
        //    }
        //    string UpdateCode = SqlHelper.ExecuteScalar(SqlHelper.defaultDB, CommandType.Text, @"select top 1 DocNum From OPDN  order by Id desc").ToString();
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
            return SqlHelper.getUpdatedDocumentNumberOnLoad("OPDN", "GR");
        }

        public IActionResult GetBatches(string itemno,string whsno)
        {
            ResponseModels response = new ResponseModels();
            try
            {

                GoodReceiptDal dal = new GoodReceiptDal();
                response.Data = dal.GetBatches(itemno,whsno);
            }
            catch (Exception ex)
            {

                return Json(response);
            }


            return Json(response);
        }
        public IActionResult GetData()
        {
            ResponseModels response = new ResponseModels();
            try
            {

                GoodReceiptDal dal = new GoodReceiptDal();
                response.Data = dal.GetData();
            }
            catch (Exception ex)
            {

                return Json(response);
            }


            return Json(response);
        }
        
        [HttpPost]
        public IActionResult AddGoodReceipt(string formData)
        {
            try
            {


                GoodReceiptDal dal = new GoodReceiptDal();
                return formData == null ? Json(new { isInserted = false, message = "Data can't be null !" }) : dal.AddGoodReceipt(formData) == true ? Json(new { isInserted = true , message = "Good Receipt Added Successfully !" }) : Json(new { isInserted = false , message = "An Error occured !" });

            }
            catch (Exception)
            {

                throw;
            }

        }
        [HttpPost]
        public IActionResult EditGoodReceipt(string formData)
        {
            try
            {
                GoodReceiptDal dal = new GoodReceiptDal();
                return formData == null ? Json(new { isInserted = false, message = "Data can't be null !" }) : dal.EditGoodReceipt(formData) == true ? Json(new { isInserted = true , message = "Good Receipt Updated Successfully !" }) : Json(new { isInserted = false , message = "An Error occured !" });
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
