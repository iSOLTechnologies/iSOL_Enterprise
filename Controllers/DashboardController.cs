using iSOL_Enterprise.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iSOL_Enterprise.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetNavAjax()
        {
            var Data = HttpContext.Session.GetObjectFromJson<_usersModels>("SessNav");
            return Json(Data);
        }
    }
}
