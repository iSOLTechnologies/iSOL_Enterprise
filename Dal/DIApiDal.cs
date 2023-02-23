using iSOL_Enterprise.Common;
using iSOL_Enterprise.Dal;
using iSOL_Enterprise.Models;
using iSOL_Enterprise.Models.sale;
using SAPbobsCOM;
using SqlHelperExtensions;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace SAP_MVC_DIAPI.BLC
{
    public class DIApiDal
    {
        private Company oCompany = new Company();
        private int connectionResult;

        public bool Connect()
        {
            oCompany.Server = "DESKTOP-AJM6HM8\\SQLSERVER19";
            oCompany.CompanyDB = "SAPDB";
            oCompany.DbServerType = BoDataServerTypes.dst_MSSQL2019;
            oCompany.DbUserName = "sa";
            oCompany.DbPassword = "n5210567";
            oCompany.UserName = "manager";
            oCompany.Password = "Bilal@123";
            oCompany.language = BoSuppLangs.ln_English_Gb;
            oCompany.UseTrusted = false;
            connectionResult = oCompany.Connect();

            if (oCompany.Connected)
            {


                return true;
            }
            else
            {

                return false;
            }
        }


        public ResponseModels PostToSap(string[] checkedIDs, int ObjectCode)
        {
            ResponseModels models = new ResponseModels();
            try
            {
                if (Connect())
                {
                    CommonDal dal = new CommonDal();

                    SAPbobsCOM.Documents oDoc = dal.getDocObj(ObjectCode, oCompany);
                    if (oDoc != null)
                    {
                        string headerTable = dal.GetMasterTable(ObjectCode);
                        string rowTable = dal.GetRowTable(ObjectCode);
                        string message = "";
                        int Series = SqlHelper.ExecuteScalar(SqlHelper.defaultDB, CommandType.Text, "select Series from Pages where ObjectCode =" + ObjectCode).ToInt();

                        foreach (var ID in checkedIDs)
                        {
                            string headerQuery = @"select DocType,CardCode,CardName,CntctCode,DocDate,NumAtCard,DocDueDate,DocCur,TaxDate ,GroupNum ,SlpCode,Comments from " + headerTable + " where Id=" + ID;
                            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, headerQuery))
                            {
                                while (rdr.Read())
                                {
                                    oDoc.Series = Series;
                                    oDoc.DocType = rdr["DocType"].ToString() == "I" ? BoDocumentTypes.dDocument_Items : BoDocumentTypes.dDocument_Service;
                                    oDoc.CardCode = rdr["CardCode"].ToString();
                                    oDoc.CardName = rdr["CardName"].ToString();
                                    oDoc.ContactPersonCode = rdr["CntctCode"].ToInt();
                                    oDoc.DocDate = Convert.ToDateTime(rdr["DocDate"] ?? DateTime.Now);
                                    oDoc.NumAtCard = rdr["NumAtCard"].ToString();
                                    oDoc.DocDueDate = Convert.ToDateTime(rdr["DocDueDate"] ?? DateTime.Now);
                                    oDoc.DocCurrency = rdr["DocCur"].ToString();
                                    oDoc.TaxDate = Convert.ToDateTime(rdr["TaxDate"] ?? DateTime.Now);
                                    oDoc.GroupNumber = rdr["GroupNum"].ToInt();
                                    oDoc.SalesPersonCode = rdr["SlpCode"].ToInt();
                                    oDoc.Comments = rdr["Comments"].ToString();

                                    string RowQuery = @"select BaseEntry,BaseLine,BaseType,Price,LineTotal,ItemCode,Quantity,DiscPrcnt,VatGroup,UomEntry ,CountryOrg , Dscription,AcctCode from " + rowTable + " where Id = " + ID;
                                    using (var rdr2 = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, RowQuery))
                                    {
                                        while (rdr2.Read())
                                        {

                                           
                                            if (rdr2["BaseLine"].ToString() != "")
                                                oDoc.Lines.BaseEntry = rdr2["BaseEntry"].ToInt();
                                            if (rdr2["BaseLine"].ToString() != "")
                                                oDoc.Lines.BaseLine = rdr2["BaseLine"].ToInt();
                                            if (rdr2["BaseLine"].ToString() != "")
                                                oDoc.Lines.BaseType = rdr2["BaseType"].ToInt();
                                            if (rdr2["BaseLine"].ToString() != "")
                                                oDoc.Lines.Price = rdr2["Price"].ToDouble();
                                            if (rdr2["BaseLine"].ToString() != "")
                                                oDoc.Lines.LineTotal = rdr2["LineTotal"].ToDouble();
                                            oDoc.Lines.ItemCode = rdr2["ItemCode"].ToString();
                                            oDoc.Lines.Quantity = rdr2["Quantity"].ToDouble();
                                            oDoc.Lines.DiscountPercent = rdr2["DiscPrcnt"].ToDouble();
                                            oDoc.Lines.VatGroup = rdr2["VatGroup"].ToString();
                                            oDoc.Lines.UoMEntry = rdr2["UomEntry"].ToInt();
                                            oDoc.Lines.CountryOrg = rdr2["CountryOrg"].ToString();
                                            oDoc.Lines.ItemDescription = rdr2["Dscription"].ToString();
                                            oDoc.Lines.AccountCode = rdr2["AcctCode"].ToString();
                                            oDoc.Lines.Add();

                                        }
                                    }
                                }
                            }
                            int res = oDoc.Add();
                            if (res < 0)
                            {

                                oCompany.GetLastError(out res, out message);
                                models.Message = message;
                                models.isSuccess = false;
                                return models;
                            }


                        }


                        models.Message = "Posted Data Successfully !!";
                        models.isSuccess = true;
                        return models;

                    }
                    else
                    {
                        models.Message = "Page not Found !!";
                        models.isSuccess = false;
                        return models;
                    }

                }
                else
                {
                    models.Message = "Connection Failure !!";
                    models.isSuccess = false;
                    return models;

                }

            }
            catch (Exception ex)
            {
                models.Message = "An Error Occured";
                models.isSuccess = false;
                return models;

            }

        }




        public int DisConnect()
        {
            oCompany.Disconnect();
            return 1;
        }
    }
}
