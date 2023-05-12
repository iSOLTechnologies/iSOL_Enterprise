﻿using iSOL_Enterprise.Dal.Inventory;
using iSOL_Enterprise.Dal.Production;
using iSOL_Enterprise.Dal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using iSOL_Enterprise.Models;

namespace iSOL_Enterprise.Controllers.Production
{
    public class IssueForProductionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult IssueForProductionMaster(string id = "")
        {
            ItemMasterDataDal Idal = new ItemMasterDataDal();
            AdministratorDal dal = new AdministratorDal();
            ViewData["Series"] = dal.GetSeries(60);
            ViewData["MySeries"] = dal.GetMySeries(60);
            ViewData["GroupNum"] = new SelectList(Idal.GetListName(), "Value", "Text");
            if (id != "")
            {
                ViewBag.OldId = id;
            }
            else
                ViewBag.OldId = 0;

            return View();
        }
        [HttpGet]
        public IActionResult GetData()
        {
            ResponseModels response = new ResponseModels();
            try
            {

                IssueForProductionDal dal = new IssueForProductionDal();
                response.Data = dal.GetData();
            }
            catch (Exception ex)
            {

                return Json(response);
            }


            return Json(response);
        }
        public IActionResult GetProductionOrders()
        {
            ResponseModels response = new ResponseModels();
            try
            {

                ReceiptFromProductionDal dal = new ReceiptFromProductionDal();
                response.Data = dal.GetProductionOrdersData();
            }
            catch (Exception ex)
            {

                return Json(response);
            }


            return Json(response);
        }

        public IActionResult GetOldData(string guid)
        {
            try
            {
                IssueForProductionDal dal = new IssueForProductionDal();
                int id = dal.GetId(guid);

                return Json(new
                {
                    success = true,
                    HeaderData = dal.GetOldHeaderData(id),
                    RowData = dal.GetOldItemsData(id)
                });

            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpPost]
        public IActionResult AddUpdateIssueForProduction(string formData)
        {
            try
            {
                IssueForProductionDal dal = new IssueForProductionDal();
                if (formData != null)
                {

                    ResponseModels response = dal.AddUpdateIssueForProduction(formData);
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