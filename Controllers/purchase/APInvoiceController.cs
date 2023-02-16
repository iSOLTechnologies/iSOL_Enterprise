using iSOL_Enterprise.Common;
using iSOL_Enterprise.Dal;
using iSOL_Enterprise.Models;
using iSOL_Enterprise.Models.sale;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SqlHelperExtensions;
using System.Data;
using System.Data.SqlClient;

namespace iSOL_Enterprise.Controllers.Sales
{
    [Authorize]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class APInvoiceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult APInvoiceMaster()
        {

            SalesQuotationDal dal = new SalesQuotationDal();

            ViewBag.SalesEmployee = new SelectList(dal.GetSalesEmployee(), "SlpCode", "SlpName");



            return View();
        }

        public IActionResult EditAPInvoiceMaster(int id)
        {
            SalesQuotationDal dal = new SalesQuotationDal();
            APInvoiceDal dal1 = new APInvoiceDal();
            ViewBag.SalesEmployee = dal.GetSalesEmployee();
            ViewBag.Taxes = dal.GetVatGroupData();
            ViewBag.Countries = dal.GetCountries();
            ViewBag.Payments = dal.GetPaymentTerms();
 
            ViewBag.Status =   "Open" ;
            return View(dal1.GetAPInvoiceEditDetails(id));
        }
        public IActionResult GetGoodReceiptData(string cardcode)
        {
            ResponseModels response = new ResponseModels();
            try
            {

                APInvoiceDal dal = new APInvoiceDal();
                response.Data = dal.GetGoodReceiptData(cardcode);
            }
            catch (Exception ex)
            {

                return Json(response);
            }


            return Json(response);
        }
        [HttpGet]

        public IActionResult GetGoodReceiptItemService(int DocId)
        {
            try
            {
                APInvoiceDal dal = new APInvoiceDal();

                return Json(new { baseDoc = dal.GetGoodReceiptType(DocId), list = dal.GetGoodReceiptItemServiceList(DocId) });
            }
            catch (Exception)
            {
                return Json("");
                throw;
            }

        }
        //public string getUpdatedDocumentNumberOnLoad()
        //{
        //    DataTable dt = SqlHelper.GetData("select top 1 DocNum From OPCH  order by Id desc");
        //    if (dt.Rows.Count <= 0)
        //    {
        //        return "API-0001";
        //    }
        //    string UpdateCode = SqlHelper.ExecuteScalar(SqlHelper.defaultDB, CommandType.Text, @"select top 1 DocNum From OPCH  order by Id desc").ToString();
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
            return SqlHelper.getUpdatedDocumentNumberOnLoad("OPCH", "API");
        }

        public IActionResult GetData()
        {
            ResponseModels response = new ResponseModels();
            try
            {

                APInvoiceDal dal = new APInvoiceDal();
                response.Data = dal.GetData();
            }
            catch (Exception ex)
            {

                return Json(response);
            }


            return Json(response);
        }
        
        [HttpPost]
        public IActionResult AddAPInvoice(string formData)
        {
            try
            {


                APInvoiceDal dal = new APInvoiceDal();
                return formData == null ? Json(new { isInserted = false, message = "Data can't be null !" }) : dal.AddAPInvoice(formData) == true ? Json(new { isInserted = true , message = "AP Invoice Added Successfully !" }) : Json(new { isInserted = false , message = "An Error occured !" });

            }
            catch (Exception)
            {

                throw;
            }

        }
        [HttpPost]
        public IActionResult EditAPInvoice(string formData)
        {
            try
            {
                APInvoiceDal dal = new APInvoiceDal();
                return formData == null ? Json(new { isInserted = false, message = "Data can't be null !" }) : dal.EditAPInvoice(formData) == true ? Json(new { isInserted = true , message = "AP Invoice Updated Successfully !" }) : Json(new { isInserted = false , message = "An Error occured !" });
            }
            catch (Exception)
            {

                throw;
            }

        }


        


    }

}
