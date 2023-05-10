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

                return Json(new { baseDoc = dal.GetBaseDocType(DocId, BaseType), list = dal.GetBaseDocItemServiceList(DocId, BaseType) , header = dal.GetBaseDocHeaderData(DocId, BaseType) });
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

                return Json(new { baseDoc = dal.GetBaseDocType(DocId, BaseType), list = dal.GetBaseDocItemServiceList_Return(DocId, BaseType)  , header = dal.GetBaseDocHeaderData(DocId, BaseType) });
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

        [HttpPost]
        public IActionResult PostGoodReceiptGR(string[] checkedIDs , int BaseType = 0)
        {
            DIApiDal dal = new DIApiDal();
            ResponseModels model = new ResponseModels();
            model = dal.PostGoodReceiptGR(checkedIDs,59,BaseType);

            return Json(new { success = model.isSuccess, message = model.Message });
        }
        [HttpPost]
        public IActionResult PostGoodIssue(string[] checkedIDs, int BaseType = 0)
        {
            DIApiDal dal = new DIApiDal();
            ResponseModels model = new ResponseModels();
            model = dal.PostGoodIssue(checkedIDs,60,BaseType);

            return Json(new { success = model.isSuccess, message = model.Message });
        }
        [HttpPost]
        public IActionResult PostInventoryTransfer(string[] checkedIDs)
        {
            DIApiDal dal = new DIApiDal();
            ResponseModels model = new ResponseModels();
            model = dal.PostInventoryTransfer(checkedIDs,67);

            return Json(new { success = model.isSuccess, message = model.Message });
        }
        [HttpPost]
        public IActionResult PostInventoryTransferRequest(string[] checkedIDs)
        {
            DIApiDal dal = new DIApiDal();
            ResponseModels model = new ResponseModels();
            model = dal.PostInventoryTransferRequest(checkedIDs, 1250000001);

            return Json(new { success = model.isSuccess, message = model.Message });
        }
        [HttpPost]
        public IActionResult PostBusinnesPartner(string[] checkedIDs)
        {
            DIApiDal dal = new DIApiDal();
            ResponseModels model = new ResponseModels();
            model = dal.PostBusinnesPartner(checkedIDs, 2);

            return Json(new { success = model.isSuccess, message = model.Message });
        }
        [HttpPost]
        public IActionResult PostBOM(string[] checkedIDs)
        {
            DIApiDal dal = new DIApiDal();
            ResponseModels model = new ResponseModels();
            model = dal.PostBOM(checkedIDs);

            return Json(new { success = model.isSuccess, message = model.Message });
        }
        [HttpPost]
        public IActionResult PostProductionOrder(string[] checkedIDs)
        {
            DIApiDal dal = new DIApiDal();
            ResponseModels model = new ResponseModels();
            model = dal.PostProductionOrder(checkedIDs);

            return Json(new { success = model.isSuccess, message = model.Message });
        }

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
        [HttpGet]
        public IActionResult GetProdGuid(string ItemCode)
        {
            try
            {

                CommonDal dal = new CommonDal();
                
                return Json(new { success = true ,guid = dal.GetProdGuid(ItemCode) });
            }
            catch (Exception ex)
            {

                return Json(ex.Message);
            }
        }
    }
}
