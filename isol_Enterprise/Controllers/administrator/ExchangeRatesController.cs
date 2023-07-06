using iSOL_Enterprise.Dal;
using iSOL_Enterprise.Dal.Administrator;
using iSOL_Enterprise.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Permissions;

namespace iSOL_Enterprise.Controllers.administrator
{
    public class ExchangeRatesController : Controller
    {
        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetInt32("UserId").ToString()) || string.IsNullOrEmpty(HttpContext.Request.Query["Source"]))
            {
                return RedirectToAction("index", "Home");
            }
            CommonDal dal = new();

            ViewBag.Currencies = dal.GetCurrencydata();

            return View();
        }

        public IActionResult ExchangeRatesMaster()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetInt32("UserId").ToString()) || string.IsNullOrEmpty(HttpContext.Request.Query["Source"]))
            {
                return RedirectToAction("index", "Home");
            }
            CommonDal dal = new();
            ViewBag.Currencies = dal.GetCurrencydata();
            return View();
        }
        public IActionResult GetData()
        {
            ResponseModels response = new ResponseModels();
            ExchangeRatesDal dal = new ();
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
        public IActionResult GetExchnRateData(DateTime Guid)
        {
            try
            {
                ExchangeRatesDal dal = new();
                if (Guid != null)
                {


                    return Json(new { isSuccess = true, exchnRateData = dal.GetExchnRateData(Guid) });
                }
                else
                {
                    return Json(new { isSuccess = false, Data = "" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IActionResult Add(string formData)
        {
            try
            {
                ExchangeRatesDal dal = new();
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
        }public IActionResult Edit(string formData)
        {
            try
            {
                ExchangeRatesDal dal = new();
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
    }
}
