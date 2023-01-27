using iSOL_Enterprise.Dal;
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
    }
}
