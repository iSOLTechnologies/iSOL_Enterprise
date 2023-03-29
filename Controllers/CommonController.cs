using iSOL_Enterprise.Dal;
using iSOL_Enterprise.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SAP_MVC_DIAPI.BLC;
using SqlHelperExtensions;
using System.Data;

namespace iSOL_Enterprise.Controllers
{
    [Authorize]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class CommonController : Controller
    {


        public IActionResult GetBaseDocData(string cardcode, int BaseType)
        {
            ResponseModels response = new ResponseModels();
            try
            {

                CommonDal dal = new CommonDal();
                response.Data = dal.GetBaseDocData(cardcode, BaseType);
            }
            catch (Exception ex)
            {

                return Json(response);
            }


            return Json(response);
        }


        [HttpGet]
        public IActionResult GetBaseDocItemService(string DocId, int BaseType)
        {
            try
            {
                CommonDal dal = new CommonDal();

                return Json(new { baseDoc = dal.GetBaseDocType(DocId, BaseType), list = dal.GetBaseDocItemServiceList(DocId, BaseType) });
            }
            catch (Exception)
            {
                return Json(""); 
                throw;
            }

        }

        [HttpGet]
        public IActionResult GetBaseDocItemServiceReturn(string DocId, int BaseType)
        {
            try
            {
                CommonDal dal = new CommonDal();

                return Json(new { baseDoc = dal.GetBaseDocType(DocId, BaseType), list = dal.GetBaseDocItemServiceList_Return(DocId, BaseType) });
            }
            catch (Exception)
            {
                return Json(""); 
                throw;
            }

        }
        [HttpPost]
        public IActionResult PostToSap(string [] checkedIDs, int ObjectCode)
        {
            DIApiDal dal = new DIApiDal();
            ResponseModels model = new ResponseModels();
            model = dal.PostToSap(checkedIDs,ObjectCode);

            return Json(new { success = model.isSuccess, message = model.Message });
        }

        [HttpPost]
        public IActionResult PostPlanningSheet(string[] checkedIDs)
        {
            DIApiDal dal = new DIApiDal();
            ResponseModels model = new ResponseModels();
            model = dal.PostPlanningSheet(checkedIDs);

            return Json(new { success = model.isSuccess, message = model.Message });
        }
        [HttpPost]
        public IActionResult PostItemMasterData(string[] checkedIDs)
        {
            DIApiDal dal = new DIApiDal();
            ResponseModels model = new ResponseModels();
            model = dal.PostItemMasterData(checkedIDs);

            return Json(new { success = model.isSuccess, message = model.Message });
        }

        //[HttpPost]
        //public IActionResult PostGoodReceiptGR(string[] checkedIDs)
        //{
        //    DIApiDal dal = new DIApiDal();
        //    ResponseModels model = new ResponseModels();
        //    model = dal.PostGoodReceiptGR(checkedIDs);

        //    return Json(new { success = model.isSuccess, message = model.Message });
        //}


        [HttpGet]
        public IActionResult GetCountries()
        {
            try
            {

                CommonDal dal = new CommonDal();

                return Json(dal.GetCountries());
            }
            catch (Exception ex)
            {

                return Json(ex.Message);
            }
        }
        [HttpGet]
        public IActionResult GetCurrencies()
        {
            try
            {

                CommonDal dal = new CommonDal();

                return Json(dal.GetCurrencydata());
            }
            catch (Exception ex)
            {

                return Json(ex.Message);
            }
        }
        [HttpGet]
        public IActionResult GetCustomerData(string cardcode,string DocModule)
        {
            try
            {
                CommonDal dal = new CommonDal();
                ResponseModels model = new ResponseModels();
                model = dal.GetCustomerData(cardcode, DocModule);
                return Json(new {success = model.isSuccess , customer = model.Data });
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpGet]
        public IActionResult GetItemData(string itemcode, string DocModule)
        {
            try
            {
                CommonDal dal = new CommonDal();
                ResponseModels model = new ResponseModels();
                model = dal.GetItemData(itemcode, DocModule);
                return Json(new { success = model.isSuccess, item = model.Data });
            }
            catch (Exception)
            {

                throw;
            }
        }



        [HttpGet]
        public IActionResult GetSelectedWareHouseData(string ItemCode, string WhsCode)
        {
            try
            {

                CommonDal dal = new CommonDal();

                return Json(dal.GetSelectedWareHouseData(ItemCode, WhsCode));
            }
            catch (Exception ex)
            {

                return Json(ex.Message);
            }
        }
        [HttpGet]
        public IActionResult GetSaleOrders()
        {
            try
            {

                CommonDal dal = new CommonDal();

                return Json(dal.GetSaleOrders());
            }
            catch (Exception ex)
            {

                return Json(ex.Message);
            }
        }
    }
}
