using iSOL_Enterprise.Common;
using iSOL_Enterprise.Models;
using iSOL_Enterprise.Models.Administration;
using iSOL_Enterprise.Models.ChartOfAccount;
using Newtonsoft.Json;
using SqlHelperExtensions;
using System.Data;
using System.Data.SqlClient;

namespace iSOL_Enterprise.Dal.Administrator
{
    public class HolidayDatesDal
    {

        public List<HolidayDatesMasterModel> GetData()
        {
            string GetQuery = @"select HldCode,b.Name as WndFrm , c.Name as WndTo,isCurYear,ignrWnd,WeekNoRule from OHLD a
                                inner join tbl_WeekDays b on a.WndFrm =  b.id
                                inner join tbl_WeekDays c on a.WndTo =  c.id
                                order by HldCode desc";


            List<HolidayDatesMasterModel> list = new List<HolidayDatesMasterModel>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {

                    HolidayDatesMasterModel models = new()
                    {

                        HldCode = rdr["HldCode"].ToString(),
                        WndFrm = rdr["WndFrm"].ToString(),
                        WndTo = rdr["WndTo"].ToString(),
                        isCurYear = rdr["isCurYear"].ToString(),
                        ignrWnd = rdr["ignrWnd"].ToString(),
                        WeekNoRule = rdr["WeekNoRule"].ToString(),
                        
                    };

                    list.Add(models);
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
               

                if (model.HeaderData != null)
                {
                    string HldCode = (model.HeaderData.HldCode).ToString();

                    int count = SqlHelper.ExecuteScalar(tran, CommandType.Text, "select count(*) from OHLD where HldCode='" + HldCode + "'").ToInt();
                    if (count == 0)
                    {
                    

                        List<SqlParameter> param = new List<SqlParameter>();

                        

                        

                        string TabHeader = "HldCode,WeekNoRule,WndFrm,WndTo,isCurYear,ignrWnd";
                        string TabHeaderP = "@HldCode,@WeekNoRule,@WndFrm,@WndTo,@isCurYear,@ignrWnd";

                        

                        string HeadQuery = @"insert into OHLD (" + TabHeader + ") " +
                                            "values(" + TabHeaderP + ")";



                        #region SqlParameters

                        #region Header data

                        param.Add(cdal.GetParameter("@HldCode", HldCode, typeof(string)));                        
                        param.Add(cdal.GetParameter("@WeekNoRule", model.HeaderData.WeekNoRule, typeof(char)));
                        param.Add(cdal.GetParameter("@WndFrm", model.HeaderData.WndFrm, typeof(string)));
                        param.Add(cdal.GetParameter("@WndTo", model.HeaderData.WndTo, typeof(string)));
                        param.Add(cdal.GetParameter("@isCurYear", model.HeaderData.isCurYear, typeof(char)));
                        param.Add(cdal.GetParameter("@ignrWnd", model.HeaderData.ignrWnd, typeof(char)));
                       
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
                        if (model.RowData != null)
                        {

                            foreach (var item in model.RowData)
                            {


                                if (item.StrDate != "" && item.StrDate != null && item.EndDate != "" && item.EndDate != null)
                                {

                                    List<SqlParameter> param2 = new List<SqlParameter>();

                                    string TabRow = @"HldCode,StrDate,EndDate,Rmrks";
                                    string TabRowP = @"@HldCode,@StrDate,@EndDate,@Rmrks";

                                    string RowQuery = @"insert into HLD1 (" + TabRow + ") " +
                                                    "values(" + TabRowP + ")";


                                    #region RowData
                                    param2.Add(cdal.GetParameter("@HldCode", HldCode, typeof(string)));
                                    param2.Add(cdal.GetParameter("@StrDate", item.StrDate, typeof(DateTime)));
                                    param2.Add(cdal.GetParameter("@EndDate", item.EndDate, typeof(DateTime)));
                                    param2.Add(cdal.GetParameter("@Rmrks", item.Rmrks, typeof(string)));


                                    #endregion


                                    res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, RowQuery, param2.ToArray()).ToInt();
                                    if (res1 <= 0)
                                    {
                                        tran.Rollback();
                                        response.isSuccess = false;
                                        response.Message = "An Error Occured";
                                        return response;
                                    }
                                    
                                }
                                else
                                {
                                    tran.Rollback();
                                    response.isSuccess = false;
                                    response.Message = "Start of End Date can't be null !";
                                    return response;
                                }
                            }
                                
                        }
                   
                        if (res1 > 0)
                        {
                            tran.Commit();
                            response.isSuccess = true;
                            response.Message = "Holiday Dates Added Successfully !";

                        }
                    }
                    else
                    {
                        response.isSuccess = false;
                        response.Message = "Holiday Code already exists !";
                    }
                }
                else
                {
                    tran.Rollback();
                    response.isSuccess = false;
                    response.Message = "An Error Occured";
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
            
        
    }
}
