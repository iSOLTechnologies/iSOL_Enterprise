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
    public class ARCreditMemoController : Controller
    {
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ARCreditMemoMaster()
        {
            SalesQuotationDal dal = new SalesQuotationDal();

            ViewBag.SalesEmployee = new SelectList(dal.GetSalesEmployee(), "SlpCode", "SlpName");
            //ViewBag.SalesEmployee = new SelectList(dal.GetSalesEmployee(), "SlpCode", "SlpName");
            return View();
        }

        public IActionResult EditARCreditMemoMaster(int id)
        {
            
            ARCreditMemoDal dal1 = new ARCreditMemoDal();
            SalesQuotationDal dal = new SalesQuotationDal();
            ViewBag.SalesEmployee = dal.GetSalesEmployee();
            ViewBag.Taxes = dal.GetVatGroupData();
            ViewBag.Countries = dal.GetCountries();
            ViewBag.Payments = dal.GetPaymentTerms();
            bool flag = CommonDal.Check_IsEditable("INV1", id);
            //ViewBag.Status = flag == false ? "Open" : "Closed";
            ViewBag.Status = "Open" ;
            return View(dal1.GetARCreditMemoDetails(id));
        }
        public IActionResult GetARInvoiceData(int cardcode)
        {
            ResponseModels response = new ResponseModels();
            try
            {

                ARCreditMemoDal dal = new ARCreditMemoDal();
                response.Data = dal.GetARInvoiceData(cardcode);
            }
            catch (Exception ex)
            {

                return Json(response);
            }


            return Json(response);
        }
        [HttpGet]
        public IActionResult GetARInvoiceItemService(int DocId)
        {
            try
            {
                ARCreditMemoDal dal = new ARCreditMemoDal();

                return Json(new { baseDoc = dal.GetInvoiceType(DocId), list = dal.GetARInvoiceItemService(DocId) });
            }
            catch (Exception)
            {
                return Json("");
                throw;
            }

        }
        
        
        public string getUpdatedDocumentNumberOnLoad()
        {
            return SqlHelper.getUpdatedDocumentNumberOnLoad("ORIN", "ARCM");
        }
        public IActionResult GetARCreditMemoData()
        {
            ResponseModels response = new ResponseModels();
            try
            {

                ARCreditMemoDal dal = new ARCreditMemoDal();
                response.Data = dal.GetARCreditMemoData();
            }
            catch (Exception ex)
            {

                return Json(response);
            }


            return Json(response);
        }
        [HttpPost]
        public IActionResult AddARCreditMemo(string formData)
        {
            try
            {
                ARCreditMemoDal dal = new ARCreditMemoDal();
                return formData == null ? Json(new { isInserted = false, message = "Data can't be null !" }) : dal.AddARCreditMemo(formData) == true ? Json(new { isInserted = true, message = "AR Credit Memo Added Successfully !" }) : Json(new { isInserted = false, message = "An Error occured !" });

            }
            catch (Exception)
            {

                throw;
            }

        }
        [HttpPost]
        public IActionResult EditARCreditMemo(string formData)
        {
            try
            {


                ARCreditMemoDal dal = new ARCreditMemoDal();
                return formData == null ? Json(new { isInserted = false, message = "Data can't be null !" }) : dal.EditARCreditMemo(formData) == true ? Json(new { isInserted = true, message = "AR Credit Memo Updated Successfully !" }) : Json(new { isInserted = false, message = "An Error occured !" });

            }
            catch (Exception)
            {

                throw;
            }

        }

    }
}
