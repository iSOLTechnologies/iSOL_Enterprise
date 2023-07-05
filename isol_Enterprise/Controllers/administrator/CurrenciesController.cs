using iSOL_Enterprise.Dal.Administrator;
using iSOL_Enterprise.Dal.Financials;
using iSOL_Enterprise.Models;
using Microsoft.AspNetCore.Mvc;

namespace iSOL_Enterprise.Controllers.administrator
{
    public class CurrenciesController : Controller
    {
        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetInt32("UserId").ToString()) || string.IsNullOrEmpty(HttpContext.Request.Query["Source"]))
            {
                return RedirectToAction("index", "Home");
            }
            return View();
        }
        public IActionResult CurrenciesMaster()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetInt32("UserId").ToString()) || string.IsNullOrEmpty(HttpContext.Request.Query["Source"]))
            {
                return RedirectToAction("index", "Home");
            }

            CurrenciesDal dal = new CurrenciesDal();
            ViewData["Currencies"] = dal.GetCurrencies();
            return View();
        }

        public IActionResult GetData()
        {
            ResponseModels response = new ResponseModels();
            CurrenciesDal dal = new CurrenciesDal();
            try
            {
                response.Data = dal.Getdata();

            }
            catch (Exception)
            {

                throw;
            }
            return Json(response);
        }
        public IActionResult Add(string formData)
        {
            try
            {
                CurrenciesDal dal = new();
                if (formData != null)
                {

                    ResponseModels response = dal.Add(formData);
                    return Json(new { isInserted = response.isSuccess, Message = response.Message });
                }
                else
                {
                    return Json(new { isInserted = false, Message = "Data can't be null" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IActionResult Edit(string formData)
        {
            try
            {
                ChartOfAccountDal dal = new();
                if (formData != null)
                {

                    ResponseModels response = dal.Edit(formData);
                    return Json(new { isInserted = response.isSuccess, Message = response.Message });
                }
                else
                {
                    return Json(new { isInserted = false, Message = "Data can't be null" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
