using SqlHelperExtensions;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;
using iSOL_Enterprise.Common;
using System;
using iSOL_Enterprise.Models;
using iSOL_Enterprise.Models.sale;

namespace iSOL_Enterprise.Dal
{
    public class PlanningSheetDal
    {

        public List<planningSheetModel> GetPlanningSheetData()
        {
            string GetQuery = "select DocEntry,U_PlanDate,U_SODate,U_ShipDate,U_SOnum,U_ItemCode,Status from [dbo].[@PSF] order by docEntry desc";
            

            List<planningSheetModel> list = new List<planningSheetModel>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {

                    planningSheetModel models = new planningSheetModel();
                    models.docEntry = rdr["DocEntry"].ToInt();
                   
                    models.u_PlanDate = rdr["U_PlanDate"].ToDateTime();
                    models.u_SODate = rdr["U_SODate"].ToDateTime();
                    models.u_ShipDate = rdr["U_ShipDate"].ToDateTime();
                    models.u_SOnum = rdr["U_SOnum"].ToString();
                    models.u_ItemCode = rdr["U_ItemCode"].ToString();
                    models.status = rdr["Status"].ToString();                   
                    list.Add(models);
                }
            }
            return list;
        }

        public bool AddPlanningSheet(string formData)
        {
            try
            {
                var model = JsonConvert.DeserializeObject<dynamic>(formData);
                

                SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                int res1 = 0;
                try
                {
                    int DocEntry = CommonDal.getPrimaryKey(tran, "DocEntry", "dbo.[@PSF]");
                    
                    if (model.HeaderData != null)
                    {

                        model.HeaderData.StatusHeader = model.HeaderData.StatusHeader == "Open" ? "O" : "C";
                        string HeadQuery = @"insert into dbo.[@PSF](DocEntry,U_PlanDate,Status,U_SOnum,U_CutomerCode,U_SODate,U_ShipDate,U_ItemCode,U_ItemDes,U_Qty,U_UOM) 
                                           values(" + DocEntry + ",'"
                                                + Convert.ToDateTime(model.HeaderData.PlanningDateHeader) + "','"
                                                + model.HeaderData.StatusHeader + "','"
                                                + model.HeaderData.saleorderno + "','"
                                                + model.HeaderData.CustomerCodeHeader + "','"
                                                + Convert.ToDateTime(model.HeaderData.SaleOrderDateHeader) + "','"
                                                + Convert.ToDateTime(model.HeaderData.ShipmentDateHeader) + "','"
                                                + model.HeaderData.ItemCodeHeader + "','"
                                                + model.HeaderData.ItemDesHeader + "','"
                                                + model.HeaderData.QuantityHeader + "','"
                                                + model.HeaderData.uom + "')";
                                                
                       
                        res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, HeadQuery).ToInt();
                        if (res1 <= 0)
                        {
                            tran.Rollback();
                            return false;
                        }
                    }
                    if (model.ListDates != null)
                    {
                        int LineID = 1;
                        foreach (var item in model.ListDates)
                        {                            

                            string RowQueryItem = @"insert into dbo.[@PSF2](DocEntry,LineId,U_PlanDate,U_PreCosPln,U_PreCosAct,U_POPln,U_POAct,
                                                    U_AudApp,U_AudAppAct,U_YarnPur,U_YarPurAct,U_YarDel,U_YarDelAct,U_YarIssSiz,U_YarIssSizAct
                                                    ,U_SizYarRec,U_SizYarRecAct,U_SizYarIssGre,U_SizYarIssGreAct,U_GreRec,U_GreRecAct,U_GreIssDye
                                                    ,U_GreIssDyeAct,U_DyeRec,U_DyeRecAct,U_DyeIssProd,U_DyeIssProdAct,U_PackPln,U_PackAct
                                                    ,U_DelPln,U_DelAct,U_GatePass,U_GatePassAct)
                                               values(" + DocEntry + ","
                                              + LineID + ",'"
                                              + Convert.ToDateTime(item.ListPlanningDate) + "','"
                                              + Convert.ToDateTime(item.PreCostingPlannedDate) + "','"
                                              + Convert.ToDateTime(item.PreCostingActualDate) + "','"
                                              + Convert.ToDateTime(item.POPlannedDate) + "','"
                                              + Convert.ToDateTime(item.POActualDate) + "','"
                                              + Convert.ToDateTime(item.AuditApprovalPlannedDate) + "','"
                                              + Convert.ToDateTime(item.AuditApprovalActualDate) + "','"
                                              + Convert.ToDateTime(item.YarnPurchasePlannedDate) + "','"
                                              + Convert.ToDateTime(item.YarnPurchaseActualDate) + "','"
                                              + Convert.ToDateTime(item.YarnDeliveryPlannedDate) + "','"
                                              + Convert.ToDateTime(item.YarnDeliveryActualDate) + "','"
                                              + Convert.ToDateTime(item.YarnIssuanceForSizzingPlannedDate) + "','"
                                              + Convert.ToDateTime(item.YarnIssuanceForSizzingActualDate) + "','"
                                              + Convert.ToDateTime(item.SizedYarnReceivedPlannedDate) + "','"
                                              + Convert.ToDateTime(item.SizedYarnReceivedActualDate) + "','"
                                              + Convert.ToDateTime(item.SizedYarnIssuanceForGreigePlannedDate) + "','"
                                              + Convert.ToDateTime(item.SizedYarnIssuanceForGreigeActualDate) + "','"
                                              + Convert.ToDateTime(item.GreigeReceivingPlannedDate) + "','"
                                              + Convert.ToDateTime(item.GreigeReceivingActualDate) + "','"
                                              + Convert.ToDateTime(item.GreigeIssuanceForDyedPlannedDate) + "','"
                                              + Convert.ToDateTime(item.GreigeIssuanceForDyedActualDate) + "','"
                                              + Convert.ToDateTime(item.DyedReceivingPlannedDate) + "','"
                                              + Convert.ToDateTime(item.DyedReceivingActualDate) + "','"
                                              + Convert.ToDateTime(item.DyedIssuanceForProdPlannedDate) + "','"
                                              + Convert.ToDateTime(item.DyedIssuanceForProdActualDate) + "','"
                                              + Convert.ToDateTime(item.PackingPlannedDate) + "','"
                                              + Convert.ToDateTime(item.PackingActualDate) + "','"
                                              + Convert.ToDateTime(item.DeliveryNotePlannedDate) + "','"
                                              + Convert.ToDateTime(item.DeliveryNoteAcutalDate) + "','"
                                              + Convert.ToDateTime(item.GatePassPlannedDate) + "','"
                                              + Convert.ToDateTime(item.GatePassActualDate) + "')";                                
                            

                            int res2 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, RowQueryItem).ToInt();
                            if (res2 <= 0)
                            {
                                tran.Rollback();
                                return false;
                            }
                            LineID += 1;
                        }



                    }
                    
                    if (res1 > 0)
                    {
                        tran.Commit();
                    }

                }
                catch (Exception)
                {
                    tran.Rollback();
                    return false;
                }

                return res1 > 0 ? true : false;

            }
            catch (Exception)
            {

                return false;
            }
        }

    }
}
