using iSOL_Enterprise.Common;
using iSOL_Enterprise.Models;
using iSOL_Enterprise.Models.Inventory;
using iSOL_Enterprise.Models.sale;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SAPbobsCOM;
using SqlHelperExtensions;
using System;
using System.Data;
using System.Data.SqlClient;

namespace iSOL_Enterprise.Dal.Inventory
{
    public class ItemMasterDataDal
    {

        public List<ItemMasterModel> GetData()
        {
            string GetQuery = "select Id,ItemCode,ItemName,PrchseItem,SellItem,InvntItem,isPosted,is_Edited  from OITM order by id DESC";


            List<ItemMasterModel> list = new List<ItemMasterModel>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {

                    ItemMasterModel models = new ItemMasterModel();
                    models.DocStatus =  "Open" ;
                    models.Id = rdr["Id"].ToInt();
                    models.ItemCode = rdr["ItemCode"].ToString();
                    models.ItemName = rdr["ItemName"].ToString();
                    models.PurchaseItem = rdr["PrchseItem"].ToString();
                    models.SalesItem = rdr["SellItem"].ToString();
                    models.InventoryItem = rdr["InvntItem"].ToString();
                    
                    models.IsPosted = rdr["isPosted"].ToString(); 
                    models.IsEdited = rdr["is_Edited"].ToString();

                    list.Add(models);
                }
            }
            return list;
        }

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

        public SqlParameter GetParameter(string name,dynamic? value,Type type)
        {
            SqlParameter param = new SqlParameter();
            if (type == typeof(int))
            {
                int value1 = (int) value ;
                param = new SqlParameter(name, value1);
            }
            else if (type == typeof(string))
            {
                string? value1 = value == "" ? null : Convert.ToString(value);
                param = new SqlParameter(name, value1);
            }
            else if (type == typeof(DateTime))
            {
                DateTime?   value1 = value == "" ? null : Convert.ToDateTime(value);
                param = new SqlParameter(name, value1);
            }
            else if (type == typeof(char))
            {
                char value1 = value == "" ? null : Convert.ToChar(value);
                param = new SqlParameter(name, value1);
            }
           // param = new SqlParameter(name,value);
            
            return param;
        }

        public ResponseModels AddItemMasterData(string formData)
        {
            ResponseModels response = new ResponseModels();
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

                        #region Comments
                        //model.HeaderData.Series = model.HeaderData.Series == "" ? "NULL" : Convert.ToInt32(model.HeaderData.Series);
                        //model.HeaderData.ItmsGrpCod = model.HeaderData.ItmsGrpCod == "" ? "NULL" : Convert.ToInt32(model.HeaderData.ItmsGrpCod);
                        //model.HeaderData.UgpEntry = model.HeaderData.UgpEntry == "" ? "NULL" : Convert.ToInt32(model.HeaderData.UgpEntry);
                        //model.HeaderData.AvgPrice = model.HeaderData.AvgPrice == "" ? "NULL" : Convert.ToInt32(model.HeaderData.AvgPrice);
                        //model.Tab_PurchasingData.CstGrpCod = model.Tab_PurchasingData.CstGrpCod == "" ? "NULL" : Convert.ToInt16(model.Tab_PurchasingData.CstGrpCod);
                        //model.Tab_General.ActivevalidFor = model.Tab_General.ActivevalidFor == "Y" ? "Y" : "N";

                        //string TabGeneralDatesQuery = "";
                        //string TabGeneralDatesInsert = "";
                        //if (model.Tab_General.ActivevalidFor == "Y")
                        //{
                        //    model.Tab_General.validFrom = model.Tab_General.validFrom == "" ? "NULL" : Convert.ToDateTime(model.Tab_General.validFrom);
                        //    model.Tab_General.validTo = model.Tab_General.validTo == "" ? "NULL" : Convert.ToDateTime(model.Tab_General.validTo);
                        //    TabGeneralDatesQuery = "validFrom,validTo,";
                        //    TabGeneralDatesInsert = model.Tab_General.validFrom + "','" + model.Tab_General.validTo + "','";
                        //}
                        //else
                        //{
                        //    model.Tab_General.validFrom = "NULL";
                        //    model.Tab_General.validTo = "NULL";
                        //    model.Tab_General.frozenFrom   =  model.Tab_General.frozenFrom == "" ? "NULL" : Convert.ToDateTime(model.Tab_General.frozenFrom) ;
                        //    model.Tab_General.frozenTo   =  model.Tab_General.frozenTo == "" ? "NULL" : Convert.ToDateTime(model.Tab_General.frozenTo);
                        //    TabGeneralDatesQuery = "frozenFrom,frozenTo,";
                        //    TabGeneralDatesInsert = model.Tab_General.frozenFrom + "','" + model.Tab_General.frozenTo + "','";
                        //}
                        #endregion
                        List<SqlParameter> param = new List<SqlParameter>();
                        string PQ = "";
                        string PQ_P = "";
                        string SQ = "";
                        string SQ_P = "";
                        string IQ = "";
                        string IQ_P = "";
                        int Id = CommonDal.getPrimaryKey(tran, "OITM");

                        param.Add(GetParameter("@Id", Id, typeof(int)));
                        param.Add(GetParameter("@Guid", CommonDal.generatedGuid(), typeof(string)));
                        if (model.Tab_PurchasingData != null)
                        {
                            PQ = "BuyUnitMsr,CstGrpCode,NumInBuy,VatGroupPu,";
                            PQ_P = "@BuyUnitMsr,@CstGrpCode,@NumInBuy,@VatGroupPu,";
                            param.Add(GetParameter("@BuyUnitMsr", model.Tab_PurchasingData.BuyUnitMsr, typeof(string)));
                            param.Add(GetParameter("@CstGrpCode", model.Tab_PurchasingData.CstGrpCode, typeof(int)));
                            param.Add(GetParameter("@NumInBuy", model.Tab_PurchasingData.NumInBuy, typeof(string)));
                            param.Add(GetParameter("@VatGroupPu", model.Tab_PurchasingData.VatGroupPu, typeof(string)));
                        }
                        if (model.Tab_SalesData != null)
                        {
                            SQ = "NumInSale,SalUnitMsr,VatGourpSa,";
                            SQ_P = "@NumInSale,@SalUnitMsr,@VatGourpSa,";
                            param.Add(GetParameter("@NumInSale", model.Tab_SalesData.NumInSale, typeof(int)));
                            param.Add(GetParameter("@SalUnitMsr", model.Tab_SalesData.SalUnitMsr, typeof(string)));
                            param.Add(GetParameter("@VatGourpSa", model.Tab_SalesData.VatGroupSa, typeof(string)));
                        }
                        if (model.Tab_InventoryData != null)
                        {
                            IQ = "ByWh,EvalSystem,GLMethod,InvntryUom,";
                            IQ_P = "@ByWh,@EvalSystem,@GLMethod,@InvntryUom,";
                            param.Add(GetParameter("@ByWh", model.Tab_InventoryData.ByWh, typeof(char)));
                            param.Add(GetParameter("@EvalSystem", model.Tab_InventoryData.EvalSystem, typeof(string)));
                            param.Add(GetParameter("@GLMethod", model.Tab_InventoryData.GLMethod, typeof(string)));
                            param.Add(GetParameter("@InvntryUom", model.Tab_InventoryData.InvntryUom, typeof(string)));
                        }

                        string HeadQuery = @"insert into OITM (Id,Guid,ItemCode, ItemName, Series, InvntItem, SellItem, FrgnName, PrchseItem, ItemType, ItmsGrpCod, UgpEntry, AvgPrice, WTLiable, FirmCode, ShipType,MngMethod, validFor, validFrom, validTo, frozenFrom, frozenTo,ManBtchNum, " + IQ+" PrcrmntMtd, PlaningSys, MinOrdrQty, InCostRoll, IssueMthd, TreeType, PrdStdCst, "+PQ+" "+SQ+ " QryGroup1, QryGroup2, QryGroup3, QryGroup4, QryGroup5, QryGroup6, QryGroup7, QryGroup8, QryGroup9, QryGroup10, QryGroup11, QryGroup12, QryGroup13, QryGroup14, QryGroup15, QryGroup16, QryGroup17, QryGroup18, QryGroup19, QryGroup20, QryGroup21, QryGroup22, QryGroup23, QryGroup24, QryGroup25, QryGroup26, QryGroup27, QryGroup28, QryGroup29, QryGroup30, QryGroup31, QryGroup32, QryGroup33, QryGroup34, QryGroup35, QryGroup36, QryGroup37, QryGroup38, QryGroup39, QryGroup40, QryGroup41, QryGroup42, QryGroup43, QryGroup44, QryGroup45, QryGroup46, QryGroup47, QryGroup48, QryGroup49, QryGroup50, QryGroup51, QryGroup52, QryGroup53, QryGroup54, QryGroup55, QryGroup56, QryGroup57, QryGroup58, QryGroup59, QryGroup60, QryGroup61, QryGroup62, QryGroup63, QryGroup64) values(@Id,@Guid, @ItemCode, @ItemName, @Series, @InvntItem, @SellItem, @FrgnName, @PrchseItem, @ItemType, @ItmsGrpCod, @UgpEntry, @AvgPrice, @WTLiable, @FirmCode, @ShipType,@MngMethod, @validFor, @validFrom, @validTo, @frozenFrom, @frozenTo,@ManBtchNum," + IQ_P+" @PrcrmntMtd, @PlaningSys, @MinOrdrQty, @InCostRoll, @IssueMthd, @TreeType, @PrdStdCst, "+PQ_P+" "+SQ_P+" @QryGroup1, @QryGroup2, @QryGroup3, @QryGroup4, @QryGroup5, @QryGroup6, @QryGroup7, @QryGroup8, @QryGroup9, @QryGroup10, @QryGroup11, @QryGroup12, @QryGroup13, @QryGroup14, @QryGroup15, @QryGroup16, @QryGroup17, @QryGroup18, @QryGroup19, @QryGroup20, @QryGroup21, @QryGroup22, @QryGroup23, @QryGroup24, @QryGroup25, @QryGroup26, @QryGroup27, @QryGroup28, @QryGroup29, @QryGroup30, @QryGroup31, @QryGroup32, @QryGroup33, @QryGroup34, @QryGroup35, @QryGroup36, @QryGroup37, @QryGroup38, @QryGroup39, @QryGroup40, @QryGroup41, @QryGroup42, @QryGroup43, @QryGroup44, @QryGroup45, @QryGroup46, @QryGroup47, @QryGroup48, @QryGroup49, @QryGroup50, @QryGroup51, @QryGroup52, @QryGroup53, @QryGroup54, @QryGroup55, @QryGroup56, @QryGroup57, @QryGroup58, @QryGroup59, @QryGroup60, @QryGroup61, @QryGroup62, @QryGroup63, @QryGroup64 )";

                        #region Parameters less query
                        //string HeadQuery = @"insert into OITM                                            
                        //                        (ItemCode,      
                        //                        ItemName,
                        //                        Series,
                        //                        InvntItem,
                        //                        SellItem,
                        //                        FrgnName,
                        //                        PrchseItem,
                        //                        ItemType,
                        //                        ItmsGrpCod,
                        //                        UgpEntry,
                        //                        AvgPrice,

                        //                        WTLiable,
                        //                        FirmCode,
                        //                        ShipType, 
                        //                        validFor,
                        //                        validFrom,
                        //                        validTo,
                        //                        frozenFrom,
                        //                        frozenTo,

                        //                        ByWh,
                        //                        EvalSystem,
                        //                        GLMethod,
                        //                        InvntryUom,

                        //                        PrcrmntMtd,
                        //                        PlaningSys,
                        //                        MinOrdrQty,

                        //                        InCostRoll,
                        //                        IssueMthd,
                        //                        TreeType,
                        //                        PrdStdCst,

                        //                        BuyUnitMsr,
                        //                        CstGrpCode,
                        //                        NumInBuy,   
                        //                        VatGroupPu,

                        //                        NumInSale,
                        //                        SalUnitMsr,
                        //                        VatGourpSa,

                        //                        QryGroup1,
                        //                        QryGroup2,
                        //                        QryGroup3,
                        //                        QryGroup4,
                        //                        QryGroup5,
                        //                        QryGroup6,
                        //                        QryGroup7,
                        //                        QryGroup8,
                        //                        QryGroup9,
                        //                        QryGroup10,
                        //                        QryGroup11,
                        //                        QryGroup12,
                        //                        QryGroup13,
                        //                        QryGroup14,
                        //                        QryGroup15,
                        //                        QryGroup16,
                        //                        QryGroup17,
                        //                        QryGroup18,
                        //                        QryGroup19,
                        //                        QryGroup20,
                        //                        QryGroup21,
                        //                        QryGroup22,
                        //                        QryGroup23,
                        //                        QryGroup24,
                        //                        QryGroup25,
                        //                        QryGroup26,
                        //                        QryGroup27,
                        //                        QryGroup28,
                        //                        QryGroup29,
                        //                        QryGroup30,
                        //                        QryGroup31,
                        //                        QryGroup32,
                        //                        QryGroup33,
                        //                        QryGroup34,
                        //                        QryGroup35,
                        //                        QryGroup36,
                        //                        QryGroup37,
                        //                        QryGroup38,
                        //                        QryGroup39,
                        //                        QryGroup40,
                        //                        QryGroup41,
                        //                        QryGroup42,
                        //                        QryGroup43,
                        //                        QryGroup44,
                        //                        QryGroup45,
                        //                        QryGroup46,
                        //                        QryGroup47,
                        //                        QryGroup48,
                        //                        QryGroup49,
                        //                        QryGroup50,
                        //                        QryGroup51,
                        //                        QryGroup52,
                        //                        QryGroup53,
                        //                        QryGroup54,
                        //                        QryGroup55,
                        //                        QryGroup56,
                        //                        QryGroup57,
                        //                        QryGroup58,
                        //                        QryGroup59,
                        //                        QryGroup60,
                        //                        QryGroup61,
                        //                        QryGroup62,
                        //                        QryGroup63,
                        //                        QryGroup64) 
                        //                        values("
                        //                        + "'" + model.HeaderData.ItemCode + "','"
                        //                        + model.HeaderData.ItemName + "',"
                        //                        + model.HeaderData.Series + ",'"
                        //                        + model.HeaderData.InvntItem + "','"
                        //                        + model.HeaderData.SellItem + "','"
                        //                        + model.HeaderData.FrgnName + "','"
                        //                        + model.HeaderData.PrchseItem + "','"
                        //                        + model.HeaderData.ItemType + "',"
                        //                        + model.HeaderData.ItmsGrpCod  +","
                        //                        + model.HeaderData.UgpEntry + ","
                        //                        + model.HeaderData.AvgPrice + ",'"

                        //                        + model.Tab_General.WTLiable + "','"
                        //                        + model.Tab_General.FirmCode + "','"
                        //                        + model.Tab_General.ShipType + "','" 
                        //                        + model.Tab_General.ActivevalidFor + "','"
                        //                        + model.Tab_General.validFrom  + "','"
                        //                        + model.Tab_General.validTo  + "','"
                        //                        + model.Tab_General.frozenFrom  + "','"
                        //                        + model.Tab_General.frozenTo + "','"

                        //                        + model.Tab_InventoryData.ByWh + "','"
                        //                        + model.Tab_InventoryData.EvalSystem + "','"
                        //                        + model.Tab_InventoryData.GLMethod + "','"
                        //                        + model.Tab_InventoryData.InvntryUom + "','"

                        //                        + model.Tab_PlanningData.PrcrmntMtd + "','"
                        //                        + model.Tab_PlanningData.PlaningSys + "','"
                        //                        + model.Tab_PlanningData.MinOrdrQty + "','"

                        //                        + model.Tab_ProductionData.InCostRoll + "','"
                        //                        + model.Tab_ProductionData.IssueMthd + "','"
                        //                        + model.Tab_ProductionData.TreeType + "','"
                        //                        + model.Tab_ProductionData.PrdStdCst + "','"

                        //                        + model.Tab_PurchasingData.BuyUnitMsr + "',"
                        //                        + model.Tab_PurchasingData.CstGrpCod  +",'"
                        //                        + model.Tab_PurchasingData.NumInBuy + "','" 
                        //                        + model.Tab_PurchasingData.VatGroupPu + "','"

                        //                        + model.Tab_SalesData.NumInSal + "','"
                        //                        + model.Tab_SalesData.SalUnitMa + "','"
                        //                        + model.Tab_SalesData.VatGroupSa + "','"

                        //                        + model.Tab_Properties.QryGroup1 + "','"
                        //                        + model.Tab_Properties.QryGroup2 + "','"
                        //                        + model.Tab_Properties.QryGroup3 + "','"
                        //                        + model.Tab_Properties.QryGroup4 + "','"
                        //                        + model.Tab_Properties.QryGroup5 + "','"
                        //                        + model.Tab_Properties.QryGroup6 + "','"
                        //                        + model.Tab_Properties.QryGroup7 + "','"
                        //                        + model.Tab_Properties.QryGroup8 + "','"
                        //                        + model.Tab_Properties.QryGroup9 + "','"
                        //                        + model.Tab_Properties.QryGroup10 + "','"
                        //                        + model.Tab_Properties.QryGroup11 + "','"
                        //                        + model.Tab_Properties.QryGroup12 + "','"
                        //                        + model.Tab_Properties.QryGroup13 + "','"
                        //                        + model.Tab_Properties.QryGroup14 + "','"
                        //                        + model.Tab_Properties.QryGroup15 + "','"
                        //                        + model.Tab_Properties.QryGroup16 + "','"
                        //                        + model.Tab_Properties.QryGroup17 + "','"
                        //                        + model.Tab_Properties.QryGroup18 + "','"
                        //                        + model.Tab_Properties.QryGroup19 + "','"
                        //                        + model.Tab_Properties.QryGroup20 + "','"
                        //                        + model.Tab_Properties.QryGroup21 + "','"
                        //                        + model.Tab_Properties.QryGroup22 + "','"
                        //                        + model.Tab_Properties.QryGroup23 + "','"
                        //                        + model.Tab_Properties.QryGroup24 + "','"
                        //                        + model.Tab_Properties.QryGroup25 + "','"
                        //                        + model.Tab_Properties.QryGroup26 + "','"
                        //                        + model.Tab_Properties.QryGroup27 + "','"
                        //                        + model.Tab_Properties.QryGroup28 + "','"
                        //                        + model.Tab_Properties.QryGroup29 + "','"
                        //                        + model.Tab_Properties.QryGroup30 + "','"
                        //                        + model.Tab_Properties.QryGroup31 + "','"
                        //                        + model.Tab_Properties.QryGroup32 + "','"
                        //                        + model.Tab_Properties.QryGroup33 + "','"
                        //                        + model.Tab_Properties.QryGroup34 + "','"
                        //                        + model.Tab_Properties.QryGroup35 + "','"
                        //                        + model.Tab_Properties.QryGroup36 + "','"
                        //                        + model.Tab_Properties.QryGroup37 + "','"
                        //                        + model.Tab_Properties.QryGroup38 + "','"
                        //                        + model.Tab_Properties.QryGroup39 + "','"
                        //                        + model.Tab_Properties.QryGroup40 + "','"
                        //                        + model.Tab_Properties.QryGroup41 + "','"
                        //                        + model.Tab_Properties.QryGroup42 + "','"
                        //                        + model.Tab_Properties.QryGroup43 + "','"
                        //                        + model.Tab_Properties.QryGroup44 + "','"
                        //                        + model.Tab_Properties.QryGroup45 + "','"
                        //                        + model.Tab_Properties.QryGroup46 + "','"
                        //                        + model.Tab_Properties.QryGroup47 + "','"
                        //                        + model.Tab_Properties.QryGroup48 + "','"
                        //                        + model.Tab_Properties.QryGroup49 + "','"
                        //                        + model.Tab_Properties.QryGroup50 + "','"
                        //                        + model.Tab_Properties.QryGroup51 + "','"
                        //                        + model.Tab_Properties.QryGroup52 + "','"
                        //                        + model.Tab_Properties.QryGroup53 + "','"
                        //                        + model.Tab_Properties.QryGroup54 + "','"
                        //                        + model.Tab_Properties.QryGroup55 + "','"
                        //                        + model.Tab_Properties.QryGroup56 + "','"
                        //                        + model.Tab_Properties.QryGroup57 + "','"
                        //                        + model.Tab_Properties.QryGroup58 + "','"
                        //                        + model.Tab_Properties.QryGroup59 + "','"
                        //                        + model.Tab_Properties.QryGroup60 + "','"
                        //                        + model.Tab_Properties.QryGroup61 + "','"
                        //                        + model.Tab_Properties.QryGroup62 + "','"
                        //                        + model.Tab_Properties.QryGroup63 + "','"
                        //                        + model.Tab_Properties.QryGroup64 + "')";

                        #endregion

                        #region SqlParameters

                        #region Header data
                        param.Add(GetParameter("@ItemCode",model.HeaderData.ItemCode,typeof(string)));                        
                        param.Add(GetParameter("@ItemName", model.HeaderData.ItemName, typeof(string)));
                        param.Add(GetParameter("@Series", model.HeaderData.Series, typeof(int)));
                        param.Add(GetParameter("@InvntItem", model.HeaderData.InvntItem, typeof(string)));
                        param.Add(GetParameter("@SellItem", model.HeaderData.SellItem, typeof(string)));
                        param.Add(GetParameter("@FrgnName", model.HeaderData.FrgnName, typeof(string)));
                        param.Add(GetParameter("@PrchseItem", model.HeaderData.PrchseItem, typeof(string)));
                        param.Add(GetParameter("@ItemType", model.HeaderData.ItemType, typeof(string)));
                        param.Add(GetParameter("@ItmsGrpCod", model.HeaderData.ItmsGrpCod, typeof(int)));
                        param.Add(GetParameter("@UgpEntry", model.HeaderData.UgpEntry, typeof(int)));
                        param.Add(GetParameter("@AvgPrice", model.HeaderData.AvgPrice, typeof(int)));
                        #endregion

                        #region General
                        param.Add(GetParameter("@WTLiable", model.Tab_General.WTLiable, typeof(string)));
                        param.Add(GetParameter("@FirmCode", model.Tab_General.FirmCode, typeof(string)));
                        param.Add(GetParameter("@ShipType", model.Tab_General.ShipType, typeof(string)));
                        param.Add(GetParameter("@MngMethod", model.Tab_General.MngMethod, typeof(char)));
                        param.Add(GetParameter("@validFor", model.Tab_General.ActivevalidFor, typeof(char)));
                        param.Add(GetParameter("@validFrom", model.Tab_General.validFrom, typeof(string)));
                        param.Add(GetParameter("@validTo", model.Tab_General.validTo, typeof(string)));
                        param.Add(GetParameter("@frozenFrom", model.Tab_General.frozenFrom, typeof(string)));
                        param.Add(GetParameter("@frozenTo", model.Tab_General.frozenTo, typeof(string)));
                        param.Add(GetParameter("@ManBtchNum", model.Tab_General.ManBtchNum, typeof(char)));
                        #endregion

                        #region Planning Data 
                        param.Add(GetParameter("@PrcrmntMtd", model.Tab_PlanningData.PrcrmntMtd, typeof(string)));
                        param.Add(GetParameter("@PlaningSys", model.Tab_PlanningData.PlaningSys, typeof(string)));
                        param.Add(GetParameter("@MinOrdrQty", model.Tab_PlanningData.MinOrdrQty, typeof(string)));
                        #endregion

                        #region Production Data
                        param.Add(GetParameter("@InCostRoll", model.Tab_ProductionData.InCostRoll, typeof(string)));
                        param.Add(GetParameter("@IssueMthd", model.Tab_ProductionData.IssueMthd, typeof(string)));
                        param.Add(GetParameter("@TreeType", model.Tab_ProductionData.TreeType, typeof(string)));
                        param.Add(GetParameter("@PrdStdCst", model.Tab_ProductionData.PrdStdCst, typeof(string)));
                        #endregion



                        #region Properties
                        param.Add(GetParameter("@QryGroup1",  model.Tab_Properties.QryGroup1, typeof(string)));
                        param.Add(GetParameter("@QryGroup2",  model.Tab_Properties.QryGroup2, typeof(string)));
                        param.Add(GetParameter("@QryGroup3",  model.Tab_Properties.QryGroup3, typeof(string)));
                        param.Add(GetParameter("@QryGroup4",  model.Tab_Properties.QryGroup4, typeof(string)));
                        param.Add(GetParameter("@QryGroup5",  model.Tab_Properties.QryGroup5, typeof(string)));
                        param.Add(GetParameter("@QryGroup6",  model.Tab_Properties.QryGroup6, typeof(string)));
                        param.Add(GetParameter("@QryGroup7",  model.Tab_Properties.QryGroup7, typeof(string)));
                        param.Add(GetParameter("@QryGroup8",  model.Tab_Properties.QryGroup8, typeof(string)));
                        param.Add(GetParameter("@QryGroup9",  model.Tab_Properties.QryGroup9, typeof(string)));
                        param.Add(GetParameter("@QryGroup10", model.Tab_Properties.QryGroup10, typeof(string)));
                        param.Add(GetParameter("@QryGroup11", model.Tab_Properties.QryGroup11, typeof(string)));
                        param.Add(GetParameter("@QryGroup12", model.Tab_Properties.QryGroup12, typeof(string)));
                        param.Add(GetParameter("@QryGroup13", model.Tab_Properties.QryGroup13, typeof(string)));
                        param.Add(GetParameter("@QryGroup14", model.Tab_Properties.QryGroup14, typeof(string)));
                        param.Add(GetParameter("@QryGroup15", model.Tab_Properties.QryGroup15, typeof(string)));
                        param.Add(GetParameter("@QryGroup16", model.Tab_Properties.QryGroup16, typeof(string)));
                        param.Add(GetParameter("@QryGroup17", model.Tab_Properties.QryGroup17, typeof(string)));
                        param.Add(GetParameter("@QryGroup18", model.Tab_Properties.QryGroup18, typeof(string)));
                        param.Add(GetParameter("@QryGroup19", model.Tab_Properties.QryGroup19, typeof(string)));
                        param.Add(GetParameter("@QryGroup20", model.Tab_Properties.QryGroup20, typeof(string)));
                        param.Add(GetParameter("@QryGroup21", model.Tab_Properties.QryGroup21, typeof(string)));
                        param.Add(GetParameter("@QryGroup22", model.Tab_Properties.QryGroup22, typeof(string)));
                        param.Add(GetParameter("@QryGroup23", model.Tab_Properties.QryGroup23, typeof(string)));
                        param.Add(GetParameter("@QryGroup24", model.Tab_Properties.QryGroup24, typeof(string)));
                        param.Add(GetParameter("@QryGroup25", model.Tab_Properties.QryGroup25, typeof(string)));
                        param.Add(GetParameter("@QryGroup26", model.Tab_Properties.QryGroup26, typeof(string)));
                        param.Add(GetParameter("@QryGroup27", model.Tab_Properties.QryGroup27, typeof(string)));
                        param.Add(GetParameter("@QryGroup28", model.Tab_Properties.QryGroup28, typeof(string)));
                        param.Add(GetParameter("@QryGroup29", model.Tab_Properties.QryGroup29, typeof(string)));
                        param.Add(GetParameter("@QryGroup30", model.Tab_Properties.QryGroup30, typeof(string)));
                        param.Add(GetParameter("@QryGroup31", model.Tab_Properties.QryGroup31, typeof(string)));
                        param.Add(GetParameter("@QryGroup32", model.Tab_Properties.QryGroup32, typeof(string)));
                        param.Add(GetParameter("@QryGroup33", model.Tab_Properties.QryGroup33, typeof(string)));
                        param.Add(GetParameter("@QryGroup34", model.Tab_Properties.QryGroup34, typeof(string)));
                        param.Add(GetParameter("@QryGroup35", model.Tab_Properties.QryGroup35, typeof(string)));
                        param.Add(GetParameter("@QryGroup36", model.Tab_Properties.QryGroup36, typeof(string)));
                        param.Add(GetParameter("@QryGroup37", model.Tab_Properties.QryGroup37, typeof(string)));
                        param.Add(GetParameter("@QryGroup38", model.Tab_Properties.QryGroup38, typeof(string)));
                        param.Add(GetParameter("@QryGroup39", model.Tab_Properties.QryGroup39, typeof(string)));
                        param.Add(GetParameter("@QryGroup40", model.Tab_Properties.QryGroup40, typeof(string)));
                        param.Add(GetParameter("@QryGroup41", model.Tab_Properties.QryGroup41, typeof(string)));
                        param.Add(GetParameter("@QryGroup42", model.Tab_Properties.QryGroup42, typeof(string)));
                        param.Add(GetParameter("@QryGroup43", model.Tab_Properties.QryGroup43, typeof(string)));
                        param.Add(GetParameter("@QryGroup44", model.Tab_Properties.QryGroup44, typeof(string)));
                        param.Add(GetParameter("@QryGroup45", model.Tab_Properties.QryGroup45, typeof(string)));
                        param.Add(GetParameter("@QryGroup46", model.Tab_Properties.QryGroup46, typeof(string)));
                        param.Add(GetParameter("@QryGroup47", model.Tab_Properties.QryGroup47, typeof(string)));
                        param.Add(GetParameter("@QryGroup48", model.Tab_Properties.QryGroup48, typeof(string)));
                        param.Add(GetParameter("@QryGroup49", model.Tab_Properties.QryGroup49, typeof(string)));
                        param.Add(GetParameter("@QryGroup50", model.Tab_Properties.QryGroup50, typeof(string)));
                        param.Add(GetParameter("@QryGroup51", model.Tab_Properties.QryGroup51, typeof(string)));
                        param.Add(GetParameter("@QryGroup52", model.Tab_Properties.QryGroup52, typeof(string)));
                        param.Add(GetParameter("@QryGroup53", model.Tab_Properties.QryGroup53, typeof(string)));
                        param.Add(GetParameter("@QryGroup54", model.Tab_Properties.QryGroup54, typeof(string)));
                        param.Add(GetParameter("@QryGroup55", model.Tab_Properties.QryGroup55, typeof(string)));
                        param.Add(GetParameter("@QryGroup56", model.Tab_Properties.QryGroup56, typeof(string)));
                        param.Add(GetParameter("@QryGroup57", model.Tab_Properties.QryGroup57, typeof(string)));
                        param.Add(GetParameter("@QryGroup58", model.Tab_Properties.QryGroup58, typeof(string)));
                        param.Add(GetParameter("@QryGroup59", model.Tab_Properties.QryGroup59, typeof(string)));
                        param.Add(GetParameter("@QryGroup60", model.Tab_Properties.QryGroup60, typeof(string)));
                        param.Add(GetParameter("@QryGroup61", model.Tab_Properties.QryGroup61, typeof(string)));
                        param.Add(GetParameter("@QryGroup62", model.Tab_Properties.QryGroup62, typeof(string)));
                        param.Add(GetParameter("@QryGroup63", model.Tab_Properties.QryGroup63, typeof(string)));
                        param.Add(GetParameter("@QryGroup64", model.Tab_Properties.QryGroup64, typeof(string)));
                        #endregion


                        #endregion
                        res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, HeadQuery ,param.ToArray()).ToInt();
                        if (res1 <= 0)
                        {
                            tran.Rollback();
                            response.isSuccess = false;
                            response.Message = "An Error Occured";
                            return response;
                        }

                        if (model.Tab_InventoryData_WareHouseList != null)
                        {
                            //int LineNo = 0;
                            foreach (var item in model.Tab_InventoryData_WareHouseList)
                            {

                                string RowQueryItem1 = @"insert into OITW
                                (ItemCode,WhsCode,WhsName,Locked,MinStock,MaxStock,MinOrder)
                                values(@ItemCode,@WhsCode,@WhsName,@Locked,@MinStock,@MaxStock,@MinOrder)";
                               
                                #region sqlparam
                                List<SqlParameter> param2 = new List<SqlParameter>();
                                //{
                                param2.Add(GetParameter("@ItemCode", model.HeaderData.ItemCode, typeof(string)));
                                param2.Add(GetParameter("@WhsCode",item.WhsCode, typeof(string)));
                                param2.Add(GetParameter("@WhsName", item.WhsName, typeof(string)));
                                param2.Add(GetParameter("@Locked", item.Locked, typeof(char)));
                                param2.Add(GetParameter("@MinStock",item.MinStock, typeof(int)));
                                param2.Add(GetParameter("@MaxStock",item.MaxStock, typeof(int)));
                                param2.Add(GetParameter("@MinOrder",item.MinOrder, typeof(int)));                                //};

                                #endregion

                                res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, RowQueryItem1, param2.ToArray()).ToInt();
                                if (res1 <= 0)
                                {
                                    tran.Rollback();
                                    response.isSuccess = false;
                                    response.Message = "An Error Occured";
                                    return response;
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
                                values(@ItemCode, @WhsCode, @WhsName)";
                              
                                #region sqlparam

                                List<SqlParameter> param3 = new List<SqlParameter>();
                                //{
                                param3.Add(GetParameter("@ItemCode", model.HeaderData.ItemCode, typeof(string)));
                                param3.Add(GetParameter("@WhsCode", item.whscode, typeof(string)));
                                param3.Add(GetParameter("@WhsName", item.whsname, typeof(string)));
                                //};

                                #endregion

                                res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, RowQueryItem2,param3.ToArray()).ToInt();
                                if (res1 <= 0)
                                {
                                    response.isSuccess = false;
                                    response.Message = "An Error Occured";
                                    return response;
                                }
                                //LineNo += 1;
                            }
                        }


                    }
                    if (res1 > 0)
                    {
                        tran.Commit();
                        response.isSuccess = true;
                        response.Message = "Item Added Successfully !";

                    }

                }
                catch (Exception e )
                {
                    tran.Rollback();
                    response.isSuccess = false;
                    response.Message = e.Message;
                    return response;
                }
                return response;
            }
            catch (Exception e )
            {

                response.isSuccess = false;
                response.Message = e.Message;
                return response;
            }
        }
    }
}
