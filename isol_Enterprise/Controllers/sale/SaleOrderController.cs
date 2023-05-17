using iSOL_Enterprise.Common;
using iSOL_Enterprise.Dal;
using iSOL_Enterprise.Dal.Sale;
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
    public class SaleOrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult SaleOrderMaster(string DocId = "", int BaseType = 0)
        {
            SalesQuotationDal dal = new SalesQuotationDal();
			AdministratorDal Adal = new AdministratorDal();
			ViewBag.GetSeries = Adal.GetSeries(17);
            if (DocId != "" && BaseType !=0)
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

        public IActionResult EditSaleOrderMaster(int id)
        {
            SalesQuotationDal dal = new SalesQuotationDal();
            SalesOrderDal dal1 = new SalesOrderDal();
            AdministratorDal Adal = new AdministratorDal();
            CommonDal cdal = new CommonDal();
            ViewBag.SalesEmployee = dal.GetSalesEmployee();
            ViewBag.Taxes = dal.GetVatGroupData("S");
            ViewBag.Countries = cdal.GetCountries();
            ViewBag.Payments = dal.GetPaymentTerms();
            ViewBag.Currency = cdal.GetCurrencydata();
            ViewBag.SaleOrderList = cdal.GetSaleOrders();
            ViewBag.GetSeries = Adal.GetSeries(17);
            DeliveryDal Ddal = new DeliveryDal();
            ViewBag.Warehouse = Ddal.GetWareHouseData();
            bool flag = CommonDal.Check_IsNotEditable("RDR1",id);
            ViewBag.Status = flag == false ? "Open" : "Closed";

            return View(dal1.GetSaleOrderEditDetails(id));
        }
        public IActionResult GetBaseDocData(string cardcode,int BaseType)
        {
            ResponseModels response = new ResponseModels();
            try
            {

                CommonDal dal = new CommonDal();
                response.Data = dal.GetBaseDocData(cardcode,BaseType);
            }
            catch (Exception ex)
            {

                return Json(response);
            }


            return Json(response);
        }
        
        
        //[HttpGet]
        //public  IActionResult GetBaseDocItemService(int DocId,int BaseType)
        //{
        //    try
        //    {
        //        CommonDal dal = new CommonDal();

        //        return Json(new { baseDoc = dal.GetBaseDocType(DocId,BaseType), list = dal.GetBaseDocItemServiceList(DocId,BaseType)});
        //    }
        //    catch (Exception)
        //    {
        //        return Json("");
        //        throw;
        //    }

        //}


        //public string getUpdatedDocumentNumberOnLoad()
        //{
        //    DataTable dt = SqlHelper.GetData("select top 1 DocNum From ORDR  order by Id desc");
        //    if (dt.Rows.Count <= 0)
        //    {
        //        return "SO-0001";
        //    }
        //    string UpdateCode = SqlHelper.ExecuteScalar(SqlHelper.defaultDB, CommandType.Text, @"select top 1 DocNum From ORDR  order by Id desc").ToString();
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
            return SqlHelper.getUpdatedDocumentNumberOnLoad("ORDR", "SO");
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
                return formData == null ? Json(new { isInserted = false, message = "Data can't be null !" }) : dal.AddSaleOrder(formData) == true ? Json(new { isInserted = true, message = "Sale Order Added Successfully !" }) : Json(new { isInserted = false, message = "An Error occured !" });
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
