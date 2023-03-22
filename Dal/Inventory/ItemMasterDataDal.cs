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
        public List<ListModel> GetTaxGroup()
        {
            string GetQuery = " select  Code, Code +'   -   '+ Name as Name  from OVTG order by Code";

            List<ListModel> list = new List<ListModel>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {
                    list.Add(new ListModel()
                    {
                        ValueDecimal = rdr["Code"].ToDecimal(),
                        Text = rdr["Name"].ToString()

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
                string DocType = model.ListItems == null ? "S" : "I";


                SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                int res1 = 0;
                try
                {
                    int Id = CommonDal.getPrimaryKey(tran, "OITM");
                    //string DocNum = SqlHelper.getUpdatedDocumentNumberOnLoad(tran, "OQUT", "SQ");
                    if (model.HeaderData != null)
                    {
                        string[] ArrayHeadQuery = { "Id", "Series", "ItemCode", "InvntItem", "ItemName", "SellItem", "FrgnName", "PrchseItem", "ItemType", "ItmsGrpCod", "UgpEntry", "ListName", "PriceUnit", "AvgPrice", " WTLiable", "FirmCode", "ShipType", "ManagedItemby", "MyMngMthd", "ManagedItemby", "validFor", "validFrom", "validTo", "frozenFrom", "frozenTo", " BuyUnitMar", "NumInBuy", "CstGrpCode", "TotalTax", "VatGroupPu", "VatGroupSa", "SalUnitMar", "NumInSale", "GLMethod", "ByWh", "InvntryUom", "EvalSystem", " PlaningSys", "PrcrmntMtd", "MinOrdrQty", "IssueMthd", "TreeType", "PrdStdCst", "InCostRoll", " QryGroup1", "QryGroup2", "QryGroup3", "QryGroup4", "QryGroup5", "QryGroup6", "QryGroup7", "QryGroup8", "QryGroup9", "QryGroup10", "QryGroup11", "QryGroup12", " QryGroup13", "QryGroup14", "QryGroup15", "QryGroup16", "QryGroup17", "QryGroup18", "QryGroup19", "QryGroup20", "QryGroup21", "QryGroup22", "QryGroup23", " QryGroup24", "QryGroup25", "QryGroup26", "QryGroup27", "QryGroup28", "QryGroup29", "QryGroup30", "QryGroup31", "QryGroup32", "QryGroup33", "QryGroup34", " QryGroup35", "QryGroup36", "QryGroup37", "QryGroup38", "QryGroup39", "QryGroup40", "QryGroup41", "QryGroup42", "QryGroup43", "QryGroup44", "QryGroup45", " QryGroup46", "QryGroup47", "QryGroup48", "QryGroup49", "QryGroup50", "QryGroup51", "QryGroup52", "QryGroup53", "QryGroup54", "QryGroup55", "QryGroup56", " QryGroup57", "QryGroup58", "QryGroup59", "QryGroup60", "QryGroup61", "QryGroup62", "QryGroup63", "QryGroup64" };
                        string HeadQuery = @"insert into OITM
                                            (Id,Series,ItemCode,InvntItem,ItemName,SellItem,FrgnName,PrchseItem,ItemType,ItmsGrpCod,UgpEntry,ListName,PriceUnit,AvgPrice,
                                            WTLiable,FirmCode,ShipType,ManagedItemby,MyMngMthd,ManagedItemby,validFor,validFrom,validTo,frozenFrom,frozenTo,
                                            BuyUnitMar,NumInBuy,CstGrpCode,TotalTax,VatGroupPu,VatGroupSa,SalUnitMar,NumInSale,GLMethod,ByWh,InvntryUom,EvalSystem,
                                            PlaningSys,PrcrmntMtd,MinOrdrQty,IssueMthd,TreeType,PrdStdCst,InCostRoll,
                                            QryGroup1,QryGroup2,QryGroup3,QryGroup4,QryGroup5,QryGroup6,QryGroup7,QryGroup8,QryGroup9,QryGroup10,QryGroup11,QryGroup12,
                                            QryGroup13,QryGroup14,QryGroup15,QryGroup16,QryGroup17,QryGroup18,QryGroup19,QryGroup20,QryGroup21,QryGroup22,QryGroup23,
                                            QryGroup24,QryGroup25,QryGroup26,QryGroup27,QryGroup28,QryGroup29,QryGroup30,QryGroup31,QryGroup32,QryGroup33,QryGroup34,
                                            QryGroup35,QryGroup36,QryGroup37,QryGroup38,QryGroup39,QryGroup40,QryGroup41,QryGroup42,QryGroup43,QryGroup44,QryGroup45,
                                            QryGroup46,QryGroup47,QryGroup48,QryGroup49,QryGroup50,QryGroup51,QryGroup52,QryGroup53,QryGroup54,QryGroup55,QryGroup56,
                                            QryGroup57,QryGroup58,QryGroup59,QryGroup60,QryGroup61,QryGroup62,QryGroup63,QryGroup64) 
                                            values(@Id,@Series,@ItemCode,@InvntItem,@ItemName,@SellItem,@FrgnName,@PrchseItem,@ItemType,@ItmsGrpCod,@UgpEntry,@ListName,@PriceUnit,@AvgPrice,
                                            @WTLiable,@FirmCode,@ShipType,@ManagedItemby,@MyMngMthd,@ManagedItemby,@validFor,@validFrom,@validTo,@frozenFrom,@frozenTo,
                                            @BuyUnitMar,@NumInBuy,@CstGrpCode,@TotalTax,@VatGroupPu,@VatGroupSa,@SalUnitMar,@NumInSale,@GLMethod,@ByWh,@InvntryUom,@EvalSystem,
                                            @PlaningSys,@PrcrmntMtd,@MinOrdrQty,@IssueMthd,@TreeType,@PrdStdCst,@InCostRoll,
                                            @QryGroup1,@QryGroup2,@QryGroup3,@QryGroup4,@QryGroup5,@QryGroup6,@QryGroup7,@QryGroup8@,@QryGroup9,@QryGroup10,@QryGroup11,@QryGroup12,
                                            @QryGroup13,@QryGroup14,@QryGroup15,@QryGroup16,@QryGroup17,@QryGroup18,@QryGroup19,@QryGroup20,@QryGroup21,@QryGroup22,@QryGroup23,
                                            @QryGroup24,@QryGroup25,@QryGroup26,@QryGroup27,@QryGroup28,@QryGroup29,@QryGroup30,@QryGroup31,@QryGroup32,@QryGroup33,@QryGroup34,
                                            @QryGroup35,@QryGroup36,@QryGroup37,@QryGroup38,@QryGroup39,@QryGroup40,@QryGroup41,@QryGroup42,@QryGroup43,@QryGroup44,@QryGroup45,
                                            @QryGroup46,@QryGroup47,@QryGroup48,@QryGroup49,@QryGroup50,@QryGroup51,@QryGroup52,@QryGroup53,@QryGroup54,@QryGroup55,@QryGroup56,
                                            @QryGroup57,@QryGroup58,@QryGroup59,@QryGroup60,@QryGroup61,@QryGroup62,@QryGroup63,@QryGroup64) ";

                        #region SqlParameters
                        List<SqlParameter> param = new List<SqlParameter>();
                        foreach (var Arr in ArrayHeadQuery)
                        {
                            param.Add(new SqlParameter("\"@" + Arr +"\"", Arr));
                        }
                         
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
                                string RowQueryItem = @"insert into OITW
                                (Id,WhsCode,WhsName,Locked,MinStock,MaxStock,MinOrder)
                                values(@Id,@WhsCode,@WhsName,@Locked,@MinStock,@MaxStock,@MinOrder)";

                                #region sqlparam
                                List<SqlParameter> param2 = new List<SqlParameter>
                                {
                                     new SqlParameter("@id",Id),
                                     new SqlParameter("@WhsCode",model.SalesData.WarehouseList.WhsCode),
                                     new SqlParameter("@WhsName",model.SalesData.WarehouseList.WhsName),
                                     new SqlParameter("@Locked",model.SalesData.WarehouseList.Locked),
                                     new SqlParameter("@MinStock",model.SalesData.WarehouseList.MinStock),
                                     new SqlParameter("@MaxStock",model.SalesData.WarehouseList.MaxStock),
                                     new SqlParameter("@MinOrder",model.SalesData.WarehouseList.MinOrder)                                     
                                };
                                #endregion

                                res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, RowQueryItem, param2.ToArray()).ToInt();
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
