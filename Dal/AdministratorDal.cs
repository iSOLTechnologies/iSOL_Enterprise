using SqlHelperExtensions;
using System.Data.SqlClient;
using System.Data;
using iSOL_Enterprise.Models.Sale;
using iSOL_Enterprise.Common;
using iSOL_Enterprise.Models;
using System.Runtime.Intrinsics.Arm;

namespace iSOL_Enterprise.Dal
{
    public class AdministratorDal
    {

        public List<tbl_pages> GetDocSeries()
        {  

            string GetQuery = "select id,ObjectCode,PageName from Pages";
            List<tbl_pages> list = new List<tbl_pages>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {
                    list.Add(new tbl_pages()
                    {   SerialNo = rdr["Id"].ToInt(),
                        ObjectCode = rdr["ObjectCode"].ToInt(),
                        PageName= rdr["PageName"].ToString(),
                        Series = GetSeries(rdr["ObjectCode"].ToInt())
                    }); 
                }
            }
            return list;
        }

        public List<tbl_series> GetSeries(int ObjectCode)
        {
            string GetSeriesQuery = "select Series,SeriesName from NNM1 where ObjectCode = " + ObjectCode + "";
            List<tbl_series> listSeries = new List<tbl_series>();
            using (var rdr1 = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetSeriesQuery))
            {
                while (rdr1.Read())
                {
                    listSeries.Add(new tbl_series()
                    {
                        Series = rdr1["Series"].ToInt(),
                        SeriesName = rdr1["SeriesName"].ToString()
                    });
                }
            }
            return listSeries;
        }
}
}
