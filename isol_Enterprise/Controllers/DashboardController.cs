using iSOL_Enterprise.Dal;
using iSOL_Enterprise.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

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
	//public static class SessionExtensions
	//{
	//	public static void SetObjectAsJson(this ISession session, string key, object value)
	//	{
	//		session.SetString(key, JsonConvert.SerializeObject(value));
	//	}

	//	public static T? GetObjectFromJson<T>(this ISession session, string key)
	//	{
	//		try
	//		{
	//			var value = session.GetString(key);

	//			return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
	//		}
	//		catch (Exception ex)
	//		{
	//			return default(T?);

	//		}

	//	}
	//}
}
