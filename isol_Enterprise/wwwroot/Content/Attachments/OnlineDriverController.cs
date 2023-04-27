using Module_Vcc.BLL;
using Module_Vcc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Module_Vcc.Controllers
{
    public class OnlineDriversController : Controller
    {
        // GET: OnlineDriver
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult DetailView()
        {
            //driverActiveLogBll dalbll = new driverActiveLogBll();
            //ViewBag.isLoggedIn = dalbll.isLoggedIn(driverno);
            return View();
        }
        [HttpGet]
        public ActionResult GetDriverslink()
        {

            ReturnModel result = new ReturnModel();
            DriverBll driverBll = new DriverBll();
            try
            {
                result.data = driverBll.getlink();
                result.msg = "success";
            }
            catch (Exception ex)
            {
                result.msg = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetDrivers()
        {

            ReturnModel result = new ReturnModel();
            DriverBll driverBll = new DriverBll();
            try
            {
                if (Session["fk_retypemst_reftype"].ToString() == "COM" || Session["fk_retypemst_reftype"].ToString() == "CL")
                {
                    result.data = driverBll.GetDataByCC(Session["client"].ToString(), Session["cocode"].ToString());
                }
                else
                {
                    result.data = driverBll.GetDataByType(Session["client"].ToString(), Session["cocode"].ToString(), Session["userid"].ToString(), Session["fk_retypemst_reftype"].ToString());
                }
                result.msg = "success";
                return Json(result, JsonRequestBehavior.AllowGet);
                //switch (Session["usertype"].ToString())
                //{
                //    case "TA":
                //        result.data = driverBll.getByTransporter(Session["userid"].ToString());
                //        result.msg = "success";
                //        return Json(result, JsonRequestBehavior.AllowGet);
                //    case "S":
                //        result.data = driverBll.get();
                //        result.msg = "success";
                //        return Json(result, JsonRequestBehavior.AllowGet);
                //    case "DA":
                //        result.data = driverBll.getByDriver(Session["userid"].ToString());
                //        result.msg = "success";
                //        return Json(result, JsonRequestBehavior.AllowGet);
                //    default:
                //        result.data = driverBll.get();
                //        result.msg = "success";
                //        return Json(result, JsonRequestBehavior.AllowGet);


                //}

            }
            catch (Exception ex)
            {
                result.msg = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Paginate(int pageNumber, int numberOfRecords)
        {

            ReturnModel result = new ReturnModel();
            DriverBll driverBll = new DriverBll();
            try
            {
                result.data = driverBll.Paginate(pageNumber, numberOfRecords);
                result.msg = "success";


            }
            catch (Exception ex)
            {
                result.msg = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult SearchDriver(string driverno = "", string driverfullname = "", string primarycontactnumber = "", string fk_vehicleobjectype_objecttypeno = "", string mobilenumber = "", string objectcategoryltxt = "", string fk_transpotmst_vendno = "")
        {

            ReturnModel result = new ReturnModel();
            DriverBll driverBll = new DriverBll();
            try
            {
                result.data = driverBll.SearchDriver(driverno, driverfullname, primarycontactnumber, fk_vehicleobjectype_objecttypeno, mobilenumber, objectcategoryltxt, fk_transpotmst_vendno);
                result.msg = "success";
            }
            catch (Exception ex)
            {
                result.msg = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFDriverDtl(string driverno)
        {
            ReturnModel result = new ReturnModel();
            DriverBll driverBll = new DriverBll();
            try
            {
                result.data = driverBll.getDetail(driverno);
                result.msg = "success";
            }
            catch (Exception ex)
            {
                result.msg = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAddressByDriverNo(string driverno)
        {
            ReturnModel result = new ReturnModel();
            DriverBll driverBll = new DriverBll();
            try
            {
                result.data = driverBll.GetAddressByDriverNo(driverno);
                result.msg = "success";

            }
            catch (Exception ex)
            {
                result.msg = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDriverCardLog(string driverno)
        {
            ReturnModel result = new ReturnModel();
            DriverBll driverBll = new DriverBll();
            try
            {
                result.data = driverBll.getDriverCardInfo(driverno);
                result.msg = "success";
            }
            catch (Exception ex)
            {
                result.msg = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTransporter(string driverno)
        {
            ReturnModel result = new ReturnModel();
            DriverBll driverBll = new DriverBll();
            try
            {
                result.data = driverBll.GetTransporter(driverno);
                result.msg = "success";
            }
            catch (Exception ex)
            {
                result.msg = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getvehicleInfo(string driverno)
        {
            ReturnModel result = new ReturnModel();
            DriverBll driverBll = new DriverBll();
            try
            {
                result.data = driverBll.GetVehicle(driverno);
                result.msg = "success";
            }
            catch (Exception ex)
            {
                result.msg = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        //public ActionResult GetFDriverDtl(string driverno)
        //{
        //    ReturnModel result = new ReturnModel();
        //    DriverBll driverBll = new DriverBll();
        //    try
        //    {
        //        result.data = driverBll.getDetail(driverno);
        //        result.msg = "success";
        //    }
        //    catch (Exception ex)
        //    {
        //        result.msg = ex.Message;
        //    }
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}


        //[HttpGet]
        //public ActionResult GetDriverCardLog(string driverno)
        //{

        //    ReturnModel result = new ReturnModel();
        //    DriverBll driverBll = new DriverBll();
        //    try
        //    {
        //        result.data = driverBll.getDriverCardInfo(driverno);
        //        result.msg = "success";
        //    }
        //    catch (Exception ex)
        //    {
        //        result.msg = ex.Message;
        //    }
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}


        //public bool insert(driverprofileCustomModels model)
        //{
        //    if (Session["fk_retypemst_reftype"] != null)
        //    {
        //        model.drivermst.createdBy_Type = Session["fk_retypemst_reftype"].ToString();
        //    }
        //    if (Session["userid"] != null)
        //    {
        //        model.drivermst.createdBy_No = Session["userid"].ToString();
        //    }
        //    if (Session["fk_retypemst_reftype"] != null)
        //    {
        //        model.usermst.createdBy_Type = Session["fk_retypemst_reftype"].ToString();
        //    }
        //    if (Session["userid"] != null)
        //    {
        //        model.usermst.createdBy_No = Session["userid"].ToString();
        //    }
        //    if (ModelState.IsValid)
        //    {
        //        driverProfileBll dpb = new driverProfileBll();
        //        return dpb.Add(model);
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}



        [HttpGet]
        public ActionResult getActivityTypefunc(string driverno, string vehicleno)
        {

            ReturnModel result = new ReturnModel();
            DriverBll driverBll = new DriverBll();
            try
            {
                result.data = driverBll.getActivityType(driverno, vehicleno);
                result.msg = "success";
            }
            catch (Exception ex)
            {
                result.msg = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDriversTransporterDtl(string transporterno)
        {
            ReturnModel result = new ReturnModel();
            DriverBll driverBll = new DriverBll();
            try
            {
                result.data = driverBll.GetDriversTransporterDtl(transporterno);
                result.msg = "success";
            }
            catch (Exception ex)
            {
                result.msg = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ActiveLog(driveractivelogModels model)
        {
            ReturnModel result = new ReturnModel();
            driverActiveLogBll driverBll = new driverActiveLogBll();
            try
            {
                result.data = driverBll.insert(model);
                result.msg = "success";
            }
            catch (Exception ex)
            {
                result.msg = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);


        }
        [HttpGet]
        public PartialViewResult detailProfile()
        {

            return PartialView();
        }

        [HttpGet]
        public PartialViewResult FeedbackandRatings()
        {

            return PartialView();
        }

        [HttpGet]
        public PartialViewResult DriverAddresses()
        {
            return PartialView();
        }

        [HttpGet]
        public PartialViewResult cardInfo()
        {

            return PartialView();
        }

        [HttpGet]
        public PartialViewResult vehicleInfo()
        {

            return PartialView();
        }

        [HttpGet]
        public PartialViewResult TransporterInfo()
        {

            return PartialView();
        }

        [HttpGet]
        public PartialViewResult inProcessOrders()
        {

            return PartialView();
        }

        [HttpGet]
        public PartialViewResult OrderHistory()
        {

            return PartialView();
        }

        #region Driver View
        public JsonResult GetDriverBySecureKey(string SecureKey)
        {

            dynamic data = new DriverBll().GetDriverBySecureKeyRoute(SecureKey);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCardsByDriverno(string DriverNo)
        {
            dynamic data = new DriverBll().GetCardsByDrivernoRoute(DriverNo);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAddressesByDriverno(string DriverNo)
        {
            dynamic data = new DriverBll().GetAddressesByDrivernoRoute(DriverNo);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTransporterByDriverno(string DriverNo)
        {
            dynamic data = new DriverBll().GetTransporterByDrivernoRoute(DriverNo);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetVehicleByDriverno(string DriverNo)
        {
            dynamic data = new DriverBll().GetVehicleByDrivernoRoute(DriverNo);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}