using iSOL_Enterprise.Dal;
using iSOL_Enterprise.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SqlHelperExtensions;
using System.Data;

namespace iSOL_Enterprise.Controllers
{
    public class DeliveryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult DeliveryMaster()
        {
            SalesQuotationDal dal = new SalesQuotationDal();

            ViewBag.SalesEmployee = new SelectList(dal.GetSalesEmployee(), "SlpCode", "SlpName");
            return View();
        }
        public string getUpdatedDocumentNumberOnLoad()
        {
            DataTable dt = SqlHelper.GetData("select top 1 DocNum From ODLN  order by Id desc");
            if (dt.Rows.Count <= 0)
            {
                return "DV-0001";
            }
            string UpdateCode = SqlHelper.ExecuteScalar(SqlHelper.defaultDB, CommandType.Text, @"select top 1 DocNum From ODLN  order by Id desc").ToString();
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
        public IActionResult GetDeliveryData()
        {
            ResponseModels response = new ResponseModels();
            try
            {

                DeliveryDal dal = new DeliveryDal();
                response.Data = dal.GetDeliveryData();
            }
            catch (Exception ex)
            {

                return Json(response);
            }


            return Json(response);
        }

    }
}
