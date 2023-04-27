using iSOL_Enterprise.Dal;
using iSOL_Enterprise.Interface;
using iSOL_Enterprise.Models;
using iSOL_Enterprise.Models.Sale;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace iSOL_Enterprise.Controllers.sale
{
    public class PlanningSheetController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetPlanningSheetData()
        {

            ResponseModels response = new ResponseModels();
            try
            {
                PlanningSheetDal dal = new PlanningSheetDal();
                response.Data = dal.GetPlanningSheetData();
            }
            catch (Exception ex)
            {

                return Json(response);
            }


            return Json(response);
        }

        public IActionResult PlanningSheetMaster()
        {
            return View();
        }

        public IActionResult GetData(string SONumber)
        {
            try
            {
                IPlanningSheet dal = new PlanningQuerySheetDal();

                List<tbl_planningSheet> list = new List<tbl_planningSheet>();

                #region Line View Data
                list.Add(new tbl_planningSheet("ListPlanningDate", dal.PlanningDateHeader(SONumber)));
                list.Add(new tbl_planningSheet("PreCostingPlannedDate",dal.PreCostingPlannedDate(SONumber)));
                list.Add(new tbl_planningSheet("PreCostingActualDate",dal.PreCostingActualDate(SONumber)));
                list.Add(new tbl_planningSheet("POPlannedDate", dal.POPlannedDate(SONumber)));
                list.Add(new tbl_planningSheet("POActualDate",dal.POActualDate(SONumber)));
                list.Add(new tbl_planningSheet("AuditApprovalPlannedDate",dal.AuditApprovalPlannedDate(SONumber)));
                list.Add(new tbl_planningSheet("AuditApprovalActualDate", dal.AuditApprovalActualDate(SONumber)));
                list.Add(new tbl_planningSheet("YarnPurchasePlannedDate",dal.YarnPurchasePlannedDate(SONumber)));
                list.Add(new tbl_planningSheet("YarnPurchaseActualDate",dal.YarnPurchaseActualDate(SONumber)));
                list.Add(new tbl_planningSheet("YarnDeliveryPlannedDate",dal.YarnDeliveryPlannedDate(SONumber)));
                list.Add(new tbl_planningSheet("YarnDeliveryActualDate", dal.YarnDeliveryActualDate(SONumber)));
                list.Add(new tbl_planningSheet("YarnIssuanceForSizzingPlannedDate", dal.YarnIssuanceForSizzingPlannedDate(SONumber)));

                list.Add(new tbl_planningSheet("SizedYarnReceivedPlannedDate", dal.SizedYarnReceivedPlannedDate(SONumber)));

                list.Add(new tbl_planningSheet("SizedYarnIssuanceForGreigePlannedDate", dal.SizedYarnIssuanceForGreigePlannedDate(SONumber)));

                list.Add(new tbl_planningSheet("GreigeReceivingPlannedDate", dal.GreigeReceivingPlannedDate(SONumber)));
                list.Add(new tbl_planningSheet("GreigeReceivingActualDate", dal.GreigeReceivingActualDate(SONumber)));
                list.Add(new tbl_planningSheet("GreigeIssuanceForDyedPlannedDate", dal.GreigeIssuanceForDyedPlannedDate(SONumber)));
                list.Add(new tbl_planningSheet("GreigeIssuanceForDyedActualDate", dal.GreigeIssuanceForDyedActualDate(SONumber)));
                list.Add(new tbl_planningSheet("DyedReceivingPlannedDate", dal.DyedReceivingPlannedDate(SONumber)));
                list.Add(new tbl_planningSheet("DyedReceivingActualDate", dal.DyedReceivingActualDate(SONumber)));
                list.Add(new tbl_planningSheet("DyedIssuanceForProdPlannedDate", dal.DyedIssuanceForProdPlannedDate(SONumber)));
                list.Add(new tbl_planningSheet("DyedIssuanceForProdActualDate", dal.DyedIssuanceForProdActualDate(SONumber)));
                list.Add(new tbl_planningSheet("PackingPlannedDate", dal.PackingPlannedDate(SONumber)));
                list.Add(new tbl_planningSheet("PackingActualDate", dal.PackingActualDate(SONumber)));
                list.Add(new tbl_planningSheet("DeliveryNotePlannedDate", dal.DeliveryNotePlannedDate(SONumber)));
                list.Add(new tbl_planningSheet("DeliveryNoteActualDate", dal.DeliveryNoteActualDate(SONumber)));
                list.Add(new tbl_planningSheet("GatePassPlannedDate", dal.GatePassPlannedDate(SONumber)));
                list.Add(new tbl_planningSheet("GatePassActualDate", dal.GatePassActualDate(SONumber)));
                #endregion

                #region Header Data
                list.Add(new tbl_planningSheet("PlanningDateHeader", dal.PlanningDateHeader(SONumber)));
                list.Add(new tbl_planningSheet("StatusHeader", dal.StatusHeader(SONumber)));
                list.Add(new tbl_planningSheet("CustomerCodeHeader", dal.CustomerCodeHeader(SONumber)));
                list.Add(new tbl_planningSheet("CustomerNameHeader", dal.CustomerNameHeader(SONumber)));
                list.Add(new tbl_planningSheet("SaleOrderDateHeader", dal.SaleOrderDateHeader(SONumber)));
                list.Add(new tbl_planningSheet("ShipmentDateHeader", dal.ShipmentDateHeader(SONumber)));
                list.Add(new tbl_planningSheet("QuantityHeader", dal.QuantityHeader(SONumber)));                
                #endregion

                return Json(new { success = true ,DocData = JsonConvert.SerializeObject(list) }) ;
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public IActionResult AddPlanningSheet(string formData)
        {
            try
            {


                PlanningSheetDal dal = new PlanningSheetDal();
                return formData == null ? Json(new { isInserted = false, message = "Data can't be null !" }) : dal.AddPlanningSheet(formData) == true ? Json(new { isInserted = true, message = "Planning Sheet Added Successfully !" }) : Json(new { isInserted = false, message = "An Error occured !" });

            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
