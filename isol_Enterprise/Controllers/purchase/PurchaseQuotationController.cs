using iSOL_Enterprise.Common;
using iSOL_Enterprise.Dal;
using iSOL_Enterprise.Dal.Purchase;
using iSOL_Enterprise.Dal.Sale;
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
    public class PurchaseQuotationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult PurchaseQuotationMaster()
        {

            SalesQuotationDal dal = new SalesQuotationDal();
            AdministratorDal Adal = new AdministratorDal();
            ViewBag.GetSeries = Adal.GetSeries(540000006);
            ViewBag.SalesEmployee = new SelectList(dal.GetSalesEmployee(), "SlpCode", "SlpName",-1);


            return View();
        }




        public IActionResult EditPurchaseQuotationMaster(int id)
        {
            PurchaseQuotationDal dal1 = new PurchaseQuotationDal();
            SalesQuotationDal dal = new SalesQuotationDal();
            CommonDal cdal = new CommonDal();
            DeliveryDal Ddal = new DeliveryDal();
            AdministratorDal Adal = new AdministratorDal();
            ViewBag.GetSeries = Adal.GetSeries(540000006);
            ViewBag.Warehouse = Ddal.GetWareHouseData();
            ViewBag.SalesEmployee = dal.GetSalesEmployee();
            ViewBag.Taxes = dal.GetVatGroupData("P");
            ViewBag.Countries = cdal.GetCountries();
            ViewBag.Payments = dal.GetPaymentTerms();

            ViewBag.Currency = cdal.GetCurrencydata();
            ViewBag.SaleOrderList = cdal.GetSaleOrders();
            bool flag = CommonDal.Check_IsNotEditable("PQT1", id);
            ViewBag.Status = flag == false ? "Open" : "Closed";
            return View(dal1.GetPurchaseQuotationDetails(id));
        }







        //public string getUpdatedDocumentNumberOnLoad()
        //{
        //    DataTable dt = SqlHelper.GetData("select top 1 DocNum From OPQT  order by Id desc");
        //    if (dt.Rows.Count <= 0)
        //    {
        //        return "PQ-0001";
        //    }
        //    string UpdateCode = SqlHelper.ExecuteScalar(SqlHelper.defaultDB, CommandType.Text, @"select top 1 DocNum From OPQT  order by Id desc").ToString();
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
            return SqlHelper.getUpdatedDocumentNumberOnLoad("OPQT", "PQ");
        }

        public IActionResult GetData()
        {
            ResponseModels response = new ResponseModels();
            try
            {

                PurchaseQuotationDal dal = new PurchaseQuotationDal();
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
        public IActionResult GetVendors()
        {
            ResponseModels response = new ResponseModels();
            try
            {

                PurchaseQuotationDal dal = new PurchaseQuotationDal();
                response.Data = dal.GetVendors();
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
        //public IActionResult GetVatGroupData()
        //{
        //    try
        //    {

        //        SalesQuotationDal dal = new SalesQuotationDal();

        //        return Json(dal.GetVatGroupData());
        //    }
        //    catch (Exception ex)
        //    {

        //        return Json(ex.Message);
        //    }
        //}
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
        [HttpPost]
        public IActionResult AddPurchaseQoutation(string formData)
        {
            try
            {


                PurchaseQuotationDal dal = new PurchaseQuotationDal();
                return formData == null ? Json(new { isInserted = false, message = "Data can't be null !" }) : dal.AddPurchaseQoutation(formData) == true ? Json(new { isInserted = true , message = "Purchase Qoutation Added Successfully !" }) : Json(new { isInserted = false , message = "An Error occured !" });

            }
            catch (Exception)
            {

                throw;
            }

        }

        [HttpPost]
        public IActionResult EditPurchaseQoutation(string formData)
        {
            try
            {


                PurchaseQuotationDal dal = new PurchaseQuotationDal();
                return formData == null ? Json(new { isInserted = false, message = "Data can't be null !" }) : dal.EditPurchaseQoutation(formData) == true ? Json(new { isInserted = true, message = "Purchase Qoutation Updated Successfully !" }) : Json(new { isInserted = false, message = "An Error occured !" });

            }
            catch (Exception)
            {

                throw;
            }

        }



    }

}
