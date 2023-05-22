using iSOL_Enterprise.Common;
using iSOL_Enterprise.Models;
using iSOL_Enterprise.Models.Inventory;
using iSOL_Enterprise.Models.sale;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SAPbobsCOM;
using SqlHelperExtensions;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Nodes;

namespace iSOL_Enterprise.Dal.Inventory
{
    public class ItemMasterDataDal
    {

        public List<ItemMasterModel> GetData()
        {
            string GetQuery = "select Id,Guid,ItemCode,ItemName,PrchseItem,SellItem,InvntItem,isPosted,is_Edited,isApproved,apprSeen  from OITM order by id DESC";


            List<ItemMasterModel> list = new List<ItemMasterModel>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {

                    ItemMasterModel models = new ItemMasterModel();
                    models.DocStatus = "Open";
                    models.Id = rdr["Id"].ToInt();
                    models.ItemCode = rdr["ItemCode"].ToString();
                    models.ItemName = rdr["ItemName"].ToString();
                    models.PurchaseItem = rdr["PrchseItem"].ToString();
                    models.SalesItem = rdr["SellItem"].ToString();
                    models.InventoryItem = rdr["InvntItem"].ToString();
                    models.Guid = rdr["Guid"].ToString();
                    models.IsPosted = rdr["isPosted"].ToString();
                    models.IsEdited = rdr["is_Edited"].ToString();
                    models.isApproved = rdr["isApproved"].ToBool();
                    models.apprSeen = rdr["apprSeen"].ToBool();

                    list.Add(models);
                }
            }
            return list;
        }
        public dynamic GetItemOldData(string ItemID)
        {
            try
            {



                DataSet ds = new DataSet();
                SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
                string headerQuery = @"select ItemCode, ItemName, Series, InvntItem, SellItem,PrchseItem, FrgnName , ItemType, ItmsGrpCod, UgpEntry, AvgPrice, WTLiable, FirmCode, ShipType,
                                            MngMethod, validFor, validFrom, validTo, frozenFrom, frozenTo,ManBtchNum, ByWh,EvalSystem,GLMethod,InvntryUom, PrcrmntMtd, PlaningSys, MinOrdrQty, InCostRoll, IssueMthd, 
                                            TreeType, PrdStdCst,BuyUnitMsr,CstGrpCode,NumInBuy,VatGroupPu,NumInSale,SalUnitMsr,VatGourpSa, QryGroup1, QryGroup2, QryGroup3, QryGroup4, QryGroup5, QryGroup6,
                                            QryGroup7,QryGroup8, QryGroup9, QryGroup10, QryGroup11, QryGroup12, QryGroup13, QryGroup14, QryGroup15, QryGroup16, QryGroup17, QryGroup18, QryGroup19, QryGroup20,
                                            QryGroup21, QryGroup22, QryGroup23, QryGroup24, QryGroup25, QryGroup26, QryGroup27, QryGroup28, QryGroup29, QryGroup30, QryGroup31, QryGroup32, QryGroup33,
                                            QryGroup34, QryGroup35, QryGroup36, QryGroup37, QryGroup38, QryGroup39, QryGroup40, QryGroup41, QryGroup42, QryGroup43, QryGroup44, QryGroup45, QryGroup46, 
                                            QryGroup47, QryGroup48, QryGroup49, QryGroup50, QryGroup51, QryGroup52, QryGroup53, QryGroup54, QryGroup55, QryGroup56, QryGroup57, QryGroup58, QryGroup59, 
                                            QryGroup60, QryGroup61, QryGroup62, QryGroup63, QryGroup64 from OITM where guid ='" + ItemID + "'";
                SqlDataAdapter sda = new SqlDataAdapter(headerQuery, conn);
                sda.Fill(ds);
                string JSONString = string.Empty;
                JSONString = Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables);
                return JSONString;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public bool IsItemGroupEditable(string ItemCode)
        {
            string ObjectCodeQuery = "Select ObjectCode from Pages";
            CommonDal dal = new CommonDal();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, ObjectCodeQuery))
            {
                while (rdr.Read())
                {
                    string RowTable = dal.GetRowTable(rdr["ObjectCode"].ToInt());
                    if (RowTable != "")
                    {


                        string FindItemCodeInRowTable = @"select Count(*) from " + RowTable + " where ItemCode ='" + ItemCode + "'";
                        int count = SqlHelper.ExecuteScalar(SqlHelper.defaultDB, CommandType.Text, FindItemCodeInRowTable).ToInt();
                        if (count > 0)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        public bool IsPurchaseItemEditable(string ItemCode)
        {

            int[] ObjectCodes = { 540000006, 22, 20, 21, 18, 19 };
            CommonDal dal = new CommonDal();
            foreach (var ObjectCode in ObjectCodes)
            {

                string RowTable = dal.GetRowTable(ObjectCode);
                if (RowTable != "")
                {
                    string FindItemCodeInRowTable = @"select Count(*) from " + RowTable + " where ItemCode ='" + ItemCode + "'";
                    int count = SqlHelper.ExecuteScalar(SqlHelper.defaultDB, CommandType.Text, FindItemCodeInRowTable).ToInt();
                    if (count > 0)
                    {
                        return false;
                    }
                }

            }
            return true;
        }
        public bool IsSalesItemEditable(string ItemCode)
        {

            int[] ObjectCodes = { 23, 17, 15, 16, 13, 14 };
            CommonDal dal = new CommonDal();
            foreach (var ObjectCode in ObjectCodes)
            {

                string RowTable = dal.GetRowTable(ObjectCode);
                if (RowTable != "")
                {
                    string FindItemCodeInRowTable = @"select Count(*) from " + RowTable + " where ItemCode ='" + ItemCode + "'";
                    int count = SqlHelper.ExecuteScalar(SqlHelper.defaultDB, CommandType.Text, FindItemCodeInRowTable).ToInt();
                    if (count > 0)
                    {
                        return false;
                    }
                }

            }
            return true;
        }
        public bool IsInventoryItemEditable(string ItemCode)
        {

            int[] ObjectCodes = { 59, 60, 67 };
            CommonDal dal = new CommonDal();
            foreach (var ObjectCode in ObjectCodes)
            {

                string RowTable = dal.GetRowTable(ObjectCode);
                if (RowTable != "")
                {
                    string FindItemCodeInRowTable = @"select Count(*) from " + RowTable + " where ItemCode ='" + ItemCode + "'";
                    int count = SqlHelper.ExecuteScalar(SqlHelper.defaultDB, CommandType.Text, FindItemCodeInRowTable).ToInt();
                    if (count > 0)
                    {
                        return false;
                    }
                }

            }
            return true;
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
            string GetItemCode = "select case when BeginStr is  null  then  RIGHT('000000' + CAST(NextNumber AS VARCHAR(6)), 6)  else BeginStr  +  CAST(  NextNumber as nvarchar(20))   end 'ItemCode' from NNM1 where  NextNumber <= LastNum and Series = " + Series;

            string? ItemCode = Convert.ToString(SqlHelper.ExecuteScalar(SqlHelper.defaultDB, CommandType.Text, GetItemCode));

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

     

        public ResponseModels AddItemMasterData(string formData)
        {
            ResponseModels response = new ResponseModels();
            try
            {


                var model = JsonConvert.DeserializeObject<dynamic>(formData);
                if (model.OldItemId != null)
                {
                    response = EditItemMaster(model);
                }
                else
                {
                    response = AddItemMaster(model);
                }

                return response;


            }
            catch (Exception e)
            {

                response.isSuccess = false;
                response.Message = e.Message;
                return response;
            }
        }


        public ResponseModels AddItemMaster(dynamic model)
        {
            ResponseModels response = new ResponseModels();
            CommonDal cdal = new CommonDal();
            SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
            conn.Open();
            int MySeries = Convert.ToInt32(model.HeaderData.MySeries);
            SqlTransaction tran = conn.BeginTransaction();
            int res1 = 0;
            try
            {
                //int Id = CommonDal.getPrimaryKey(tran, "OITM");
                //string DocNum = SqlHelper.getUpdatedDocumentNumberOnLoad(tran, "OQUT", "SQ");
                if (model.HeaderData != null)
                {

                    List<SqlParameter> param = new List<SqlParameter>();
                    string PQ = "";
                    string PQ_P = "";
                    string SQ = "";
                    string SQ_P = "";
                    string IQ = "";
                    string IQ_P = "";
                    int Id = CommonDal.getPrimaryKey(tran, "OITM");
                    string Guid = CommonDal.generatedGuid();
                    param.Add(cdal.GetParameter("@Id", Id, typeof(int)));
                    param.Add(cdal.GetParameter("@DocEntry", Id, typeof(int)));
                    param.Add(cdal.GetParameter("@Guid", Guid, typeof(string)));
                    if (model.Tab_PurchasingData != null)
                    {
                        PQ = "BuyUnitMsr,CstGrpCode,NumInBuy,VatGroupPu,";
                        PQ_P = "@BuyUnitMsr,@CstGrpCode,@NumInBuy,@VatGroupPu,";
                        param.Add(cdal.GetParameter("@BuyUnitMsr", model.Tab_PurchasingData.BuyUnitMsr, typeof(string)));
                        param.Add(cdal.GetParameter("@CstGrpCode", model.Tab_PurchasingData.CstGrpCode, typeof(int)));
                        param.Add(cdal.GetParameter("@NumInBuy", model.Tab_PurchasingData.NumInBuy, typeof(string)));
                        param.Add(cdal.GetParameter("@VatGroupPu", model.Tab_PurchasingData.VatGroupPu, typeof(string)));
                    }
                    if (model.Tab_SalesData != null)
                    {
                        SQ = "NumInSale,SalUnitMsr,VatGourpSa,";
                        SQ_P = "@NumInSale,@SalUnitMsr,@VatGourpSa,";
                        param.Add(cdal.GetParameter("@NumInSale", model.Tab_SalesData.NumInSale, typeof(int)));
                        param.Add(cdal.GetParameter("@SalUnitMsr", model.Tab_SalesData.SalUnitMsr, typeof(string)));
                        param.Add(cdal.GetParameter("@VatGourpSa", model.Tab_SalesData.VatGroupSa, typeof(string)));
                    }
                    if (model.Tab_InventoryData != null)
                    {
                        IQ = "ByWh,EvalSystem,GLMethod,InvntryUom,";
                        IQ_P = "@ByWh,@EvalSystem,@GLMethod,@InvntryUom,";
                        param.Add(cdal.GetParameter("@ByWh", model.Tab_InventoryData.ByWh, typeof(char)));
                        param.Add(cdal.GetParameter("@EvalSystem", model.Tab_InventoryData.EvalSystem, typeof(string)));
                        param.Add(cdal.GetParameter("@GLMethod", model.Tab_InventoryData.GLMethod, typeof(string)));
                        param.Add(cdal.GetParameter("@InvntryUom", model.Tab_InventoryData.InvntryUom, typeof(string)));
                    }
                    if (MySeries != -1)
                    {

                    #region BackendCheck For Series
                        string? ItemCode = SqlHelper.MySeriesUpdate_GetItemCode(MySeries, tran);
                        if (ItemCode == null)
                        {
                            tran.Rollback();
                            response.isSuccess = false;
                            response.Message = "An Error Occured";
                            return response;
                        }
                    model.HeaderData.ItemCode = ItemCode;
                    }
                    #endregion
                    else
                    {
                        int count = SqlHelper.ExecuteScalar(tran, CommandType.Text, "select Count(*) from OITM where ItemCode ='" + model.HeaderData.ItemCode.ToString() + "'");
                        if (count > 0)
                        {
                            tran.Rollback();
                            response.isSuccess = false;
                            response.Message = "Duplicate Item Number !";
                            return response;
                        }
                    }

                    int ObjectCode = 4;
                    int isApproved = ObjectCode.GetApprovalStatus(tran);
                    #region Insert in Approval Table

                    if (isApproved == 0)
                    {
                        ApprovalModel approvalModel = new()
                        {
                            Id = CommonDal.getPrimaryKey(tran, "tbl_DocumentsApprovals"),
                            ObjectCode = ObjectCode,
                            DocEntry = Id,
                            DocNum = model.HeaderData.ItemCode.ToString(),
                            Guid = Guid

                        };
                        bool resp = cdal.AddApproval(tran, approvalModel);
                        if (!resp)
                        {
                            
                            response.isSuccess = false;
                            response.Message = "An Error occured !";
                            return response;
                        }
                    }

                    #endregion

                    string HeadQuery = @"insert into OITM (Id,Guid,ItemCode, ItemName, Series, InvntItem, SellItem, FrgnName, PrchseItem, ItemType, ItmsGrpCod, UgpEntry, AvgPrice, WTLiable, FirmCode, ShipType,MngMethod, 
                                        validFor, validFrom, validTo, frozenFrom, frozenTo,ManBtchNum, " + IQ + " PrcrmntMtd, PlaningSys, MinOrdrQty, InCostRoll, IssueMthd, TreeType, PrdStdCst, " + PQ + " " + SQ + " " +
                                        "QryGroup1, QryGroup2, QryGroup3, QryGroup4, QryGroup5, QryGroup6, QryGroup7, QryGroup8, QryGroup9, QryGroup10, QryGroup11, QryGroup12, QryGroup13, QryGroup14, QryGroup15, QryGroup16, " +
                                        "QryGroup17, QryGroup18, QryGroup19, QryGroup20, QryGroup21, QryGroup22, QryGroup23, QryGroup24, QryGroup25, QryGroup26, QryGroup27, QryGroup28, QryGroup29, QryGroup30, QryGroup31, QryGroup32, " +
                                        "QryGroup33, QryGroup34, QryGroup35, QryGroup36, QryGroup37, QryGroup38, QryGroup39, QryGroup40, QryGroup41, QryGroup42, QryGroup43, QryGroup44, QryGroup45, QryGroup46, QryGroup47, QryGroup48, " +
                                        "QryGroup49, QryGroup50, QryGroup51, QryGroup52, QryGroup53, QryGroup54, QryGroup55, QryGroup56, QryGroup57, QryGroup58, QryGroup59, QryGroup60, QryGroup61, QryGroup62, QryGroup63, QryGroup64,isApproved ,MySeries) " +
                                        "values(@Id,@Guid, @ItemCode, @ItemName, @Series, @InvntItem, @SellItem, @FrgnName, @PrchseItem, @ItemType, @ItmsGrpCod, @UgpEntry, @AvgPrice, @WTLiable, @FirmCode, @ShipType,@MngMethod, @validFor, @validFrom, @validTo, " +
                                        "@frozenFrom, @frozenTo,@ManBtchNum," + IQ_P + " @PrcrmntMtd, @PlaningSys, @MinOrdrQty, @InCostRoll, @IssueMthd, @TreeType, @PrdStdCst, " + PQ_P + " " + SQ_P + " @QryGroup1, @QryGroup2, @QryGroup3, @QryGroup4, @QryGroup5, " +
                                        "@QryGroup6, @QryGroup7, @QryGroup8, @QryGroup9, @QryGroup10, @QryGroup11, @QryGroup12, @QryGroup13, @QryGroup14, @QryGroup15, @QryGroup16, @QryGroup17, @QryGroup18, @QryGroup19, @QryGroup20, @QryGroup21, @QryGroup22, " +
                                        "@QryGroup23, @QryGroup24, @QryGroup25, @QryGroup26, @QryGroup27, @QryGroup28, @QryGroup29, @QryGroup30, @QryGroup31, @QryGroup32, @QryGroup33, @QryGroup34, @QryGroup35, @QryGroup36, @QryGroup37, @QryGroup38, @QryGroup39, " +
                                        "@QryGroup40, @QryGroup41, @QryGroup42, @QryGroup43, @QryGroup44, @QryGroup45, @QryGroup46, @QryGroup47, @QryGroup48, @QryGroup49, @QryGroup50, @QryGroup51, @QryGroup52, @QryGroup53, @QryGroup54, @QryGroup55, @QryGroup56, " +
                                        "@QryGroup57, @QryGroup58, @QryGroup59, @QryGroup60, @QryGroup61, @QryGroup62, @QryGroup63, @QryGroup64,@isApproved, " + MySeries + ")";



                    #region SqlParameters

                    #region Header data
                    param.Add(cdal.GetParameter("@ItemCode", model.HeaderData.ItemCode, typeof(string)));
                    param.Add(cdal.GetParameter("@ItemName", model.HeaderData.ItemName, typeof(string)));
                    param.Add(cdal.GetParameter("@Series", model.HeaderData.Series, typeof(int)));
                    param.Add(cdal.GetParameter("@InvntItem", model.HeaderData.InvntItem, typeof(string)));
                    param.Add(cdal.GetParameter("@SellItem", model.HeaderData.SellItem, typeof(string)));
                    param.Add(cdal.GetParameter("@FrgnName", model.HeaderData.FrgnName, typeof(string)));
                    param.Add(cdal.GetParameter("@PrchseItem", model.HeaderData.PrchseItem, typeof(string)));
                    param.Add(cdal.GetParameter("@ItemType", model.HeaderData.ItemType, typeof(string)));
                    param.Add(cdal.GetParameter("@ItmsGrpCod", model.HeaderData.ItmsGrpCod, typeof(int)));
                    param.Add(cdal.GetParameter("@UgpEntry", model.HeaderData.UgpEntry, typeof(int)));
                    param.Add(cdal.GetParameter("@AvgPrice", model.HeaderData.AvgPrice, typeof(decimal)));
                    #endregion

                    #region General
                    param.Add(cdal.GetParameter("@WTLiable", model.Tab_General.WTLiable, typeof(string)));
                    param.Add(cdal.GetParameter("@FirmCode", model.Tab_General.FirmCode, typeof(string)));
                    param.Add(cdal.GetParameter("@ShipType", model.Tab_General.ShipType, typeof(string)));
                    param.Add(cdal.GetParameter("@MngMethod", model.Tab_General.MngMethod, typeof(char)));
                    param.Add(cdal.GetParameter("@validFor", model.Tab_General.ActivevalidFor, typeof(char)));
                    param.Add(cdal.GetParameter("@validFrom", model.Tab_General.validFrom, typeof(string)));
                    param.Add(cdal.GetParameter("@validTo", model.Tab_General.validTo, typeof(string)));
                    param.Add(cdal.GetParameter("@frozenFrom", model.Tab_General.frozenFrom, typeof(string)));
                    param.Add(cdal.GetParameter("@frozenTo", model.Tab_General.frozenTo, typeof(string)));
                    param.Add(cdal.GetParameter("@ManBtchNum", model.Tab_General.ManBtchNum, typeof(char)));
                    #endregion

                    #region Planning Data 
                    param.Add(cdal.GetParameter("@PrcrmntMtd", model.Tab_PlanningData.PrcrmntMtd, typeof(string)));
                    param.Add(cdal.GetParameter("@PlaningSys", model.Tab_PlanningData.PlaningSys, typeof(string)));
                    param.Add(cdal.GetParameter("@MinOrdrQty", model.Tab_PlanningData.MinOrdrQty, typeof(string)));
                    #endregion

                    #region Production Data
                    param.Add(cdal.GetParameter("@InCostRoll", model.Tab_ProductionData.InCostRoll, typeof(string)));
                    param.Add(cdal.GetParameter("@IssueMthd", model.Tab_ProductionData.IssueMthd, typeof(string)));
                    param.Add(cdal.GetParameter("@TreeType", model.Tab_ProductionData.TreeType, typeof(string)));
                    param.Add(cdal.GetParameter("@PrdStdCst", model.Tab_ProductionData.PrdStdCst, typeof(string)));
                    #endregion



                    #region Properties
                    param.Add(cdal.GetParameter("@QryGroup1", model.Tab_Properties.QryGroup1, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup2", model.Tab_Properties.QryGroup2, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup3", model.Tab_Properties.QryGroup3, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup4", model.Tab_Properties.QryGroup4, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup5", model.Tab_Properties.QryGroup5, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup6", model.Tab_Properties.QryGroup6, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup7", model.Tab_Properties.QryGroup7, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup8", model.Tab_Properties.QryGroup8, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup9", model.Tab_Properties.QryGroup9, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup10", model.Tab_Properties.QryGroup10, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup11", model.Tab_Properties.QryGroup11, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup12", model.Tab_Properties.QryGroup12, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup13", model.Tab_Properties.QryGroup13, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup14", model.Tab_Properties.QryGroup14, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup15", model.Tab_Properties.QryGroup15, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup16", model.Tab_Properties.QryGroup16, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup17", model.Tab_Properties.QryGroup17, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup18", model.Tab_Properties.QryGroup18, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup19", model.Tab_Properties.QryGroup19, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup20", model.Tab_Properties.QryGroup20, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup21", model.Tab_Properties.QryGroup21, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup22", model.Tab_Properties.QryGroup22, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup23", model.Tab_Properties.QryGroup23, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup24", model.Tab_Properties.QryGroup24, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup25", model.Tab_Properties.QryGroup25, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup26", model.Tab_Properties.QryGroup26, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup27", model.Tab_Properties.QryGroup27, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup28", model.Tab_Properties.QryGroup28, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup29", model.Tab_Properties.QryGroup29, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup30", model.Tab_Properties.QryGroup30, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup31", model.Tab_Properties.QryGroup31, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup32", model.Tab_Properties.QryGroup32, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup33", model.Tab_Properties.QryGroup33, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup34", model.Tab_Properties.QryGroup34, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup35", model.Tab_Properties.QryGroup35, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup36", model.Tab_Properties.QryGroup36, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup37", model.Tab_Properties.QryGroup37, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup38", model.Tab_Properties.QryGroup38, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup39", model.Tab_Properties.QryGroup39, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup40", model.Tab_Properties.QryGroup40, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup41", model.Tab_Properties.QryGroup41, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup42", model.Tab_Properties.QryGroup42, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup43", model.Tab_Properties.QryGroup43, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup44", model.Tab_Properties.QryGroup44, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup45", model.Tab_Properties.QryGroup45, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup46", model.Tab_Properties.QryGroup46, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup47", model.Tab_Properties.QryGroup47, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup48", model.Tab_Properties.QryGroup48, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup49", model.Tab_Properties.QryGroup49, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup50", model.Tab_Properties.QryGroup50, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup51", model.Tab_Properties.QryGroup51, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup52", model.Tab_Properties.QryGroup52, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup53", model.Tab_Properties.QryGroup53, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup54", model.Tab_Properties.QryGroup54, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup55", model.Tab_Properties.QryGroup55, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup56", model.Tab_Properties.QryGroup56, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup57", model.Tab_Properties.QryGroup57, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup58", model.Tab_Properties.QryGroup58, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup59", model.Tab_Properties.QryGroup59, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup60", model.Tab_Properties.QryGroup60, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup61", model.Tab_Properties.QryGroup61, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup62", model.Tab_Properties.QryGroup62, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup63", model.Tab_Properties.QryGroup63, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup64", model.Tab_Properties.QryGroup64, typeof(string)));
                    #endregion

                    param.Add(cdal.GetParameter("@isApproved", isApproved, typeof(int)));

                    #endregion

                    res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, HeadQuery, param.ToArray()).ToInt();
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
                            param2.Add(cdal.GetParameter("@ItemCode", model.HeaderData.ItemCode, typeof(string)));
                            param2.Add(cdal.GetParameter("@WhsCode", item.WhsCode, typeof(string)));
                            param2.Add(cdal.GetParameter("@WhsName", item.WhsName, typeof(string)));
                            param2.Add(cdal.GetParameter("@Locked", item.Locked, typeof(char)));
                            param2.Add(cdal.GetParameter("@MinStock", item.MinStock, typeof(decimal)));
                            param2.Add(cdal.GetParameter("@MaxStock", item.MaxStock, typeof(decimal)));
                            param2.Add(cdal.GetParameter("@MinOrder", item.MinOrder, typeof(decimal)));                                //};

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
                            param3.Add(cdal.GetParameter("@ItemCode", model.HeaderData.ItemCode, typeof(string)));
                            param3.Add(cdal.GetParameter("@WhsCode", item.whscode, typeof(string)));
                            param3.Add(cdal.GetParameter("@WhsName", item.whsname, typeof(string)));
                            //};

                            #endregion

                            res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, RowQueryItem2, param3.ToArray()).ToInt();
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
            catch (Exception e)
            {
                tran.Rollback();
                response.isSuccess = false;
                response.Message = e.Message;
                return response;
            }
            return response;

        }

        public ResponseModels EditItemMaster(dynamic model)
        {
            ResponseModels response = new ResponseModels();
            CommonDal cdal = new CommonDal();
            SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
            conn.Open();
            int MySeries = Convert.ToInt32(model.HeaderData.MySeries);
            SqlTransaction tran = conn.BeginTransaction();
            int res1 = 0;
            try
            {
                //int Id = CommonDal.getPrimaryKey(tran, "OITM");
                //string DocNum = SqlHelper.getUpdatedDocumentNumberOnLoad(tran, "OQUT", "SQ");
                if (model.HeaderData != null)
                {

                    List<SqlParameter> param = new List<SqlParameter>();
                    string PQ = "";
                    string InvntItem = "";
                    string SQ = "";
                    string SellItem = "";
                    string IQ = "";
                    string PrchseItem = "";
                    string ItmsGrpCod = "";




                    if (IsPurchaseItemEditable(model.HeaderData.ItemCode.ToString()))
                    {
                        PrchseItem = "PrchseItem=@PrchseItem,";
                        param.Add(cdal.GetParameter("@PrchseItem", model.HeaderData.PrchseItem, typeof(string)));
                        if (model.Tab_PurchasingData != null)
                        {
                            PQ = "BuyUnitMsr=@BuyUnitMsr,CstGrpCode=@CstGrpCode,NumInBuy=@NumInBuy,VatGroupPu=@VatGroupPu,";

                            param.Add(cdal.GetParameter("@BuyUnitMsr", model.Tab_PurchasingData.BuyUnitMsr, typeof(string)));
                            param.Add(cdal.GetParameter("@CstGrpCode", model.Tab_PurchasingData.CstGrpCode, typeof(int)));
                            param.Add(cdal.GetParameter("@NumInBuy", model.Tab_PurchasingData.NumInBuy, typeof(string)));
                            param.Add(cdal.GetParameter("@VatGroupPu", model.Tab_PurchasingData.VatGroupPu, typeof(string)));
                        }
                    }
                    if (IsSalesItemEditable(model.HeaderData.ItemCode.ToString()))
                    {
                        SellItem = "SellItem =@SellItem,";
                        param.Add(cdal.GetParameter("@SellItem", model.HeaderData.SellItem, typeof(string)));

                        if (model.Tab_SalesData != null)
                        {
                            SQ = "NumInSale=@NumInSale,SalUnitMsr=@SalUnitMsr,VatGourpSa=@VatGourpSa,";

                            param.Add(cdal.GetParameter("@NumInSale", model.Tab_SalesData.NumInSale, typeof(decimal)));
                            param.Add(cdal.GetParameter("@SalUnitMsr", model.Tab_SalesData.SalUnitMsr, typeof(string)));
                            param.Add(cdal.GetParameter("@VatGourpSa", model.Tab_SalesData.VatGroupSa, typeof(string)));
                        }
                    }
                    if (IsInventoryItemEditable(model.HeaderData.ItemCode.ToString()))
                    {
                        InvntItem = "InvntItem=@InvntItem,";
                        param.Add(cdal.GetParameter("@InvntItem", model.HeaderData.InvntItem, typeof(string)));
                        if (model.Tab_InventoryData != null)
                        {
                            IQ = "ByWh=@ByWh,EvalSystem=@EvalSystem,GLMethod=@GLMethod,InvntryUom=@InvntryUom,";

                            param.Add(cdal.GetParameter("@ByWh", model.Tab_InventoryData.ByWh, typeof(char)));
                            param.Add(cdal.GetParameter("@EvalSystem", model.Tab_InventoryData.EvalSystem, typeof(string)));
                            param.Add(cdal.GetParameter("@GLMethod", model.Tab_InventoryData.GLMethod, typeof(string)));
                            param.Add(cdal.GetParameter("@InvntryUom", model.Tab_InventoryData.InvntryUom, typeof(string)));
                        }
                    }
                    if (IsPurchaseItemEditable(model.HeaderData.ItemCode.ToString()) && IsSalesItemEditable(model.HeaderData.ItemCode.ToString()) && IsInventoryItemEditable(model.HeaderData.ItemCode.ToString()))
                    {
                        ItmsGrpCod = "ItmsGrpCod=@ItmsGrpCod,";
                        param.Add(cdal.GetParameter("@ItmsGrpCod", model.HeaderData.ItmsGrpCod, typeof(int)));
                    }

                    int ObjectCode = 4;
                    int isApproved = ObjectCode.GetApprovalStatus(tran);
                    #region Insert in Approval Table

                    if (isApproved == 0)
                    {
                        ApprovalModel approvalModel = new()
                        {
                            Id = CommonDal.getPrimaryKey(tran, "tbl_DocumentsApprovals"),
                            ObjectCode = ObjectCode,
                            DocEntry = Convert.ToInt32(SqlHelper.ExecuteScalar(tran, CommandType.Text, @"select Id from OITM where guid='" + model.OldItemId + "'")),
                            DocNum = SqlHelper.ExecuteScalar(tran, CommandType.Text, @"select ItemCode from OITM where guid='" + model.OldItemId + "'").ToString(),
                            Guid = model.OldItemId
                        };
                        //ApprovalModel approvalModel = new ApprovalModel();

                        //approvalModel.Id = CommonDal.getPrimaryKey(tran, "tbl_DocumentsApprovals");
                        //approvalModel.ObjectCode = ObjectCode;
                        //approvalModel.DocEntry = Convert.ToInt32( SqlHelper.ExecuteScalar(tran, CommandType.Text, @"select Id from OITM where guid='" + model.OldItemId + "'") );
                        //approvalModel.DocNum = SqlHelper.ExecuteScalar(tran, CommandType.Text, @"select ItemCode from OITM where guid='" + model.OldItemId + "'").ToString();
                        //approvalModel.Guid = model.OldItemId;
                        bool resp = cdal.AddApproval(tran, approvalModel);
                        if (!resp)
                        {
                            response.isSuccess = false;
                            response.Message = "An Error Occured";
                            return response;
                        }

                    }

                    #endregion

                    string HeadQuery = @"update OITM set ItemName=@ItemName, Series=@Series, " + InvntItem + " " + SellItem + " FrgnName=@FrgnName, " + PrchseItem + " ItemType=@ItemType, " + ItmsGrpCod + " UgpEntry=@UgpEntry," +
                            " AvgPrice=@AvgPrice, WTLiable=@WTLiable, FirmCode=@FirmCode, ShipType=@ShipType,MngMethod=@MngMethod, validFor=@validFor, validFrom=@validFrom, " +
                            "validTo=@validTo, frozenFrom=@frozenFrom, frozenTo=@frozenTo,ManBtchNum=@ManBtchNum, " + IQ + " PrcrmntMtd=@PrcrmntMtd, PlaningSys=@PlaningSys, " +
                            "MinOrdrQty=@MinOrdrQty, InCostRoll=@InCostRoll, IssueMthd=@IssueMthd, TreeType=@TreeType, PrdStdCst=@PrdStdCst, " + PQ + " " + SQ + " " +
                            "QryGroup1=@QryGroup1, QryGroup2=@QryGroup2, QryGroup3=@QryGroup3, QryGroup4=@QryGroup4, QryGroup5=@QryGroup5, QryGroup6=@QryGroup6, QryGroup7=@QryGroup7, " +
                            "QryGroup8=@QryGroup8, QryGroup9=@QryGroup9, QryGroup10=@QryGroup10, QryGroup11=@QryGroup11, QryGroup12=@QryGroup12, QryGroup13=@QryGroup13, QryGroup14=@QryGroup14," +
                            " QryGroup15=@QryGroup15, QryGroup16=@QryGroup16, QryGroup17=@QryGroup17, QryGroup18=@QryGroup18, QryGroup19=@QryGroup19, QryGroup20=@QryGroup20, QryGroup21=@QryGroup21, " +
                            "QryGroup22=@QryGroup22, QryGroup23=@QryGroup23, QryGroup24=@QryGroup24, QryGroup25=@QryGroup25, QryGroup26=@QryGroup26, QryGroup27=@QryGroup27, QryGroup28=@QryGroup28, " +
                            "QryGroup29=@QryGroup29, QryGroup30=@QryGroup30, QryGroup31=@QryGroup31, QryGroup32=@QryGroup32, QryGroup33=@QryGroup33, QryGroup34=@QryGroup34, QryGroup35=@QryGroup35, " +
                            "QryGroup36=@QryGroup36, QryGroup37=@QryGroup37, QryGroup38=@QryGroup38, QryGroup39=@QryGroup39, QryGroup40=@QryGroup40, QryGroup41=@QryGroup41, QryGroup42=@QryGroup42, " +
                            "QryGroup43=@QryGroup43, QryGroup44=@QryGroup44, QryGroup45=@QryGroup45, QryGroup46=@QryGroup46, QryGroup47=@QryGroup47, QryGroup48=@QryGroup48, QryGroup49=@QryGroup49, " +
                            "QryGroup50=@QryGroup50, QryGroup51=@QryGroup51, QryGroup52=@QryGroup52, QryGroup53=@QryGroup53, QryGroup54=@QryGroup54, QryGroup55=@QryGroup55, QryGroup56=@QryGroup56, " +
                            "QryGroup57=@QryGroup57, QryGroup58=@QryGroup58, QryGroup59=@QryGroup59, QryGroup60=@QryGroup60, QryGroup61=@QryGroup61, QryGroup62=@QryGroup62, QryGroup63=@QryGroup63, " +
                            "QryGroup64=@QryGroup64,isApproved =@isApproved,apprSeen =0  where guid= '" + model.OldItemId + "'";



                    #region SqlParameters

                    #region Header data

                    param.Add(cdal.GetParameter("@ItemName", model.HeaderData.ItemName, typeof(string)));
                    param.Add(cdal.GetParameter("@Series", model.HeaderData.Series, typeof(int)));

                    param.Add(cdal.GetParameter("@FrgnName", model.HeaderData.FrgnName, typeof(string)));

                    param.Add(cdal.GetParameter("@ItemType", model.HeaderData.ItemType, typeof(string)));

                    param.Add(cdal.GetParameter("@UgpEntry", model.HeaderData.UgpEntry, typeof(int)));
                    param.Add(cdal.GetParameter("@AvgPrice", model.HeaderData.AvgPrice, typeof(decimal)));
                    #endregion

                    #region General
                    param.Add(cdal.GetParameter("@WTLiable", model.Tab_General.WTLiable, typeof(string)));
                    param.Add(cdal.GetParameter("@FirmCode", model.Tab_General.FirmCode, typeof(string)));
                    param.Add(cdal.GetParameter("@ShipType", model.Tab_General.ShipType, typeof(string)));
                    param.Add(cdal.GetParameter("@MngMethod", model.Tab_General.MngMethod, typeof(char)));
                    param.Add(cdal.GetParameter("@validFor", model.Tab_General.ActivevalidFor, typeof(char)));
                    param.Add(cdal.GetParameter("@validFrom", model.Tab_General.validFrom, typeof(string)));
                    param.Add(cdal.GetParameter("@validTo", model.Tab_General.validTo, typeof(string)));
                    param.Add(cdal.GetParameter("@frozenFrom", model.Tab_General.frozenFrom, typeof(string)));
                    param.Add(cdal.GetParameter("@frozenTo", model.Tab_General.frozenTo, typeof(string)));
                    param.Add(cdal.GetParameter("@ManBtchNum", model.Tab_General.ManBtchNum, typeof(char)));
                    #endregion

                    #region Planning Data 
                    param.Add(cdal.GetParameter("@PrcrmntMtd", model.Tab_PlanningData.PrcrmntMtd, typeof(string)));
                    param.Add(cdal.GetParameter("@PlaningSys", model.Tab_PlanningData.PlaningSys, typeof(string)));
                    param.Add(cdal.GetParameter("@MinOrdrQty", model.Tab_PlanningData.MinOrdrQty, typeof(string)));
                    #endregion

                    #region Production Data
                    param.Add(cdal.GetParameter("@InCostRoll", model.Tab_ProductionData.InCostRoll, typeof(string)));
                    param.Add(cdal.GetParameter("@IssueMthd", model.Tab_ProductionData.IssueMthd, typeof(string)));
                    param.Add(cdal.GetParameter("@TreeType", model.Tab_ProductionData.TreeType, typeof(string)));
                    param.Add(cdal.GetParameter("@PrdStdCst", model.Tab_ProductionData.PrdStdCst, typeof(string)));
                    #endregion



                    #region Properties
                    param.Add(cdal.GetParameter("@QryGroup1", model.Tab_Properties.QryGroup1, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup2", model.Tab_Properties.QryGroup2, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup3", model.Tab_Properties.QryGroup3, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup4", model.Tab_Properties.QryGroup4, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup5", model.Tab_Properties.QryGroup5, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup6", model.Tab_Properties.QryGroup6, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup7", model.Tab_Properties.QryGroup7, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup8", model.Tab_Properties.QryGroup8, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup9", model.Tab_Properties.QryGroup9, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup10", model.Tab_Properties.QryGroup10, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup11", model.Tab_Properties.QryGroup11, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup12", model.Tab_Properties.QryGroup12, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup13", model.Tab_Properties.QryGroup13, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup14", model.Tab_Properties.QryGroup14, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup15", model.Tab_Properties.QryGroup15, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup16", model.Tab_Properties.QryGroup16, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup17", model.Tab_Properties.QryGroup17, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup18", model.Tab_Properties.QryGroup18, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup19", model.Tab_Properties.QryGroup19, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup20", model.Tab_Properties.QryGroup20, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup21", model.Tab_Properties.QryGroup21, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup22", model.Tab_Properties.QryGroup22, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup23", model.Tab_Properties.QryGroup23, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup24", model.Tab_Properties.QryGroup24, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup25", model.Tab_Properties.QryGroup25, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup26", model.Tab_Properties.QryGroup26, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup27", model.Tab_Properties.QryGroup27, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup28", model.Tab_Properties.QryGroup28, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup29", model.Tab_Properties.QryGroup29, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup30", model.Tab_Properties.QryGroup30, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup31", model.Tab_Properties.QryGroup31, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup32", model.Tab_Properties.QryGroup32, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup33", model.Tab_Properties.QryGroup33, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup34", model.Tab_Properties.QryGroup34, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup35", model.Tab_Properties.QryGroup35, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup36", model.Tab_Properties.QryGroup36, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup37", model.Tab_Properties.QryGroup37, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup38", model.Tab_Properties.QryGroup38, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup39", model.Tab_Properties.QryGroup39, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup40", model.Tab_Properties.QryGroup40, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup41", model.Tab_Properties.QryGroup41, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup42", model.Tab_Properties.QryGroup42, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup43", model.Tab_Properties.QryGroup43, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup44", model.Tab_Properties.QryGroup44, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup45", model.Tab_Properties.QryGroup45, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup46", model.Tab_Properties.QryGroup46, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup47", model.Tab_Properties.QryGroup47, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup48", model.Tab_Properties.QryGroup48, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup49", model.Tab_Properties.QryGroup49, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup50", model.Tab_Properties.QryGroup50, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup51", model.Tab_Properties.QryGroup51, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup52", model.Tab_Properties.QryGroup52, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup53", model.Tab_Properties.QryGroup53, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup54", model.Tab_Properties.QryGroup54, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup55", model.Tab_Properties.QryGroup55, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup56", model.Tab_Properties.QryGroup56, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup57", model.Tab_Properties.QryGroup57, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup58", model.Tab_Properties.QryGroup58, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup59", model.Tab_Properties.QryGroup59, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup60", model.Tab_Properties.QryGroup60, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup61", model.Tab_Properties.QryGroup61, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup62", model.Tab_Properties.QryGroup62, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup63", model.Tab_Properties.QryGroup63, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup64", model.Tab_Properties.QryGroup64, typeof(string)));
                    #endregion

                    param.Add(cdal.GetParameter("@isApproved", isApproved, typeof(int)));

                    #endregion

                    res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, HeadQuery, param.ToArray()).ToInt();
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

                            string isWhExits = @"select COUNT(*) from OITW where WhsCode='" + item.WhsCode + "' and ItemCode='" + model.HeaderData.ItemCode + "'";
                            if (SqlHelper.ExecuteScalar(SqlHelper.defaultDB, CommandType.Text, isWhExits).ToInt() > 0)
                            {
                                if (CommonDal.IsWareHouseTransactionAdded(item.WhsCode.ToString(), (model.HeaderData.ItemCode).ToString()))
                                {

                                

                                    string RowQueryItem1 = @"update OITW set
                                        Locked=@Locked,MinStock=@MinStock,MaxStock=@MaxStock,MinOrder=@MinOrder 
                                        where WhsCode='" + item.WhsCode + "' and ItemCode='" + model.HeaderData.ItemCode + "'";

                                    #region sqlparam
                                    List<SqlParameter> param2 = new List<SqlParameter>();

                                    param2.Add(cdal.GetParameter("@Locked", item.Locked, typeof(char)));
                                    param2.Add(cdal.GetParameter("@MinStock", item.MinStock, typeof(decimal)));
                                    param2.Add(cdal.GetParameter("@MaxStock", item.MaxStock, typeof(decimal)));
                                    param2.Add(cdal.GetParameter("@MinOrder", item.MinOrder, typeof(decimal)));                                //};

                                    #endregion

                                    res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, RowQueryItem1, param2.ToArray()).ToInt();
                                    if (res1 <= 0)
                                    {
                                        tran.Rollback();
                                        response.isSuccess = false;
                                        response.Message = "An Error Occured";
                                        return response;
                                    }
                                }
                            }
                            else
                            {
                                string RowQueryItem1 = @"insert into OITW
                                    (ItemCode,WhsCode,WhsName,Locked,MinStock,MaxStock,MinOrder)
                                    values(@ItemCode,@WhsCode,@WhsName,@Locked,@MinStock,@MaxStock,@MinOrder)";

                                #region sqlparam
                                List<SqlParameter> param2 = new List<SqlParameter>();
                                //{
                                param2.Add(cdal.GetParameter("@ItemCode", model.HeaderData.ItemCode, typeof(string)));
                                param2.Add(cdal.GetParameter("@WhsCode", item.WhsCode, typeof(string)));
                                param2.Add(cdal.GetParameter("@WhsName", item.WhsName, typeof(string)));
                                param2.Add(cdal.GetParameter("@Locked", item.Locked, typeof(char)));
                                param2.Add(cdal.GetParameter("@MinStock", item.MinStock, typeof(decimal)));
                                param2.Add(cdal.GetParameter("@MaxStock", item.MaxStock, typeof(decimal)));
                                param2.Add(cdal.GetParameter("@MinOrder", item.MinOrder, typeof(decimal)));                                //};

                                #endregion

                                res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, RowQueryItem1, param2.ToArray()).ToInt();
                                if (res1 <= 0)
                                {
                                    tran.Rollback();
                                    response.isSuccess = false;
                                    response.Message = "An Error Occured";
                                    return response;
                                }
                            }
                            //LineNo += 1;
                        }



                    }
                    else
                    {
                        //int LineNo = 0;
                        foreach (var item in GetWareHouseList())
                        {

                            string isWhExits = @"select COUNT(*) from OITW where WhsCode='" + item.whscode + "'and ItemCode='" + model.HeaderData.ItemCode + "'";
                            if (SqlHelper.ExecuteScalar(SqlHelper.defaultDB, CommandType.Text, isWhExits).ToInt() == 0 )
                            {
                                string RowQueryItem2 = @"insert into OITW
                                (ItemCode,WhsCode,WhsName)
                                values(@ItemCode, @WhsCode, @WhsName)";

                                #region sqlparam

                                List<SqlParameter> param3 = new List<SqlParameter>();

                                param3.Add(cdal.GetParameter("@ItemCode", model.HeaderData.ItemCode, typeof(string)));
                                param3.Add(cdal.GetParameter("@WhsCode", item.whscode, typeof(string)));
                                param3.Add(cdal.GetParameter("@WhsName", item.whsname, typeof(string)));


                                #endregion

                                res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, RowQueryItem2, param3.ToArray()).ToInt();
                                if (res1 <= 0)
                                {
                                    response.isSuccess = false;
                                    response.Message = "An Error Occured";
                                    return response;
                                }
                            }
                            
                            //LineNo += 1;
                        }
                    }


                }
                if (res1 > 0)
                {
                    tran.Commit();
                    response.isSuccess = true;
                    response.Message = "Item Updated Successfully !";

                }

            }
            catch (Exception e)
            {
                tran.Rollback();
                response.isSuccess = false;
                response.Message = e.Message;
                return response;
            }


            return response;
        }
    }
}
