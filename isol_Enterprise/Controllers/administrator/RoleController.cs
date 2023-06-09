using iSOL_Enterprise.Dal;
using iSOL_Enterprise.Models;
using Microsoft.AspNetCore.Mvc;

namespace iSOL_Enterprise.Controllers.administrator
{
    public class RoleController : Controller
    {
        public IActionResult Index()
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

        public IActionResult RolesMaster()
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

        public IActionResult GetPages()
        {
            ResponseModels models = new ResponseModels();
            RoleDal dal = new RoleDal();
            try
            {
                models.Data = dal.GetPages();
                return Json(models);
            }
            catch (Exception ex)
            {

                return Json(models);
            }
        }


        public IActionResult Add(RoleModels input)
        {
            ResponseModels response = new ResponseModels();
            RoleDal RoleDal = new RoleDal();
            string Name = HttpContext.Session.GetString("Name");
            int? UserId = HttpContext.Session.GetInt32("UserId");

            try
            {
                bool RoleNameCheck = CommonDal.Count("Roles", "RoleName", input.RoleName);
                if (RoleNameCheck)
                {
                    response.isError = true;
                    response.Message = "RoleName already registered try different";
                    return Json(response);
                }
                input.CreatedOn = DateTime.Now;
                input.CreatedBy = UserId;
                bool result = RoleDal.Add(input);
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

        public IActionResult Edit(RoleModels input)
        {
            ResponseModels response = new ResponseModels();
            RoleDal RoleDal = new RoleDal();
            string Name = HttpContext.Session.GetString("Name");
            int? UserId = HttpContext.Session.GetInt32("UserId");

            try
            {
                bool RoleNameCheck = CommonDal.CountOnEdit("Roles", "RoleName", input.RoleName, input.Guid);
                if (RoleNameCheck)
                {
                    response.isError = true;
                    response.Message = "Role Name already registered try different";
                    return Json(response);
                }
                input.ModifiedOn = DateTime.Now;
                input.ModifiedBy = UserId;
                bool result = RoleDal.Edit(input);
                if (result)
                {
                    response.isInserted = true;
                    response.Message = "Record Successfully Updated!";
                }
                else
                {
                    response.isError = true;
                    response.Message = "Record could not be Updated";
                }
            }
            catch (Exception ex)
            {

                response.isError = true;
                response.Message = "An exception occured";
            }
            return Json(response);
        }

        public IActionResult GetData(string Guid)
        {
            ResponseModels models = new ResponseModels();
            try
            {
                RoleDal RoleDal = new RoleDal();
                models.Data = RoleDal.Get(Guid);
                return Json(models);
            }
            catch (Exception ex)
            {

                return Json(models);
            }

        }

        public IActionResult Delete(string Guid)
        {
            ResponseModels response = new ResponseModels();
            try
            {
                bool delete = CommonDal.Delete("Roles", new KeyValuePair<string, string>("Guid", Guid));
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
