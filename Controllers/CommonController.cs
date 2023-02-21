using iSOL_Enterprise.Dal;
using iSOL_Enterprise.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SqlHelperExtensions;
using System.Data;

namespace iSOL_Enterprise.Controllers
{
    [Authorize]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class CommonController : Controller
    {


        public IActionResult GetBaseDocData(int cardcode, int BaseType)
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
        public IActionResult GetBaseDocItemService(int DocId, int BaseType)
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

    }
}
