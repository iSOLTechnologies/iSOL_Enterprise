using iSOL_Enterprise.Dal;
using iSOL_Enterprise.Models;
using Microsoft.AspNetCore.Mvc;

namespace iSOL_Enterprise.Controllers.administrator
{
    public class ApprovalsSetupController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetPagesApprovals()
        {
            ResponseModels response = new ResponseModels();
            try
            {
                AdministratorDal dal = new AdministratorDal();
                response.Data = dal.GetPagesApprovals();
            }
            catch (Exception ex)
            {

                return Json(response);
            }


            return Json(response);
        }
        [HttpPost]
        public IActionResult UpdateApprovalStatus(int id,string approvalType)
        {
            try
            {
                AdministratorDal dal = new AdministratorDal();
                if (dal.UpdateApprovalStatus(id, approvalType))
                {
                    return Json(new { success = true, message = "Saved Successfully !!" });
                }
                else
                    return Json(new { success = false, message = "An error Occured !!" });

            }
            catch (Exception)
            {
                return Json(new { success = false, message = "An error Occured !!" });
                throw;
            }
        }
    }
}
