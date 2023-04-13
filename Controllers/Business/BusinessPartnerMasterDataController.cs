using iSOL_Enterprise.Dal;
using iSOL_Enterprise.Dal.Business;
using iSOL_Enterprise.Dal.Inventory;
using iSOL_Enterprise.Dal.Sale;
using iSOL_Enterprise.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace iSOL_Enterprise.Controllers.Business
{
    public class BusinessPartnerMasterDataController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetEmailGroup()
        {
            try
            {
                BusinessPartnerMasterDataDal dal = new BusinessPartnerMasterDataDal();
                return Json(dal.GetEmailGroup());
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        [HttpGet]
        public IActionResult GetStateCode()
        {
            try
            {

                BusinessPartnerMasterDataDal dal = new BusinessPartnerMasterDataDal();

                return Json(dal.GetStateCode());
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        public IActionResult BusinessPartnerMasterDataMaster()
        {
            AdministratorDal addal = new AdministratorDal();
            BusinessPartnerMasterDataDal dal = new BusinessPartnerMasterDataDal();
            SalesQuotationDal sDal = new SalesQuotationDal();

            ViewData["Series"] = addal.GetSeries(2);
            ViewData["MySeries"] = addal.GetMySeries(2);
            ViewData["Groups"] = new SelectList(dal.GetGroups(), "Value", "Text"); 
            ViewData["ShipType"] = new SelectList(dal.GetShipTypes(), "Value", "Text");
            ViewData["SalesEmployee"] = new SelectList(sDal.GetSalesEmployee(), "SlpCode", "SlpName");
            ViewData["Name"] = new SelectList(dal.GetNames(), "Value", "Text"); 
            ViewData["ProjectCode"] = new SelectList(dal.GetProjectCodes(), "Value", "Text"); 
            ViewData["BusinessPartners"] = new SelectList(dal.GetBusinessPartners(), "Value", "Text"); 
            ViewData["Industry"] = new SelectList(dal.GetIndustries(), "Value", "Text"); 
            ViewData["Technician"] = new SelectList(dal.GetTechnicians(), "Value", "Text"); 
            ViewData["Territory"] = new SelectList(dal.GetTerritories(), "Value", "Text");
            ViewData["properties"] = dal.GetProperties();
            return View();
        }
        [HttpPost]
        public IActionResult AddBusinessMasterData(string formData)
        {
            try
            {
                BusinessPartnerMasterDataDal dal = new BusinessPartnerMasterDataDal();
                if (formData != null)
                {

                    ResponseModels response = dal.AddBusinessMasterData(formData);
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
