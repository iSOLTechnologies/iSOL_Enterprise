using iSOL_Enterprise.Common;
using iSOL_Enterprise.Dal;
using iSOL_Enterprise.Models;
using iSOL_Enterprise.Models.sale;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SqlHelperExtensions;
using System.Data;
using System.Data.SqlClient;

namespace iSOL_Enterprise.Controllers.Sales
{
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



        public string getUpdatedDocumentNumberOnLoad()
        {
            DataTable dt = SqlHelper.GetData("select top 1 DocNum From OPCH  order by Id desc");
            if (dt.Rows.Count <= 0)
            {
                return "API-0001";
            }
            string UpdateCode = SqlHelper.ExecuteScalar(SqlHelper.defaultDB, CommandType.Text, @"select top 1 DocNum From OPCH  order by Id desc").ToString();
            // DataTable dt1 = SqlHelper.GetData("select top 1 ItemCode From[dbo].[Item_Master]  order by Id desc");
            //   string data = dt.Rows[1]["ItemCode"].ToString();
            string[] str = UpdateCode.Split('-');
            string lastItem = str[str.Length - 1];
            int no = int.Parse(lastItem);
            no = no + 1;
            string code = str[0];
            string a = string.Format("{0:D" + lastItem.Length + "}", no);
            return code + "-" + a;
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
        public IActionResult GetItems()
        {
            ResponseModels response = new ResponseModels();
            try
            {

                SalesQuotationDal dal = new SalesQuotationDal();
                response.Data = dal.GetItemsData();
            }
            catch (Exception ex)
            {

                return Json(response);
            }


            return Json(response);
        }
        public IActionResult GetCustomers()
        {
            ResponseModels response = new ResponseModels();
            try
            {

                SalesQuotationDal dal = new SalesQuotationDal();
                response.Data = dal.GetCustomerData();
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
        public IActionResult GetVatGroupData()
        {
            try
            {

                SalesQuotationDal dal = new SalesQuotationDal();

                return Json(dal.GetVatGroupData());
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
        public IActionResult GetContactPersons(int cardCode)
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


        


    }

}
