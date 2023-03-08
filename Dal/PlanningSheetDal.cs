using iSOL_Enterprise.Common;
using iSOL_Enterprise.Interface;
using iSOL_Enterprise.Models;
using SqlHelperExtensions;
using System.Data;

namespace iSOL_Enterprise.Dal
{
    public class PlanningSheetDal : IPlanningSheet
    {
        public DateTime? AuditApprovalActualDate(string SONumber)
        {
            try
            {            
                    string GetQuery = "SELECT  (P.UpdateDate) FROM OWDD T0 inner join WDD1 P on P.WddCode = T0.WddCode inner join POR1 R on R.ObjType = T0.ObjType and R.DocEntry = T0.DocEntry  WHERE  R.U_SO = '"+ SONumber + "' and   T0.objtype = 22 and R.Dscription like '%YARN%' and P.UpdateDate is not null";                  
                    using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
                    {
                        while (rdr.Read())
                        {


                            return (DateTime)rdr["UpdateDate"].ToDateTime();
                    
                    
                        }
                    }
            }
            catch (Exception)
            {
                return null;
                
            }
            return null;
        }

        public DateTime? AuditApprovalPlannedDate(string SONumber)
        {
            try
            {
                string GetQuery = "SELECT  (T0.DocDate + 2) as AuditApprovalPlannedDate FROM ORDR T0 WHERE   T0.DocNum = '" + SONumber+"'";
                
                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
                {
                    while (rdr.Read())
                    {


                        return (DateTime)rdr["AuditApprovalPlannedDate"].ToDateTime();


                    }
                }
            }
            catch (Exception)
            {
                return null;

            }
            return null;
        }
        public DateTime? DeliveryNoteActualDate(string SONumber)
        {
            try
            {
                string GetQuery = "SELECT  Max(T0.dOCdATE) as DeliveryNoteActualDate FROM odln T0 inner join dln1 P on P.DocEntry = T0.DocEntry inner join OITM M on M.itemCode = P.ItemCode  WHERE M.ItmsGrpCod in (101,102,122,103) and  P.BaseRef = '" + SONumber + "' group by T0.dOCdATE, p.ItemCode,p.Dscription,p.Quantity";

                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
                {
                    while (rdr.Read())
                    {


                        return (DateTime)rdr["DeliveryNoteActualDate"].ToDateTime();


                    }
                }
            }
            catch (Exception)
            {
                return null;

            }
            return null;
        }
        public DateTime? DeliveryNotePlannedDate(string SONumber)
        {
            try
            {
                string GetQuery = "SELECT  (T0.DocDate + 51) as DeliveryNotePlannedDate FROM ORDR T0 WHERE   T0.DocNum = '" + SONumber+"'";

                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
                {
                    while (rdr.Read())
                    {


                        return (DateTime)rdr["DeliveryNotePlannedDate"].ToDateTime();


                    }
                }
            }
            catch (Exception)
            {
                return null;

            }
            return null;
        }
        public DateTime? DyedIssuanceForProdActualDate(string SONumber)
        {
            try
            {
                string GetQuery = "SELECT distinct  Max(P.DocDate) as DyedIssuanceForProdActualDate FROM IGN1 T0 inner join OIGN P on P.DocEntry = T0.DocEntry and T0.BaseType = 202 left join OWOR R on R.DocNum = T0.BaseRef inner join OITM M on M.itemCode = T0.ItemCode   WHERE  R.OriginNum = '" + SONumber+"' and R.ProdName like '%Weaved%' group by R.ProdName,T0.Quantity,R.OriginNum,P.DocDate";

                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
                {
                    while (rdr.Read())
                    {


                        return (DateTime)rdr["DyedIssuanceForProdActualDate"].ToDateTime();


                    }
                }
            }
            catch (Exception)
            {
                return null;

            }
            return null;
        }
        public DateTime? DyedIssuanceForProdPlannedDate(string SONumber)
        {
            try
            {
                string GetQuery = "SELECT  (T0.DocDate + 36) as DyedIssuanceForProdPlannedDate FROM ORDR T0 WHERE   T0.DocNum = '" + SONumber+"'";

                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
                {
                    while (rdr.Read())
                    {


                        return (DateTime)rdr["DyedIssuanceForProdPlannedDate"].ToDateTime();


                    }
                }
            }
            catch (Exception)
            {
                return null;

            }
            return null;
        }
        public DateTime? DyedReceivingActualDate(string SONumber)
        {
            try
            {
                string GetQuery = "SELECT distinct  Max(P.DocDate) as DyedReceivingActualDate FROM IGN1 T0 inner join OIGN P on P.DocEntry = T0.DocEntry and T0.BaseType = 202 left join OWOR R on R.DocNum = T0.BaseRef inner join OITM M on M.itemCode = T0.ItemCode WHERE  R.OriginNum = '" + SONumber+"' and  R.ProdName like '%Dyed%' group by R.ProdName,T0.Quantity,R.OriginNum,P.DocDate";

                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
                {
                    while (rdr.Read())
                    {


                        return (DateTime)rdr["DyedReceivingActualDate"].ToDateTime();


                    }
                }
            }
            catch (Exception)
            {
                return null;

            }
            return null;
        }
        public DateTime? DyedReceivingPlannedDate(string SONumber)
        {
            try
            {
                string GetQuery = "SELECT  (T0.DocDate + 36) as DyedReceivingPlannedDate  FROM ORDR T0 WHERE   T0.DocNum = '" + SONumber+"'";

                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
                {
                    while (rdr.Read())
                    {


                        return (DateTime)rdr["DyedReceivingPlannedDate"].ToDateTime();


                    }
                }
            }
            catch (Exception)
            {
                return null;

            }
            return null;
        }
        public DateTime? GatePassActualDate(string SONumber)
        {
            try
            {
                string GetQuery = "SELECT  Max(T0.U_Date) as GatePassActualDate FROM [@OCGP] T0 inner join [@cgp1] P on P.DocEntry = T0.DocEntry  WHERE P.U_SO = '" + SONumber+"' group by T0.U_Date, T0.U_Cardcode,T0.U_Conatiner_numb";

                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
                {
                    while (rdr.Read())
                    {


                        return (DateTime)rdr["GatePassActualDate"].ToDateTime();


                    }
                }
            }
            catch (Exception)
            {
                return null;

            }
            return null;
        }
        public DateTime? GatePassPlannedDate(string SONumber)
        {
            try
            {
                string GetQuery = "SELECT  (T0.DocDate + 51) as GatePassPlannedDate FROM ORDR T0 WHERE T0.DocNum = '" + SONumber + "'";

                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
                {
                    while (rdr.Read())
                    {


                        return (DateTime)rdr["GatePassPlannedDate"].ToDateTime();


                    }
                }
            }
            catch (Exception)
            {
                return null;

            }
            return null;
        }
        public DateTime? GreigeIssuanceForDyedActualDate(string SONumber)
        {
            try
            {
                string GetQuery = "SELECT  Max(T0.DocDate) as GreigeIssuanceForDyedActualDate FROM OWTR T0 inner join WTR1 P on P.DocEntry = T0.DocEntry inner join OITM M on M.itemCode = P.ItemCode WHERE M.ItmsGrpCod in (115) and  P.U_Sale_ord = '" + SONumber+"' group by T0.DocDate,P.Dscription, P.Quantity";

                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
                {
                    while (rdr.Read())
                    {


                        return (DateTime)rdr["GreigeIssuanceForDyedActualDate"].ToDateTime();


                    }
                }
            }
            catch (Exception)
            {
                return null;

            }
            return null;
        }
        public DateTime? GreigeIssuanceForDyedPlannedDate(string SONumber)
        {
            try
            {
                string GetQuery = "SELECT  (T0.DocDate + 33) as GreigeIssuanceForDyedPlannedDate FROM ORDR T0 WHERE T0.DocNum = '" + SONumber + "'";

                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
                {
                    while (rdr.Read())
                    {


                        return (DateTime)rdr["GreigeIssuanceForDyedPlannedDate"].ToDateTime();


                    }
                }
            }
            catch (Exception)
            {
                return null;

            }
            return null;
        }
        public DateTime? GreigeReceivingActualDate(string SONumber)
        {
            try
            {
                string GetQuery = "SELECT distinct  Max(P.DocDate) as GreigeReceivingActualDate FROM IGN1 T0 inner join OIGN P on P.DocEntry = T0.DocEntry and T0.BaseType = 202 left join OWOR R on R.DocNum = T0.BaseRef inner join OITM M on M.itemCode = T0.ItemCode   WHERE  R.OriginNum = '" + SONumber+"' and R.ProdName like '%Weaved%' group by R.ProdName,T0.Quantity,R.OriginNum,P.DocDate";

                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
                {
                    while (rdr.Read())
                    {


                        return (DateTime)rdr["GreigeReceivingActualDate"].ToDateTime();


                    }
                }
            }
            catch (Exception)
            {
                return null;

            }
            return null;
        }
        public DateTime? GreigeReceivingPlannedDate(string SONumber)
        {
            try
            {
                string GetQuery = "SELECT  (T0.DocDate + 30) as GreigeReceivingPlannedDate FROM ORDR T0 WHERE T0.DocNum = '" + SONumber + "'";

                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
                {
                    while (rdr.Read())
                    {


                        return (DateTime)rdr["GreigeReceivingPlannedDate"].ToDateTime();


                    }
                }
            }
            catch (Exception)
            {
                return null;

            }
            return null;
        }
        public DateTime? PackingActualDate(string SONumber)
        {
            try
            {
                string GetQuery = "SELECT distinct  Max(P.DocDate) as PackingActualDate FROM IGN1 T0 inner join OIGN P on P.DocEntry = T0.DocEntry and T0.BaseType = 202 left join OWOR R on R.DocNum = T0.BaseRef inner join OITM M on M.itemCode = T0.ItemCode   WHERE  R.OriginNum = '" + SONumber+"' and R.ProdName like '%Finished%' group by R.ProdName,T0.Quantity,R.OriginNum,P.DocDateQCategory";

                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
                {
                    while (rdr.Read())
                    {


                        return (DateTime)rdr["PackingActualDate"].ToDateTime();


                    }
                }
            }
            catch (Exception)
            {
                return null;

            }
            return null;
        }
        public DateTime? PackingPlannedDate(string SONumber)
        {
            try
            {
                string GetQuery = "SELECT  (T0.DocDate + 50) as PackingPlannedDate FROM ORDR T0 WHERE T0.DocNum = '" + SONumber + "'";

                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
                {
                    while (rdr.Read())
                    {


                        return (DateTime)rdr["PackingPlannedDate"].ToDateTime();


                    }
                }
            }
            catch (Exception)
            {
                return null;

            }
            return null;
        }
        public DateTime? POActualDate(string SONumber)
        {
            try
            {
                string GetQuery = "SELECT  (T0.DocDate) as POActualDate FROM POR1 T0 WHERE  T0.U_SO = '" + SONumber+"' and T0.Dscription like '%YARN%' --group by  T0.Dscription, T0.Quantity,T0.DocDate";

                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
                {
                    while (rdr.Read())
                    {


                        return (DateTime)rdr["POActualDate"].ToDateTime();


                    }
                }
            }
            catch (Exception)
            {
                return null;

            }
            return null;
        }
        public DateTime? POPlannedDate(string SONumber)
        {
            try
            {
                string GetQuery = "SELECT  (T0.DocDate + 1) as POPlannedDate FROM ORDR T0 WHERE   T0.DocNum = '" + SONumber+"'";

                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
                {
                    while (rdr.Read())
                    {


                        return (DateTime)rdr["POPlannedDate"].ToDateTime();


                    }
                }
            }
            catch (Exception)
            {
                return null;

            }
            return null;
        }
        public DateTime? PreCostingActualDate(string SONumber)
        {
            try
            {
                string GetQuery = "SELECT  S.CreateDate as PreCostingActualDate FROM [@PCS1] T0 inner join [@OPCS] S on S.DocEntry = T0.DocEntry WHERE  S.U_SO = '" + SONumber+"'";

                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
                {
                    while (rdr.Read())
                    {


                        return (DateTime)rdr["PreCostingActualDate"].ToDateTime();


                    }
                }
            }
            catch (Exception)
            {
                return null;

            }
            return null;
        }
        public DateTime? PreCostingPlannedDate(string SONumber)
        {
            try
            {
                string GetQuery = "SELECT  (T0.DocDate + 0) as PreCostingPlannedDate FROM ORDR T0 WHERE   T0.DocNum = '" + SONumber + "'";

                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
                {
                    while (rdr.Read())
                    {


                        return (DateTime)rdr["PreCostingPlannedDate"].ToDateTime();


                    }
                }
            }
            catch (Exception)
            {
                return null;

            }
            return null;
        }

        public DateTime? SizedYarnIssuanceForGreigePlannedDate(string SONumber)
        {
            try
            {
                string GetQuery = "SELECT  (T0.DocDate + 11) as SizedYarnIssuanceForGreigePlannedDate FROM ORDR T0 WHERE   T0.DocNum = '" + SONumber+"'";

                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
                {
                    while (rdr.Read())
                    {


                        return (DateTime)rdr["SizedYarnIssuanceForGreigePlannedDate"].ToDateTime();


                    }
                }
            }
            catch (Exception)
            {
                return null;

            }
            return null;
        }
        public DateTime? YarnDeliveryActualDate(string SONumber)
        {
            try
            {
                string GetQuery = "SELECT  Max(P.DocDate) as YarnDeliveryActualDate FROM PDN1 T0 inner join OPDN P on P.DocEntry = T0.DocEntry inner join OITM M on M.itemCode = T0.ItemCode   WHERE M.ItmsGrpCod in (104,126) and T0.U_SO = '" + SONumber+"' group by  T0.Dscription, T0.Quantity,T0.DocDate";

                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
                {
                    while (rdr.Read())
                    {


                        return (DateTime)rdr["YarnDeliveryActualDate"].ToDateTime();


                    }
                }
            }
            catch (Exception)
            {
                return null;

            }
            return null;
        }
        public DateTime? YarnDeliveryPlannedDate(string SONumber)
        {
            try
            {
                string GetQuery = "SELECT  (T0.DocDate + 7) as YarnDeliveryPlannedDate FROM ORDR T0 WHERE   T0.DocNum = '" + SONumber+"'";

                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
                {
                    while (rdr.Read())
                    {


                        return (DateTime)rdr["YarnDeliveryPlannedDate"].ToDateTime();


                    }
                }
            }
            catch (Exception)
            {
                return null;

            }
            return null;
        }
        public DateTime? SizedYarnReceivedPlannedDate(string SONumber)
        {
            try
            {
                string GetQuery = "SELECT  (T0.DocDate + 11) as SizedYarnReceivedPlannedDate FROM ORDR T0 WHERE   T0.DocNum = '" + SONumber+"'";

                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
                {
                    while (rdr.Read())
                    {


                        return (DateTime)rdr["SizedYarnReceivedPlannedDate"].ToDateTime();


                    }
                }
            }
            catch (Exception)
            {
                return null;

            }
            return null;
        }
        public DateTime? YarnIssuanceForSizzingPlannedDate(string SONumber)
        {
            try
            {
                string GetQuery = "SELECT  (T0.DocDate + 7) as YarnIssuanceForSizzingPlannedDate FROM ORDR T0 WHERE   T0.DocNum = '" + SONumber+"'";

                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
                {
                    while (rdr.Read())
                    {


                        return (DateTime)rdr["YarnIssuanceForSizzingPlannedDate"].ToDateTime();


                    }
                }
            }
            catch (Exception)
            {
                return null;

            }
            return null;
        }
        public DateTime? YarnPurchaseActualDate(string SONumber)
        {
            try
            {
                string GetQuery = "SELECT  (T0.DocDate) as YarnPurchaseActualDate FROM POR1 T0 WHERE  T0.U_SO = '" + SONumber+"' and T0.Dscription like '%YARN%' --group by  T0.Dscription, T0.Quantity,T0.DocDate";

                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
                {
                    while (rdr.Read())
                    {


                        return (DateTime)rdr["YarnPurchaseActualDate"].ToDateTime();


                    }
                }
            }
            catch (Exception)
            {
                return null;

            }
            return null;
        }
        public DateTime? YarnPurchasePlannedDate(string SONumber)
        {
            try
            {
                string GetQuery = "SELECT  (T0.DocDate + 3) as YarnPurchasePlannedDate  FROM ORDR T0 WHERE   T0.DocNum = '" + SONumber+"'";

                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
                {
                    while (rdr.Read())
                    {


                        return (DateTime)rdr["YarnPurchasePlannedDate"].ToDateTime();


                    }
                }
            }
            catch (Exception)
            {
                return null;

            }
            return null;
        }
        public DateTime? PlanningDateHeader(string SONumber)
        {
            try
            {
                string GetQuery = "SELECT  (T0.DocDate)  as PlanningDateHeader FROM ORDR T0 WHERE T0.DocNum = '" + SONumber + "'";

                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
                {
                    while (rdr.Read())
                    {


                        return (DateTime)rdr["PlanningDateHeader"].ToDateTime();


                    }
                }
            }
            catch (Exception)
            {
                return null;

            }
            return null;
        }
        public string? CustomerCodeHeader(string SONumber)
        {
            try
            {
                string GetQuery = "SELECT  (T0.Cardcode) as CustomerCode  FROM ORDR T0 WHERE   T0.DocNum = '" + SONumber+"'";

                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
                {
                    while (rdr.Read())
                    {


                        return rdr["CustomerCode"].ToString();


                    }
                }
            }
            catch (Exception)
            {
                return null;

            }
            return null;
        }
        public string? CustomerNameHeader(string SONumber)
        {
            try
            {
                string GetQuery = "SELECT  (T0.CardName) as CardName  FROM ORDR T0 WHERE   T0.DocNum = '" + SONumber + "'";

                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
                {
                    while (rdr.Read())
                    {


                        return rdr["CardName"].ToString();


                    }
                }
            }
            catch (Exception)
            {
                return null;

            }
            return null;
        }
        public int? QuantityHeader(string SONumber)
        {
            try
            {
                string GetQuery = "SELECT  sum(R.Quantity) as Quantity FROM ORDR T0 inner join RDR1 R on R.DocEntry = T0.DocEntry WHERE   T0.DocNum = '" + SONumber+"'";

                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
                {
                    while (rdr.Read())
                    {


                        return rdr["Quantity"].ToInt();


                    }
                }
            }
            catch (Exception)
            {
                return null;

            }
            return null;
        }
        public DateTime? SaleOrderDateHeader(string SONumber)
        {
            try
            {
                string GetQuery = "SELECT  (T0.DocDate) as SaleOrderDate FROM ORDR T0 WHERE   T0.DocNum = '" + SONumber+"'";

                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
                {
                    while (rdr.Read())
                    {


                        return (DateTime)rdr["SaleOrderDate"].ToDateTime();


                    }
                }
            }
            catch (Exception)
            {
                return null;

            }
            return null;
        }
        public DateTime? ShipmentDateHeader(string SONumber)
        {
            try
            {
                string GetQuery = "SELECT  (T0.DocDueDate) as ShipmentDate FROM ORDR T0 WHERE   T0.DocNum = '" + SONumber + "'";

                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
                {
                    while (rdr.Read())
                    {


                        return (DateTime)rdr["ShipmentDate"].ToDateTime();


                    }
                }
            }
            catch (Exception)
            {
                return null;

            }
            return null;
        }
        public string? StatusHeader(string SONumber)
        {
            try
            {
                string GetQuery = "SELECT  case when (T0.DocStatus)  = 'O' then 'Open' when (T0.DocStatus)  = 'C' then 'Closed'  Else 'Status' end PreCosted FROM ORDR T0 WHERE   T0.DocNum = '"+SONumber+"' ";

                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
                {
                    while (rdr.Read())
                    {


                        return rdr["PreCosted"].ToString();


                    }
                }
            }
            catch (Exception)
            {
                return null;

            }
            return null;
        }
    }
}
