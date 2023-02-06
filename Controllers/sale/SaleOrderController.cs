using iSOL_Enterprise.Dal;
using iSOL_Enterprise.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SqlHelperExtensions;
using System.Data;

namespace iSOL_Enterprise.Controllers
{
    public class SaleOrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult SaleOrderMaster()
        {
            SalesQuotationDal dal = new SalesQuotationDal();

            ViewBag.SalesEmployee = new SelectList(dal.GetSalesEmployee(), "SlpCode", "SlpName");
            return View();
        }

        public IActionResult EditSaleOrderMaster(int id)
        {
            SalesQuotationDal dal = new SalesQuotationDal();
            SalesOrderDal dal1 = new SalesOrderDal();
            ViewBag.SalesEmployee = new SelectList(dal.GetSalesEmployee(), "SlpCode", "SlpName");
            ViewBag.Taxes = dal.GetVatGroupData();
            ViewBag.Countries = dal.GetCountries();
            ViewBag.Payments = dal.GetPaymentTerms();
            return View(dal1.GetSaleOrderEditDetails(id));
        }
        public IActionResult GetSalesQuotationData(int cardcode)
        {
            ResponseModels response = new ResponseModels();
            try
            {

                SalesOrderDal dal = new SalesOrderDal();
                response.Data = dal.GetSalesQuotationData(cardcode);
            }
            catch (Exception ex)
            {

                return Json(response);
            }


            return Json(response);
        }
        [HttpGet]

        public  IActionResult GetQuotationItemService(int DocId)
        {
            try
            {
                SalesOrderDal dal = new SalesOrderDal();

                return Json(new { baseDoc = dal.GetQuotationType(DocId), list = dal.GetQuotationItemServiceList(DocId) });
            }
            catch (Exception)
            {
                return Json("");
                throw;
            }

        }


        public string getUpdatedDocumentNumberOnLoad()
        {
            DataTable dt = SqlHelper.GetData("select top 1 DocNum From ORDR  order by Id desc");
            if (dt.Rows.Count <= 0)
            {
                return "SO-0001";
            }
            string UpdateCode = SqlHelper.ExecuteScalar(SqlHelper.defaultDB, CommandType.Text, @"select top 1 DocNum From ORDR  order by Id desc").ToString();
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
        public IActionResult GetSaleOrderData()
        {
            ResponseModels response = new ResponseModels();
            try
            {

                SalesOrderDal dal = new SalesOrderDal();
                response.Data = dal.GetSaleOrderData();
            }
            catch (Exception ex)
            {

                return Json(response);
            }


            return Json(response);
        }
        [HttpPost]
        public IActionResult AddSaleOrder(string formData)
        {
            try
            { 
                SalesOrderDal dal = new SalesOrderDal();
                return formData == null ? Json(new { isInserted = false, message = "Data can't be null !" }) : dal.AddSaleOrder(formData) == true ? Json(new { isInserted = true, message = "Sale Qoutation Added Successfully !" }) : Json(new { isInserted = false, message = "An Error occured !" });
            }
            catch (Exception)
            {

                throw;
            }

        }
        [HttpPost]
        public IActionResult EditSaleOrder(string formData)
        {
            try
            {


                SalesOrderDal dal = new SalesOrderDal();
                return formData == null ? Json(new { isInserted = false, message = "Data can't be null !" }) : dal.EditSaleOrder(formData) == true ? Json(new { isInserted = true, message = "Sale Order Updated Successfully !" }) : Json(new { isInserted = false, message = "An Error occured !" });

            }
            catch (Exception)
            {

                throw;
            }

        }

    }
}
