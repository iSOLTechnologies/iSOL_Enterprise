using iSOL_Enterprise.Dal;
using iSOL_Enterprise.Models;
using iSOL_Enterprise.Models.Series;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace iSOL_Enterprise.Controllers.administrator
{

    [Authorize]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class MySeriesController : Controller
    {
        public IActionResult Index()
        {
            AdministratorDal dal = new AdministratorDal();
            ViewBag.SeriesDrpDwn = new SelectList(dal.GetSeriesDrpDwn(), "ObjectCode", "PageName");
            return View();
        }

        [HttpPost]
        public IActionResult AddSeries(tbl_NNM1 obj)
        {
            AdministratorDal dal = new AdministratorDal();
            bool result = dal.InsertSeries(obj);
            return RedirectToAction("Index");
        }
         
        public IActionResult GetNNM1(string ObjectCode)
        {
            ResponseModels response = new ResponseModels();
            try
            {
                AdministratorDal dal = new AdministratorDal();
                response.Data = dal.GetNNM1(ObjectCode);
            }
            catch (Exception)
            {

                return Json(response);
            }


            return Json(response);
        }

    }
}
