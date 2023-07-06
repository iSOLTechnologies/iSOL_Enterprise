using iSOL_Enterprise.Common;
using iSOL_Enterprise.Models;
using iSOL_Enterprise.Models.Administration;
using Newtonsoft.Json;
using SqlHelperExtensions;
using System.Data;
using System.Data.SqlClient;

namespace iSOL_Enterprise.Dal.Administrator
{
    public class ExchangeRatesDal
    {
        public List<ExchangeRatesMasterModel> Getdata()
        {
            string GetQuery = "select ROW_NUMBER() Over (order by RateDate desc) as sno, RateDate,Currency,Rate,UpdateDate from ORTT";


            List<ExchangeRatesMasterModel> list = new List<ExchangeRatesMasterModel>();

            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {

                    list.Add(
                        new ExchangeRatesMasterModel()
                        {
                            sno = rdr["sno"].ToInt(),
                            RateDate = rdr["RateDate"].ToDateTime(),
                            Currency = rdr["Currency"].ToString(),
                            Rate = rdr["Rate"].ToDecimal(),
                            UpdateDate = rdr["UpdateDate"].ToDateTime()
                        });

                }
            }

            return list;
        }
        
        public dynamic GetExchnRateData(DateTime GUID)
        {
            try
            {



                DataSet ds = new DataSet();
                SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
                string headerQuery = @"select RateDate,Currency,Rate,UpdateDate
                                            from ORTT where RateDate ='" + GUID + "'";

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
        public ResponseModels Add(string formData)
        {
            ResponseModels response = new ResponseModels();
            CommonDal cdal = new CommonDal();
            SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
            conn.Open();
            SqlTransaction tran = conn.BeginTransaction();
            int res1 = 0;
            var model = JsonConvert.DeserializeObject<dynamic>(formData);

            try
            {
                DateTime RateDate = Convert.ToDateTime(model.HeaderData.RateDate);

                int count = SqlHelper.ExecuteScalar(tran, CommandType.Text, "select count(*) from ORTT where RateDate='" + RateDate + "'").ToInt();
                if (count == 0)
                {

                    if (model.HeaderData != null)
                    {
                        List<SqlParameter> param = new List<SqlParameter>();
                        

                        
                        string TabHeader = "RateDate,Currency,Rate,UpdateDate";
                        string TabHeaderP = "@RateDate,@Currency,@Rate,@UpdateDate";

                        

                        string HeadQuery = @"insert into ORTT (" + TabHeader + ") " +
                                            "values(" + TabHeaderP + ")";



                        #region SqlParameters

                        #region Header data
                        
                        param.Add(cdal.GetParameter("@RateDate", RateDate, typeof(DateTime)));                        
                        param.Add(cdal.GetParameter("@Currency", model.HeaderData.Currency, typeof(string)));
                        param.Add(cdal.GetParameter("@Rate", model.HeaderData.Rate, typeof(decimal)));
                        param.Add(cdal.GetParameter("@UpdateDate", RateDate, typeof(DateTime)));                        
                        
                        #endregion


                        #endregion

                        res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, HeadQuery, param.ToArray()).ToInt();
                        if (res1 <= 0)
                        {
                            tran.Rollback();
                            response.isSuccess = false;
                            response.Message = "An Error Occured";
                            return response;
                        }




                    }
                    if (res1 > 0)
                    {
                        tran.Commit();
                        response.isSuccess = true;
                        response.Message = "Exchange Rate Added Successfully !";

                    }
                }
                else
                {                    
                    response = Edit(formData);
                    return response;
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

        public ResponseModels Edit(string formData)
        {
            ResponseModels response = new ResponseModels();
            CommonDal cdal = new CommonDal();
            SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
            conn.Open();
            SqlTransaction tran = conn.BeginTransaction();
            int res1 = 0;
            var model = JsonConvert.DeserializeObject<dynamic>(formData);

            try
            {
                DateTime RateDate = Convert.ToDateTime(model.HeaderData.RateDate);

                int count = SqlHelper.ExecuteScalar(tran, CommandType.Text, "select count(*) from ORTT where RateDate=@RateDate",new SqlParameter("@RateDate", RateDate)).ToInt();
                if (count > 0)
                {

                    if (model.HeaderData != null)
                    {
                        List<SqlParameter> param = new List<SqlParameter>();



                        string TabHeader = "Rate=@Rate,UpdateDate=@UpdateDate";
                        



                        string HeadQuery = @"update ORTT set " + TabHeader +
                                            " where RateDate = @RateDate";



                        #region SqlParameters

                        #region Header data

                        param.Add(cdal.GetParameter("@RateDate", RateDate, typeof(DateTime)));
                        param.Add(cdal.GetParameter("@Rate", model.HeaderData.Rate, typeof(decimal)));
                        param.Add(cdal.GetParameter("@UpdateDate", DateTime.Now, typeof(DateTime)));

                        #endregion


                        #endregion

                        res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, HeadQuery, param.ToArray()).ToInt();
                        if (res1 <= 0)
                        {
                            tran.Rollback();
                            response.isSuccess = false;
                            response.Message = "An Error Occured";
                            return response;
                        }




                    }
                    if (res1 > 0)
                    {
                        tran.Commit();
                        response.isSuccess = true;
                        response.Message = "Exchange Rate Updated Successfully !";

                    }
                }
                else
                {
                    response.isSuccess = false;
                    response.Message = "Exchange Rate on this Date doesn't exists !";
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
