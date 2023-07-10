using iSOL_Enterprise.Dal;
using iSOL_Enterprise.Dal.Administrator;
using iSOL_Enterprise.Models;
using Microsoft.AspNetCore.Mvc;

namespace iSOL_Enterprise.Controllers.administrator
{
    public class CompanyDetailsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CompanyDetailsMaster()
        {

            if (string.IsNullOrEmpty(HttpContext.Session.GetInt32("UserId").ToString()) || string.IsNullOrEmpty(HttpContext.Request.Query["Source"]))
            {
                return RedirectToAction("index", "Home");
            }
            CommonDal cdal = new ();

            ViewData["Currencies"] = cdal.GetCurrencydata();
            ViewData["Countries"] = cdal.GetCountries();
            ViewData["States"] = cdal.GetStateCode();
            ViewData["Holidays"] = cdal.GetHolidays();
            
            return View();
        }

        public IActionResult GetCompanyDetails()
        {
            try
            {
                    CompanyDetailsDal dal = new();
               
                    return Json(new { isSuccess = true, oadmData = dal.GetOADMData() , adm1Data = dal.GetADM1Data() });                

                
            }
            catch (Exception)
            {
                return Json(new { isSuccess = false, oadmData = "", adm1Data ="" });
                throw;
            }
        }
        public IActionResult Update(string formData)
        {
            try
            {
                CompanyDetailsDal dal = new();
                if (formData != null)
                {

                    ResponseModels response = dal.Update(formData);
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
