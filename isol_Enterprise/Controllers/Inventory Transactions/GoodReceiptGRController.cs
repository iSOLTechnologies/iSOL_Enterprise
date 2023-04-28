﻿using iSOL_Enterprise.Controllers.Inventory;
using iSOL_Enterprise.Dal;
using iSOL_Enterprise.Dal.Inventory;
using iSOL_Enterprise.Dal.Inventory_Transactions;
using iSOL_Enterprise.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace iSOL_Enterprise.Controllers.Inventory_Transactions
{
    [Authorize]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class GoodReceiptGRController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetData()
        {
            ResponseModels response = new ResponseModels();
            try
            {

                GoodReceiptGRDal dal = new GoodReceiptGRDal();
                response.Data = dal.GetData();
            }
            catch (Exception ex)
            {

                return Json(response);
            }


            return Json(response);
        }
        public IActionResult GoodReceiptGRMaster(int id = 0)
        {
            ItemMasterDataDal Idal = new ItemMasterDataDal(); 
            AdministratorDal dal = new AdministratorDal();
            ViewData["Series"] = dal.GetSeries(59);
            ViewData["MySeries"] = dal.GetMySeries(59);
            ViewData["GroupNum"] = new SelectList(Idal.GetListName(), "Value", "Text");
			if (id > 0)
			{
				ViewBag.OldId = id;
			}
			else
				ViewBag.OldId = 0;

			return View();
        }




		public IActionResult GetOldData(int ItemID)
		{
			try
			{
				GoodReceiptGRDal dal = new GoodReceiptGRDal();
				 
				return Json(new
				{
					success = true,
					HeaderData = dal.GetHeaderOldData(ItemID),
					RowData = dal.GetRowOldData(ItemID) 
				});
			}
			catch (Exception)
			{

				throw;
			}
		}

		[HttpPost]
        public IActionResult AddGoodReceiptGR(string formData)
        {
            try
            {
                GoodReceiptGRDal dal = new GoodReceiptGRDal();
                if (formData != null)
                {

                    ResponseModels response = dal.AddGoodReceiptGR(formData);
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