using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Numerics;
using static iSOL_Enterprise.Models.RequestModels;

namespace iSOL_Enterprise.Interface
{
    public interface IPlanningSheet
    {
           
             DateTime? AuditApprovalActualDate(string SONumber);
             DateTime? AuditApprovalPlannedDate(string SONumber);
             DateTime? DeliveryNoteActualDate(string SONumber);
             DateTime? DeliveryNotePlannedDate(string SONumber);
             DateTime? DyedIssuanceForProdActualDate(string SONumber);
             DateTime? DyedIssuanceForProdPlannedDate(string SONumber);
             DateTime? DyedReceivingActualDate(string SONumber);
             DateTime? DyedReceivingPlannedDate(string SONumber);
             DateTime? GatePassActualDate(string SONumber);
             DateTime? GatePassPlannedDate(string SONumber);
             DateTime? GreigeIssuanceForDyedActualDate(string SONumber);
             DateTime? GreigeIssuanceForDyedPlannedDate(string SONumber);
             DateTime? GreigeReceivingActualDate(string SONumber);
             DateTime? GreigeReceivingPlannedDate(string SONumber);
             DateTime? PackingActualDate(string SONumber);
             DateTime? PackingPlannedDate(string SONumber);
             DateTime? POActualDate(string SONumber);
             DateTime? POPlannedDate(string SONumber);
             DateTime? PreCostingActualDate(string SONumber);
             DateTime? PreCostingPlannedDate(string SONumber);
             DateTime? SizedYarnIssuanceForGreigePlannedDate(string SONumber);
             DateTime? SizedYarnReceivedPlannedDate(string SONumber);
             DateTime? YarnDeliveryActualDate(string SONumber);
             DateTime? YarnDeliveryPlannedDate(string SONumber);
             DateTime? YarnIssuanceForSizzingPlannedDate(string SONumber);
             DateTime? YarnPurchaseActualDate(string SONumber);
             DateTime? YarnPurchasePlannedDate(string SONumber);
             DateTime? PlanningDateHeader(string SONumber);
             string CustomerCodeHeader (string SONumber);
             string CustomerNameHeader (string SONumber);
             string StatusHeader (string SONumber);
             int? QuantityHeader (string SONumber);
             DateTime? SaleOrderDateHeader(string SONumber);
             DateTime? ShipmentDateHeader(string SONumber);


    }
}
