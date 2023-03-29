using iSOL_Enterprise.Controllers.Inventory;
using iSOL_Enterprise.Dal;
using iSOL_Enterprise.Dal.Inventory;
using iSOL_Enterprise.Dal.Inventory_Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace iSOL_Enterprise.Controllers.Inventory_Transactions
{
    [Authorize]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class GoodsIssueController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GoodsIssueMaster()
        {
            ItemMasterDataDal Idal = new ItemMasterDataDal(); 
            AdministratorDal dal = new AdministratorDal();

            ViewData["Series"] = dal.GetSeries(59);
            ViewData["MySeries"] = dal.GetMySeries(59);

            ViewData["GroupNum"] = new SelectList(Idal.GetListName(), "Value", "Text"); 

            return View();
        }

        [HttpPost]
        public IActionResult AddGoodsIssue(string formData)
        {
            try
            {


                GoodReceiptGRDal dal = new GoodReceiptGRDal();
                return formData == null ? Json(new { isInserted = false, message = "Data can't be null !" }) : dal.AddGoodReceiptGR(formData) == true ? Json(new { isInserted = true, message = "Good Receipt Added Successfully !" }) : Json(new { isInserted = false, message = "An Error occured !" });

            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
