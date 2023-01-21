using iSOL_Enterprise.Dal;
using iSOL_Enterprise.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SqlHelperExtensions;
using System;
using System.Data.SqlClient;
using System.Diagnostics;

namespace iSOL_Enterprise.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        IConfiguration _configuration;
      //  private readonly HttpContextAccessor _accessor;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            //_accessor = accessor;
            SqlHelperExtensions.SqlHelper.defaultDB = configuration["ConnectionStrings:ConStr"].ToString();
        }

        public IActionResult Index()
        {
            SqlHelperExtensions.SqlHelper.defaultDB = _configuration["ConnectionStrings:ConStr"].ToString();
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult ChangePassword(UsersModels input)
        {
            ResponseModels models = new ResponseModels();
            try
            {

                LoginDal loginDal = new LoginDal();
                models = loginDal.ChangePassword(input);
                return Json(models);
            }
            catch (Exception ex)
            {
                models.isError = true;
                return Json(models);
            }

        }

        public IActionResult Login(UsersModels input)
        {
            ResponseModels response = new ResponseModels();
            try
            {
                string MachineName = Environment.MachineName.ToString();
              //  input.IpAddress = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString();
                string guid = Guid.NewGuid() + DateTime.Now.ToString("mmddhhmmssms");
                input.MachineName = MachineName;
                input.IpAddress = input.IpAddress;
                input.Guid = guid;
                //UsersModels user = new LoginDal().Get(input); //For easy <commented byAnnas>  
                UsersModels user = new UsersModels()
                {
                    Id = 101,
                    FirstName = "Muhammad Annas",
                    LastName = "Raza",
                    RoleCode = "",
                    Guid = guid,
                    UserPic = "Assets/images/img.png",
                    Gender = "Male",
                    ContactNumber = "0123",
                    Email="annas@gmail.com",
                    IsSession =true

                };

                if ((user != null) && (user.Id > 0))
                {
                    if (user.IsSession)
                    {
                        response.Data = true;
                        AddValuesToSession(user);
                        new LoginDal().GenerateJson(input.Guid, user.Id,"");

                    }
                    else
                    {
                        response.Data = false;
                        response.isLogin = false;
                        response.Message = "Session Not Created Successfully";
                    }


                }
                else
                {
                    response.Data = false;
                    response.isError = true;
                    response.Message = "Username and Password do not match";
                }
            }
            catch (Exception ex)
            {

                response.isError = true;
                response.Message = "An exception occured";
            }
            return Json(response);
        }


        public void AddValuesToSession(UsersModels user)
        {
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("GoldenTicket", "True");
            HttpContext.Session.SetString("Email", user.Email);
            HttpContext.Session.SetString("Name", user.FirstName + ' ' + user.LastName);
            HttpContext.Session.SetString("Contact", user.ContactNumber);
            HttpContext.Session.SetString("Gender", user.Gender);
            HttpContext.Session.SetString("UserPic", user.UserPic);
            HttpContext.Session.SetString("SessionId", user.Guid);
            HttpContext.Session.SetString("RoleCode", user.RoleCode);

            _usersModels model = new _usersModels();


            model.ListPages = new NavDal().getMenu(user.RoleCode);

            HttpContext.Session.SetObjectAsJson("SessNav", model);
            // HttpContext.Session.SetObjectAsJson("SessNav", user.ListPages);


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public void RenewSession()
        {
            HttpContext.Session.SetString("GoldenTicket", "True");
        }

        protected void Session_End(object sender, EventArgs e)
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("GoldenTicket");
        }
        public ActionResult CheckIfSessionValid()
        {
            if (HttpContext.Session.GetString("GoldenTicket") == null)
            {
                HttpContext.Session.Clear();
                HttpContext.Session.Remove("GoldenTicket");
                return Json("False");
            }

            return Json("True");
        }
        public IActionResult Logout()
        {
            int? id = HttpContext.Session.GetInt32("UserId");

            SqlHelper.ExecuteNonQuery("update Users set IsLoggedIn=0 where id = @id", new SqlParameter("@id", id));

            HttpContext.Session.Clear();

            return Json("Success");
        }
    }

    public static class SessionExtensions
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}