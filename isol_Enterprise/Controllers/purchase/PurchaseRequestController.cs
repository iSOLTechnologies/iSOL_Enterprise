using iSOL_Enterprise.Dal;
using iSOL_Enterprise.Dal.Business;
using iSOL_Enterprise.Dal.Inventory;
using iSOL_Enterprise.Dal.Inventory_Transactions;
using iSOL_Enterprise.Dal.Purchase;
using iSOL_Enterprise.Dal.Sale;
using iSOL_Enterprise.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace iSOL_Enterprise.Controllers.purchase
{
    public class PurchaseRequestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult PurchaseRequestMaster()
		{
		    PurchaseRequestDal PRdal = new PurchaseRequestDal(); 
			AdministratorDal dal = new AdministratorDal();
            BusinessPartnerMasterDataDal Bdal = new BusinessPartnerMasterDataDal(); 
			ViewData["Series"] = dal.GetSeries(1470000113);
			ViewData["MySeries"] = dal.GetMySeries(1470000113); 
            ViewData["Branch"] = new SelectList(PRdal.GetBranch(), "SlpCode", "SlpName");
            ViewData["Department"] = new SelectList(PRdal.GetDepartment(), "SlpCode", "SlpName");
			ViewData["GetSeries"] = dal.GetSeries(1470000113);
            ViewData["Technician"] = new SelectList(Bdal.GetTechnicians(), "Value", "Text");
            return View();
        }

		public IActionResult EditPurchaseRequestMaster(int id, int aprv1ghas = 0)
		{
			PurchaseRequestDal Pdal = new PurchaseRequestDal();
			AdministratorDal dal = new AdministratorDal(); 
			SalesQuotationDal Sdal = new SalesQuotationDal();
			DeliveryDal Ddal = new DeliveryDal();
			BusinessPartnerMasterDataDal Bdal = new BusinessPartnerMasterDataDal();
			ViewData["Branch"] = Pdal.GetBranch();
			ViewData["Department"] = Pdal.GetDepartment();
			ViewData["Series"] = dal.GetSeries(1470000113);
			ViewData["MySeries"] = dal.GetMySeries(1470000113);
			ViewData["Taxes"]  = Sdal.GetVatGroupData("P");
			ViewData["Warehouse"]  = Ddal.GetWareHouseData();
			ViewData["Technician"] = Bdal.GetTechnicians();
            ViewBag.ApprovalView = aprv1ghas;
            //ViewBag.SalesEmployee = new SelectList(dal.GetSalesEmployee(), "SlpCode", "SlpName",-1);
            //ViewBag.SalesEmployee = dal.GetSalesEmployee();			 
            //bool flag = CommonDal.Check_IsNotEditable("PRQ1", id);
            //ViewBag.Status = flag == false ? "Open" : "Closed";
            ViewBag.Status = "Open";

			return View(Pdal.GetPurchaseRequestEditDetails(id));
		}


		[HttpPost]
		public IActionResult AddPurchaseRequest(string formData)
		{
			try
			{
				PurchaseRequestDal dal = new PurchaseRequestDal();
				if (formData != null)
				{
					ResponseModels response = dal.AddPurchaseRequest(formData);
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
		[HttpPost]
		public IActionResult EditPurchaseRequest(string formData)
		{
			try
			{
				PurchaseRequestDal dal = new PurchaseRequestDal();
				if (formData != null)
				{
					ResponseModels response = dal.EditPurchaseRequest(formData);
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
		public IActionResult GetData()
        {
            ResponseModels response = new ResponseModels();
            try
            {
                PurchaseRequestDal dal = new PurchaseRequestDal();
                response.Data = dal.GetData();
            }
            catch (Exception ex)
            {

                return Json(response);
            }


            return Json(response);
        }
        public IActionResult GetUsers()
        {
            ResponseModels response = new ResponseModels();
            try
            {

                PurchaseRequestDal dal = new PurchaseRequestDal();
                response.Data = dal.GetUsers();
            }
            catch (Exception ex)
            {

                return Json(response);
            }


            return Json(response);
        }
        public IActionResult GetEmployes()
        {
            ResponseModels response = new ResponseModels();
            try
            {
                PurchaseRequestDal dal = new PurchaseRequestDal();
                response.Data = dal.GetEmployes();
            }
            catch (Exception ex)
            {
                return Json(response);
            }
            return Json(response);
        }

    }
}
