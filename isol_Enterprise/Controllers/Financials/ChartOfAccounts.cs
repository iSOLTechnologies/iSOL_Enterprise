using iSOL_Enterprise.Dal.Financials;
using iSOL_Enterprise.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace iSOL_Enterprise.Controllers.Financials
{
    public class ChartOfAccounts : Controller
    {
        public IActionResult Index()
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetInt32("UserId").ToString()) || string.IsNullOrEmpty(HttpContext.Request.Query["Source"]))
                {
                    return RedirectToAction("index", "Home");
                }


                return View();


            }
            catch (Exception ex)
            {
                return RedirectToAction("index", "Home");

            }
        }
        public IActionResult ChartOfAccountsMaster()
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetInt32("UserId").ToString()) || string.IsNullOrEmpty(HttpContext.Request.Query["Source"]))
                {
                    return RedirectToAction("index", "Home");
                }

                ChartOfAccountDal dal = new ();
                ViewBag.Drawers = dal.GetDrawers();
                return View();


            }
            catch (Exception ex)
            {
                return RedirectToAction("index", "Home");

            }
        }
        public IActionResult Getlevels(string drawer)
        {
            ResponseModels models = new ResponseModels();
            ChartOfAccountDal dal = new ();
            try
            {
                models.Data = dal.GetLevels(drawer);
                return Json(JsonConvert.SerializeObject(models.Data));
            }
            catch (Exception ex)
            {

                return Json(models);
            }
        }
    }
}
