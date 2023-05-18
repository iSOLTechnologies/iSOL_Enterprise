using iSOL_Enterprise.Dal;
using iSOL_Enterprise.Dal.Administrator;
using iSOL_Enterprise.Models;
using Microsoft.AspNetCore.Mvc;

namespace iSOL_Enterprise.Controllers.administrator
{
    public class AllApprovalsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetAllApprovals()
        {
            ResponseModels response = new ResponseModels();
            try
            {
                AllApprovalsDal dal = new AllApprovalsDal();
                response.Data = dal.GetAllApprovals();
            }
            catch (Exception ex)
            {

                return Json(response);
            }


            return Json(response);
        }
    }
}
