using iSOL_Enterprise.Common;
using iSOL_Enterprise.Models;
using iSOL_Enterprise.Models.Inventory;
using SqlHelperExtensions;
using System.Data;

namespace iSOL_Enterprise.Dal.Business
{
    public class BusinessPartnerMasterDataDal
    {

        public List<ListModel> GetGroups()
        {
            string GetQuery = "select GroupCode,GroupName from OCRG  where GroupType = 'C' ORDER BY GroupCode";

            List<ListModel> list = new List<ListModel>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {
                    list.Add(new ListModel()
                    {
                        Value = rdr["GroupCode"].ToInt(),
                        Text = rdr["GroupName"].ToString()

                    });
                }
            }
            return list;
        }
        public List<ListModel> GetShipTypes()
        {
            string GetQuery = "select TrnspCode,TrnspName from OSHP  where Active = 'Y' ORDER BY TrnspCode";

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
        public List<ListModel> GetNames()
        {
            string GetQuery = "select Code,Name from OIDC  ORDER BY Code";

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
        public List<ListModel> GetProjectCodes()
        {
            string GetQuery = "select PrjCode,PrjName from OPRJ  ORDER BY PrjCode";

            List<ListModel> list = new List<ListModel>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {
                    list.Add(new ListModel()
                    {
                        Value = rdr["PrjCode"].ToInt(),
                        Text = rdr["PrjName"].ToString()

                    });
                }
            }
            return list;
        }
        public List<ListModel> GetBusinessPartners()
        {
            string GetQuery = "select CardCode,CardName,Balance from OCRD where CardType = 'C'  ORDER BY CardCode";

            List<ListModel> list = new List<ListModel>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {
                    list.Add(new ListModel()
                    {
                        Value = rdr["CardCode"].ToInt(),
                        Text = rdr["CardName"].ToString() + " -- " + rdr["Balance"].ToString()

                    });
                }
            }
            return list;
        }
        public List<ListModel> GetIndustries()
        {
            string GetQuery = "select IndCode,IndName from OOND  ORDER BY IndCode";

            List<ListModel> list = new List<ListModel>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {
                    list.Add(new ListModel()
                    {
                        Value = rdr["IndCode"].ToInt(),
                        Text = rdr["IndName"].ToString() 

                    });
                }
            }
            return list;
        }
        public List<ListModel> GetTechnicians()
        {
            string GetQuery = "select empID,LastName,firstName from OHEM  ORDER BY empID";

            List<ListModel> list = new List<ListModel>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {
                    list.Add(new ListModel()
                    {
                        Value = rdr["empID"].ToInt(),
                        Text = rdr["LastName"].ToString() +" "+ rdr["firstName"].ToString()

                    });
                }
            }
            return list;
        }
        public List<ListModel> GetTerritories()
        {
            string GetQuery = "select territryID,descript from OTER where inactive = 'N'";

            List<ListModel> list = new List<ListModel>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {
                    list.Add(new ListModel()
                    {
                        Value = rdr["territryID"].ToInt(),
                        Text = rdr["descript"].ToString() 

                    });
                }
            }
            return list;
        }
        public List<tbl_OITG> GetProperties()
        {
            string GetQuery = "select GroupCode,GroupName from OCQG ORDER BY GroupCode";

            List<tbl_OITG> list = new List<tbl_OITG>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {
                    list.Add(new tbl_OITG()
                    {
                        ItmsTypCod = rdr["GroupCode"].ToInt(),
                        ItmsGrpNam = rdr["GroupName"].ToString()

                    });
                }
            }
            return list;
        }
    }
}
