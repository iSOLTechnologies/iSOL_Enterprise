using iSOL_Enterprise.Dal;
using iSOL_Enterprise.Models;
using iSOL_Enterprise.Models.Sale;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SqlHelperExtensions;
using System.Data;

namespace iSOL_Enterprise.Controllers
{
    [Authorize]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class DocSeriesController : Controller
    {

            public IActionResult Index()
        {
            return View();
        }



        public IActionResult GetDocSeries()
        {
            ResponseModels response = new ResponseModels();
            try
            {
                AdministratorDal dal = new AdministratorDal();
                response.Data = dal.GetDocSeries();
            }
            catch (Exception ex)
            {

                return Json(response);
            }


            return Json(response);
        }
        [HttpPost]
        public IActionResult UpdateSeries(int objCode , int series)
        {
            try
            {
                AdministratorDal dal = new AdministratorDal();
                if (dal.UpdateSeries(objCode,series))
                {
                    return Json(new { success = true , message = "Series Updated Successfully !!" }) ;
                }
                else
                    return Json(new { success = false, message = "An error Occured !!" });

            }
            catch (Exception)
            {
                return Json(new { success = false, message = "An error Occured !!" });
                throw;
            }
        }
         

    }
}
