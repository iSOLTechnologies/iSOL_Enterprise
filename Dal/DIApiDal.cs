using iSOL_Enterprise.Common;
using iSOL_Enterprise.Dal;
using iSOL_Enterprise.Models;
using iSOL_Enterprise.Models.sale;
using SAPbobsCOM;
using SqlHelperExtensions;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SAP_MVC_DIAPI.BLC
{
    public class DIApiDal
    {
        private Company oCompany = new Company();
        private int connectionResult;

        public bool Connect()
        {
            oCompany.Server = CommonDal.DiAPI_Server;
            oCompany.CompanyDB = CommonDal.DiAPI_companydb;
            oCompany.DbServerType = CommonDal.dst_HANADB;
            oCompany.DbUserName = CommonDal.DiAPI_dbusername;
            oCompany.DbPassword = CommonDal.DiAPI_dbpassword;
            oCompany.UserName = CommonDal.DiAPI_username;
            oCompany.Password = CommonDal.DiAPI_password;
            oCompany.language = CommonDal.DiAPI_langauge;
            oCompany.UseTrusted = CommonDal.DiAPI_UserTrusted;

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
                        //int Series = SqlHelper.ExecuteScalar(SqlHelper.defaultDB, CommandType.Text, "select Series from Pages where ObjectCode =" + ObjectCode).ToInt();
                        SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
                        conn.Open();
                        SqlTransaction tran = conn.BeginTransaction();



                        foreach (var ID in checkedIDs)
                        {
                            string UDF = "";
                            if (headerTable == "ORDR")
                                UDF = ",CETnum";
                            string headerQuery = @"select DocType,Series,CardCode,CardName,CntctCode,DocDate,NumAtCard,DocDueDate,DocCur,TaxDate ,GroupNum ,SlpCode,Comments,Id " + UDF + " from " + headerTable + " where Id=" + ID + " and isPosted = 0";
                            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, headerQuery))
                            {
                                try
                                {


                                    while (rdr.Read())
                                    {
                                        #region Insert In Header

                                        oDoc.Series = rdr["Series"].ToInt();
                                        oDoc.DocType = rdr["DocType"].ToString() == "I" ? BoDocumentTypes.dDocument_Items : BoDocumentTypes.dDocument_Service;
                                        oDoc.CardCode = rdr["CardCode"].ToString();
                                        oDoc.CardName = rdr["CardName"].ToString();

                                        oDoc.ContactPersonCode = rdr["CntctCode"].ToInt();
                                        oDoc.DocDate = rdr["DocDate"].ToString() == "" ? DateTime.Now : Convert.ToDateTime(rdr["DocDate"].ToString());
                                        oDoc.NumAtCard = rdr["NumAtCard"].ToString();
                                        oDoc.DocDueDate = rdr["DocDueDate"].ToString() == "" ? DateTime.Now : Convert.ToDateTime(rdr["DocDueDate"].ToString());
                                        oDoc.DocCurrency = rdr["DocCur"].ToString();
                                        oDoc.TaxDate = rdr["TaxDate"].ToString() == "" ? DateTime.Now : Convert.ToDateTime(rdr["TaxDate"].ToString());
                                        oDoc.GroupNumber = rdr["GroupNum"].ToInt();
                                        oDoc.SalesPersonCode = rdr["SlpCode"].ToInt();
                                        oDoc.Comments = rdr["Comments"].ToString();
                                        if (headerTable == "ORDR")      //For UDF
                                            oDoc.UserFields.Fields.Item("U_CETnum").Value = rdr["CETnum"].ToString();
                                        #endregion

                                        #region Insert in Row
                                        string RowQuery = @"select BaseEntry,BaseLine,BaseType,Price,LineTotal,ItemCode,Quantity,DiscPrcnt,VatGroup,UomEntry ,CountryOrg , Dscription,AcctCode,LineNum from " + rowTable + " where Id = " + ID;
                                        using (var rdr2 = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, RowQuery))
                                        {
                                            try
                                            {


                                                while (rdr2.Read())
                                                {


                                                    if (rdr2["BaseEntry"].ToString() != "")
                                                        oDoc.Lines.BaseEntry = rdr2["BaseEntry"].ToInt();
                                                    if (rdr2["BaseLine"].ToString() != "")
                                                        oDoc.Lines.BaseLine = rdr2["BaseLine"].ToInt();
                                                    if (rdr2["BaseType"].ToString() != "")
                                                        oDoc.Lines.BaseType = rdr2["BaseType"].ToInt();
                                                    if (rdr2["Price"].ToString() != "")
                                                        oDoc.Lines.Price = rdr2["Price"].ToDouble();
                                                    if (rdr2["LineTotal"].ToString() != "")
                                                        oDoc.Lines.LineTotal = rdr2["LineTotal"].ToDouble();

                                                    oDoc.Lines.ItemCode = rdr2["ItemCode"].ToString();
                                                    oDoc.Lines.Quantity = rdr2["Quantity"].ToDouble();
                                                    oDoc.Lines.DiscountPercent = rdr2["DiscPrcnt"].ToDouble();
                                                    //oDoc.Lines.VatGroup = rdr2["VatGroup"].ToString();
                                                    oDoc.Lines.UoMEntry = rdr2["UomEntry"].ToInt();
                                                    oDoc.Lines.CountryOrg = rdr2["CountryOrg"].ToString();
                                                    oDoc.Lines.ItemDescription = rdr2["Dscription"].ToString();
                                                    oDoc.Lines.AccountCode = rdr2["AcctCode"].ToString();


                                                    if (rowTable == "DLN1")
                                                    {

                                                        string BatchQuery = @" select ITL1.ItemCode,ITL1.SysNumber,ITL1.Quantity,ITL1.AllocQty,OITL.CreateDate, OBTN.ExpDate,OBTN.DistNumber from OITL 
                                                                           inner join ITL1 on OITL.LogEntry = ITL1.LogEntry 
                                                                           inner join OBTQ on ITL1.MdAbsEntry = OBTQ.AbsEntry 
                                                                           inner join OBTN on OBTQ.MdAbsEntry = OBTN.AbsEntry
                                                                           where DocLine = '" + rdr2["LineNum"].ToString() + "' and DocNum = '" + rdr["Id"].ToString() + "'";
                                                        using (var rdr3 = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, BatchQuery))
                                                        {
                                                            while (rdr3.Read())
                                                            {

                                                                oDoc.Lines.BatchNumbers.SetCurrentLine(0);

                                                                oDoc.Lines.BatchNumbers.ItemCode = rdr3["ItemCode"].ToString();
                                                                oDoc.Lines.BatchNumbers.BatchNumber = rdr3["DistNumber"].ToString();
                                                                // oDoc.Lines.BatchNumbers.SystemSerialNumber = rdr3["SysNumber"].ToInt();
                                                                oDoc.Lines.BatchNumbers.Quantity = rdr3["Quantity"].ToInt() > 0 ? rdr3["Quantity"].ToInt() : (-1 * rdr3["Quantity"].ToInt());
                                                                oDoc.Lines.BatchNumbers.AddmisionDate = rdr3["CreateDate"].ToString() == "" ? DateTime.Now : Convert.ToDateTime(rdr3["CreateDate"].ToString());
                                                                if (rdr3["ExpDate"].ToString() != "")
                                                                    oDoc.Lines.BatchNumbers.ExpiryDate = Convert.ToDateTime(rdr3["ExpDate"].ToString());
                                                                oDoc.Lines.BatchNumbers.Add();

                                                            }
                                                        }

                                                    }

                                                    if (rowTable == "PDN1")
                                                    {

                                                        try
                                                        {


                                                            string BatchQuery = @" select ITL1.ItemCode,ITL1.SysNumber,ITL1.Quantity,ITL1.AllocQty,OITL.CreateDate, OBTN.ExpDate,OBTN.DistNumber from OITL 
                                                                           inner join ITL1 on OITL.LogEntry = ITL1.LogEntry 
                                                                           inner join OBTQ on ITL1.MdAbsEntry = OBTQ.AbsEntry 
                                                                           inner join OBTN on OBTQ.MdAbsEntry = OBTN.AbsEntry
                                                                           where DocLine = '" + rdr2["LineNum"].ToString() + "' and DocNum = '" + rdr["Id"].ToString() + "'";
                                                            using (var rdr3 = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, BatchQuery))
                                                            {

                                                                while (rdr3.Read())
                                                                {

                                                                    //SAPbobsCOM.BatchNumbers oBatchNumber = oDoc.Lines.BatchNumbers;
                                                                    //SAPbobsCOM.Batc oBatchNumber = oBatchNumbers.BatchNumber;

                                                                    //oBatchNumber.BatchNumber = rdr3["DistNumber"].ToString();
                                                                    //oBatchNumber.Quantity = rdr3["Quantity"].ToInt() > 0 ? rdr3["Quantity"].ToInt() : (-1 * rdr3["Quantity"].ToInt());
                                                                    //oBatchNumber.ManufacturerSerialNumber = "MSN001";
                                                                    //oBatchNumber.InternalSerialNumber = "ISN001";
                                                                    //oBatchNumber.ManufacturingDate = DateTime.Today;
                                                                    //oBatchNumber.Location = "LOC001";
                                                                    //oBatchNumber.Notes = "New batch added via DI API";

                                                                    //oBatchNumber.Add();

                                                                    //if (i == 0)
                                                                    //    oDoc.Lines.BatchNumbers.SetCurrentLine(0);
                                                                    oDoc.Lines.BatchNumbers.ItemCode = rdr3["ItemCode"].ToString();
                                                                    oDoc.Lines.BatchNumbers.BatchNumber = rdr3["DistNumber"].ToString();
                                                                    oDoc.Lines.BatchNumbers.BaseLineNumber = rdr2["BaseLine"].ToInt();

                                                                    // oDoc.Lines.BatchNumbers.SystemSerialNumber = rdr3["SysNumber"].ToInt();
                                                                    oDoc.Lines.BatchNumbers.Quantity = rdr3["Quantity"].ToInt() > 0 ? rdr3["Quantity"].ToInt() : (-1 * rdr3["Quantity"].ToInt());
                                                                    //oDoc.Lines.BatchNumbers.AddmisionDate = rdr3["CreateDate"].ToString() == "" ? DateTime.Now : Convert.ToDateTime(rdr3["CreateDate"].ToString());
                                                                    //if (rdr3["ExpDate"].ToString() != "")
                                                                    //    oDoc.Lines.BatchNumbers.ExpiryDate = Convert.ToDateTime(rdr3["ExpDate"].ToString());
                                                                    oDoc.Lines.BatchNumbers.Add();




                                                                    //i += 1;
                                                                }
                                                            }
                                                        }
                                                        catch (Exception)
                                                        {

                                                            throw;
                                                        }
                                                    }
                                                    oDoc.Lines.Add();
                                                }
                                            }
                                            catch (Exception)
                                            {

                                                throw;
                                            }
                                        }
                                    }
                                    #endregion


                                }
                                catch (Exception e)
                                {
                                    models.Message = e.Message;
                                    models.isSuccess = false;
                                    return models;
                                    throw;
                                }
                            }

                            #region Updating Table Row as Posted
                            string BatchQueryOBTN = @"Update " + headerTable + " set isPosted = 1 where Id =" + ID;    //For Updating master table row as this data is posted to SAP
                            int res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, BatchQueryOBTN).ToInt();
                            if (res1 <= 0)
                            {
                                tran.Rollback();

                                models.Message = "An Error occured";
                                models.isSuccess = false;
                                return models;
                            }
                            #endregion
                            else
                            {

                                #region Posting data to SAP
                                int res = oDoc.Add();
                                if (res < 0)
                                {

                                    oCompany.GetLastError(out res, out message);
                                    models.Message = message;
                                    models.isSuccess = false;
                                    tran.Rollback();
                                    return models;
                                }
                                else
                                    tran.Commit();
                                #endregion
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



        public ResponseModels PostPlanningSheet(string[] checkedIDs)
        {
            ResponseModels models = new ResponseModels();
            try
            {
                if (Connect())
                {
                    CommonDal dal = new CommonDal();
                    SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
                    conn.Open();
                    SqlTransaction tran = conn.BeginTransaction();

                    foreach (var docEntry in checkedIDs)
                    {
                        string headerQuery = @"select DocEntry,U_PlanDate,Status,U_SOnum,U_CutomerCode,U_SODate,U_ShipDate,U_ItemCode,U_ItemDes,U_Qty,U_UOM from dbo.[@PSF] where DocEntry=" + docEntry;

                        using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, headerQuery))
                        {
                            try
                            {

                                while (rdr.Read())
                                {
                                    UserTable PSFTable = oCompany.UserTables.Item("dbo.[@PSF]");
                                    if (PSFTable != null)
                                    {
                                        #region Insert in PSF
                                        PSFTable.UserFields.Fields.Item("DocEntry").Value = "DocEntry";
                                        PSFTable.UserFields.Fields.Item("U_PlanDate").Value = "U_PlanDate";
                                        PSFTable.UserFields.Fields.Item("Status").Value = "Status";
                                        PSFTable.UserFields.Fields.Item("U_SOnum").Value = "U_SOnum";
                                        PSFTable.UserFields.Fields.Item("U_CutomerCode").Value = "U_CutomerCode";
                                        PSFTable.UserFields.Fields.Item("U_SODate").Value = "U_SODate";
                                        PSFTable.UserFields.Fields.Item("U_ShipDate").Value = "U_ShipDate";
                                        PSFTable.UserFields.Fields.Item("U_ItemCode").Value = "U_ItemCode";
                                        PSFTable.UserFields.Fields.Item("U_ItemDes").Value = "U_ItemDes";
                                        PSFTable.UserFields.Fields.Item("U_Qty").Value = "U_Qty";
                                        PSFTable.UserFields.Fields.Item("U_UOM").Value = "U_UOMPSFTable";
                                        #region Updating Table Row as Posted
                                        string updatePSF = @"Update dbo.[@PSF] set isPosted = 1 where DocEntry =" + docEntry;    //For Updating master table row as this data is posted to SAP
                                        int res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, updatePSF).ToInt();
                                        if (res1 <= 0)
                                        {
                                            tran.Rollback();

                                            models.Message = "An Error occured";
                                            models.isSuccess = false;
                                            return models;
                                        }
                                        else
                                        {
                                            #endregion
                                            
                                            int rc = PSFTable.Add();
                                            if (rc != 0)
                                            {
                                                models.Message = oCompany.GetLastErrorDescription();
                                                models.isSuccess = false;

                                                return models;
                                            }
                                        
                                        #endregion
                                            #region Insert in PSF2
                                        else
                                        {
                                            string rowQuery = @"select DocEntry,LineId,U_PlanDate,U_PreCosPln,U_PreCosAct,U_POPln,U_POAct,
                                                    U_AudApp,U_AudAppAct,U_YarnPur,U_YarPurAct,U_YarDel,U_YarDelAct,U_YarIssSiz,U_YarIssSizAct
                                                    ,U_SizYarRec,U_SizYarRecAct,U_SizYarIssGre,U_SizYarIssGreAct,U_GreRec,U_GreRecAct,U_GreIssDye
                                                    ,U_GreIssDyeAct,U_DyeRec,U_DyeRecAct,U_DyeIssProd,U_DyeIssProdAct,U_PackPln,U_PackAct
                                                    ,U_DelPln,U_DelAct,U_GatePass,U_GatePassAct from dbo.[@PSF2] where DocEntry=" + docEntry;

                                            using (var rdr2 = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, rowQuery))
                                            {
                                                try
                                                {
                                                    while (rdr2.Read())
                                                    {
                                                        UserTable PSF2Table = oCompany.UserTables.Item("@PSF2");
                                                        if (PSF2Table != null)
                                                        {
                                                            PSF2Table.UserFields.Fields.Item("DocEntry").Value = "DocEntry";
                                                            PSF2Table.UserFields.Fields.Item("LineId").Value = "LineId";
                                                            PSF2Table.UserFields.Fields.Item("U_PlanDate").Value = "U_PlanDate";
                                                            PSF2Table.UserFields.Fields.Item("U_PreCosPln").Value = "U_PreCosPln";
                                                            PSF2Table.UserFields.Fields.Item("U_PreCosAct").Value = "U_PreCosAct";
                                                            PSF2Table.UserFields.Fields.Item("U_POPln").Value = "U_POPln";
                                                            PSF2Table.UserFields.Fields.Item("U_POAct").Value = "U_POAct";
                                                            PSF2Table.UserFields.Fields.Item("U_AudApp").Value = "U_AudApp";
                                                            PSF2Table.UserFields.Fields.Item("U_AudAppAct").Value = "U_AudAppAct";
                                                            PSF2Table.UserFields.Fields.Item("U_YarnPur").Value = "U_YarnPur";
                                                            PSF2Table.UserFields.Fields.Item("U_YarPurAct").Value = "U_YarPurAct";
                                                            PSF2Table.UserFields.Fields.Item("U_YarDel").Value = "U_YarDel";
                                                            PSF2Table.UserFields.Fields.Item("U_YarDelAct").Value = "U_YarDelAct";
                                                            PSF2Table.UserFields.Fields.Item("U_YarIssSiz").Value = "U_YarIssSiz";
                                                            PSF2Table.UserFields.Fields.Item("U_YarIssSizAct").Value = "U_YarIssSizAct";
                                                            PSF2Table.UserFields.Fields.Item("U_SizYarRec").Value = "U_SizYarRec";
                                                            PSF2Table.UserFields.Fields.Item("U_SizYarRecAct").Value = "U_SizYarRecAct";
                                                            PSF2Table.UserFields.Fields.Item("U_SizYarIssGre").Value = "U_SizYarIssGre";
                                                            PSF2Table.UserFields.Fields.Item("U_SizYarIssGreAct").Value = "U_SizYarIssGreAct";
                                                            PSF2Table.UserFields.Fields.Item("U_GreRec").Value = "U_GreRec";
                                                            PSF2Table.UserFields.Fields.Item("U_GreRecAct").Value = "U_GreRecAct";
                                                            PSF2Table.UserFields.Fields.Item("U_GreIssDye").Value = "U_GreIssDye";
                                                            PSF2Table.UserFields.Fields.Item("U_GreIssDyeAct").Value = "U_GreIssDyeAct";
                                                            PSF2Table.UserFields.Fields.Item("U_DyeRec").Value = "U_DyeRec";
                                                            PSF2Table.UserFields.Fields.Item("U_DyeRecAct").Value = "U_DyeRecAct";
                                                            PSF2Table.UserFields.Fields.Item("U_DyeIssProd").Value = "U_DyeIssProd";
                                                            PSF2Table.UserFields.Fields.Item("U_DyeIssProdAct").Value = "U_DyeIssProdAct";
                                                            PSF2Table.UserFields.Fields.Item("U_PackPln").Value = "U_PackPln";
                                                            PSF2Table.UserFields.Fields.Item("U_PackAct").Value = "U_PackAct";
                                                            PSF2Table.UserFields.Fields.Item("U_DelPln").Value = "U_DelPln";
                                                            PSF2Table.UserFields.Fields.Item("U_DelAct").Value = "U_DelAct";
                                                            PSF2Table.UserFields.Fields.Item("U_GatePass").Value = "U_GatePass";
                                                            PSF2Table.UserFields.Fields.Item("U_GatePassAct").Value = "U_GatePassAct";
                                                            int rc2 = PSF2Table.Add();
                                                            if (rc2 != 0)
                                                            {
                                                                models.Message = oCompany.GetLastErrorDescription();
                                                                models.isSuccess = false;

                                                                return models;
                                                            }



                                                        }
                                                    }
                                                }

                                                catch (Exception ex)
                                                {
                                                    models.Message = "An Error Occured";
                                                    models.isSuccess = false;
                                                    return models;

                                                }
                                            }
                                        }
                                            #endregion
                                        }
                                    }
                                    else
                                    {
                                        models.Message = "Table Not Found !";
                                        models.isSuccess = false;
                                        return models;
                                    }
                                }



                            }
                            catch (Exception ex)
                            {
                                models.Message = ex.Message;
                                models.isSuccess = false;
                                return models;

                            }
                        }
                    }
                    models.Message = "Posted Data Succesfully !";
                    models.isSuccess = true;
                    return models;
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
