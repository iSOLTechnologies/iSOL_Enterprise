﻿using iSOL_Enterprise.Dal;
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
    public class APCreditMemoController : Controller
    {
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult APCreditMemoMaster()
        {
            SalesQuotationDal dal = new SalesQuotationDal();

            ViewBag.SalesEmployee = new SelectList(dal.GetSalesEmployee(), "SlpCode", "SlpName");
            //ViewBag.SalesEmployee = new SelectList(dal.GetSalesEmployee(), "SlpCode", "SlpName");
            return View();
        }

        public IActionResult EditAPCreditMemoMaster(int id)
        {
            
            APCreditMemoDal dal1 = new APCreditMemoDal();
            SalesQuotationDal dal = new SalesQuotationDal();
            ViewBag.SalesEmployee = dal.GetSalesEmployee();
            ViewBag.Taxes = dal.GetVatGroupData();
            ViewBag.Countries = dal.GetCountries();
            ViewBag.Payments = dal.GetPaymentTerms();
            bool flag = CommonDal.Check_IsEditable("INV1", id);
            //ViewBag.Status = flag == false ? "Open" : "Closed";
            ViewBag.Status = "Open" ;
            return View(dal1.GetAPCreditMemoDetails(id));
        }
        public IActionResult GetAPInvoiceData(int cardcode)
        {
            ResponseModels response = new ResponseModels();
            try
            {

                APCreditMemoDal dal = new APCreditMemoDal();
                response.Data = dal.GetAPInvoiceData(cardcode);
            }
            catch (Exception ex)
            {

                return Json(response);
            }


            return Json(response);
        }
        [HttpGet]
        public IActionResult GetAPInvoiceItemService(int DocId)
        {
            try
            {
                APCreditMemoDal dal = new APCreditMemoDal();

                return Json(new { baseDoc = dal.GetInvoiceType(DocId), list = dal.GetAPInvoiceItemService(DocId) });
            }
            catch (Exception)
            {
                return Json("");
                throw;
            }

        }
        
        
        public string getUpdatedDocumentNumberOnLoad()
        {
            return SqlHelper.getUpdatedDocumentNumberOnLoad("ORPC", "APCM");
        }
        public IActionResult GetAPCreditMemoData()
        {
            ResponseModels response = new ResponseModels();
            try
            {

                APCreditMemoDal dal = new APCreditMemoDal();
                response.Data = dal.GetAPCreditMemoData();
            }
            catch (Exception ex)
            {

                return Json(response);
            }


            return Json(response);
        }
        [HttpPost]
        public IActionResult AddAPCreditMemo(string formData)
        {
            try
            {
                APCreditMemoDal dal = new APCreditMemoDal();
                return formData == null ? Json(new { isInserted = false, message = "Data can't be null !" }) : dal.AddAPCreditMemo(formData) == true ? Json(new { isInserted = true, message = "AP Credit Memo Added Successfully !" }) : Json(new { isInserted = false, message = "An Error occured !" });

            }
            catch (Exception)
            {

                throw;
            }

        }
        [HttpPost]
        public IActionResult EditAPCreditMemo(string formData)
        {
            try
            {


                APCreditMemoDal dal = new APCreditMemoDal();
                return formData == null ? Json(new { isInserted = false, message = "Data can't be null !" }) : dal.EditAPCreditMemo(formData) == true ? Json(new { isInserted = true, message = "AP Credit Memo Updated Successfully !" }) : Json(new { isInserted = false, message = "An Error occured !" });

            }
            catch (Exception)
            {

                throw;
            }

        }

    }
}