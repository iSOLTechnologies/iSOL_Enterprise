using iSOL_Enterprise.Dal;
using iSOL_Enterprise.Dal.Business;
using iSOL_Enterprise.Dal.Inventory;
using iSOL_Enterprise.Dal.Production;
using iSOL_Enterprise.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace iSOL_Enterprise.Controllers.Production
{
    public class BillOfMaterialController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult BillOfMaterialMaster(int id = 0)
        {
            ItemMasterDataDal Idal = new ItemMasterDataDal();
            BusinessPartnerMasterDataDal Bdal = new BusinessPartnerMasterDataDal();
            ViewData["GroupNum"] = new SelectList(Idal.GetListName(), "Value", "Text");
            ViewData["ProjectCode"] = new SelectList(Bdal.GetProjectCodes(), "Value", "Text");
            if (id > 0)
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

                BillOfMaterialDal dal = new BillOfMaterialDal();
                response.Data = dal.GetData();
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
                BillOfMaterialDal dal = new BillOfMaterialDal();
                int id = dal.GetId(guid);
                string? CardCode = dal.GetCardCode(guid);
                return Json(new
                {
                    success = true,
                    Header = dal.GetOldHeaderData(guid),
                    TabItems = dal.GetOldItemsData(CardCode)
                });

            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        public IActionResult AddUpdateBillOfMaterial(string formData)
        {
            try
            {
                BillOfMaterialDal dal = new BillOfMaterialDal();
                if (formData != null)
                {

                    ResponseModels response = dal.AddUpdateBillOfMaterial(formData);
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
