using iSOL_Enterprise.Common;
using iSOL_Enterprise.Models;
using iSOL_Enterprise.Models.Inventory;
using iSOL_Enterprise.Models.sale;
using Newtonsoft.Json;
using SAPbobsCOM;
using SqlHelperExtensions;
using System;
using System.Data;
using System.Data.SqlClient;

namespace iSOL_Enterprise.Dal.Inventory
{
    public class ItemMasterDataDal
    {
        public List<tbl_OITG> GetProperties()
        {
            string GetQuery = "select ItmsTypCod,ItmsGrpNam from OITG ORDER BY ItmsTypCod";

            List<tbl_OITG> list = new List<tbl_OITG>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {
                    list.Add(new tbl_OITG() 
                    { 
                        ItmsTypCod = rdr["ItmsTypCod"].ToInt(),
                        ItmsGrpNam = rdr["ItmsGrpNam"].ToString()

                });
                }
            }
            return list;
        }

        public List<ListModel> GetItemsGroup()
        {
            string GetQuery = "select ItmsGrpCod,ItmsGrpNam from OITB  where Locked = 'N' ORDER BY ItmsGrpCod";

            List<ListModel> list = new List<ListModel>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {
                    list.Add(new ListModel()
                    {
                        Value = rdr["ItmsGrpCod"].ToInt(),
                        Text = rdr["ItmsGrpNam"].ToString()

                    });
                }
            }
            return list;
        }

        public List<ListModel> GetListName()
        {
            string GetQuery = "select ListNum,ListName from OPLN   ORDER BY ListNum";

            List<ListModel> list = new List<ListModel>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {
                    list.Add(new ListModel()
                    {
                        Value = rdr["ListNum"].ToInt(),
                        Text = rdr["ListName"].ToString()

                    });
                }
            }
            return list;
        }
        public List<ListModel> GetManufacturer()
        {
            string GetQuery = "select FirmCode,FirmName from OMRC   ORDER BY FirmCode";

            List<ListModel> list = new List<ListModel>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {
                    list.Add(new ListModel()
                    {
                        Value = rdr["FirmCode"].ToInt(),
                        Text = rdr["FirmName"].ToString()

                    });
                }
            }
            return list;
        }
        public List<ListModel> GetShipType()
        {
            string GetQuery = "select TrnspCode,TrnspName from OSHP where Active = 'Y'  ORDER BY TrnspCode";

            List<ListModel> list = new List<ListModel>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {
                    list.Add(new ListModel()
                    {
                        Value = rdr["TrnspCode"].ToInt(),
                        Text = rdr["TrnspName"].ToString()

                    });
                }
            }
            return list;
        }
        public List<ListModel> GetCustomsGroup()
        {
            string GetQuery = "select CstGrpCode,CstGrpName from OARG   ORDER BY CstGrpCode";

            List<ListModel> list = new List<ListModel>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {
                    list.Add(new ListModel()
                    {
                        Value = rdr["CstGrpCode"].ToInt(),
                        Text = rdr["CstGrpName"].ToString()

                    });
                }
            }
            return list;
        }
        public List<tbl_OVTG> GetTaxGroup()
        {
            string GetQuery = " select  Rate,Code, Code +'   -   '+ Name as Name  from OVTG order by Code";

            List<tbl_OVTG> list = new List<tbl_OVTG>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {
                    list.Add(new tbl_OVTG()
                    {
                        Rate = rdr["Rate"].ToDecimal(),
                        code = rdr["Code"].ToString(),
                        vatName = rdr["Name"].ToString()

                    });
                }
            }
            return list;
        }

        public string? GetNewItemCode(int Series)
        {
            string GetItemCode = "select case when BeginStr is  null  then  RIGHT('000000' + CAST(NextNumber AS VARCHAR(6)), 6)  else BeginStr  +  CAST(  NextNumber as nvarchar(20))   end 'ItemCode' from NNM1 where  NextNumber <= LastNum and ObjectCode = 4 and Series = "+Series;

            string? ItemCode = Convert.ToString(SqlHelper.ExecuteScalar(SqlHelper.defaultSapDB, CommandType.Text, GetItemCode));
            
            return ItemCode;
        }
        public List<tbl_OWHS> GetWareHouseList()
        {

            string GetQuery = "select WhsCode , WhsName = WhsName  from OWHS order by WhsCode";


            List<tbl_OWHS> list = new List<tbl_OWHS>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {

                    list.Add(
                        new tbl_OWHS()
                        {
                            whscode = rdr["WhsCode"].ToString(),
                            whsname = rdr["WhsName"].ToString()

                        });

                }
            }

            return list;
    
        }

        public List<ListModel> GetUomName()
        {
            string GetQuery = " select UomEntry,UomCode  from OUOM order by UomEntry";

            List<ListModel> list = new List<ListModel>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {
                    list.Add(new ListModel()
                    {
                        Value = rdr["UomEntry"].ToInt(),
                        Text = rdr["UomCode"].ToString()

                    });
                }
            }
            return list;
        }

        public bool AddItemMasterData(string formData)
        {
            try
            {
                var model = JsonConvert.DeserializeObject<dynamic>(formData);
                //string DocType = model.ListItems == null ? "S" : "I";


                SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                int res1 = 0;
                try
                {
                    //int Id = CommonDal.getPrimaryKey(tran, "OITM");
                    //string DocNum = SqlHelper.getUpdatedDocumentNumberOnLoad(tran, "OQUT", "SQ");
                    if (model.HeaderData != null)
                    {
                        // string[] ArrayHeadQuery = {"ItemCode", "ItemName", "Series", "InvntItem", "SellItem", "FrgnName", "PrchseItem", "ItemType", "ItmsGrpCod", "UgpEntry", "ListName", "PriceUnit", "AvgPrice", " WTLiable", "FirmCode", "ShipType", "ManagedItemby", "MyMngMthd", "ManagedItemby", "validFor", "validFrom", "validTo", "frozenFrom", "frozenTo", " BuyUnitMar", "NumInBuy", "CstGrpCode", "TotalTax", "VatGroupPu", "VatGroupSa", "SalUnitMar", "NumInSale", "GLMethod", "ByWh", "InvntryUom", "EvalSystem", " PlaningSys", "PrcrmntMtd", "MinOrdrQty", "IssueMthd", "TreeType", "PrdStdCst", "InCostRoll", " QryGroup1", "QryGroup2", "QryGroup3", "QryGroup4", "QryGroup5", "QryGroup6", "QryGroup7", "QryGroup8", "QryGroup9", "QryGroup10", "QryGroup11", "QryGroup12", " QryGroup13", "QryGroup14", "QryGroup15", "QryGroup16", "QryGroup17", "QryGroup18", "QryGroup19", "QryGroup20", "QryGroup21", "QryGroup22", "QryGroup23", " QryGroup24", "QryGroup25", "QryGroup26", "QryGroup27", "QryGroup28", "QryGroup29", "QryGroup30", "QryGroup31", "QryGroup32", "QryGroup33", "QryGroup34", " QryGroup35", "QryGroup36", "QryGroup37", "QryGroup38", "QryGroup39", "QryGroup40", "QryGroup41", "QryGroup42", "QryGroup43", "QryGroup44", "QryGroup45", " QryGroup46", "QryGroup47", "QryGroup48", "QryGroup49", "QryGroup50", "QryGroup51", "QryGroup52", "QryGroup53", "QryGroup54", "QryGroup55", "QryGroup56", " QryGroup57", "QryGroup58", "QryGroup59", "QryGroup60", "QryGroup61", "QryGroup62", "QryGroup63", "QryGroup64" };
                        //,PriceUnit,,@PriceUnit,
                        model.HeaderData.Series = model.HeaderData.Series == "" ? "NULL" : Convert.ToInt32(model.HeaderData.Series);
                        model.HeaderData.ItmsGrpCod = model.HeaderData.ItmsGrpCod == "" ? "NULL" : Convert.ToInt32(model.HeaderData.ItmsGrpCod) ;
                        model.HeaderData.UgpEntry = model.HeaderData.UgpEntry == "" ? "NULL" : Convert.ToInt32(model.HeaderData.UgpEntry);
                        model.HeaderData.AvgPrice =model.HeaderData.AvgPrice == "" ? "NULL" : Convert.ToInt32(model.HeaderData.AvgPrice) ;
                        model.Tab_General.ActivevalidFor = model.Tab_General.ActivevalidFor == "Y" ? "Y" : "N";
                          model.Tab_General.validFrom   =  model.Tab_General.validFrom == "" ? "null" : Convert.ToDateTime(model.Tab_General.validFrom) ;
                          model.Tab_General.validTo   =  model.Tab_General.validTo == "" ? "null" : Convert.ToDateTime(model.Tab_General.validTo) ;
                          model.Tab_General.frozenFrom   =  model.Tab_General.frozenFrom == "" ? "null" : Convert.ToDateTime(model.Tab_General.frozenFrom) ;
                        model.Tab_General.frozenTo   =  model.Tab_General.frozenTo == "" ? "null" : Convert.ToDateTime(model.Tab_General.frozenTo);
                        model.Tab_PurchasingData.CstGrpCod = model.Tab_PurchasingData.CstGrpCod == "" ? "NULL" : Convert.ToInt16(model.Tab_PurchasingData.CstGrpCod);
                        string HeadQuery = @"insert into OITM
                                            
(ItemCode,
ItemName,
Series,
InvntItem,
SellItem,
FrgnName,
PrchseItem,
ItemType,
ItmsGrpCod,
UgpEntry,

AvgPrice,
WTLiable,
FirmCode,
ShipType, 
validFor,
validFrom,
validTo,
frozenFrom,
frozenTo,
ByWh,
EvalSystem,
GLMethod,
InvntryUom,
PrcrmntMtd,
PlaningSys,
MinOrdrQty,
InCostRoll,
IssueMthd,
TreeType,
PrdStdCst,
BuyUnitMsr,
CstGrpCode,
NumInBuy,   
VatGroupPu,
NumInSale,
SalUnitMsr,
VatGourpSa,
QryGroup1,
QryGroup2,
QryGroup3,
QryGroup4,
QryGroup5,
QryGroup6,
QryGroup7,
QryGroup8,
QryGroup9,
QryGroup10,
QryGroup11,
QryGroup12,
QryGroup13,
QryGroup14,
QryGroup15,
QryGroup16,
QryGroup17,
QryGroup18,
QryGroup19,
QryGroup20,
QryGroup21,
QryGroup22,
QryGroup23,
QryGroup24,
QryGroup25,
QryGroup26,
QryGroup27,
QryGroup28,
QryGroup29,
QryGroup30,
QryGroup31,
QryGroup32,
QryGroup33,
QryGroup34,
QryGroup35,
QryGroup36,
QryGroup37,
QryGroup38,
QryGroup39,
QryGroup40,
QryGroup41,
QryGroup42,
QryGroup43,
QryGroup44,
QryGroup45,
QryGroup46,
QryGroup47,
QryGroup48,
QryGroup49,
QryGroup50,
QryGroup51,
QryGroup52,
QryGroup53,
QryGroup54,
QryGroup55,
QryGroup56,
QryGroup57,
QryGroup58,
QryGroup59,
QryGroup60,
QryGroup61,
QryGroup62,
QryGroup63,
QryGroup64) 
values("
+ "'" + model.HeaderData.ItemCode + "','"
+ model.HeaderData.ItemName + "',"
+ model.HeaderData.Series + ",'"
+ model.HeaderData.InvntItem + "','"
+ model.HeaderData.SellItem + "','"
+ model.HeaderData.FrgnName + "','"
+ model.HeaderData.PrchseItem + "','"
+ model.HeaderData.ItemType + "',"
+ model.HeaderData.ItmsGrpCod  +","
+ model.HeaderData.UgpEntry + ","
+ model.HeaderData.AvgPrice + ",'"
+ model.Tab_General.WTLiable + "','"
+ model.Tab_General.FirmCode + "','"
+ model.Tab_General.ShipType + "','" 
+ model.Tab_General.ActivevalidFor + "','"
+ model.Tab_General.validFrom  + "','"
+ model.Tab_General.validTo  + "','"
+ model.Tab_General.frozenFrom  + "','"
+ model.Tab_General.frozenTo + "','"
+ model.Tab_InventoryData.ByWh + "','"
+ model.Tab_InventoryData.EvalSystem + "','"
+ model.Tab_InventoryData.GLMethod + "','"
+ model.Tab_InventoryData.InvntryUom + "','"
+ model.Tab_PlanningData.PrcrmntMtd + "','"
+ model.Tab_PlanningData.PlaningSys + "','"
+ model.Tab_PlanningData.MinOrdrQty + "','"
+ model.Tab_ProductionData.InCostRoll + "','"
+ model.Tab_ProductionData.IssueMthd + "','"
+ model.Tab_ProductionData.TreeType + "','"
+ model.Tab_ProductionData.PrdStdCst + "','"
+ model.Tab_PurchasingData.BuyUnitMsr + "',"
+ model.Tab_PurchasingData.CstGrpCod  +",'"
+ model.Tab_PurchasingData.NumInBuy + "','" 
+ model.Tab_PurchasingData.VatGroupPu + "','"
+ model.Tab_SalesData.NumInSal + "','"
+ model.Tab_SalesData.SalUnitMa + "','"
+ model.Tab_SalesData.VatGroupS + "','"
+ model.Tab_Properties.QryGroup1 + "','"
+ model.Tab_Properties.QryGroup2 + "','"
+ model.Tab_Properties.QryGroup3 + "','"
+ model.Tab_Properties.QryGroup4 + "','"
+ model.Tab_Properties.QryGroup5 + "','"
+ model.Tab_Properties.QryGroup6 + "','"
+ model.Tab_Properties.QryGroup7 + "','"
+ model.Tab_Properties.QryGroup8 + "','"
+ model.Tab_Properties.QryGroup9 + "','"
+ model.Tab_Properties.QryGroup10 + "','"
+ model.Tab_Properties.QryGroup11 + "','"
+ model.Tab_Properties.QryGroup12 + "','"
+ model.Tab_Properties.QryGroup13 + "','"
+ model.Tab_Properties.QryGroup14 + "','"
+ model.Tab_Properties.QryGroup15 + "','"
+ model.Tab_Properties.QryGroup16 + "','"
+ model.Tab_Properties.QryGroup17 + "','"
+ model.Tab_Properties.QryGroup18 + "','"
+ model.Tab_Properties.QryGroup19 + "','"
+ model.Tab_Properties.QryGroup20 + "','"
+ model.Tab_Properties.QryGroup21 + "','"
+ model.Tab_Properties.QryGroup22 + "','"
+ model.Tab_Properties.QryGroup23 + "','"
+ model.Tab_Properties.QryGroup24 + "','"
+ model.Tab_Properties.QryGroup25 + "','"
+ model.Tab_Properties.QryGroup26 + "','"
+ model.Tab_Properties.QryGroup27 + "','"
+ model.Tab_Properties.QryGroup28 + "','"
+ model.Tab_Properties.QryGroup29 + "','"
+ model.Tab_Properties.QryGroup30 + "','"
+ model.Tab_Properties.QryGroup31 + "','"
+ model.Tab_Properties.QryGroup32 + "','"
+ model.Tab_Properties.QryGroup33 + "','"
+ model.Tab_Properties.QryGroup34 + "','"
+ model.Tab_Properties.QryGroup35 + "','"
+ model.Tab_Properties.QryGroup36 + "','"
+ model.Tab_Properties.QryGroup37 + "','"
+ model.Tab_Properties.QryGroup38 + "','"
+ model.Tab_Properties.QryGroup39 + "','"
+ model.Tab_Properties.QryGroup40 + "','"
+ model.Tab_Properties.QryGroup41 + "','"
+ model.Tab_Properties.QryGroup42 + "','"
+ model.Tab_Properties.QryGroup43 + "','"
+ model.Tab_Properties.QryGroup44 + "','"
+ model.Tab_Properties.QryGroup45 + "','"
+ model.Tab_Properties.QryGroup46 + "','"
+ model.Tab_Properties.QryGroup47 + "','"
+ model.Tab_Properties.QryGroup48 + "','"
+ model.Tab_Properties.QryGroup49 + "','"
+ model.Tab_Properties.QryGroup50 + "','"
+ model.Tab_Properties.QryGroup51 + "','"
+ model.Tab_Properties.QryGroup52 + "','"
+ model.Tab_Properties.QryGroup53 + "','"
+ model.Tab_Properties.QryGroup54 + "','"
+ model.Tab_Properties.QryGroup55 + "','"
+ model.Tab_Properties.QryGroup56 + "','"
+ model.Tab_Properties.QryGroup57 + "','"
+ model.Tab_Properties.QryGroup58 + "','"
+ model.Tab_Properties.QryGroup59 + "','"
+ model.Tab_Properties.QryGroup60 + "','"
+ model.Tab_Properties.QryGroup61 + "','"
+ model.Tab_Properties.QryGroup62 + "','"
+ model.Tab_Properties.QryGroup63 + "','"
+ model.Tab_Properties.QryGroup64 + "')";


                        #region SqlParameters
                        //List<SqlParameter> param = new List<SqlParameter>(){
                        ////foreach (var Arr in ArrayHeadQuery)
                        ////{
                        ////    param.Add(new SqlParameter("\"@" + Arr +"\"", Arr));
                        ////}
                        
                        //new SqlParameter("@ItemCode", model.HeaderData.ItemCode),
                        //new SqlParameter("@ItemName", model.HeaderData.ItemName),
                        //new SqlParameter("@Series",     model.HeaderData.Series),
                        //new SqlParameter("@InvntItem", model.HeaderData.InvntItem),
                        //new SqlParameter("@SellItem", model.HeaderData.SellItem),
                        //new SqlParameter("@FrgnName", model.HeaderData.FrgnName),
                        //new SqlParameter("@PrchseItem", model.HeaderData.PrchseItem),
                        //new SqlParameter("@ItemType", model.HeaderData.ItemType),
                        //new SqlParameter("@ItmsGrpCod", model.HeaderData.ItmsGrpCod),
                        //new SqlParameter("@UgpEntry", model.HeaderData.UgpEntry),
                        //new SqlParameter("@ListName", model.HeaderData.ListName),
                        ////new SqlParameter("@PriceUnit", model.HeaderData.PriceUnit),
                        //new SqlParameter("@AvgPrice", model.HeaderData.AvgPrice),

                        //new SqlParameter("@WTLiable", model.Tab_General.WTLiable),
                        //new SqlParameter("@FirmCode", model.Tab_General.FirmCode),
                        //new SqlParameter("@ShipType", model.Tab_General.ShipType),
                        //new SqlParameter("@ManagedItemby", model.Tab_General.ManagedItemby),
                        //new SqlParameter("@validFor", model.Tab_General.ActivevalidFor == "Y" ? "Y" :"N"),
                        //new SqlParameter("@validFrom", model.Tab_General.validFrom == "" ? "NULL" : Convert.ToDateTime(model.Tab_General.validFrom)),
                        //new SqlParameter("@validTo", model.Tab_General.validTo == "" ? "NULL" : Convert.ToDateTime(model.Tab_General.validTo)),
                        //new SqlParameter("@frozenFrom", model.Tab_General.frozenFrom == "" ? "NULL" : Convert.ToDateTime(model.Tab_General.frozenFrom)),
                        //new SqlParameter("@frozenTo", model.Tab_General.frozenTo == "" ? "NULL" : Convert.ToDateTime(model.Tab_General.frozenTo)),
                        
                        ////new SqlParameter("@ BuyUnitMar", model.HeaderData.BuyUnitMar == "" ? "NULL" : Convert.ToDecimal(model.HeaderData.BuyUnitMar)),
                        //new SqlParameter("@ByWh", model.Tab_InventoryData.ByWh),                        
                        //new SqlParameter("@EvalSystem", model.Tab_InventoryData.EvalSystem ),
                        //new SqlParameter("@GLMethod", model.Tab_InventoryData.GLMethod),  
                        //new SqlParameter("@InvntryUom", model.Tab_InventoryData.InvntryUom ),

                        //new SqlParameter("@PrcrmntMtd", model.Tab_PlanningData.PrcrmntMtd),
                        //new SqlParameter("@ PlaningSys", model.Tab_PlanningData.PlaningSys),
                        //new SqlParameter("@MinOrdrQty", model.Tab_PlanningData.MinOrdrQty),

                        //new SqlParameter("@InCostRoll", model.Tab_ProductionData.InCostRoll),
                        //new SqlParameter("@IssueMthd", model.Tab_ProductionData.IssueMthd ),
                        //new SqlParameter("@TreeType", model.Tab_ProductionData.TreeType),
                        //new SqlParameter("@PrdStdCst", model.Tab_ProductionData.PrdStdCst ),
                          


                        //new SqlParameter("@BuyUnitMar", model.Tab_PurchasingData.BuyUnitMsr),
                        //new SqlParameter("@CstGrpCode", model.Tab_PurchasingData.CstGrpCod),
                        //new SqlParameter("@NumInBuy", model.Tab_PurchasingData.NumInBuy),
                        //new SqlParameter("@TotalTax", model.Tab_PurchasingData.TotalTax),
                        //new SqlParameter("@VatGroupPu", model.Tab_PurchasingData.VatGroupPu),

                        //new SqlParameter("@NumInSale", model.Tab_SalesData.NumInSal),
                        //new SqlParameter("@SalUnitMar", model.Tab_SalesData.SalUnitMa),
                        //new SqlParameter("@VatGroupSa", model.Tab_SalesData.VatGroupS),

                        //new SqlParameter("@QryGroup1", model.Tab_Properties.QryGroup1),
                        //new SqlParameter("@QryGroup2", model.Tab_Properties.QryGroup2),
                        //new SqlParameter("@QryGroup3", model.Tab_Properties.QryGroup3),
                        //new SqlParameter("@QryGroup4", model.Tab_Properties.QryGroup4),
                        //new SqlParameter("@QryGroup5", model.Tab_Properties.QryGroup5),
                        //new SqlParameter("@QryGroup6", model.Tab_Properties.QryGroup6),
                        //new SqlParameter("@QryGroup7", model.Tab_Properties.QryGroup7),
                        //new SqlParameter("@QryGroup8", model.Tab_Properties.QryGroup8),
                        //new SqlParameter("@QryGroup9", model.Tab_Properties.QryGroup9),
                        //new SqlParameter("@QryGroup10", model.Tab_Properties.QryGroup10),
                        //new SqlParameter("@QryGroup11", model.Tab_Properties.QryGroup11),
                        //new SqlParameter("@QryGroup12", model.Tab_Properties.QryGroup12),
                        //new SqlParameter("@QryGroup13", model.Tab_Properties.QryGroup13),
                        //new SqlParameter("@QryGroup14", model.Tab_Properties.QryGroup14),
                        //new SqlParameter("@QryGroup15", model.Tab_Properties.QryGroup15),
                        //new SqlParameter("@QryGroup16", model.Tab_Properties.QryGroup16),
                        //new SqlParameter("@QryGroup17", model.Tab_Properties.QryGroup17),
                        //new SqlParameter("@QryGroup18", model.Tab_Properties.QryGroup18),
                        //new SqlParameter("@QryGroup19", model.Tab_Properties.QryGroup19),
                        //new SqlParameter("@QryGroup20", model.Tab_Properties.QryGroup20),
                        //new SqlParameter("@QryGroup21", model.Tab_Properties.QryGroup21),
                        //new SqlParameter("@QryGroup22", model.Tab_Properties.QryGroup22),
                        //new SqlParameter("@QryGroup23", model.Tab_Properties.QryGroup23),
                        //new SqlParameter("@QryGroup24", model.Tab_Properties.QryGroup24),
                        //new SqlParameter("@QryGroup25", model.Tab_Properties.QryGroup25),
                        //new SqlParameter("@QryGroup26", model.Tab_Properties.QryGroup26),
                        //new SqlParameter("@QryGroup27", model.Tab_Properties.QryGroup27),
                        //new SqlParameter("@QryGroup28", model.Tab_Properties.QryGroup28),
                        //new SqlParameter("@QryGroup29", model.Tab_Properties.QryGroup29),
                        //new SqlParameter("@QryGroup30", model.Tab_Properties.QryGroup30),
                        //new SqlParameter("@QryGroup31", model.Tab_Properties.QryGroup31),
                        //new SqlParameter("@QryGroup32", model.Tab_Properties.QryGroup32),
                        //new SqlParameter("@QryGroup33", model.Tab_Properties.QryGroup33),
                        //new SqlParameter("@QryGroup34", model.Tab_Properties.QryGroup34),
                        //new SqlParameter("@QryGroup35", model.Tab_Properties.QryGroup35),
                        //new SqlParameter("@QryGroup36", model.Tab_Properties.QryGroup36),
                        //new SqlParameter("@QryGroup37", model.Tab_Properties.QryGroup37),
                        //new SqlParameter("@QryGroup38", model.Tab_Properties.QryGroup38),
                        //new SqlParameter("@QryGroup39", model.Tab_Properties.QryGroup39),
                        //new SqlParameter("@QryGroup40", model.Tab_Properties.QryGroup40),
                        //new SqlParameter("@QryGroup41", model.Tab_Properties.QryGroup41),
                        //new SqlParameter("@QryGroup42", model.Tab_Properties.QryGroup42),
                        //new SqlParameter("@QryGroup43", model.Tab_Properties.QryGroup43),
                        //new SqlParameter("@QryGroup44", model.Tab_Properties.QryGroup44),
                        //new SqlParameter("@QryGroup45", model.Tab_Properties.QryGroup45),
                        //new SqlParameter("@QryGroup46", model.Tab_Properties.QryGroup46),
                        //new SqlParameter("@QryGroup47", model.Tab_Properties.QryGroup47),
                        //new SqlParameter("@QryGroup48", model.Tab_Properties.QryGroup48),
                        //new SqlParameter("@QryGroup49", model.Tab_Properties.QryGroup49),
                        //new SqlParameter("@QryGroup50", model.Tab_Properties.QryGroup50),
                        //new SqlParameter("@QryGroup51", model.Tab_Properties.QryGroup51),
                        //new SqlParameter("@QryGroup52", model.Tab_Properties.QryGroup52),
                        //new SqlParameter("@QryGroup53", model.Tab_Properties.QryGroup53),
                        //new SqlParameter("@QryGroup54", model.Tab_Properties.QryGroup54),
                        //new SqlParameter("@QryGroup55", model.Tab_Properties.QryGroup55),
                        //new SqlParameter("@QryGroup56", model.Tab_Properties.QryGroup56),
                        //new SqlParameter("@QryGroup57", model.Tab_Properties.QryGroup57),
                        //new SqlParameter("@QryGroup58", model.Tab_Properties.QryGroup58),
                        //new SqlParameter("@QryGroup59", model.Tab_Properties.QryGroup59),
                        //new SqlParameter("@QryGroup60", model.Tab_Properties.QryGroup60),
                        //new SqlParameter("@QryGroup61", model.Tab_Properties.QryGroup61),
                        //new SqlParameter("@QryGroup62", model.Tab_Properties.QryGroup62),
                        //new SqlParameter("@QryGroup63", model.Tab_Properties.QryGroup63),
                        //new SqlParameter("@QryGroup64", model.Tab_Properties.QryGroup64)

                        //};
                        #endregion
                        res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, HeadQuery).ToInt();
                        if (res1 <= 0)
                        {
                            tran.Rollback();
                            return false;
                        }

                        if (model.SalesData.WarehouseList != null)
                        {
                            //int LineNo = 0;
                            foreach (var item in model.SalesData.WarehouseList)
                            {
                                string RowQueryItem1 = @"insert into OITW
                                (ItemCode,WhsCode,WhsName,Locked,MinStock,MaxStock,MinOrder)
                                values(@ItemCode,@WhsCode,@WhsName,@Locked,@MinStock,@MaxStock,@MinOrder)";

                                #region sqlparam
                                List<SqlParameter> param2 = new List<SqlParameter>
                                {
                                     new SqlParameter("@ItemCode",model.HeaderData.ItemCode),
                                     new SqlParameter("@WhsCode",model.Tab_InventoryData_WarehouseList.WhsCode),
                                     new SqlParameter("@WhsName",model.Tab_InventoryData_WarehouseList.WhsName),
                                     new SqlParameter("@Locked",model.Tab_InventoryData_WarehouseList.Locked),
                                     new SqlParameter("@MinStock",model.Tab_InventoryData_WarehouseList.MinStock),
                                     new SqlParameter("@MaxStock",model.Tab_InventoryData_WarehouseList.MaxStock),
                                     new SqlParameter("@MinOrder",model.Tab_InventoryData_WarehouseList.MinOrder)                                     
                                };
                                #endregion

                                res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, RowQueryItem1, param2.ToArray()).ToInt();
                                if (res1 <= 0)
                                {
                                    tran.Rollback();
                                    return false;
                                }
                                //LineNo += 1;
                            }



                        }
                        else
                        {
                            //int LineNo = 0;
                            foreach (var item in GetWareHouseList())
                            {
                                string RowQueryItem2 = @"insert into OITW
                                (ItemCode,WhsCode,WhsName)
                                values(@ItemCode,@WhsCode,@WhsName)";

                                #region sqlparam
                                List<SqlParameter> param3 = new List<SqlParameter>
                                {
                                     new SqlParameter("@ItemCode",model.HeaderData.ItemCode),
                                     new SqlParameter("@WhsCode",item.whscode),
                                     new SqlParameter("@WhsName",item.whsname) 
                                };
                                #endregion

                                res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, RowQueryItem2, param3.ToArray()).ToInt();
                                if (res1 <= 0)
                                {
                                    tran.Rollback();
                                    return false;
                                }
                                //LineNo += 1;
                            }
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
