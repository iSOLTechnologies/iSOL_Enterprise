using iSOL_Enterprise.Dal;
using iSOL_Enterprise.Dal.Inventory;
using iSOL_Enterprise.Dal.Inventory_Transactions;
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
			ViewData["Series"] = dal.GetSeries(1470000113);
			ViewData["MySeries"] = dal.GetMySeries(1470000113); 
            ViewData["Branch"] = new SelectList(PRdal.GetBranch(), "SlpCode", "SlpName");
            ViewData["Department"] = new SelectList(PRdal.GetDepartment(), "SlpCode", "SlpName");
			ViewData["GetSeries"] = dal.GetSeries(1470000113);

            return View();
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
