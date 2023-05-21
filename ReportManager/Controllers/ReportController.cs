using ReportManager.Dal;
using ReportManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReportManager.Controllers
{
    public class ReportController : Controller
    {
        public ActionResult Index()
        {

            try
            {


                return View();
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpGet]
        public JsonResult AccesoriesConsumptionReport(int DocNum)
        {
            try
            {
                ReportDal model = new ReportDal();
                Document_MasterModel input = new Document_MasterModel();                
                input.DocNum = DocNum;

                ResponseModels response = model.GenerateAccesoriesConsumptionReport(input);

                return Json(response , JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpGet]
        public JsonResult PurchaseOrderReports(string DocNum, string ReportName)
        {
            try
            {
                ReportDal model = new ReportDal();
                Document_MasterModel input = new Document_MasterModel();                
                input.DocNum = DocNum;

                ResponseModels response = model.PurchaseOrderReport(DocNum,ReportName);

                return Json(response , JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpGet]
        public JsonResult PurchaseTrackingReportV1(int DocNum)
        {
            try
            {
                ReportDal model = new ReportDal();
                Document_MasterModel input = new Document_MasterModel();                
                input.DocNum = DocNum;

                ResponseModels response = model.GeneratePurchaseTrackingReportV1(input);

                return Json(response , JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpGet]
        public JsonResult SaleOrderWiseYarnReport(DateTime DateFrom,DateTime DateTo)
        {
            try
            {
                ReportDal model = new ReportDal();
                Document_MasterModel input = new Document_MasterModel();                
                input.DateFrom = DateFrom;
                input.DateTo = DateTo;

                ResponseModels response = model.GenerateSaleOrderWiseYarnReport(input);

                return Json(response , JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}