using iSOL_Enterprise.Common;
using iSOL_Enterprise.Dal;
using iSOL_Enterprise.Dal.Sale;
using iSOL_Enterprise.Models;
using iSOL_Enterprise.Models.sale;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SqlHelperExtensions;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace iSOL_Enterprise.Controllers.Sales
{
    [Authorize]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class SaleQuotationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult SaleQuotationMaster()
        {
            AdministratorDal Adal = new AdministratorDal();
            SalesQuotationDal dal = new SalesQuotationDal();

            ViewBag.SalesEmployee = new SelectList(dal.GetSalesEmployee(), "SlpCode", "SlpName");
            ViewBag.GetSeries = Adal.GetSeries(23);


            return View();
        }
         
            
            public IActionResult EditSaleQuotationMaster(int id)
        {
            SalesQuotationDal dal = new SalesQuotationDal();
            AdministratorDal Adal = new AdministratorDal();
            CommonDal cdal = new CommonDal();
            DeliveryDal Ddal = new DeliveryDal();
            //ViewBag.SalesEmployee = new SelectList(dal.GetSalesEmployee(), "SlpCode", "SlpName");
            ViewBag.SalesEmployee = dal.GetSalesEmployee();
            ViewBag.Taxes = dal.GetVatGroupData("S");
            ViewBag.Countries = cdal.GetCountries();
            ViewBag.Payments = dal.GetPaymentTerms();
            ViewBag.Currency = cdal.GetCurrencydata();
            ViewBag.Warehouse = Ddal.GetWareHouseData();
            ViewBag.SaleOrderList = cdal.GetSaleOrders();
            ViewBag.GetSeries = Adal.GetSeries(23);
            bool flag = CommonDal.Check_IsNotEditable("QUT1", id);
            ViewBag.Status = flag == false ? "Open" : "Closed";
            return View(dal.GetSaleQuotationEditDetails(id));
        }


        public string getUpdatedDocumentNumberOnLoad()
        { 
            return SqlHelper.getUpdatedDocumentNumberOnLoad("OQUT","SQ");
        }


        public IActionResult GetData()
        {
            ResponseModels response = new ResponseModels();
            try
            {
                SalesQuotationDal dal = new SalesQuotationDal();
                response.Data = dal.GetData();
            }
            catch (Exception ex)
            {

                return Json(response);
            }


            return Json(response);
        }
        public IActionResult GetItems(string DocModule)
        {
            ResponseModels response = new ResponseModels();
            try
            {

                SalesQuotationDal dal = new SalesQuotationDal();
                response.Data = dal.GetItemsData(DocModule);
            }
            catch (Exception ex)
            {

                return Json(response);
            }


            return Json(response);
        }
        public IActionResult GetBusinessPartners(string DocModule)
        {
            ResponseModels response = new ResponseModels();
            try
            {

                CommonDal dal = new CommonDal();
                response.Data = dal.GetBusinessPartners(DocModule);
            }
            catch (Exception ex)
            {

                return Json(response);
            }


            return Json(response);
        }
        public IActionResult GetAccounts()
        {
            ResponseModels response = new ResponseModels();
            try
            {

                SalesQuotationDal dal = new SalesQuotationDal();
                response.Data = dal.GetGLAccountData();
            }
            catch (Exception ex)
            {

                return Json(response);
            }


            return Json(response);
        } 

        public IActionResult GetPayments()
        {
            //ResponseModels response = new ResponseModels();
            try
            {

                SalesQuotationDal dal = new SalesQuotationDal();
                
                return Json(dal.GetPaymentTerms());
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }


            
        }
        public IActionResult GetVatGroupData(string DocModule)
        {
            try
            {

                SalesQuotationDal dal = new SalesQuotationDal();

                return Json(dal.GetVatGroupData(DocModule));
            }
            catch (Exception ex)
            {

                return Json(ex.Message);
            }
        }
        
        public IActionResult GetSaleEmployee()
        {
            //ResponseModels response = new ResponseModels();
            try
            {

                SalesQuotationDal dal = new SalesQuotationDal();
                return Json(dal.GetSalesEmployee());
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }



        }
        public IActionResult GetContactPersons(string cardCode)
        {
            //ResponseModels response = new ResponseModels();
            try
            {
                SalesQuotationDal dal = new SalesQuotationDal();
                return Json(dal.GetContactPersons(cardCode));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            } 
        }
        public IActionResult GetUomGroup(string itemno)
        {
            try
            {
                SalesQuotationDal dal = new SalesQuotationDal();
                var OITM_Data = dal.GetOITMRowData(itemno);
                int UgpEntry = (int)(OITM_Data[0].UgpEntry);
                return Json(new { oitmdata = OITM_Data, uomdata = dal.GetOUOMList(UgpEntry)});
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public IActionResult AddSalesQoutation(string formData)
        {
            try
            {
                SalesQuotationDal dal = new SalesQuotationDal();
                return formData == null ? Json(new { isInserted = false, message = "Data can't be null !" }) : dal.AddSalesQoutation(formData) == true ? Json(new { isInserted = true , message = "Sale Qoutation Added Successfully !" }) : Json(new { isInserted = false , message = "An Error occured !" });
            }
            catch (Exception)
            {

                throw;
            }

        }


        [HttpPost]
        public IActionResult EditSalesQoutation(string formData)
        {
            try
            {
                SalesQuotationDal dal = new SalesQuotationDal();
                return formData == null ? Json(new { isInserted = false, message = "Data can't be null !" }) : dal.EditSalesQoutation(formData) == true ? Json(new { isInserted = true, message = "Sale Qoutation Updated Successfully !" }) : Json(new { isInserted = false, message = "An Error occured !" });
            }
            catch (Exception)
            {
                throw;
            }

            }



    }

}
