using iSOL_Enterprise.Dal.Inventory;
using iSOL_Enterprise.Models;
using Microsoft.AspNetCore.Mvc;

namespace iSOL_Enterprise.Controllers.Reports
{
	public class InventoryReportController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult InventoryInWarehouseReport()
		{



			return View();
		}
		public ResponseModels GetInventoryInWarehouseReportData()
		{

			try
			{
				ResponseModels response = new();
				InventoryReportDal dal = new();

				response.Data = dal.GetInventoryInWarehouseReportData();

                return response;
            }
			catch (Exception)
			{

				throw;
			}

			
		}
	}
}
