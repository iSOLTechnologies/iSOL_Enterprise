using SqlHelperExtensions;
using System.Data.SqlClient;
using System.Data;
using iSOL_Enterprise.Common;
using iSOL_Enterprise.Models;
using Newtonsoft.Json;

namespace iSOL_Enterprise.Dal.Administrator
{
    public class CompanyDetailsDal
    {

        public dynamic GetOADMData()
        {
            try
            {



                DataSet ds = new DataSet();
                SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
                string headerQuery = @"select Top(1) RevOffice,TaxIdNum,TaxIdNum2,HldCode,MainCurncy,SysCurrncy,DfActCurr,InvntSystm,DeprecCalc,                                
                                            SnBDfltSB,ContInvnt,PriceSys,RelStkNoPr,InstFixAst,MltpBrnchs,NewAcctDe,CompnyName,CompnyAddr,
                                            State,Country,PrintHeadr,Manager,AliasName,Phone1,Phone2,Fax,E_Mail from OADM ";

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
        public dynamic GetADM1Data()
        {
            try
            {



                DataSet ds = new DataSet();
                SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
                string headerQuery = @"select Top(1) Street,StreetNo,Block,Building,City,ZipCode,County,IntrntAdrs,GlblLocNum from ADM1 ";

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
        public ResponseModels Update(string formData)
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
                
                
                int count = SqlHelper.ExecuteScalar(tran, CommandType.Text, "delete from OADM ").ToInt();
                if (count > 0 || count == 0)
                {
                    count = SqlHelper.ExecuteScalar(tran, CommandType.Text, "delete from ADM1 ").ToInt();
                    if (count <0)
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
                    response.Message = "An Error Occured";
                    return response;
                }

                if (model.TabBasicInitialization != null)
                {
                        List<SqlParameter> param = new List<SqlParameter>();



                        string TabHeader = @"RevOffice,TaxIdNum,TaxIdNum2,HldCode,MainCurncy,SysCurrncy,DfActCurr,InvntSystm,DeprecCalc,                                
                                            SnBDfltSB,ContInvnt,PriceSys,RelStkNoPr,InstFixAst,MltpBrnchs,NewAcctDe,CompnyName,CompnyAddr,
                                            State,Country,PrintHeadr,Manager,AliasName,Phone1,Phone2,Fax,E_Mail";
                        string TabHeaderP = @"@RevOffice,@TaxIdNum,@TaxIdNum2,@HldCode,@MainCurncy,@SysCurrncy,@DfActCurr,@InvntSystm,@DeprecCalc,                                
                                            @SnBDfltSB,@ContInvnt,@PriceSys,@RelStkNoPr,@InstFixAst,@MltpBrnchs,@NewAcctDe,@CompnyName,@CompnyAddr,
                                            @State,@Country,@PrintHeadr,@Manager,@AliasName,@Phone1,@Phone2,@Fax,@E_Mail";



                        string HeadQuery = @"insert into OADM (" + TabHeader + ") " +
                                            " values(" + TabHeaderP + ")";



                        #region SqlParameters

                        #region Header data

                        param.Add(cdal.GetParameter("@RevOffice", model.TabAccountingData.RevOffice, typeof(string)));
                        param.Add(cdal.GetParameter("@TaxIdNum", model.TabAccountingData.TaxIdNum, typeof(string)));
                        param.Add(cdal.GetParameter("@TaxIdNum2", model.TabAccountingData.TaxIdNum2, typeof(string)));
                        param.Add(cdal.GetParameter("@HldCode", model.TabAccountingData.HldCode, typeof(string)));

                        param.Add(cdal.GetParameter("@MainCurncy", model.TabBasicInitialization.MainCurncy, typeof(string)));
                        param.Add(cdal.GetParameter("@SysCurrncy", model.TabBasicInitialization.SysCurrncy, typeof(string)));
                        param.Add(cdal.GetParameter("@DfActCurr", model.TabBasicInitialization.DfActCurr, typeof(string)));
                        param.Add(cdal.GetParameter("@InvntSystm", model.TabBasicInitialization.InvntSystm, typeof(char)));
                        param.Add(cdal.GetParameter("@DeprecCalc", model.TabBasicInitialization.DeprecCalc, typeof(char)));
                        param.Add(cdal.GetParameter("@SnBDfltSB", model.TabBasicInitialization.SnBDfltSB, typeof(char)));
                        param.Add(cdal.GetParameter("@ContInvnt", model.TabBasicInitialization.ContInvnt, typeof(char)));
                        param.Add(cdal.GetParameter("@PriceSys", model.TabBasicInitialization.PriceSys, typeof(char)));
                        param.Add(cdal.GetParameter("@RelStkNoPr", model.TabBasicInitialization.RelStkNoPr, typeof(char)));
                        param.Add(cdal.GetParameter("@InstFixAst", model.TabBasicInitialization.InstFixAst, typeof(char)));
                        param.Add(cdal.GetParameter("@MltpBrnchs", model.TabBasicInitialization.MltpBrnchs, typeof(char)));
                        param.Add(cdal.GetParameter("@NewAcctDe", model.TabBasicInitialization.NewAcctDe, typeof(char)));
                        
                        param.Add(cdal.GetParameter("@CompnyName", model.TabGeneral.CompnyName, typeof(string)));
                        param.Add(cdal.GetParameter("@CompnyAddr", model.TabGeneral.CompnyAddr, typeof(string)));
                        param.Add(cdal.GetParameter("@State", model.TabGeneral.State, typeof(string)));
                        param.Add(cdal.GetParameter("@Country", model.TabGeneral.Country, typeof(string)));
                        param.Add(cdal.GetParameter("@PrintHeadr", model.TabGeneral.PrintHeadr, typeof(string)));
                        param.Add(cdal.GetParameter("@Manager", model.TabGeneral.Manager, typeof(string)));
                        param.Add(cdal.GetParameter("@AliasName", model.TabGeneral.AliasName, typeof(string)));
                        param.Add(cdal.GetParameter("@Phone1", model.TabGeneral.Phone1, typeof(string)));
                        param.Add(cdal.GetParameter("@Phone2", model.TabGeneral.Phone2, typeof(string)));
                        param.Add(cdal.GetParameter("@Fax", model.TabGeneral.Fax, typeof(string)));
                        param.Add(cdal.GetParameter("@E_Mail", model.TabGeneral.E_Mail, typeof(string)));


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



                        TabHeader = @"Street,StreetNo,Block,Building,City,ZipCode,County,IntrntAdrs,GlblLocNum";
                        TabHeaderP = @"@Street,@StreetNo,@Block,@Building,@City,@ZipCode,@County,@IntrntAdrs,@GlblLocNum";

                        HeadQuery = @"insert into ADM1 (" + TabHeader + ") " +
                                            " values(" + TabHeaderP + ")";
                       
                        #region SqlParameters

                        #region Header data

                        param.Clear();

                        param.Add(cdal.GetParameter("@Street", model.TabGeneral.Street, typeof(string)));
                        param.Add(cdal.GetParameter("@StreetNo", model.TabGeneral.StreetNo, typeof(string)));
                        param.Add(cdal.GetParameter("@Block", model.TabGeneral.Block, typeof(string)));
                        param.Add(cdal.GetParameter("@Building", model.TabGeneral.Building, typeof(string)));
                        param.Add(cdal.GetParameter("@City", model.TabGeneral.City, typeof(string)));
                        param.Add(cdal.GetParameter("@ZipCode", model.TabGeneral.ZipCode, typeof(string)));
                        param.Add(cdal.GetParameter("@County", model.TabGeneral.County, typeof(string)));
                        param.Add(cdal.GetParameter("@IntrntAdrs", model.TabGeneral.IntrntAdrs, typeof(string)));
                        param.Add(cdal.GetParameter("@GlblLocNum", model.TabGeneral.GlblLocNum, typeof(decimal)));


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
                else
                {
                    tran.Rollback();
                    response.isSuccess = false;
                    response.Message = "Data can't be null";
                    return response;
                }

                if (res1 > 0)
                {
                    tran.Commit();
                    response.isSuccess = true;
                    response.Message = "Company detail Upated Successfully !";

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
