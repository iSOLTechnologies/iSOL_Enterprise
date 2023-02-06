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
    public class SaleQuotationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult SaleQuotationMaster()
        {

            //List<tbl_item> lst = new List<tbl_item>();
            //SqlConnection connection = new SqlConnection("Data Source=DESKTOP-AJM6HM8\\SQLSERVER19;Initial Catalog=SAPDB;User ID=sa;Password=n5210567");
            //SqlDataAdapter sda = new SqlDataAdapter("select ItemCode,ItemName,OnHand from OITM", connection);
            //DataSet ds = new DataSet();
            //sda.Fill(ds);
            //foreach (DataRow row in ds.Tables[0].Rows)
            //{
            //    lst.Add(new tbl_item()
            //    {
            //        ItemCode = row[0].ToString(),
            //        ItemName = row[1].ToString(),
            //        OnHand   = (decimal)row[2]
            //    });
            //}
            //ViewData["ItemList"] = lst;



            //List<tbl_customer> lst1 = new List<tbl_customer>();
            //SqlConnection connection1 = new SqlConnection("Data Source=DESKTOP-AJM6HM8\\SQLSERVER19;Initial Catalog=SAPDB;User ID=sa;Password=n5210567");
            //SqlDataAdapter sda1 = new SqlDataAdapter("select CardCode,CardName,Balance from OCRD", connection);
            //DataSet ds1 = new DataSet();
            //sda1.Fill(ds1);
            //foreach (DataRow row in ds1.Tables[0].Rows)
            //{
            //    lst1.Add(new tbl_customer()
            //    {
            //        CardCode = row[0].ToString(),
            //        CardName = row[1].ToString(),
            //        Balance = (decimal)row[2]
            //    });
            //}
            //ViewData["CustomerList"] = lst1;

            //List<tbl_account> lst2 = new List<tbl_account>();            
            //SqlDataAdapter sda2 = new SqlDataAdapter("select AcctCode,AcctName,CurrTotal from OACT", connection);
            //DataSet ds2 = new DataSet();
            //sda2.Fill(ds2);
            //foreach (DataRow row in ds2.Tables[0].Rows)
            //{
            //    lst2.Add(new tbl_account()
            //    {
            //        AcctCode = row[0].ToString(),
            //        AcctName = row[1].ToString(),
            //        CurrTotal = (decimal)row[2]
            //    });
            //}
            //ViewData["AccountList"] = lst2;


            SalesQuotationDal dal = new SalesQuotationDal();

            ViewBag.SalesEmployee = new SelectList(dal.GetSalesEmployee(), "SlpCode", "SlpName");



            return View();
        }


        public IActionResult EditSaleQuotationMaster(int id)
        {
            SalesQuotationDal dal = new SalesQuotationDal();
            ViewBag.SalesEmployee = new SelectList(dal.GetSalesEmployee(), "SlpCode", "SlpName");
            ViewBag.Taxes = dal.GetVatGroupData();
            ViewBag.Countries = dal.GetCountries();
            return View(dal.GetSaleQuotationEditDetails(id));
        }


        public string getUpdatedDocumentNumberOnLoad()
        {
            DataTable dt = SqlHelper.GetData("select top 1 DocNum From OQUT  order by Id desc");
            if (dt.Rows.Count <= 0)
            {
                return "SQ-0001";
            }
            string UpdateCode = SqlHelper.ExecuteScalar(SqlHelper.defaultDB, CommandType.Text, @"select top 1 DocNum From OQUT  order by Id desc").ToString();
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

                SalesQuotationDal dal = new SalesQuotationDal();
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
        public IActionResult GetCountries()
        {
            try
            {

                SalesQuotationDal dal = new SalesQuotationDal();

                return Json(dal.GetCountries());
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
