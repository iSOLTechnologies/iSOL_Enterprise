using iSOL_Enterprise.Dal;
using iSOL_Enterprise.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace iSOL_Enterprise.Controllers
{
    public class ARInvoiceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ARInvoiceMaster()
        {
            SalesQuotationDal dal = new SalesQuotationDal();

            ViewBag.SalesEmployee = new SelectList(dal.GetSalesEmployee(), "SlpCode", "SlpName");
            return View();
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
    }
}
