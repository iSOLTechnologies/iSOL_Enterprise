using iSOL_Enterprise.Dal;
using iSOL_Enterprise.Dal.Business;
using iSOL_Enterprise.Dal.Financials;
using iSOL_Enterprise.Dal.Production;
using iSOL_Enterprise.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Server;
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
                CommonDal cdal = new ();
                BusinessPartnerMasterDataDal bdal = new ();
                ViewBag.Drawers = dal.GetDrawers();
                ViewBag.Currency = cdal.GetCurrencydata();
                ViewBag.Projects = bdal.GetProjectCodes();
                return View();


            }
            catch (Exception ex)
            {
                return RedirectToAction("index", "Home");

            }
        }
        public IActionResult GetChartOfAccounts()
        {
            ResponseModels response = new ResponseModels();
            ChartOfAccountDal dal = new ();
            try
            {
                response.Data = dal.GetChartOfAccounts();
                return Json(response);
            }
            catch (Exception ex)
            {

                return Json(response);
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
        public IActionResult GetUpdatedAcctCode(string FatherNum)
        {
            ChartOfAccountDal dal = new();
            try
            {
                string AccCode = dal.GetUpdatedAcctCode(FatherNum);
                return Json(AccCode); 
            }
            catch (Exception)
            {
                return Json("");
                throw;
            }
        }
        public IActionResult Add(string formData)
        {
            try
            {
                ChartOfAccountDal dal = new ();
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
                ChartOfAccountDal dal = new ();
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
        public IActionResult Delete(string Guid)
        {
            try
            {
                ChartOfAccountDal dal = new ();
                if (Guid != null)
                {

                    ResponseModels response = dal.Delete(Guid);
                    return Json(new { isDeleted = response.isSuccess, Message = response.Message });
                }
                else
                {
                    return Json(new { isError = true, Message = "Data can't be null" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IActionResult GetGlAccountData(string Guid)
        {
            try
            {
                ChartOfAccountDal dal = new ();
                if (Guid != null)
                {

                    
                    return Json(new { isSuccess = true, gLAcctData = dal.GetGlAccountData(Guid) , drawer = dal.GetGlAccountDrawer(Guid) });
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
    }
}
