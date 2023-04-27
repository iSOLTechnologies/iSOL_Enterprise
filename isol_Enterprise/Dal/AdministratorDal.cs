using SqlHelperExtensions;
using System.Data.SqlClient;
using System.Data;
using iSOL_Enterprise.Models.Sale;
using iSOL_Enterprise.Common;
using iSOL_Enterprise.Models;
using System.Runtime.Intrinsics.Arm;
using iSOL_Enterprise.Models.Series;

namespace iSOL_Enterprise.Dal
{
    public class AdministratorDal
    {
//=====================================================
        public List<tbl_pages> GetSeriesDrpDwn()
        {

            string GetQuery = "select ObjectCode,PageName from Pages";
            List<tbl_pages> list = new List<tbl_pages>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {
                    list.Add(new tbl_pages()
                    {
                        ObjectCode = rdr["ObjectCode"].ToInt(),
                        PageName = rdr["PageName"].ToString()
                    });
                }
            }
            return list;
        }



        public List<tbl_NNM1> GetNNM1(string ObjectCode)
        {
            CommonDal dal = new CommonDal();
            string GetQuery = "";
            if (ObjectCode == "All")
            {
                GetQuery = "select * from NNM1";
            }
            else
            {
                GetQuery = "select * from NNM1 where ObjectCode = " + ObjectCode;
            }
            List<tbl_NNM1> list = new List<tbl_NNM1>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {
                    list.Add(new tbl_NNM1()
                    {
                        PageName = (GetTableName(Convert.ToInt32(rdr["ObjectCode"]))).ToString(),
                        InitialNum = Convert.ToInt32(rdr["InitialNum"]),
                        LastNum = Convert.ToInt32(rdr["LastNum"]),
                        BeginStr = rdr["BeginStr"].ToString(),
                        SeriesName = rdr["SeriesName"].ToString()
                    });
                }
            }
            return list;
        }




        public string? GetTableName(int ObjectCode)
        {

            string GetQuery = "select PageName from Pages where ObjectCode = " + ObjectCode;

            string? PageName = (SqlHelper.ExecuteScalar(SqlHelper.defaultDB, CommandType.Text, GetQuery)).ToString();

            return PageName;
        }

        public bool InsertSeries(tbl_NNM1 obj)
        {
            try
            {
                SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                int res1 = 0;
                int Series = CommonDal.getPrimaryKey(tran, "Series", "NNM1");
                string HeadQuery = @"INSERT INTO NNM1(ObjectCode,Series,SeriesName,InitialNum,LastNum,NextNumber,BeginStr) 
                                    VALUES('" + obj.ObjectCode + "','" + Series + "','" + obj.SeriesName + "'," + obj.InitialNum + "," + obj.LastNum + "," + obj.InitialNum + ",'" + obj.BeginStr + "')";
                res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, HeadQuery).ToInt();
                if (res1 <= 0)
                {
                    tran.Rollback();
                    return false;
                }
                else
                {

                    tran.Commit();
                }
                conn.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }

        }

//=====================================================
        public List<tbl_pages> GetDocSeries()
        {  

            string GetQuery = "select id,ObjectCode,PageName,Series from Pages";
            List<tbl_pages> list = new List<tbl_pages>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {
                    list.Add(new tbl_pages()
                    {   SerialNo = rdr["Id"].ToInt(),
                        ObjectCode = rdr["ObjectCode"].ToInt(),
                        PageName= rdr["PageName"].ToString(),
                        PageSeries = rdr["Series"].ToInt(),
                        Series = GetSeries(rdr["ObjectCode"].ToInt())
                    }); 
                }
            }
            return list;
        }

        public List<tbl_series> GetSeries(int ObjectCode)
        {
            string GetSeriesQuery = "select Series,SeriesName from NNM1 where NextNumber <= LastNum and ObjectCode = '" + ObjectCode + "'";
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
        public List<tbl_NNM1> GetMySeries(int ObjectCode)
        {
            string GetSeriesQuery = "select Series,SeriesName,BeginStr,NextNumber from NNM1 where NextNumber <= LastNum and ObjectCode = '" + ObjectCode + "'";
            List<tbl_NNM1> listSeries = new List<tbl_NNM1>();
            using (var rdr1 = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, GetSeriesQuery))
            {
                while (rdr1.Read())
                {
                    listSeries.Add(new tbl_NNM1()
                    {                        
                        Series = rdr1["Series"].ToInt(),
                        SeriesName = rdr1["SeriesName"].ToString(),
                        BeginStr = (rdr1["BeginStr"].ToString() + rdr1["NextNumber"].ToString()).ToString()
                    });
                }
            }
            return listSeries;
        }

        public bool UpdateSeries(int ObjCode , int Series)
        {
            try
            {
                SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                int res1 = 0;

                string HeadQuery = @" Update Pages set 
                                                        Series = " + Series  +
                                                       "WHERE ObjectCode = " + ObjCode;


                res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, HeadQuery).ToInt();
                if (res1 <= 0)
                {
                    tran.Rollback();
                    return false;
                }
                else
                    tran.Commit();
                    conn.Close();
                    return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }

        }
}
}
