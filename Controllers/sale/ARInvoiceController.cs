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
    public class ARInvoiceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ARInvoiceMaster(string DocId = "", int BaseType = 0)
        {
            SalesQuotationDal dal = new SalesQuotationDal();
			AdministratorDal Adal = new AdministratorDal();
			ViewBag.GetSeries = Adal.GetSeries(13);
            if (DocId != "" && BaseType != 0)
            {
                ViewBag.DocId = DocId;
                ViewBag.BaseType = BaseType;
            }
            else
            {
                ViewBag.DocId = 0;
                ViewBag.BaseType = 0;
            }
            ViewBag.SalesEmployee = new SelectList(dal.GetSalesEmployee(), "SlpCode", "SlpName");
            return View();
        }

        public IActionResult EditARInvoiceMaster(int id)
        {
            DeliveryDal Ddal = new DeliveryDal();
            ARInvoiceDal dal1 = new ARInvoiceDal();
            SalesQuotationDal dal = new SalesQuotationDal();
            CommonDal cdal = new CommonDal();
            AdministratorDal Adal = new AdministratorDal();
            ViewBag.GetSeries = Adal.GetSeries(13);
            ViewBag.SalesEmployee = dal.GetSalesEmployee();
            ViewBag.Warehouse = Ddal.GetWareHouseData();
            ViewBag.Taxes = dal.GetVatGroupData("S");
            ViewBag.Countries = cdal.GetCountries();
            ViewBag.Currency = cdal.GetCurrencydata();
            ViewBag.Payments = dal.GetPaymentTerms();
            ViewBag.SaleOrderList = cdal.GetSaleOrders();
            bool flag = CommonDal.Check_IsNotEditable("INV1", id);
            ViewBag.Status = flag == false ? "Open" : "Closed";
            return View(dal1.GetARInvoiceDetails(id));
        }

        public IActionResult GetDeliveryData(int cardcode)
        {
            ResponseModels response = new ResponseModels();
            try
            {

                ARInvoiceDal dal = new ARInvoiceDal();
                response.Data = dal.GetDeliveryData(cardcode);
            }
            catch (Exception ex)
            {

                return Json(response);
            }


            return Json(response);
        }
        [HttpGet]

        public IActionResult GetDeliveryItemService(int DocId)
        {
            try
            {
                ARInvoiceDal dal = new ARInvoiceDal();

                return Json(new { baseDoc = dal.GetDeliveryType(DocId), list = dal.GetDeliveryItemServiceList(DocId) });
            }
            catch (Exception)
            {
                return Json("");
                throw;
            }

        }
        //public string getUpdatedDocumentNumberOnLoad()
        //{
        //    DataTable dt = SqlHelper.GetData("select top 1 DocNum From OINV  order by Id desc");
        //    if (dt.Rows.Count <= 0)
        //    {
        //        return "AR-0001";
        //    }
        //    string UpdateCode = SqlHelper.ExecuteScalar(SqlHelper.defaultDB, CommandType.Text, @"select top 1 DocNum From OINV  order by Id desc").ToString();
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
            return SqlHelper.getUpdatedDocumentNumberOnLoad("OINV", "AR");
        }





        public IActionResult GetARInvoiceData()
        {
            ResponseModels response = new ResponseModels();
            try
            {

                ARInvoiceDal dal = new ARInvoiceDal();
                response.Data = dal.GetARInvoiceData();
            }
            catch (Exception ex)
            {

                return Json(response);
            }


            return Json(response);
        }
        [HttpPost]
        public IActionResult AddARInvoice(string formData)
        {
            try
            {


                ARInvoiceDal dal = new ARInvoiceDal();
                return formData == null ? Json(new { isInserted = false, message = "Data can't be null !" }) : dal.AddARInvoice(formData) == true ? Json(new { isInserted = true, message = "AR Invoice Added Successfully !" }) : Json(new { isInserted = false, message = "An Error occured !" });

            }
            catch (Exception)
            {

                throw;
            }

        }

        [HttpPost]
        public IActionResult EditARInvoice(string formData)
        {
            try
            {


                ARInvoiceDal dal = new ARInvoiceDal();
                return formData == null ? Json(new { isInserted = false, message = "Data can't be null !" }) : dal.EditARInvoice(formData) == true ? Json(new { isInserted = true, message = "AR Invoice Updated Successfully !" }) : Json(new { isInserted = false, message = "An Error occured !" });

            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
