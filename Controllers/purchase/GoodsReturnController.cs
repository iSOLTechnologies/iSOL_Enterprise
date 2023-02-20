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
    public class GoodsReturnController : Controller
    {
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GoodsReturnMaster()
        {
            SalesQuotationDal dal = new SalesQuotationDal();

            ViewBag.SalesEmployee = new SelectList(dal.GetSalesEmployee(), "SlpCode", "SlpName");
            //ViewBag.SalesEmployee = new SelectList(dal.GetSalesEmployee(), "SlpCode", "SlpName");
            return View();
        }

        public IActionResult EditGoodsReturnMaster(int id)
        {
            
            ReturnDal dal1 = new ReturnDal();
            SalesQuotationDal dal = new SalesQuotationDal();
            ViewBag.SalesEmployee = dal.GetSalesEmployee();
            ViewBag.Taxes = dal.GetVatGroupData();
            ViewBag.Countries = dal.GetCountries();
            ViewBag.Payments = dal.GetPaymentTerms();
            bool flag = CommonDal.Check_IsEditable("INV1", id);
            //ViewBag.Status = flag == false ? "Open" : "Closed";
            ViewBag.Status = "Open" ;
            return View(dal1.GetReturnDetails(id));
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


                ReturnDal dal = new ReturnDal();
                return formData == null ? Json(new { isInserted = false, message = "Data can't be null !" }) : dal.EditReturn(formData) == true ? Json(new { isInserted = true, message = "Return Updated Successfully !" }) : Json(new { isInserted = false, message = "An Error occured !" });

            }
            catch (Exception)
            {

                throw;
            }

        }

    }
}