using iSOL_Enterprise.Dal;
using iSOL_Enterprise.Dal.Administrator;
using iSOL_Enterprise.Dal.Financials;
using iSOL_Enterprise.Models;
using Microsoft.AspNetCore.Mvc;

namespace iSOL_Enterprise.Controllers.administrator
{
    public class HolidayDatesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult HolidayDatesMaster()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetInt32("UserId").ToString()) || string.IsNullOrEmpty(HttpContext.Request.Query["Source"]))
            {
                return RedirectToAction("index", "Home");
            }
            CommonDal cdal = new();

            
            ViewData["WeekDays"] = cdal.GetWeekDays();

            return View();
        
        }
        public IActionResult GetData()
        {
            ResponseModels response = new ResponseModels();
            HolidayDatesDal dal = new();
            try
            {
                response.Data = dal.GetData();
                return Json(response);
            }
            catch (Exception ex)
            {

                return Json(response);
            }
        }
        public IActionResult GetHolidayDateData(string Guid)
        {
            try
            {
                HolidayDatesDal dal = new();
                if (Guid != null)
                {


                    return Json(new { isSuccess = true, headerData = dal.GetHDHeaderData(Guid) , rowData = dal.GetHDRowData(Guid) });
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
                HolidayDatesDal dal = new();
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
                HolidayDatesDal dal = new();
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
