using iSOL_Enterprise.Controllers.Inventory;
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
    public class GoodsIssueController : Controller
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
                GoodsIssueDal dal = new GoodsIssueDal();
                response.Data = dal.GetData();
            }
            catch (Exception ex)
            {
                return Json(response);
            }
            return Json(response);
        }

        public IActionResult GoodsIssueMaster(int id = 0)
        {
            ItemMasterDataDal Idal = new ItemMasterDataDal(); 
            AdministratorDal dal = new AdministratorDal();

            ViewData["Series"] = dal.GetSeries(60);
            ViewData["MySeries"] = dal.GetMySeries(60);
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
                GoodsIssueDal dal = new GoodsIssueDal();

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
        public IActionResult AddGoodIssue(string formData)
        {
            try
            {
                GoodsIssueDal dal = new GoodsIssueDal();
                if (formData != null)
                {

                    ResponseModels response = dal.AddGoodsIssue(formData);
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
