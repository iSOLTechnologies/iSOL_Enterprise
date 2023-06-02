using iSOL_Enterprise.Common;
using iSOL_Enterprise.Models.Inventory;
using SqlHelperExtensions;
using System.Data;

namespace iSOL_Enterprise.Dal.Inventory
{
    public class InventoryReportDal
    {


        public List<ItemMasterModel> GetInventoryInWarehouseReportData()
        {
            string countquery = @"select Count(*) from (

                                SELECT
                                     OITM.ItemCode AS 'ItemNo',
                                     OITM.ItemName AS 'ItemDescription',
	                                 OITM.InvntryUom AS 'InventoryUom',
                                     OITW.OnHand AS 'InStock',	
                                     OITW.IsCommited AS 'Committed',
                                     OITW.OnOrder AS 'Ordered',
	                                 OITW.IsCommited - OITW.OnOrder - OITW.OnHand AS 'Available',
	                                 OITM.LastPurPrc As 'ItemPrice',
	                                 OITM.LastPurPrc * OITW.OnHand AS 'Total',
                                     OWHS.WhsCode AS 'WarehouseCode'
                                 FROM
                                     OITM 
                                     INNER JOIN OITW ON OITM.ItemCode = OITW.ItemCode
                                     INNER JOIN OWHS ON OITW.WhsCode = OWHS.WhsCode
                                ) c";

            int count = Convert.ToInt32 ( SqlHelper.ExecuteScalar(SqlHelper.defaultSapDB, CommandType.Text, countquery));


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

                            ORDER BY
                                OWHS.WhsCode,
                                OITM.ItemCode";


            List<ItemMasterModel> list = new List<ItemMasterModel>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
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

                    list.Add(models);
                };

            }
            return list;
        }
    }
    
}
