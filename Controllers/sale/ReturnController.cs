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
    public class ReturnController : Controller
    {
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ReturnMaster()
        {
            SalesQuotationDal dal = new SalesQuotationDal();

            ViewBag.SalesEmployee = new SelectList(dal.GetSalesEmployee(), "SlpCode", "SlpName");
            //ViewBag.SalesEmployee = new SelectList(dal.GetSalesEmployee(), "SlpCode", "SlpName");
            return View();
        }

        public IActionResult EditReturnMaster(int id)
        {
            
            ReturnDal dal1 = new ReturnDal();
            SalesQuotationDal dal = new SalesQuotationDal();
            ViewBag.SalesEmployee = dal.GetSalesEmployee();
            CommonDal cdal = new CommonDal();
            ViewBag.Taxes = dal.GetVatGroupData();
            ViewBag.Countries = cdal.GetCountries();
            ViewBag.Payments = dal.GetPaymentTerms();
            bool flag = CommonDal.Check_IsEditable("INV1", id);
            //ViewBag.Status = flag == false ? "Open" : "Closed";
            ViewBag.Status = "Open" ;
            return View(dal1.GetReturnDetails(id));
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
        //    DataTable dt = SqlHelper.GetData("select top 1 DocNum From ODLN  order by Id desc");
        //    if (dt.Rows.Count <= 0)
        //    {
        //        return "DV-0001";
        //    }
        //    string UpdateCode = SqlHelper.ExecuteScalar(SqlHelper.defaultDB, CommandType.Text, @"select top 1 DocNum From ODLN  order by Id desc").ToString();
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
            return SqlHelper.getUpdatedDocumentNumberOnLoad("ORDN", "R");
        }
        public IActionResult GetReturnData()
        {
            ResponseModels response = new ResponseModels();
            try
            {

                ReturnDal dal = new ReturnDal();
                response.Data = dal.GetReturnData();
            }
            catch (Exception ex)
            {

                return Json(response);
            }


            return Json(response);
        }
        [HttpPost]
        public IActionResult AddReturn(string formData)
        {
            try
            {
                ReturnDal dal = new ReturnDal();
                return formData == null ? Json(new { isInserted = false, message = "Data can't be null !" }) : dal.AddReturn(formData) == true ? Json(new { isInserted = true, message = "Return Added Successfully !" }) : Json(new { isInserted = false, message = "An Error occured !" });

            }
            catch (Exception)
            {

                throw;
            }

        }
        [HttpPost]
        public IActionResult EditReturn(string formData)
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
