using Microsoft.AspNetCore.Mvc;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using iSOL_Enterprise.Dal;
using iSOL_Enterprise.Models;

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

        public IActionResult GetData(int Id)
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
            ResponseModels response = new ResponseModels();
            UsersDal UsersDal = new UsersDal();
            string Name = HttpContext.Session.GetString("Name");
            int? UserId = HttpContext.Session.GetInt32("UserId");

            try
            {
                bool EmailCheck = CommonDal.Count("Users", "Email", input.Email);
                if (EmailCheck)
                {
                    response.isError = true;
                    response.Message = "Email already registered try different";
                    return Json(response);
                }

                input.Name = Name;
                input.CreatedOn = DateTime.Now;
                input.CreatedBy = UserId;
                input.WebRootPath = _env.WebRootPath;
                bool result = UsersDal.Add(input);
                if (result)
                {
                    response.isInserted = true;
                    response.Message = "Record Successfully Added!";
                }
                else
                {
                    response.isError = true;
                    response.Message = "Record could not be added";
                }
            }
            catch (Exception ex)
            {

                response.isError = true;
                response.Message = "An exception occured";
            }
            return Json(response);
        }

        public IActionResult Edit(_usersModels input)
        {
            ResponseModels response = new ResponseModels();
            UsersDal UsersDal = new UsersDal();
            string Name = HttpContext.Session.GetString("Name");
            int? UserId = HttpContext.Session.GetInt32("UserId");

            try
            {
                bool EmailCheck = CommonDal.CountOnEdit("Users", "Email", input.Email, input.Guid);
                if (EmailCheck)
                {
                    response.isError = true;
                    response.Message = "Email already registered try different";
                    return Json(response);
                }
                input.Name = Name;
                input.ModifiedOn = DateTime.Now;
                input.ModifiedBy = UserId;
                input.WebRootPath = _env.WebRootPath;
                bool result = UsersDal.Edit(input);
                if (result)
                {
                    response.isInserted = true;
                    response.Message = "Record Successfully Added!";
                }
                else
                {
                    response.isError = true;
                    response.Message = "Record could not be added";
                }
            }
            catch (Exception ex)
            {

                response.isError = true;
                response.Message = "An exception occured";
            }
            return Json(response);
        }

        public IActionResult Delete(int Id)
        {
            ResponseModels response = new ResponseModels();
            try
            {
                bool delete = CommonDal.Delete("Users", new KeyValuePair<string, string>("Id", Id.ToString()));
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
