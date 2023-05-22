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
    public class PurchaseOrderController : Controller
    {
        IConfiguration _configuration;
        public PurchaseOrderController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            ViewBag.Url = _configuration["ReportUrl"].ToString();
            return View();
        }
        public IActionResult PurchaseOrderMaster(string DocId = "", int BaseType = 0)
        {

            SalesQuotationDal dal = new SalesQuotationDal();
            AdministratorDal Adal = new AdministratorDal();
            ViewBag.GetSeries = Adal.GetSeries(22);
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
            ViewBag.SalesEmployee = new SelectList(dal.GetSalesEmployee(), "SlpCode", "SlpName",-1);



            return View();
        }
          public IActionResult EditPurchaseOrderMaster(int id, int aprv1ghas = 0)
        {
            SalesQuotationDal dal = new SalesQuotationDal();
            PurchaseOrderDal dal1 = new PurchaseOrderDal();
            CommonDal cdal = new CommonDal();
            DeliveryDal Ddal = new DeliveryDal();
            AdministratorDal Adal = new AdministratorDal();
            ViewBag.GetSeries = Adal.GetSeries(22);
            ViewBag.Warehouse = Ddal.GetWareHouseData();
            ViewBag.SalesEmployee = dal.GetSalesEmployee();
            ViewBag.Taxes = dal.GetVatGroupData("P");
            ViewBag.Countries = cdal.GetCountries();
            ViewBag.Payments = dal.GetPaymentTerms();
            ViewBag.Currency = cdal.GetCurrencydata();
            ViewBag.SaleOrderList = cdal.GetSaleOrders();
            ViewBag.ApprovalView = aprv1ghas;
            bool flag = CommonDal.Check_IsNotEditable("POR1", id);
            ViewBag.Status = flag == false && aprv1ghas == 0 ? "Open" : "Closed";
            return View(dal1.GetPurchaseOrderEditDetails(id));
        }

        public IActionResult GetPurchaseQuotationData(string cardcode)
        {
            ResponseModels response = new ResponseModels();
            try
            {
                PurchaseOrderDal dal = new PurchaseOrderDal();
                response.Data = dal.GetPurchaseQuotationData(cardcode);
            }
            catch (Exception ex)
            {

                return Json(response);
            }


            return Json(response);
        }
        [HttpGet]

        public IActionResult GetQuotationItemService(int DocId)
        {
            try
            {
                PurchaseOrderDal dal = new PurchaseOrderDal();
                return Json(new { baseDoc = dal.GetQuotationType(DocId), list = dal.GetQuotationItemServiceList(DocId) });
            }
            catch (Exception)
            {
                return Json("");
                throw;
            }

        }
        //public string getUpdatedDocumentNumberOnLoad()
        //{
        //    DataTable dt = SqlHelper.GetData("select top 1 DocNum From OPOR  order by Id desc");
        //    if (dt.Rows.Count <= 0)
        //    {
        //        return "PO-0001";
        //    }
        //    string UpdateCode = SqlHelper.ExecuteScalar(SqlHelper.defaultDB, CommandType.Text, @"select top 1 DocNum From OPOR  order by Id desc").ToString();
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
            return SqlHelper.getUpdatedDocumentNumberOnLoad("OPOR", "PO");
        }

        public IActionResult GetData()
        {
            ResponseModels response = new ResponseModels();
            try
            {

                PurchaseOrderDal dal = new PurchaseOrderDal();
                response.Data = dal.GetData();
            }
            catch (Exception ex)
            {

                return Json(response);
            }


            return Json(response);
        }
       
        [HttpPost]
        public IActionResult AddPurchaseOrder(string formData)
        {
            try
            {


                PurchaseOrderDal dal = new PurchaseOrderDal();
                return formData == null ? Json(new { isInserted = false, message = "Data can't be null !" }) : dal.AddPurchaseOrder(formData) == true ? Json(new { isInserted = true , message = "Purchase Order Added Successfully !" }) : Json(new { isInserted = false , message = "An Error occured !" });

            }
            catch (Exception)
            {

                throw;
            }

        }

        [HttpPost]
        public IActionResult EditPurchaseOrder(string formData)
        {
            try
            {


                PurchaseOrderDal dal = new PurchaseOrderDal();
                return formData == null ? Json(new { isInserted = false, message = "Data can't be null !" }) : dal.EditPurchaseOrder(formData) == true ? Json(new { isInserted = true, message = "Purchase Order Added Successfully !" }) : Json(new { isInserted = false, message = "An Error occured !" });

            }
            catch (Exception)
            {

                throw;
            }

        }



    }

}
