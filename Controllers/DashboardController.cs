using iSOL_Enterprise.Dal;
using iSOL_Enterprise.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iSOL_Enterprise.Controllers
{
    [Authorize]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetDocCounts() {
            DashboardDal dal = new DashboardDal();
            try
            {
                return Json(dal.GetDocCounts());
            }
            catch (Exception)
            {

                throw;
            }
        
        }
        public IActionResult GetNavAjax()
        {
            var Data = HttpContext.Session.GetObjectFromJson<_usersModels>("SessNav");
            return Json(Data);
        }
    }
}
