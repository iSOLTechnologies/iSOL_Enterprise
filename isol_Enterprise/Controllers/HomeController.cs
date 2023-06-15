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
using System.Data;
using iSOL_Enterprise.Common;
using Microsoft.AspNetCore.Http;

namespace iSOL_Enterprise.Controllers
{
    
    [ResponseCache(Location = ResponseCacheLocation.None,NoStore = true)]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
       
        IConfiguration _configuration;
        //private readonly HttpContextAccessor _accessor;
        private readonly IHttpContextAccessor context;

        public HomeController(IHttpContextAccessor context, ILogger<HomeController> logger, IConfiguration configuration)
        {
            this.context = context;
            _logger = logger;
            _configuration = configuration;
            //_accessor = accessor;
            SqlHelperExtensions.SqlHelper.defaultDB = configuration["ConnectionStrings:iSolConStr"].ToString();
            SqlHelperExtensions.SqlHelper.SAPIntegration = SqlHelper.ExecuteScalar(SqlHelper.defaultDB, CommandType.Text, @"SELECT TOP (1) [ManageSap] FROM tbl_SapIntegration").ToBool();
            if (SqlHelper.SAPIntegration)
            SqlHelperExtensions.SqlHelper.defaultSapDB = configuration["ConnectionStrings:SapConStr"].ToString();
            else
            SqlHelperExtensions.SqlHelper.defaultDB = configuration["ConnectionStrings:iSolConStr"].ToString();
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
             

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
                UsersModels input = new UsersModels();
                ResponseModels response = new ResponseModels();
                
                try
                {
                    string MachineName = Environment.MachineName.ToString();
                    
                    string guid = Guid.NewGuid() + DateTime.Now.ToString("mmddhhmmssms");
                    input.MachineName = MachineName;
                    input.IpAddress = new CommonDal().GetLocalIPAddress();
                    input.Guid = guid;
                    input.Username = Username;
                    input.Password = Password;

                    //response = new LoginDal().IsAlreadyLogin(input);

                    //if (!response.isSuccess)
                    //{
                    //    return Json(new { success = false, message = response.Message });
                    //}

                    UsersModels user = new LoginDal().Get(input);
                   

                    if ((user != null) && (user.Id > 0))
                    {
                        if (user.IsSession)
                        {

                            List<Claim> claims = new List<Claim>()
                            {
                            new Claim(ClaimTypes.NameIdentifier,Username ?? user.Username),

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

                            response.Data = true;
                            response.isError = false;
                            response.Message = "/Dashboard/Index";
                            AddValuesToSession(user);
                            new LoginDal().GenerateJson(input.Guid, user.Id, "");
                            
                        }
                        else
                        {
                            response.Data = false;
                            response.isError = true;
                            response.isLogin = false;
                            response.Message = "Session Not Created Successfully";
                          
                        }


                    }
                    else
                    {
                        response.Data = false;
                        response.isError = true;
                        response.Message = "Invalid Email or Password !";
                        
                    }
                }
                catch (Exception ex)
                {

                    response.isError = true;
                    response.Message = "An exception occured";
                }
                return Json(new { success = !(response.isError), message = response.Message });


                #region Login Model by Annas
                //LoginDal dal = new LoginDal();
                //bool LogRes = dal.ChkCredentials(Username, Password);

                //if (LogRes.Equals(false))
                //{
                //    return Json(new { success = false, message = "User not found !" });
                //}
                //else
                //{

                //    List<Claim> claims = new List<Claim>()
                //    {
                //       new Claim(ClaimTypes.NameIdentifier,Username ?? "ABC Name"),

                //    };

                //    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                //        CookieAuthenticationDefaults.AuthenticationScheme);

                //    AuthenticationProperties properties = new AuthenticationProperties()
                //    {
                //        AllowRefresh = true,
                //        IsPersistent = false
                //    };
                //    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                //        new ClaimsPrincipal(claimsIdentity), properties);
                //    return Json(new { success = true, message = "/Dashboard/Index" });
                //}
                #endregion
            }
            else
            return Json(new { success = false , message = "Username & password can't be empty !"});

            
        }


        
        public async Task<IActionResult> Logout()
        {
            //HttpContext.Session.Remove("Role");
            //HttpContext.Session.Clear();
            string Email = HttpContext.Session.GetString("Email");
            new LoginDal().ReomveSession(Email);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }



        public async void AddValuesToSession(UsersModels user)
        {

            context.HttpContext.Session.SetInt32("UserId", user.Id);
            context.HttpContext.Session.SetString("GoldenTicket", "True");
            context.HttpContext.Session.SetString("Email", user.Email);
            context.HttpContext.Session.SetString("Name", user.FirstName + ' ' + user.LastName);
            context.HttpContext.Session.SetString("FirstChar", user.FirstName.Substring(0, 1).ToUpper());
            context.HttpContext.Session.SetString("Contact", user.ContactNumber);
            context.HttpContext.Session.SetString("Gender", user.Gender);
            context.HttpContext.Session.SetString("UserPic", user.UserPic);
            context.HttpContext.Session.SetString("SessionId", user.Guid);
            context.HttpContext.Session.SetString("RoleCode", user.RoleCode);
            context.HttpContext.Session.SetString("RoleName", user.RoleName);
            context.HttpContext.Session.SetString("SessionTimeout", Convert.ToString( DateTime.Now.AddMinutes(60)));
          
            //SessionExpirationMiddleware.Email = user.Email;
            //SessionExpirationMiddleware.SessionTimeout = DateTime.Now.AddMinutes(1);

             _usersModels model = new _usersModels();


            model.listModules = new NavDal().getMenu(user.RoleCode);

            HttpContext.Session.SetObjectAsJson("SessNav", model);

            List<Claim> claims = new List<Claim>()
                    {
                       new Claim(ClaimTypes.NameIdentifier,user.Username ?? "ABC Name"),

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
            //    return Json(new { success = true, message = "/Dashboard/Index" });


           //  HttpContext.Session.SetObjectAsJson("SessNav", user.ListPages);


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