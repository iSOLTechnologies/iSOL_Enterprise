using Microsoft.AspNetCore.Mvc;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using iSOL_Enterprise.Dal;
using iSOL_Enterprise.Models;
using iSOL_Enterprise.Dal.Sale;

namespace iSOL_Enterprise.Controllers.administrator
{
    public class UsersController : Controller
    {

        IHostingEnvironment _env;

        public UsersController(IHostingEnvironment environment)
        {
            _env = environment;
        }


        public IActionResult Index()
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetInt32("UserId").ToString()) || string.IsNullOrEmpty(HttpContext.Request.Query["Source"]))
                {
                    return RedirectToAction("index", "Home");
                }
                if (CommonDal.CheckPage(HttpContext.Request.Query["Source"], HttpContext.Session.GetInt32("UserId")))
                {

                    return View();
                }
                return RedirectToAction("index", "Dashboard");
            }
            catch (Exception ex)
            {
                return RedirectToAction("index", "Home");

            }
        }

        public IActionResult UserMaster()
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetInt32("UserId").ToString()) || string.IsNullOrEmpty(HttpContext.Request.Query["Source"]))
                {
                    return RedirectToAction("index", "Home");
                }
                //if (CommonDal.CheckPage(HttpContext.Request.Query["Source"], HttpContext.Session.GetInt32("UserId")))
                //{

                //}
                _usersModels model = new _usersModels();
                model.ListRoles = new UsersDal().ListRoles();
              
                return View(model);
                //  return RedirectToAction("index", "Dashboard");
            }
            catch (Exception ex)
            {
                return RedirectToAction("index", "Home");

            }
        }
        public IActionResult ChangePassword()
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetInt32("UserId").ToString()) || string.IsNullOrEmpty(HttpContext.Request.Query["Source"]))
                {
                    return RedirectToAction("index", "Home");
                }
               
              
                return View();
                
            }
            catch (Exception ex)
            {
                return RedirectToAction("index", "Home");

            }
        }

        public IActionResult GetALlUsers()
        {
            ResponseModels response = new ResponseModels();
            try
            {
                UsersDal dal = new ();
                response.Data = dal.GetAllUsers();

            }
            catch (Exception ex)
            {

                return Json(response);
            }


            return Json(response);
        }
        public IActionResult GetData(string Id)
        {
            ResponseModels models = new ResponseModels();
            try
            {
                UsersDal UsersDal = new UsersDal();
                models.Data = UsersDal.GetDataByUser(Id);
                return Json(models);
            }
            catch (Exception ex)
            {

                return Json(models);
            }

        }
        public IActionResult GetAllUsersByRole(string RoleCode)
        {
            ResponseModels models = new ResponseModels();
            try
            {
                UsersDal UsersDal = new UsersDal();
                models.Data = UsersDal.GetAllUsersByRole(RoleCode);
                return Json(models);
            }
            catch (Exception ex)
            {

                return Json(models);
            }

        }

        public IActionResult Add(_usersModels input)
        {

            try
            {
                UsersDal dal = new ();
                if (input.FirstName != null && input.Password != null)
                {
                    string Name = HttpContext.Session.GetString("Name");
                    int? UserId = HttpContext.Session.GetInt32("UserId");

                    input.Name = Name;
                    input.CreatedOn = DateTime.Now;
                    input.CreatedBy = UserId;
                    input.WebRootPath = _env.WebRootPath;

                    ResponseModels response = dal.Add(input);

                    return Json(new { isInserted = response.isSuccess, Message = response.Message });
                }
                else
                {
                    return Json(new { isInserted = false, Message = "Data can't be null" });
                }
            }
            catch (Exception)
            {
                throw;
            }


            
        }

        public IActionResult Edit(_usersModels input)
        {
            ResponseModels response = new ResponseModels();
            UsersDal dal = new UsersDal();
            

            try
            {

                if (input.FirstName != null && input.RoleCode != null)
                {
                    string Name = HttpContext.Session.GetString("Name");
                    int? UserId = HttpContext.Session.GetInt32("UserId");

                    input.Name = Name;
                    input.ModifiedOn = DateTime.Now;
                    input.ModifiedBy = UserId;
                    input.WebRootPath = _env.WebRootPath;

                     response = dal.Edit(input);

                    return Json(new { isInserted = response.isSuccess, Message = response.Message });
                }
                else
                {
                    return Json(new { isInserted = false, Message = "Data can't be null" });
                }

            }
            catch (Exception ex)
            {

                response.isError = true;
                response.Message = "An exception occured";
            }
            return Json(response);
        }
        [HttpPost]
        public IActionResult ChangePassword(string formData)
        {
            try
            {
                UsersDal dal = new UsersDal();
                if (formData != null)
                {

                    ResponseModels response = dal.ChangePassword(formData);
                    return Json(new { isInserted = response.isSuccess, Message = response.Message });
                }
                else
                {
                    return Json(new { isInserted = false, Message = "Data can't be null" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IActionResult Delete(string Id)
        {
            ResponseModels response = new ResponseModels();
            try
            {
                bool delete = CommonDal.Delete("Users", new KeyValuePair<string, string>("Guid", Id.ToString()));
                if (delete)
                {
                    response.isDeleted = true;
                    response.Message = "Record Successfully Deleted!";
                }
                else
                {
                    response.isError = true;
                    response.Message = "Record could not be Deleted";
                }
            }
            catch (Exception)
            {

                response.isError = true;
                response.Message = "An exception occured";
            }
            return Json(response);
        }

    }
}
