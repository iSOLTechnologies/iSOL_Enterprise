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
                        Value = rdr["Code"].ToInt(),
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
    }
}
