using iSOL_Enterprise.Dal;
using iSOL_Enterprise.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SqlHelperExtensions;
using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace iSOL_Enterprise.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        IConfiguration _configuration;
        //private readonly HttpContextAccessor _accessor;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            //_accessor = accessor;
            SqlHelperExtensions.SqlHelper.defaultDB = configuration["ConnectionStrings:iSolConStr"].ToString();
            SqlHelperExtensions.SqlHelper.defaultSapDB = configuration["ConnectionStrings:SapConStr"].ToString();
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            SqlHelperExtensions.SqlHelper.defaultDB = _configuration["ConnectionStrings:iSolConStr"].ToString();
            SqlHelperExtensions.SqlHelper.defaultSapDB = _configuration["ConnectionStrings:SapConStr"].ToString();
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
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(string Username,string Password)
        {

            if (Username != null && Password != null)
            {

                LoginDal dal = new LoginDal();
                bool LogRes = dal.ChkCredentials(Username, Password);


                //tbl_user row = await Tbl_user.ChkCredentials(u.user_email_phone, u.user_password.Encrypt_password() ?? "");
                if (LogRes.Equals(false))
                {
                    return Json(new { success = false, message = "User not found !" });
                }
                else
                {
                    //    HttpContext.Session.SetString("Name", row.user_name ?? "Nothing");
                    //    HttpContext.Session.SetString("Role", row.user_role ?? "Nothing");
                    //    HttpContext.Session.SetString("UsrId", row.user_id.ToString());
                    //    return RedirectToAction("emailVerification", "Home");
                    //}
                    //else if (row.user_approved == false)
                    //{
                    //    return RedirectToAction("Waiting", "BusOwner");
                    //}
                    //else
                    //{
                    //HttpContext.Session.SetString("Name", row.user_name ?? "Nothing");
                    //HttpContext.Session.SetString("Role", row.user_role ?? "Nothing");
                    //HttpContext.Session.SetString("UsrId", row.user_id.ToString());
                    List<Claim> claims = new List<Claim>()
                    {
                       new Claim(ClaimTypes.NameIdentifier,Username ?? "ABC Name"),
                        
                    };

                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                        CookieAuthenticationDefaults.AuthenticationScheme);

                    AuthenticationProperties properties = new AuthenticationProperties()
                    {
                        AllowRefresh = true,
                        IsPersistent = false
                    };
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity), properties);
                    return Json(new { success = true, message = "/Dashboard/Index" });
                }

            }
            else
            return Json(new { success = false , message = "Username & password can't be empty !"});
           





            //ResponseModels response = new ResponseModels();
            //try
            //{
            //    string MachineName = Environment.MachineName.ToString();
            //  //  input.IpAddress = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString();
            //    string guid = Guid.NewGuid() + DateTime.Now.ToString("mmddhhmmssms");
            //    input.MachineName = MachineName;
            //    input.IpAddress = input.IpAddress;
            //    input.Guid = guid;
            //    UsersModels user = new LoginDal().Get(input); //For easy <commented byAnnas>  
            //    //UsersModels user = new UsersModels()
            //    //{
            //    //    Id = 101,
            //    //    FirstName = "Muhammad Annas",
            //    //    LastName = "Raza",
            //    //    RoleCode = "",
            //    //    Guid = guid,
            //    //    UserPic = "Assets/images/img.png",
            //    //    Gender = "Male",
            //    //    ContactNumber = "0123",
            //    //    Email="annas@gmail.com",
            //    //    IsSession =true

            //    //};

            //    if ((user != null) && (user.Id > 0))
            //    {
            //        if (user.IsSession)
            //        {
            //            response.Data = true;
            //            AddValuesToSession(user);
            //            new LoginDal().GenerateJson(input.Guid, user.Id,"");

            //        }
            //        else
            //        {
            //            response.Data = false;
            //            response.isLogin = false;
            //            response.Message = "Session Not Created Successfully";
            //        }


            //    }
            //    else
            //    {
            //        response.Data = false;
            //        response.isError = true;
            //        response.Message = "Username and Password do not match";
            //    }
            //}
            //catch (Exception ex)
            //{

            //    response.isError = true;
            //    response.Message = "An exception occured";
            //}
            //return Json(response);
        }


        
        public async Task<IActionResult> Logout()
        {
            //HttpContext.Session.Remove("Role");
            //HttpContext.Session.Clear();
             
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
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


            model.listModules = new NavDal().getMenu(user.RoleCode);

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
        //public IActionResult Logout()
        //{
        //    int? id = HttpContext.Session.GetInt32("UserId");

        //    SqlHelper.ExecuteNonQuery("update Users set IsLoggedIn=0 where id = @id", new SqlParameter("@id", id));

        //    HttpContext.Session.Clear();

        //    return Json("Success");
        //}
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