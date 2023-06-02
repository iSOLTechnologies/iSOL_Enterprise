using iSOL_Enterprise.Common;
using iSOL_Enterprise.Models.Inventory;
using SAPbobsCOM;
using SqlHelperExtensions;
using System.Data;
using System.Data.SqlClient;

namespace iSOL_Enterprise.Dal.Inventory
{
    public class InventoryReportDal
    {


        public async Task< List<ItemMasterModel> > GetInventoryInWarehouseReportData(int? start = null, int? length = null,bool zeroStock = true , string? searchWhsValue = null, string? searchItemValue = null)
        {

            string Stock0 = " and OITW.OnHand > 0";
            if (zeroStock)
            {
                Stock0 = "";
            }

            string GetQuery = @"SELECT
                                OITM.ItemCode AS 'ItemNo',
                                OITM.ItemName AS 'ItemDescription',
	                            OITM.InvntryUom AS 'InventoryUom',
                                OITW.OnHand AS 'InStock',	
                                OITW.IsCommited AS 'Committed',
                                OITW.OnOrder AS 'Ordered',
	                            OITW.IsCommited - OITW.OnOrder - OITW.OnHand AS 'Available',
	                            OITM.LastPurPrc As 'ItemPrice',
	                            OITM.LastPurPrc * OITW.OnHand AS 'Total',
                                OWHS.WhsCode AS 'WarehouseCode',
                                OWHS.WhsName AS 'WarehouseName'
                            FROM
                                OITM
                                INNER JOIN OITW ON OITM.ItemCode = OITW.ItemCode
                                INNER JOIN OWHS ON OITW.WhsCode = OWHS.WhsCode

                            where OWHS.WhsCode in ("+ searchWhsValue + "'') "+ Stock0 + " and OITM.ItemCode in ("+ searchItemValue + "'') "+
                            "ORDER BY " +
                                "OWHS.WhsCode,OITM.ItemCode OFFSET " + start + " ROWS " +
                                "FETCH NEXT " + length + " ROWS ONLY";



            SqlConnection conn = new SqlConnection(SqlHelper.defaultSapDB);
            conn.Open();

            List<ItemMasterModel> list = new List<ItemMasterModel>();

            
                using (SqlCommand cmd = new SqlCommand(GetQuery, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 60;
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {

                            ItemMasterModel models = new()
                            {

                                ItemCode = rdr["ItemNo"].ToString(),
                                ItemName = rdr["ItemDescription"].ToString(),
                                InventoryUom = rdr["InventoryUom"].ToString(),
                                onHand = rdr["InStock"].ToDecimal(),
                                isCommitted = rdr["Committed"].ToDecimal(),
                                OnOrder = rdr["Ordered"].ToDecimal(),
                                Available = rdr["Available"].ToDecimal(),
                                LastPurPrc = rdr["ItemPrice"].ToDecimal(),
                                Total = rdr["Total"].ToDecimal(),
                                WhsCode = rdr["WarehouseCode"].ToString(),
                                WarehouseName = rdr["WarehouseName"].ToString()
                            };

                            await Task.Run(() => list.Add(models));
                        };

                    }
                }



            //using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery ))
            //{
            //    while (rdr.Read())
            //    {

            //        ItemMasterModel models = new()
            //        {

            //            ItemCode = rdr["ItemNo"].ToString(),
            //            ItemName = rdr["ItemDescription"].ToString(),
            //            InventoryUom = rdr["InventoryUom"].ToString(),
            //            onHand = rdr["InStock"].ToDecimal(),
            //            isCommitted = rdr["Committed"].ToDecimal(),
            //            OnOrder = rdr["Ordered"].ToDecimal(),
            //            Available = rdr["Available"].ToDecimal(),
            //            LastPurPrc = rdr["ItemPrice"].ToDecimal(),
            //            Total = rdr["Total"].ToDecimal(),
            //            WhsCode = rdr["WarehouseCode"].ToString(),
            //            WarehouseName = rdr["WarehouseName"].ToString()
            //        }; 

            //       await Task.Run ( () => list.Add(models) );
            //    };

            //}
            conn.Close();
            return  list;
        }
        
    }
    
}
