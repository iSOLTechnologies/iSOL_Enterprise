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
            ItemMasterDataDal dal = new ItemMasterDataDal();
            ViewData["ItemCodes"] = dal.GetItemCodes();

            return View();
		}
		public async Task<IActionResult>  GetInventoryInWarehouseReportData(int draw, int start, int length, bool ZeroStock = true,string searchWhsValue = null , string searchItemValue = null)
		{

			try
			{
				//ResponseModels response = new();
				InventoryReportDal dal = new();
                var response = new
                {
                    draw = draw,
                    recordsTotal = 5000000,
                    recordsFiltered = 5000000,
                    data = await Task.Run(() => dal.GetInventoryInWarehouseReportData(start, length , ZeroStock , searchWhsValue , searchItemValue))
                };
                

                return  Json (response );
                
            }
			catch (Exception)
			{

				throw;
			}

			
		}
		
	}
}
