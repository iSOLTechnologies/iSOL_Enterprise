﻿using iSOL_Enterprise.Common;
using iSOL_Enterprise.Models;
using iSOL_Enterprise.Models.Inventory;
using iSOL_Enterprise.Models.sale;
using Newtonsoft.Json;
using SAPbobsCOM;
using SqlHelperExtensions;
using System.Data;
using System.Data.SqlClient;

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
        public List<tbl_OSLP> GetEmailGroup()
        { 
            string GetQuery = "select EmlGrpCode,EmlGrpName From OEGP";


            List<tbl_OSLP> list = new List<tbl_OSLP>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {

                    list.Add(
                        new tbl_OSLP()
                        {
                            SlpCode = Convert.ToInt32(rdr["EmlGrpCode"]),
                            SlpName = rdr["EmlGrpName"].ToString()
                        });

                }
            }

            return list;
        }
        public List<tbl_OSLP> GetStateCode()
        { 
            string GetQuery = "select Code,[Name] From OCST";


            List<tbl_OSLP> list = new List<tbl_OSLP>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {

                    list.Add(
                        new tbl_OSLP()
                        { 
                            Code = rdr["Code"].ToString(),
                            SlpName = rdr["Name"].ToString()
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











        public ResponseModels AddBusinessMasterData(string formData)
        {
            var model = JsonConvert.DeserializeObject<dynamic>(formData);
            ResponseModels response = new ResponseModels();
            CommonDal cdal = new CommonDal();
            SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
            conn.Open();
            int MySeries = Convert.ToInt32(model.HeaderData.MySeries);
            SqlTransaction tran = conn.BeginTransaction();
            int res1 = 0;
            try
            {
                //int Id = CommonDal.getPrimaryKey(tran, "OITM");
                //string DocNum = SqlHelper.getUpdatedDocumentNumberOnLoad(tran, "OQUT", "SQ");
                if (model.HeaderData != null && model.Tabs_General != null && model.Tabs_PaymentTerms != null && model.Tabs_Properties != null && model.Tabs_Remarks != null)
                {
                    List<SqlParameter> param = new List<SqlParameter>();
                    int Id = CommonDal.getPrimaryKey(tran, "OCRD");

                    param.Add(cdal.GetParameter("@Id", Id, typeof(int)));
                    param.Add(cdal.GetParameter("@Guid", CommonDal.generatedGuid(), typeof(string)));

                    #region BackendCheck For Series
                    if (MySeries != -1)
                    {
                        string? CardCode = SqlHelper.MySeriesUpdate_GetItemCode(MySeries, tran);
                        if (CardCode == null)
                        {
                            tran.Rollback();
                            response.isSuccess = false;
                            response.Message = "An Error Occured";
                            return response;
                        }
                        model.HeaderData.CardCode = CardCode;
                    }
                    #endregion
                    else
                    {
                        int count = SqlHelper.ExecuteScalar(tran, CommandType.Text, "select Count(*) from OIGE where DocNum ='" + model.HeaderData.DocNum.ToString() + "'");
                        if (count > 0)
                        {
                            tran.Rollback();
                            response.isSuccess = false;
                            response.Message = "Duplicate Document Number !";
                            return response;
                        }
                    }
                    //string TabContactPersons = "Position,Address,Tel1,Tel2,Cellolar,Fax,E_Mail,EmlGrpCode,Pager,Notes1,Notes2,Password,CountryOrg,BirthDate,Gender,Profession,BirthCity,ConnectedAddress";
                    string TabHeader = "Id,Guid,MySeries,CardCode,CardType,Series,CardName,CardFName,GroupCode,Currency,LicTradNum";
                    string TabGeneral = "Phone1,CntctPrsn,Phone2,AddID,Cellular,VatIdUnCmp,Fax,RegNum,E_Mail,Notes,IntrntSite,ShipType,SlpCode,Password,Indicator,ProjectCod,ChannlBP,IndustryC,DfTcnician,CmpPrivate,Territory,AliasName,GlblLocNum,validFor,validFrom,validTo,frozenFor,frozenFrom,frozenTo,FrozenComm";
                    string TabPaymentTerms = "GroupNum ,Discount,CreditLine,BankCountr,BankCode,DflAccount,DflSwift,DflBranch,BankCtlKey,DflIBAN,MandateID,SignDate";
                    string TabProperties = "QryGroup1,QryGroup2,QryGroup3,QryGroup4,QryGroup5,QryGroup6,QryGroup7,QryGroup8,QryGroup9,QryGroup10,QryGroup11,QryGroup12,QryGroup13,QryGroup14,QryGroup15,QryGroup16,QryGroup17,QryGroup18,QryGroup19,QryGroup20,QryGroup21,QryGroup22,QryGroup23,QryGroup24,QryGroup25,QryGroup26,QryGroup27,QryGroup28,QryGroup29,QryGroup30,QryGroup31,QryGroup32,QryGroup33,QryGroup34,QryGroup35,QryGroup36,QryGroup37,QryGroup38,QryGroup39,QryGroup40,QryGroup41,QryGroup42,QryGroup43,QryGroup44,QryGroup45,QryGroup46,QryGroup47,QryGroup48,QryGroup49,QryGroup50,QryGroup51,QryGroup52,QryGroup53,QryGroup54,QryGroup55,QryGroup56,QryGroup57,QryGroup58,QryGroup59,QryGroup60,QryGroup61,QryGroup62,QryGroup63,QryGroup64";
                    string TabRemarks = "Free_Text";
                    
                    string HeadQuery = @"insert into OCRD ("+TabHeader+","+TabGeneral+","+TabPaymentTerms+ ","+TabProperties+ ","+ TabRemarks + ") " +
                                        "values(@Id,@Guid,@MySeries,@CardCode,@CardType,@Series,@CardName,@CardFName,@GroupCode,@Currency,@LicTradNum,@Phone1,@CntctPrsn,@Phone2,@AddID,@Cellular,@VatIdUnCmp,@Fax,@RegNum,@E_Mail,@Notes,@IntrntSite,@ShipType,@SlpCode,@Password,@Indicator,@ProjectCod,@ChannlBP,@IndustryC,@DfTcnician,@CmpPrivate,@Territory,@AliasName,@GlblLocNum,@validFor,@validFrom,@validTo,@frozenFor,@frozenFrom,@frozenTo,@FrozenComm,@GroupNum ,@Discount,@CreditLine,@BankCountr,@BankCode,@DflAccount,@DflSwift,@DflBranch,@BankCtlKey,@DflIBAN,@MandateID,@SignDate,@QryGroup1,@QryGroup2,@QryGroup3,@QryGroup4,@QryGroup5,@QryGroup6,@QryGroup7,@QryGroup8,@QryGroup9,@QryGroup10,@QryGroup11,@QryGroup12,@QryGroup13,@QryGroup14,@QryGroup15,@QryGroup16,@QryGroup17,@QryGroup18,@QryGroup19,@QryGroup20,@QryGroup21,@QryGroup22,@QryGroup23,@QryGroup24,@QryGroup25,@QryGroup26,@QryGroup27,@QryGroup28,@QryGroup29,@QryGroup30,@QryGroup31,@QryGroup32,@QryGroup33,@QryGroup34,@QryGroup35,@QryGroup36,@QryGroup37,@QryGroup38,@QryGroup39,@QryGroup40,@QryGroup41,@QryGroup42,@QryGroup43,@QryGroup44,@QryGroup45,@QryGroup46,@QryGroup47,@QryGroup48,@QryGroup49,@QryGroup50,@QryGroup51,@QryGroup52,@QryGroup53,@QryGroup54,@QryGroup55,@QryGroup56,@QryGroup57,@QryGroup58,@QryGroup59,@QryGroup60,@QryGroup61,@QryGroup62,@QryGroup63,@QryGroup64,@Free_Text)";



                    #region SqlParameters

                    #region Header data
                    param.Add(cdal.GetParameter("@MySeries" , model.HeaderData.MySeries, typeof(string)));
                    param.Add(cdal.GetParameter("@CardCode" , model.HeaderData.CardCode, typeof(string)));
                    param.Add(cdal.GetParameter("@CardType" , model.HeaderData.CardType, typeof(string)));
                    param.Add(cdal.GetParameter("@Series" , model.HeaderData.Series,     typeof(string)));
                    param.Add(cdal.GetParameter("@CardName" , model.HeaderData.CardName, typeof(string)));
                    param.Add(cdal.GetParameter("@CardFName" , model.HeaderData.CardFName, typeof(string)));
                    param.Add(cdal.GetParameter("@GroupCode" , model.HeaderData.GroupCode, typeof(int)));
                    param.Add(cdal.GetParameter("@Currency", model.HeaderData.Currency,    typeof(string)));
                    param.Add(cdal.GetParameter("@LicTradNum", model.HeaderData.LicTradNum, typeof(string)));
                    #endregion


                    #region General data
                        param.Add(cdal.GetParameter("@Phone1", model.Tabs_General.Phone1, typeof(string))); 
                        param.Add(cdal.GetParameter("@CntctPrsn", model.Tabs_General.CntctPrsn, typeof(string))); 
                        param.Add(cdal.GetParameter("@Phone2", model.Tabs_General.Phone2, typeof(string)));
                        param.Add(cdal.GetParameter("@AddID", model.Tabs_General.AddID, typeof(string)));
                        param.Add(cdal.GetParameter("@Cellular", model.Tabs_General.Cellular, typeof(string)));
                        param.Add(cdal.GetParameter("@VatIdUnCmp", model.Tabs_General.VatIdUnCmp, typeof(string)));
                        param.Add(cdal.GetParameter("@Fax", model.Tabs_General.Fax, typeof(string)));
                        param.Add(cdal.GetParameter("@RegNum", model.Tabs_General.RegNum, typeof(string)));
                        param.Add(cdal.GetParameter("@E_Mail", model.Tabs_General.E_Mail, typeof(string)));
                        param.Add(cdal.GetParameter("@Notes", model.Tabs_General.Notes, typeof(string)));
                        param.Add(cdal.GetParameter("@IntrntSite", model.Tabs_General.IntrntSite, typeof(string)));
                        param.Add(cdal.GetParameter("@ShipType", model.Tabs_General.ShipType, typeof(int)));
                        param.Add(cdal.GetParameter("@SlpCode", model.Tabs_General.SlpCode, typeof(int)));
                        param.Add(cdal.GetParameter("@Password", model.Tabs_General.Password, typeof(string)));
                        param.Add(cdal.GetParameter("@Indicator", model.Tabs_General.Indicator, typeof(string)));
                        //param.Add(cdal.GetParameter("@Name", model.Tabs_General.Name, typeof(string)));
                        param.Add(cdal.GetParameter("@ProjectCod", model.Tabs_General.ProjectCod, typeof(string)));
                        param.Add(cdal.GetParameter("@ChannlBP", model.Tabs_General.ChannlBP, typeof(string)));
                        param.Add(cdal.GetParameter("@IndustryC", model.Tabs_General.IndustryC, typeof(int)));
                        param.Add(cdal.GetParameter("@DfTcnician", model.Tabs_General.DfTcnician, typeof(int)));
                        param.Add(cdal.GetParameter("@CmpPrivate", model.Tabs_General.CmpPrivate, typeof(string)));
                        param.Add(cdal.GetParameter("@Territory", model.Tabs_General.Territory, typeof(int)));
                        param.Add(cdal.GetParameter("@AliasName", model.Tabs_General.AliasName, typeof(string)));
                        param.Add(cdal.GetParameter("@GlblLocNum", model.Tabs_General.GlblLocNum, typeof(string)));
                        param.Add(cdal.GetParameter("@validFor", model.Tabs_General.validFor, typeof(string)));
                        param.Add(cdal.GetParameter("@validFrom", model.Tabs_General.validFrom, typeof(DateTime)));
                        param.Add(cdal.GetParameter("@validTo", model.Tabs_General.validTo, typeof(DateTime)));
                        param.Add(cdal.GetParameter("@frozenFor", model.Tabs_General.frozenFor, typeof(string)));
                        param.Add(cdal.GetParameter("@frozenFrom", model.Tabs_General.frozenFrom, typeof(DateTime)));
                        param.Add(cdal.GetParameter("@frozenTo", model.Tabs_General.frozenTo, typeof(DateTime)));
                        param.Add(cdal.GetParameter("@FrozenComm", model.Tabs_General.FrozenComm, typeof(string)));
                    #endregion
                    
                    #region Payment Terms data
                    param.Add(cdal.GetParameter("@GroupNum", model.Tabs_PaymentTerms.GroupNum, typeof(int)));
                    param.Add(cdal.GetParameter("@Discount", model.Tabs_PaymentTerms.Discount, typeof(decimal)));
                    param.Add(cdal.GetParameter("@CreditLine" , model.Tabs_PaymentTerms.CreditLine, typeof(decimal)));
                    param.Add(cdal.GetParameter("@BankCountr" , model.Tabs_PaymentTerms.BankCountr, typeof(string)));
                    param.Add(cdal.GetParameter("@BankCode" , model.Tabs_PaymentTerms.BankCode, typeof(string)));
                    //param.Add(cdal.GetParameter("@BankCode1" , model.Tabs_PaymentTerms.BankCode1, typeof(string)));
                    param.Add(cdal.GetParameter("@DflAccount" , model.Tabs_PaymentTerms.DflAccount, typeof(string)));
                    param.Add(cdal.GetParameter("@DflSwift" , model.Tabs_PaymentTerms.DflSwift, typeof(string)));
                    //param.Add(cdal.GetParameter("@BankAccountName" , model.Tabs_PaymentTerms.BankAccountName, typeof(string)));
                    param.Add(cdal.GetParameter("@DflBranch" , model.Tabs_PaymentTerms.DflBranch, typeof(string)));
                    param.Add(cdal.GetParameter("@BankCtlKey" , model.Tabs_PaymentTerms.BankCtlKey, typeof(string)));
                    param.Add(cdal.GetParameter("@DflIBAN" , model.Tabs_PaymentTerms.DflIBAN, typeof(string)));
                    param.Add(cdal.GetParameter("@MandateID" , model.Tabs_PaymentTerms.MandateID, typeof(string)));
                    param.Add(cdal.GetParameter("@SignDate", model.Tabs_PaymentTerms.SignDate, typeof(DateTime)));
                    #endregion

                    #region Properties data
                    param.Add(cdal.GetParameter("@QryGroup1", model.Tabs_Properties.QryGroup1, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup2", model.Tabs_Properties.QryGroup2, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup3", model.Tabs_Properties.QryGroup3, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup4", model.Tabs_Properties.QryGroup4, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup5", model.Tabs_Properties.QryGroup5, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup6", model.Tabs_Properties.QryGroup6, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup7", model.Tabs_Properties.QryGroup7, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup8", model.Tabs_Properties.QryGroup8, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup9", model.Tabs_Properties.QryGroup9, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup10", model.Tabs_Properties.QryGroup10, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup11", model.Tabs_Properties.QryGroup11, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup12", model.Tabs_Properties.QryGroup12, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup13", model.Tabs_Properties.QryGroup13, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup14", model.Tabs_Properties.QryGroup14, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup15", model.Tabs_Properties.QryGroup15, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup16", model.Tabs_Properties.QryGroup16, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup17", model.Tabs_Properties.QryGroup17, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup18", model.Tabs_Properties.QryGroup18, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup19", model.Tabs_Properties.QryGroup19, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup20", model.Tabs_Properties.QryGroup20, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup21", model.Tabs_Properties.QryGroup21, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup22", model.Tabs_Properties.QryGroup22, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup23", model.Tabs_Properties.QryGroup23, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup24", model.Tabs_Properties.QryGroup24, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup25", model.Tabs_Properties.QryGroup25, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup26", model.Tabs_Properties.QryGroup26, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup27", model.Tabs_Properties.QryGroup27, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup28", model.Tabs_Properties.QryGroup28, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup29", model.Tabs_Properties.QryGroup29, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup30", model.Tabs_Properties.QryGroup30, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup31", model.Tabs_Properties.QryGroup31, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup32", model.Tabs_Properties.QryGroup32, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup33", model.Tabs_Properties.QryGroup33, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup34", model.Tabs_Properties.QryGroup34, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup35", model.Tabs_Properties.QryGroup35, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup36", model.Tabs_Properties.QryGroup36, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup37", model.Tabs_Properties.QryGroup37, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup38", model.Tabs_Properties.QryGroup38, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup39", model.Tabs_Properties.QryGroup39, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup40", model.Tabs_Properties.QryGroup40, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup41", model.Tabs_Properties.QryGroup41, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup42", model.Tabs_Properties.QryGroup42, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup43", model.Tabs_Properties.QryGroup43, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup44", model.Tabs_Properties.QryGroup44, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup45", model.Tabs_Properties.QryGroup45, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup46", model.Tabs_Properties.QryGroup46, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup47", model.Tabs_Properties.QryGroup47, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup48", model.Tabs_Properties.QryGroup48, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup49", model.Tabs_Properties.QryGroup49, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup50", model.Tabs_Properties.QryGroup50, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup51", model.Tabs_Properties.QryGroup51, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup52", model.Tabs_Properties.QryGroup52, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup53", model.Tabs_Properties.QryGroup53, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup54", model.Tabs_Properties.QryGroup54, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup55", model.Tabs_Properties.QryGroup55, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup56", model.Tabs_Properties.QryGroup56, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup57", model.Tabs_Properties.QryGroup57, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup58", model.Tabs_Properties.QryGroup58, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup59", model.Tabs_Properties.QryGroup59, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup60", model.Tabs_Properties.QryGroup60, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup61", model.Tabs_Properties.QryGroup61, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup62", model.Tabs_Properties.QryGroup62, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup63", model.Tabs_Properties.QryGroup63, typeof(string)));
                    param.Add(cdal.GetParameter("@QryGroup64", model.Tabs_Properties.QryGroup64, typeof(string)));
                    #endregion


                    #region Remarks data
                    param.Add(cdal.GetParameter("@Free_Text", model.Tabs_Remarks.Free_Text, typeof(string)));
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

                    if (model.Tabs_ContactPersons != null)
                    {
                        foreach (var item in model.Tabs_attachment)
                        {
                            int OCPR_Id = CommonDal.getPrimaryKey(tran, "OCPR");
                        
                        
                        string OCPR_Query = @"insert into OCPR (id,CntctCode,CardCode,Name,FirstName,MiddleName,LastName,Title,Position,Address,Tel1,Tel2,Cellolar,Fax,E_Mail,EmlGrpCode,Pager,Notes1,Notes2,Password,BirthDate,Gender,Profession,BirthCity) 
                                        values(@id,@CntctCode,@CardCode,@Name,@FirstName,@MiddleName,@LastName,@Title,@Position,@Address,@Tel1,@Tel2,@Cellolar,@Fax,@E_Mail,@EmlGrpCode,@Pager,@Notes1,@Notes2,@Password,@BirthDate,@Gender,@Profession,@BirthCity)";

                        #region Contact Persons data
                        param.Add(cdal.GetParameter("@id", OCPR_Id, typeof(int)));
                        param.Add(cdal.GetParameter("@CntctCode", model.Tabs_ContactPersons.CntctCode, typeof(int)));
                        param.Add(cdal.GetParameter("@CardCode", model.HeaderData.CardCode, typeof(string)));
                        param.Add(cdal.GetParameter("@Name", model.HeaderData.Name, typeof(string)));
                        param.Add(cdal.GetParameter("@FirstName", model.HeaderData.FirstName, typeof(string)));
                        param.Add(cdal.GetParameter("@MiddleName", model.HeaderData.MiddleName, typeof(string)));
                        param.Add(cdal.GetParameter("@LastName", model.HeaderData.LastName, typeof(string)));
                        param.Add(cdal.GetParameter("@Title", model.HeaderData.Title, typeof(string)));
                        param.Add(cdal.GetParameter("@Position", model.HeaderData.Position, typeof(string)));
                        param.Add(cdal.GetParameter("@Address", model.HeaderData.Address, typeof(string)));
                        param.Add(cdal.GetParameter("@Tel1", model.HeaderData.Tel1, typeof(string)));
                        param.Add(cdal.GetParameter("@Tel2", model.HeaderData.Tel2, typeof(string)));
                        param.Add(cdal.GetParameter("@Cellolar", model.HeaderData.Cellolar, typeof(string)));
                        param.Add(cdal.GetParameter("@Fax", model.HeaderData.Fax, typeof(string)));
                        param.Add(cdal.GetParameter("@E_Mail", model.HeaderData.E_Mail, typeof(string)));
                        param.Add(cdal.GetParameter("@EmlGrpCode", model.HeaderData.EmlGrpCode, typeof(string)));
                        param.Add(cdal.GetParameter("@Pager", model.HeaderData.Pager, typeof(string)));
                        param.Add(cdal.GetParameter("@Notes1", model.HeaderData.Notes1, typeof(string)));
                        param.Add(cdal.GetParameter("@Notes2", model.HeaderData.Notes2, typeof(string)));
                        param.Add(cdal.GetParameter("@Password", model.HeaderData.Password, typeof(string)));
                       // param.Add(cdal.GetParameter("@CountryOrg", model.HeaderData.CountryOrg, typeof(string)));
                        param.Add(cdal.GetParameter("@BirthDate", model.HeaderData.BirthDate, typeof(string)));
                        param.Add(cdal.GetParameter("@Gender", model.HeaderData.Gender, typeof(string)));
                        param.Add(cdal.GetParameter("@Profession", model.HeaderData.Profession, typeof(string)));
                        param.Add(cdal.GetParameter("@BirthCity", model.HeaderData.BirthCity, typeof(string)));
                            #endregion
                            res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, OCPR_Query).ToInt();
                            if (res1 <= 0)
                            {
                                tran.Rollback();
                                response.isSuccess = false;
                                response.Message = "An Error Occured";
                                return response;

                            }
                        }
                    }

                    if (model.Tabs_attachment != null)
                    {
                        int LineNo = 0;
                        int ATC1Id = CommonDal.getPrimaryKey(tran, "AbsEntry", "ATC1");
                        foreach (var item in model.Tabs_attachment)
                        {
                            if (item.selectedFilePath != "" && item.selectedFileName != "" && item.selectedFileDate != "")
                            {


                                string RowQueryAttachment = @"insert into ATC1(AbsEntry,Line,trgtPath,FileName,Date)
                                                  values(" + ATC1Id + ","
                                                        + LineNo + ",'"
                                                        + item.selectedFilePath + "','"
                                                        + item.selectedFileName + "','"
                                                        + Convert.ToDateTime(item.selectedFileDate) + "')";
                                #region sqlparam
                                //List<SqlParameter> param3 = new List<SqlParameter>
                                //            {
                                //                new SqlParameter("@AbsEntry",ATC1Id),
                                //                new SqlParameter("@Line",ATC1Line),
                                //                new SqlParameter("@trgtPath",item.trgtPath),
                                //                new SqlParameter("@FileName",item.FileName),
                                //                new SqlParameter("@Date",item.Date),
                                //            };
                                #endregion

                                res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, RowQueryAttachment).ToInt();
                                if (res1 <= 0)
                                {
                                    tran.Rollback();
                                    response.isSuccess = false;
                                    response.Message = "An Error Occured";
                                    return response;

                                }
                                LineNo += 1;
                            }
                        }
                    }
                }
                if (res1 > 0)
                {
                    tran.Commit();
                    response.isSuccess = true;
                    response.Message = "Business Partner Added Successfully !";

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
