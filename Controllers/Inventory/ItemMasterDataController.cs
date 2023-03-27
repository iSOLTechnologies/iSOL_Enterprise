﻿using iSOL_Enterprise.Dal;
using iSOL_Enterprise.Dal.Inventory;
using iSOL_Enterprise.Models;
using Microsoft.AspNetCore.Mvc;

namespace iSOL_Enterprise.Controllers.Inventory
{
    public class ItemMasterDataController : Controller
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
                ItemMasterDataDal dal = new ItemMasterDataDal();
                response.Data = dal.GetData();
            }
            catch (Exception ex)
            {

                return Json(response);
            }


            return Json(response);
        }
        public IActionResult ItemMasterDataMaster(int id)
        {

            ItemMasterDataDal dal = new ItemMasterDataDal();
            AdministratorDal addal = new AdministratorDal();
            
            ViewData["properties"] = dal.GetProperties();
            ViewData["Series"] = addal.GetSeries(4);
            ViewData["MySeries"] = addal.GetMySeries(4);
            ViewData["ItemGroup"] = dal.GetItemsGroup();
            ViewData["ListName"] = dal.GetListName();
            ViewData["Manufacturer"] = dal.GetManufacturer();
            ViewData["Shiptype"] = dal.GetShipType();
            ViewData["CustomsGroup"] = dal.GetCustomsGroup();
            ViewData["TaxGroup"] = dal.GetTaxGroup();
            ViewData["UomName"] = dal.GetUomName();
            if (id > 0)
            {
                ViewBag.OldItemId = id;
            }
            else
            ViewBag.OldItemId = null;

            return View();
        }
        public IActionResult EditItemMasterData(int id)
        {

            ItemMasterDataDal dal = new ItemMasterDataDal();
            AdministratorDal addal = new AdministratorDal();
            
            ViewData["properties"] = dal.GetProperties();
            ViewData["Series"] = addal.GetSeries(4);
            ViewData["ItemGroup"] = dal.GetItemsGroup();
            ViewData["ListName"] = dal.GetListName();
            ViewData["Manufacturer"] = dal.GetManufacturer();
            ViewData["Shiptype"] = dal.GetShipType();
            ViewData["CustomsGroup"] = dal.GetCustomsGroup();
            ViewData["TaxGroup"] = dal.GetTaxGroup();
            ViewData["UomName"] = dal.GetUomName();
            ViewBag.OldItemId = id;

            return View();
        }
        public IActionResult GetItemOldData(int ItemID)
        {
            try
            {
                ItemMasterDataDal dal = new ItemMasterDataDal();
                string ItemCode = CommonDal.GetItemCode(ItemID);
                return Json(new {success = true , OITMData =  dal.GetItemOldData(ItemID) , WHSData = CommonDal.GetWareHouseList(ItemCode)});

            }
            catch (Exception)
            {

                throw;
            }
        }
        public IActionResult GetNewItemCode(int Series)
        {
            try
            {
                ItemMasterDataDal dal = new ItemMasterDataDal();
                ResponseModels response = new ResponseModels();
                return Json(new { Success = true, ItemCode = dal.GetNewItemCode(Series) });

            }
            catch (Exception)
            {

                throw;
            }
        }
        public IActionResult GetWareHouseList()
        {

            ResponseModels response = new ResponseModels();
            try
            {

                ItemMasterDataDal dal = new ItemMasterDataDal();
                response.Data = dal.GetWareHouseList();
            }
            catch (Exception ex)
            {

                return Json(response);
            }


            return Json(response);
        }
        [HttpPost]
        public IActionResult AddItemMasterData(string formData)
        {
            try
            {
                ItemMasterDataDal dal = new ItemMasterDataDal();
                if (formData != null)
                {

                ResponseModels response = dal.AddItemMasterData(formData);
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
