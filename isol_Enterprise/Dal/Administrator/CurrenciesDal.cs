using iSOL_Enterprise.Common;
using iSOL_Enterprise.Models;
using iSOL_Enterprise.Models.Administration;
using Newtonsoft.Json;
using SqlHelperExtensions;
using System.Data;
using System.Data.SqlClient;

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
        public List<ListModel> GetCurrencies()
        {
            string GetQuery = "select Code,Code + ' - ' + Country + ', ' + Currency as Name from Currencies order by Country ";

            List<ListModel> list = new List<ListModel>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {
                    list.Add(new ListModel()
                    {
                        ValueString = rdr["Code"].ToString(),
                        Text = rdr["Name"].ToString()

                    });
                }
            }
            return list;
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
                int count = SqlHelper.ExecuteScalar(tran, CommandType.Text, "select count(*) from OCRN where CurrCode='" + (model.HeaderData.ISOCurrCod).ToString() + "'");
                if (count == 0)
                {


                    if (model.HeaderData != null)
                    {
                        List<SqlParameter> param = new List<SqlParameter>();
                        

                        
                        string TabHeader = "CurrCode,CurrName,DocCurrCod,ChkName,Chk100Name,FrgnName,F100Name,ISOCurrCod,MaxInDiff";
                        string TabHeaderP = "@CurrCode,@CurrName,@DocCurrCod,@ChkName,@Chk100Name,@FrgnName,@F100Name,@ISOCurrCod,@MaxInDiff";

                        

                        string HeadQuery = @"insert into OCRN (" + TabHeader + ") " +
                                            "values(" + TabHeaderP + ")";



                        #region SqlParameters

                        #region Header data
                        
                        param.Add(cdal.GetParameter("@CurrCode", model.HeaderData.ISOCurrCod, typeof(string)));                        
                        param.Add(cdal.GetParameter("@CurrName", model.HeaderData.CurrName, typeof(string)));
                        param.Add(cdal.GetParameter("@DocCurrCod", model.HeaderData.DocCurrCod, typeof(string)));
                        param.Add(cdal.GetParameter("@ChkName", model.HeaderData.ChkName, typeof(string)));                        
                        param.Add(cdal.GetParameter("@Chk100Name", model.HeaderData.Chk100Name, typeof(string)));
                        param.Add(cdal.GetParameter("@FrgnName", model.HeaderData.FrgnName, typeof(string)));
                        param.Add(cdal.GetParameter("@F100Name", model.HeaderData.F100Name, typeof(string)));
                        param.Add(cdal.GetParameter("@ISOCurrCod", model.HeaderData.ISOCurrCod, typeof(string)));                        
                        param.Add(cdal.GetParameter("@MaxInDiff", model.HeaderData.MaxInDiff, typeof(decimal)));


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
                        response.Message = "Currency Added Successfully !";

                    }
                }
                else
                {
                    response.isSuccess = false;
                    response.Message = "Currency Code already exists !";
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
