using iSOL_Enterprise.Common;
using iSOL_Enterprise.Models;
using iSOL_Enterprise.Models.Inventory;
using iSOL_Enterprise.Models.sale;
using SqlHelperExtensions;
using System.Data;

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
    }
}
