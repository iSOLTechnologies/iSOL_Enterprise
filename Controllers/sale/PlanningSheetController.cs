using iSOL_Enterprise.Dal;
using iSOL_Enterprise.Interface;
using Microsoft.AspNetCore.Mvc;

namespace iSOL_Enterprise.Controllers.sale
{
    public class PlanningSheetController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult GetData(string SONumber)
        {
            try
            {
                IPlanningSheet dal = new PlanningSheetDal();

                dal.AuditApprovalActualDate(SONumber);
                dal.AuditApprovalPlannedDate(SONumber);
                dal.DeliveryNoteActualDate(SONumber);
                dal.DeliveryNotePlannedDate(SONumber);
                dal.DyedIssuanceForProdActualDate(SONumber);
                dal.DyedIssuanceForProdPlannedDate(SONumber);
                dal.CustomerCodeHeader(SONumber);
                dal.CustomerNameHeader(SONumber);
                
                return Json(new {});
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
