using iSOL_Enterprise.Dal.Administrator;
using iSOL_Enterprise.Models;
using Microsoft.AspNetCore.Mvc;

namespace iSOL_Enterprise.Controllers.administrator
{
    public class CurrenciesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetData()
        {
            ResponseModels response = new ResponseModels();
            CurrenciesDal dal = new CurrenciesDal();
            try
            {
                response.Data = dal.Getdata();

            }
            catch (Exception)
            {

                throw;
            }
            return Json(response);
        }
    }
}
