using iSOL_Enterprise.Dal;
using iSOL_Enterprise.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
