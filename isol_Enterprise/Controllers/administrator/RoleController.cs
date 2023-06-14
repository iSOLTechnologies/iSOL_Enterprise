using iSOL_Enterprise.Dal;
using iSOL_Enterprise.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        public IActionResult GetALlRoles()
        {
            ResponseModels response = new ResponseModels();
            try
            {
                RoleDal dal = new();
                response.Data = dal.GetAllRole();

            }
            catch (Exception ex)
            {

                return Json(response);
            }


            return Json(response);
        }
        public IActionResult GetPages()
        {
            ResponseModels models = new ResponseModels();
            RoleDal dal = new RoleDal();
            try
            {
                models.Data = dal.GetPages();
                return Json(JsonConvert.SerializeObject(models.Data));
            }
            catch (Exception ex)
            {

                return Json(models);
            }
        }
        public IActionResult GetRolePages(string RoleCode)
        {
            ResponseModels models = new ResponseModels();
            RoleDal dal = new RoleDal();
            try
            {
                models.Data = dal.GetPages(RoleCode);
                return Json(JsonConvert.SerializeObject(models.Data));
            }
            catch (Exception ex)
            {

                return Json(models);
            }
        }


        public IActionResult Add(string formData)
        {
            ResponseModels response = new ResponseModels();
            RoleDal RoleDal = new RoleDal();
            string Name = HttpContext.Session.GetString("Name");
            int? UserId = HttpContext.Session.GetInt32("UserId");

            try
            {


                response = RoleDal.Add(formData,Name,UserId);
                if (response.isSuccess)
                {
                    response.isInserted = true;
                    response.Message = "Role Successfully Added!";
                }
                else
                {
                    response.isError = true;
                    response.Message = response.Message;
                }
            }
            catch (Exception ex)
            {

                response.isError = true;
                response.Message = "An exception occured";
            }
            return Json(response);
        }

        public IActionResult Edit(string formData)
        {
            ResponseModels response = new ResponseModels();
            RoleDal RoleDal = new RoleDal();
            string Name = HttpContext.Session.GetString("Name");
            int? UserId = HttpContext.Session.GetInt32("UserId");

            try
            {

                response = RoleDal.Edit(formData, Name, UserId);
                if (response.isSuccess)
                {
                    response.isInserted = true;
                    response.Message = "Role Updated Successfully !";
                }
                else
                {
                    response.isError = true;
                    response.Message = response.Message;
                }

                
            }
            catch (Exception ex)
            {

                response.isError = true;
                response.Message = "An exception occured";
            }
            return Json(response);
        }

        public IActionResult GetRoleData(string Guid)
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
