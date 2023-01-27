using iSOL_Enterprise.Dal;
using iSOL_Enterprise.Models;
using iSOL_Enterprise.Models.sale;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
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
        [HttpPost]
        public IActionResult SaveForm(string data)
        {
            try
            {

            
            var model = JsonConvert.DeserializeObject<dynamic>(data);
            return Json("");
            }
            catch (Exception)
            {

                throw;
            }

        }


        


    }

}
