using iSOL_Enterprise.Models.Administration;
using SqlHelperExtensions;
using System.Data;

namespace iSOL_Enterprise.Dal.Administrator
{
    public class CurrenciesDal
    {
        public List<CurrencyMasterModel> Getdata()
        {
            string GetQuery = "select CurrCode,CurrName,ChkName,Chk100Name,DocCurrCod,FrgnName from OCRN order by CurrCode";


            List<CurrencyMasterModel> list = new List<CurrencyMasterModel>();

            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {

                    list.Add(
                        new CurrencyMasterModel()
                        {
                            CurrCode = rdr["CurrCode"].ToString(),
                            CurrName = rdr["CurrName"].ToString(),
                            ChkName = rdr["ChkName"].ToString(),
                            Chk100Name = rdr["Chk100Name"].ToString(),
                            DocCurrCod = rdr["DocCurrCod"].ToString(),
                            FrgnName = rdr["FrgnName"].ToString()
                        });

                }
            }

            return list;
        }
    }
}
