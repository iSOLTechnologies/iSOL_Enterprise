using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Numerics;
using static iSOL_Enterprise.Models.RequestModels;

namespace iSOL_Enterprise.Interface
{
    public interface IPlanningSheet
    {
           
             dynamic AuditApprovalActualDate(string SONumber);
             dynamic AuditApprovalPlannedDate(string SONumber);
             dynamic DeliveryNoteActualDate(string SONumber);
             dynamic DeliveryNotePlannedDate(string SONumber);
             dynamic DyedIssuanceForProdActualDate(string SONumber);
             dynamic DyedIssuanceForProdPlannedDate(string SONumber);
             dynamic DyedReceivingActualDate(string SONumber);
             dynamic DyedReceivingPlannedDate(string SONumber);
             dynamic GatePassActualDate(string SONumber);
             dynamic GatePassPlannedDate(string SONumber);
             dynamic GreigeIssuanceForDyedActualDate(string SONumber);
             dynamic GreigeIssuanceForDyedPlannedDate(string SONumber);
             dynamic GreigeReceivingActualDate(string SONumber);
             dynamic GreigeReceivingPlannedDate(string SONumber);
             dynamic PackingActualDate(string SONumber);
             dynamic PackingPlannedDate(string SONumber);
             dynamic POActualDate(string SONumber);
             dynamic POPlannedDate(string SONumber);
             dynamic PreCostingActualDate(string SONumber);
             dynamic PreCostingPlannedDate(string SONumber);
             dynamic SizedYarnIssuanceForGreigePlannedDate(string SONumber);
             dynamic SizedYarnReceivedPlannedDate(string SONumber);
             dynamic YarnDeliveryActualDate(string SONumber);
             dynamic YarnDeliveryPlannedDate(string SONumber);
             dynamic YarnIssuanceForSizzingPlannedDate(string SONumber);
             dynamic YarnPurchaseActualDate(string SONumber);
             dynamic YarnPurchasePlannedDate(string SONumber);
             dynamic PlanningDateHeader(string SONumber);
             string CustomerCodeHeader (string SONumber);
             string CustomerNameHeader (string SONumber);
             string StatusHeader (string SONumber);
             int? QuantityHeader (string SONumber);
             dynamic SaleOrderDateHeader(string SONumber);
             dynamic ShipmentDateHeader(string SONumber);


    }
}
