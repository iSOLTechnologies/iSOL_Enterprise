﻿using iSOL_Enterprise.Common;
using iSOL_Enterprise.Dal;
using iSOL_Enterprise.Models;
using iSOL_Enterprise.Models.sale;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SAPbobsCOM;
using SqlHelperExtensions;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;

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
                            bool isOld = false;
                            if (ObjectCode == 23 || ObjectCode == 17 || ObjectCode == 15 || ObjectCode == 16 || ObjectCode == 13 || ObjectCode == 14)
                            {
                                UDF = ",PurchaseType,TypeDetail,ProductionOrderNo,ChallanNo,ContainerNo,ManualGatePassNo,SaleOrderNo";
                            }
                            else if (ObjectCode == 540000006 || ObjectCode == 22 || ObjectCode == 20 || ObjectCode == 21 || ObjectCode == 18 || ObjectCode == 19)
                            {
                                UDF = ",PurchaseType,TypeDetail,ProductionOrderNo,ChallanNo,DONo,SaleOrderNo";
                            }
                            
                            string headerQuery = @"select DocType,Series,CardCode,CardName,CntctCode,DocDate,NumAtCard,DocDueDate,DocCur,TaxDate ,GroupNum,DocTotal ,
                                                   SlpCode,DiscPrcnt,Comments,Id,Sap_Ref_No " + UDF + " from " + headerTable + " where Id=" + ID + " and isPosted = 0";
                            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, headerQuery))
                            {
                                try
                                {


                                    while (rdr.Read())
                                    {
                                        #region Insert In Header
                                        isOld = oDoc.GetByKey(rdr["Sap_Ref_No"].ToInt());

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
                                        oDoc.DocTotal = rdr["DocTotal"].ToDouble();
                                        oDoc.SalesPersonCode = rdr["SlpCode"].ToInt();
                                        if (rdr["DiscPrcnt"].ToString() != "")
                                        {
                                            oDoc.DiscountPercent = rdr["DiscPrcnt"].ToDouble();

                                        }
                                        oDoc.Comments = rdr["Comments"].ToString();

                                        
                                            #region UDFs
                                                oDoc.UserFields.Fields.Item("U_WBS_DocNum").Value = ID;
                                        
                                            if (rdr["SaleOrderNo"].ToString() != "")
                                                oDoc.UserFields.Fields.Item("U_Old_SO").Value = Convert.ToInt32(rdr["SaleOrderNo"]);
                                            if (rdr["ProductionOrderNo"].ToString() != "")
                                                oDoc.UserFields.Fields.Item("U_Production_ordr").Value = Convert.ToInt32(rdr["ProductionOrderNo"]);
                                            if (rdr["PurchaseType"].ToString() != "")
                                                oDoc.UserFields.Fields.Item("U_Type").Value = Convert.ToInt16(rdr["PurchaseType"]);
                                            oDoc.UserFields.Fields.Item("U_Challan_no").Value = rdr["ChallanNo"].ToString();

                                            if (ObjectCode == 23 || ObjectCode == 17 || ObjectCode == 15 || ObjectCode == 16 || ObjectCode == 13 || ObjectCode == 14)
                                            {
                                                if (rdr["ManualGatePassNo"].ToString() != "")
                                                    oDoc.UserFields.Fields.Item("U_manual_cgp").Value = Convert.ToInt32(rdr["ManualGatePassNo"]);

                                                oDoc.UserFields.Fields.Item("U_Cont_no").Value = rdr["ContainerNo"].ToString();

                                            }
                                            else if (ObjectCode == 540000006 || ObjectCode == 22 || ObjectCode == 20 || ObjectCode == 21 || ObjectCode == 18 || ObjectCode == 19)
                                            {                                            
                                                    oDoc.UserFields.Fields.Item("U_DO_Num").Value = rdr["DONo"].ToString();
                                            }


                                        if (rdr["TypeDetail"].ToString() != "")
                                        {
                                            if (rdr["TypeDetail"].ToInt() >= 1 || rdr["TypeDetail"].ToInt() <= 9)                                        
                                                oDoc.UserFields.Fields.Item("U_Type_d").Value = "0" + Math.Round(rdr["TypeDetail"].ToDouble()) + "";
                                            else
                                                oDoc.UserFields.Fields.Item("U_Type_d").Value =   rdr["TypeDetail"].ToString();
                                            #endregion
                                        }

                                        #endregion

                                        #region Insert in Row
                                        string RowQuery = @"select BaseEntry,BaseLine,BaseType,Price,LineTotal,ItemCode,Quantity,DiscPrcnt,VatGroup,UomEntry ,CountryOrg,ItemName , Dscription,AcctCode,LineNum,WhsCode from " + rowTable + " where Id = " + ID;
                                        using (var rdr2 = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, RowQuery))
                                        {
                                            try
                                            {


                                                while (rdr2.Read())
                                                {
                                                    string BaseTable = "";
                                                    int BaseEntry;

                                                    if (rdr2["BaseType"].ToString() != "" && rdr2["BaseType"].ToString() != "-1")
                                                    {
                                                        BaseTable = dal.GetMasterTable(rdr2["BaseType"].ToInt());
                                                        string GetBaseEntryQuery = "select Sap_Ref_No from " + BaseTable + " where Id =" + rdr2["BaseEntry"].ToInt();
                                                        BaseEntry = Convert.ToInt32(SqlHelper.ExecuteScalar(tran, CommandType.Text, GetBaseEntryQuery));

                                                        oDoc.Lines.BaseEntry = BaseEntry;

                                                        oDoc.Lines.BaseLine = rdr2["BaseLine"].ToInt();

                                                        oDoc.Lines.BaseType = rdr2["BaseType"].ToInt();

                                                        oDoc.Lines.UserFields.Fields.Item("U_WSB_BaseRef").Value = ID;
                                                    }
                                                    oDoc.Lines.Quantity = Convert.ToDouble(rdr2["Quantity"]);
                                                    if (rdr2["Price"].ToString() != "")
                                                    {
                                                        oDoc.Lines.Price = rdr2["Price"].ToDouble();
                                                        oDoc.Lines.DiscountPercent = rdr2["DiscPrcnt"].ToDouble();
                                                        oDoc.Lines.VatGroup = rdr2["VatGroup"].ToString();
                                                    }
                                                    if (rdr2["LineTotal"].ToString() != "")
                                                    {
                                                        oDoc.Lines.LineTotal = rdr2["LineTotal"].ToDouble();
                                                        //oDoc.Lines.PriceAfterVAT = rdr2["LineTotal"].ToDouble();
                                                    }
                                                    oDoc.Lines.ItemCode = rdr2["ItemCode"].ToString();
                                                    //oDoc.Lines.Quantity = Convert.ToDouble(10);// rdr2["Quantity"].ToDouble();
                                                    oDoc.Lines.UoMEntry = rdr2["UomEntry"].ToInt();
                                                    oDoc.Lines.CountryOrg = rdr2["CountryOrg"].ToString();
                                                    oDoc.Lines.ItemDescription = rdr2["ItemName"].ToString();
                                                    //oDoc.Lines.AccountCode = rdr2["AcctCode"].ToString();
                                                    oDoc.Lines.WarehouseCode = rdr2["WhsCode"].ToString();
                                                    if (rowTable == "DLN1")
                                                    {

                                                        string BatchQuery = @" select ITL1.ItemCode,ITL1.SysNumber,ITL1.Quantity,ITL1.AllocQty,OITL.CreateDate, OBTN.ExpDate,OBTN.DistNumber from OITL 
                                                                           inner join ITL1 on OITL.LogEntry = ITL1.LogEntry 
                                                                           inner join OBTQ on ITL1.MdAbsEntry = OBTQ.MdAbsEntry 
                                                                           inner join OBTN on OBTQ.MdAbsEntry = OBTN.AbsEntry
                                                                           where DocLine = '" + rdr2["LineNum"].ToString() + "' and DocNum = '" + rdr["Id"].ToString() + "' and DocType =15";
                                                        using (var rdr3 = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, BatchQuery))
                                                        {
                                                            int i = 0;
                                                            while (rdr3.Read())
                                                            {


                                                                oDoc.Lines.BatchNumbers.BaseLineNumber = oDoc.Lines.LineNum;
                                                                oDoc.Lines.BatchNumbers.SetCurrentLine(i);
                                                                oDoc.Lines.BatchNumbers.ItemCode = rdr3["ItemCode"].ToString();
                                                                oDoc.Lines.BatchNumbers.BatchNumber = rdr3["DistNumber"].ToString();
                                                                oDoc.Lines.BatchNumbers.Quantity = Convert.ToDouble(Convert.ToDouble(rdr3["Quantity"]) > 0 ? Convert.ToDouble(rdr3["Quantity"]) : (-1 * Convert.ToDouble(rdr3["Quantity"])));
                                                                oDoc.Lines.BatchNumbers.AddmisionDate = rdr3["CreateDate"].ToString() == "" ? DateTime.Now : Convert.ToDateTime(rdr3["CreateDate"].ToString());
                                                                if (rdr3["ExpDate"].ToString() != "")
                                                                    oDoc.Lines.BatchNumbers.ExpiryDate = Convert.ToDateTime(rdr3["ExpDate"].ToString());
                                                                oDoc.Lines.BatchNumbers.Add();
                                                                i += 1;
                                                            }
                                                        }

                                                    }

                                                    if (rowTable == "PDN1")
                                                    {

                                                        try
                                                        {


                                                            string BatchQuery = @" select ITL1.ItemCode,ITL1.SysNumber,ITL1.Quantity,ITL1.AllocQty,OITL.CreateDate, OBTN.ExpDate,OBTN.DistNumber from OITL 
                                                                           inner join ITL1 on OITL.LogEntry = ITL1.LogEntry 
                                                                           inner join OBTQ on ITL1.MdAbsEntry = OBTQ.MdAbsEntry 
                                                                           inner join OBTN on OBTQ.MdAbsEntry = OBTN.AbsEntry
                                                                           where DocLine = '" + rdr2["LineNum"].ToString() + "' and DocNum = '" + rdr["Id"].ToString() + "' and DocType =20";
                                                            using (var rdr3 = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, BatchQuery))
                                                            {
                                                                int i = 0;
                                                                while (rdr3.Read())
                                                                {
                                                                    oDoc.Lines.BatchNumbers.BaseLineNumber = oDoc.Lines.LineNum;
                                                                    oDoc.Lines.BatchNumbers.SetCurrentLine(i);
                                                                    oDoc.Lines.BatchNumbers.ItemCode = rdr3["ItemCode"].ToString();
                                                                    oDoc.Lines.BatchNumbers.BatchNumber = rdr3["DistNumber"].ToString();

                                                                    oDoc.Lines.BatchNumbers.Quantity = Convert.ToDouble(rdr3["Quantity"]) > 0 ? Convert.ToDouble(rdr3["Quantity"]) : (-1 * Convert.ToDouble(rdr3["Quantity"]));
                                                                    oDoc.Lines.BatchNumbers.AddmisionDate = rdr3["CreateDate"].ToString() == "" ? DateTime.Now : Convert.ToDateTime(rdr3["CreateDate"].ToString());
                                                                    if (rdr3["ExpDate"].ToString() != "")
                                                                        oDoc.Lines.BatchNumbers.ExpiryDate = Convert.ToDateTime(rdr3["ExpDate"].ToString());
                                                                    oDoc.Lines.BatchNumbers.Add();
                                                                    i += 1;
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

                            #region Posting data to SAP
                            int res = -1;
                            if (!isOld)
                                res = oDoc.Add();
                            else
                                res = oDoc.Update();

                            if (res < 0)
                            {

                                oCompany.GetLastError(out res, out message);
                                models.Message = message;
                                models.isSuccess = false;
                                tran.Rollback();
                                return models;
                            }
                            else
                            {

                                string getWBSDocNum = @"select DocEntry from " + headerTable + " where U_WBS_DocNum =" + ID;
                                tbl_docRow docRowModel = new tbl_docRow();
                                using (var rdr3 = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, getWBSDocNum))
                                {
                                    while (rdr3.Read())
                                    {
                                        docRowModel.DocEntry = rdr3["DocEntry"].ToInt();

                                    }
                                }
                                
                                if (docRowModel.DocEntry == null || docRowModel.DocEntry.ToString() == "")
                                {
                                    string getWBSDocNumApprvl = @"select DocEntry from ODRF where U_WBS_DocNum =" + ID + " and ElCoStatus=1 and ObjType="+ObjectCode;
                                    docRowModel.DocEntry = SqlHelper.ExecuteScalar(tran,CommandType.Text,getWBSDocNumApprvl).ToInt();
                                }
                                if (docRowModel.DocEntry !=null)
                                {

                                #region Updating Table Row as Posted , Add Sap Base Entry
                                    
                                    string UpdateHeaderTable = @"Update " + headerTable + " set isPosted = 1,Sap_Ref_No = " + docRowModel.DocEntry + ",is_Edited = 0  where Id =" + ID;    //For Updating master table row as this data is posted to SAP
                                    int res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, UpdateHeaderTable).ToInt();
                                    if (res1 <= 0)
                                    {
                                        tran.Rollback();

                                        models.Message = "Document Posted but Error Occured while updating Documnet !";
                                        models.isSuccess = true;
                                        return models;
                                    }

                                    string UpdateRowTable = @"Update " + rowTable + " set Sap_Ref_No = " + docRowModel.DocEntry + " where Id =" + ID;
                                    res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, UpdateRowTable).ToInt();
                                    if (res1 <= 0)
                                    {
                                        tran.Rollback();

                                        models.Message = "Document Posted but Error Occured while updating Document !";
                                        models.isSuccess = true;
                                        return models;
                                    }
                                    #endregion
                                }
                                else
                                {
                                    tran.Rollback();

                                    models.Message = "Document Posted but Error Occured while updating Document !";
                                    models.isSuccess = true;
                                    return models;
                                }
                                tran.Commit();


                            }
                            #endregion

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

        public ResponseModels PostItemMasterData(string[] checkedIDs)
        {
            ResponseModels models = new ResponseModels();
            try
            {
                if (Connect())
                {

                    SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
                    conn.Open();
                    SqlTransaction tran = conn.BeginTransaction();
                    foreach (var ID in checkedIDs)
                    {
                        bool isOld = false;
                        SAPbobsCOM.Items item = (SAPbobsCOM.Items)oCompany.GetBusinessObject(BoObjectTypes.oItems);

                        string headerQuery = @"select Id,Guid,ItemCode, ItemName, Series, InvntItem, SellItem,PrchseItem, FrgnName , ItemType, ItmsGrpCod, UgpEntry, AvgPrice, WTLiable, FirmCode, ShipType,
                                            MngMethod, validFor, validFrom, validTo, frozenFrom, frozenTo,ManBtchNum, ByWh,EvalSystem,GLMethod,InvntryUom, PrcrmntMtd, PlaningSys, MinOrdrQty, InCostRoll, IssueMthd, 
                                            TreeType, PrdStdCst,BuyUnitMsr,CstGrpCode,NumInBuy,VatGroupPu,NumInSale,SalUnitMsr,VatGourpSa, QryGroup1, QryGroup2, QryGroup3, QryGroup4, QryGroup5, QryGroup6,
                                            QryGroup7,QryGroup8, QryGroup9, QryGroup10, QryGroup11, QryGroup12, QryGroup13, QryGroup14, QryGroup15, QryGroup16, QryGroup17, QryGroup18, QryGroup19, QryGroup20,
                                            QryGroup21, QryGroup22, QryGroup23, QryGroup24, QryGroup25, QryGroup26, QryGroup27, QryGroup28, QryGroup29, QryGroup30, QryGroup31, QryGroup32, QryGroup33,
                                            QryGroup34, QryGroup35, QryGroup36, QryGroup37, QryGroup38, QryGroup39, QryGroup40, QryGroup41, QryGroup42, QryGroup43, QryGroup44, QryGroup45, QryGroup46, 
                                            QryGroup47, QryGroup48, QryGroup49, QryGroup50, QryGroup51, QryGroup52, QryGroup53, QryGroup54, QryGroup55, QryGroup56, QryGroup57, QryGroup58, QryGroup59, 
                                            QryGroup60, QryGroup61, QryGroup62, QryGroup63, QryGroup64,Sap_ItemCode from OITM where Id =" + ID;

                        using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, headerQuery))
                        {
                            while (rdr.Read())
                            {
                                isOld = item.GetByKey(rdr["Sap_ItemCode"].ToString());
                                
                                item.ItemName = rdr["ItemName"].ToString();
                                item.Series = rdr["Series"].ToInt();
                                item.InventoryItem = rdr["InvntItem"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.SalesItem = rdr["SellItem"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.PurchaseItem = rdr["PrchseItem"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.ForeignName = rdr["FrgnName"].ToString();
                                item.ItemType = rdr["ItemType"].ToString() == "I" ? ItemTypeEnum.itItems : rdr["ItemType"].ToString() == "L" ? ItemTypeEnum.itLabor : rdr["ItemType"].ToString() == "T" ? ItemTypeEnum.itTravel : ItemTypeEnum.itFixedAssets;
                                item.ItemsGroupCode = rdr["ItmsGrpCod"].ToInt();
                                item.UoMGroupEntry = rdr["UgpEntry"].ToInt();
                                item.AvgStdPrice = rdr["AvgPrice"].ToDouble();
                                item.WTLiable = rdr["WTLiable"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.Manufacturer = rdr["FirmCode"].ToInt();
                                item.ShipType = rdr["ShipType"].ToInt();

                                if (rdr["ManBtchNum"].ToString() == "Y")
                                {
                                    item.ManageBatchNumbers = BoYesNoEnum.tYES;
                                    item.SRIAndBatchManageMethod = rdr["MngMethod"].ToString() == "R" ? BoManageMethod.bomm_OnReleaseOnly : BoManageMethod.bomm_OnEveryTransaction;
                                }
                                else
                                {
                                    item.ManageSerialNumbers = BoYesNoEnum.tYES;
                                    item.ManageSerialNumbersOnReleaseOnly = rdr["MngMethod"].ToString() == "R" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                }

                                if (rdr["validFor"].ToString() == "Y")
                                {
                                    item.ValidFrom = Convert.ToDateTime(rdr["validFrom"]);
                                    item.ValidTo = Convert.ToDateTime(rdr["validTo"]);
                                    item.Valid = BoYesNoEnum.tYES;
                                }
                                else
                                {
                                    item.Frozen = BoYesNoEnum.tYES;
                                    item.FrozenFrom = Convert.ToDateTime(rdr["frozenFrom"]);
                                    item.FrozenTo = Convert.ToDateTime(rdr["frozenTo"]);
                                }

                                item.ManageStockByWarehouse = rdr["ByWh"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.GLMethod = rdr["GLMethod"].ToString() == "W" ? BoGLMethods.glm_WH : rdr["GLMethod"].ToString() == "C" ? BoGLMethods.glm_ItemClass : BoGLMethods.glm_ItemLevel;
                                //item.Va = BoInventorySystem.bis_FIFO;
                                item.InventoryUOM = rdr["InvntryUom"].ToString();
                                item.ProcurementMethod = rdr["PrcrmntMtd"].ToString() == "B" ? BoProcurementMethod.bom_Buy : BoProcurementMethod.bom_Make;
                                item.PlanningSystem = rdr["PlaningSys"].ToString() == "M" ? BoPlanningSystem.bop_MRP : BoPlanningSystem.bop_None;
                                item.MinOrderQuantity = rdr["MinOrdrQty"].ToDouble();
                                item.InCostRollup = rdr["InCostRoll"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.IssueMethod = rdr["IssueMthd"].ToString() == "B" ? BoIssueMethod.im_Backflush : BoIssueMethod.im_Manual;

                                item.ProdStdCost = rdr["PrdStdCst"].ToDouble();
                                item.DefaultPurchasingUoMEntry = rdr["BuyUnitMsr"].ToInt();

                                item.CustomsGroupCode = rdr["CstGrpCode"].ToInt();
                                item.PurchaseItemsPerUnit = rdr["NumInBuy"].ToInt();
                                item.PurchaseVATGroup = rdr["VatGroupPu"].ToString();

                                item.SalesItemsPerUnit = rdr["NumInSale"].ToInt();
                                item.DefaultSalesUoMEntry = rdr["SalUnitMsr"].ToInt();
                                item.SalesVATGroup = rdr["VatGourpSa"].ToString();
                                #region Properties
                                item.Properties[1] = rdr["QryGroup1"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.Properties[2] = rdr["QryGroup2"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.Properties[3] = rdr["QryGroup3"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.Properties[4] = rdr["QryGroup4"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.Properties[5] = rdr["QryGroup5"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.Properties[6] = rdr["QryGroup6"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.Properties[7] = rdr["QryGroup7"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.Properties[8] = rdr["QryGroup8"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.Properties[9] = rdr["QryGroup9"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.Properties[10] = rdr["QryGroup10"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.Properties[11] = rdr["QryGroup11"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.Properties[12] = rdr["QryGroup12"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.Properties[13] = rdr["QryGroup13"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.Properties[14] = rdr["QryGroup14"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.Properties[15] = rdr["QryGroup15"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.Properties[16] = rdr["QryGroup16"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.Properties[17] = rdr["QryGroup17"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.Properties[18] = rdr["QryGroup18"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.Properties[19] = rdr["QryGroup19"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.Properties[20] = rdr["QryGroup20"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.Properties[21] = rdr["QryGroup21"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.Properties[22] = rdr["QryGroup22"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.Properties[23] = rdr["QryGroup23"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.Properties[24] = rdr["QryGroup24"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.Properties[25] = rdr["QryGroup25"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.Properties[26] = rdr["QryGroup26"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.Properties[27] = rdr["QryGroup27"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.Properties[28] = rdr["QryGroup28"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.Properties[29] = rdr["QryGroup29"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.Properties[30] = rdr["QryGroup30"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.Properties[31] = rdr["QryGroup31"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.Properties[32] = rdr["QryGroup32"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.Properties[33] = rdr["QryGroup33"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.Properties[34] = rdr["QryGroup34"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.Properties[35] = rdr["QryGroup35"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.Properties[36] = rdr["QryGroup36"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.Properties[37] = rdr["QryGroup37"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.Properties[38] = rdr["QryGroup38"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.Properties[39] = rdr["QryGroup39"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.Properties[40] = rdr["QryGroup40"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.Properties[41] = rdr["QryGroup41"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.Properties[42] = rdr["QryGroup42"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.Properties[43] = rdr["QryGroup43"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.Properties[44] = rdr["QryGroup44"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.Properties[45] = rdr["QryGroup45"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.Properties[46] = rdr["QryGroup46"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.Properties[47] = rdr["QryGroup47"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.Properties[48] = rdr["QryGroup48"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.Properties[49] = rdr["QryGroup49"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.Properties[50] = rdr["QryGroup50"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.Properties[51] = rdr["QryGroup51"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.Properties[52] = rdr["QryGroup52"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.Properties[53] = rdr["QryGroup53"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.Properties[54] = rdr["QryGroup54"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.Properties[55] = rdr["QryGroup55"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.Properties[56] = rdr["QryGroup56"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.Properties[57] = rdr["QryGroup57"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.Properties[58] = rdr["QryGroup58"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.Properties[59] = rdr["QryGroup59"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.Properties[60] = rdr["QryGroup60"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.Properties[61] = rdr["QryGroup61"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.Properties[62] = rdr["QryGroup62"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.Properties[63] = rdr["QryGroup63"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                item.Properties[64] = rdr["QryGroup64"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;

                                #endregion
                                
                                item.UserFields.Fields.Item("U_WBS_ItemNo").Value = ID;
                                //Set warehouse management properties

                                if (rdr["ByWh"].ToString() == "Y")
                                {


                                    //Add warehouse information to the WhsInfo property for each warehouse code
                                    foreach (var warehouse in CommonDal.GetWareHouseList(rdr["ItemCode"].ToString()))
                                    {

                                        item.WhsInfo.WarehouseCode = warehouse.whscode;

                                        item.WhsInfo.MinimalStock = warehouse.MinStock;
                                        item.WhsInfo.MaximalStock = warehouse.MaxStock;
                                        item.WhsInfo.MinimalOrder = warehouse.MinOrder;
                                        item.WhsInfo.Locked = warehouse.Locked == 'Y' ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                        item.WhsInfo.Add();
                                    }


                                }
                            }

                        }
                        int res = -1;
                        if (!isOld)
                            res = item.Add();
                        else
                            res = item.Update();                        

                        if (res < 0)
                        {


                            models.Message = oCompany.GetLastErrorDescription();
                            models.isSuccess = false;

                            return models;
                        }
                        else
                        {
                            string getWBSDocNum = @"select ItemCode from OITM where WBS_ItemNo =" + ID;
                            tbl_OITM itemModel = new tbl_OITM();
                            using (var rdr3 = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, getWBSDocNum))
                            {
                                while (rdr3.Read())
                                {
                                    itemModel.ItemCode = rdr3["ItemCode"].ToString();

                                }
                            }
                            if (itemModel.ItemCode != null)
                            {
                                #region Updating Table Row as Posted , Add Sap Base Entry
                                string UpdateHeaderTable = @"Update OITM set isPosted = 1,Sap_ItemCode = '" + itemModel.ItemCode + "',is_Edited = 0  where Id =" + ID;    //For Updating master table row as this data is posted to SAP
                                int res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, UpdateHeaderTable).ToInt();
                                if (res1 <= 0)
                                {
                                    tran.Rollback();

                                    models.Message = "Document Posted but Error Occured while updating Documnet !";
                                    models.isSuccess = true;
                                    return models;
                                }
                            #endregion
                            }
                            else
                            {
                                tran.Rollback();

                                models.Message = "Document Posted but Error Occured while updating Documnet !";
                                models.isSuccess = true;
                                return models;
                            }
                            tran.Commit();
                            models.Message = "Item Added Successfully !";
                            models.isSuccess = true;

                            return models;
                        }


                    }

                    models.Message = "No records found !";
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

        public ResponseModels PostGoodReceiptGR(string[] checkedIDs, int ObjectCode)
        {
            ResponseModels models = new ResponseModels();
            try
            {
                if (Connect())
                {
                    CommonDal dal = new CommonDal();

                    SAPbobsCOM.Documents oDoc = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(BoObjectTypes.oInventoryGenEntry);
                    if (oDoc != null)
                    {
                        string headerTable = dal.GetMasterTable(ObjectCode);
                        string rowTable = dal.GetRowTable(ObjectCode);
                        string message = "";

                        SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
                        conn.Open();
                        SqlTransaction tran = conn.BeginTransaction();



                        foreach (var ID in checkedIDs)
                        {
                            string UDF = "";
                            bool isOld = false;
                           
                            string headerQuery = @"select Id,Guid,MySeries,DocNum,Series,DocDate,GroupNum,TaxDate,DocTotal,Ref2,Comments,JrnlMemo,Sap_Ref_No " + UDF + " from " + headerTable + " where Id=" + ID + " and isPosted = 0";
                            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, headerQuery))
                            {
                                try
                                {


                                    while (rdr.Read())
                                    {
                                        #region Insert In Header
                                        isOld = oDoc.GetByKey(rdr["Sap_Ref_No"].ToInt());

                                        oDoc.Series = rdr["Series"].ToInt();
                                        
                                        oDoc.DocDate = rdr["DocDate"].ToString() == "" ? DateTime.Now : Convert.ToDateTime(rdr["DocDate"].ToString());                                        
                                        oDoc.GroupNumber = rdr["GroupNum"].ToInt();
                                        oDoc.TaxDate = rdr["TaxDate"].ToString() == "" ? DateTime.Now : Convert.ToDateTime(rdr["TaxDate"].ToString());
                                        //oDoc.DocTotal = rdr["DocTotal"].ToDouble();
                                        oDoc.Reference2 = rdr["Ref2"].ToString();
                                        oDoc.Comments = rdr["Comments"].ToString();
                                        oDoc.JournalMemo = rdr["JrnlMemo"].ToString();
                                        oDoc.UserFields.Fields.Item("U_WBS_DocNum").Value = ID;
                                       
                                        #endregion

                                        #region Insert in Row
                                        string RowQuery = @"select Id,LineNum,ItemCode,Dscription,WhsCode,Quantity,Price,LineTotal,AcctCode,UomEntry from " + rowTable + " where Id = " + ID;
                                        using (var rdr2 = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, RowQuery))
                                        {
                                            try
                                            {


                                                while (rdr2.Read())
                                                {
                                                       
                                                    //oDoc.Lines.UserFields.Fields.Item("U_WSB_BaseRef").Value = ID;                                                    
                                                    oDoc.Lines.ItemCode = rdr2["ItemCode"].ToString();
                                                    oDoc.Lines.ItemDescription = rdr2["Dscription"].ToString();
                                                    oDoc.Lines.WarehouseCode = rdr2["WhsCode"].ToString();
                                                    oDoc.Lines.Quantity = Convert.ToDouble(rdr2["Quantity"]);
                                                    if (rdr2["Price"].ToString() != "")
                                                        oDoc.Lines.Price = rdr2["Price"].ToDouble();
                                                    if (rdr2["LineTotal"].ToString() != "")
                                                        oDoc.Lines.LineTotal = rdr2["LineTotal"].ToDouble();
                                                    //oDoc.Lines.AccountCode = rdr2["AcctCode"].ToString();
                                                    oDoc.Lines.UoMEntry = rdr2["UomEntry"].ToInt();
                                                   
                                                    

                                                    try
                                                    {


                                                        string BatchQuery = @" select ITL1.ItemCode,ITL1.SysNumber,ITL1.Quantity,ITL1.AllocQty,OITL.CreateDate, OBTN.ExpDate,OBTN.DistNumber from OITL 
                                                                           inner join ITL1 on OITL.LogEntry = ITL1.LogEntry 
                                                                           inner join OBTQ on ITL1.MdAbsEntry = OBTQ.MdAbsEntry 
                                                                           inner join OBTN on OBTQ.MdAbsEntry = OBTN.AbsEntry
                                                                           where DocLine = '" + rdr2["LineNum"].ToString() + "' and DocNum = '" + rdr["Id"].ToString() + "' and DocType ="+ObjectCode;
                                                        using (var rdr3 = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, BatchQuery))
                                                        {
                                                            int i = 0;
                                                            while (rdr3.Read())
                                                            {
                                                                oDoc.Lines.BatchNumbers.BaseLineNumber = oDoc.Lines.LineNum;
                                                                oDoc.Lines.BatchNumbers.SetCurrentLine(i);
                                                                oDoc.Lines.BatchNumbers.ItemCode = rdr3["ItemCode"].ToString();
                                                                oDoc.Lines.BatchNumbers.BatchNumber = rdr3["DistNumber"].ToString();
                                                                oDoc.Lines.BatchNumbers.Quantity = Convert.ToDouble(rdr3["Quantity"]) > 0 ? Convert.ToDouble(rdr3["Quantity"]) : (-1 * Convert.ToDouble(rdr3["Quantity"]));
                                                                oDoc.Lines.BatchNumbers.AddmisionDate = rdr3["CreateDate"].ToString() == "" ? DateTime.Now : Convert.ToDateTime(rdr3["CreateDate"].ToString());
                                                                if (rdr3["ExpDate"].ToString() != "")
                                                                    oDoc.Lines.BatchNumbers.ExpiryDate = Convert.ToDateTime(rdr3["ExpDate"].ToString());
                                                                oDoc.Lines.BatchNumbers.Add();
                                                                i += 1;
                                                            }
                                                        }
                                                    }
                                                    catch (Exception)
                                                    {

                                                        throw;
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

                            #region Posting data to SAP
                            int res = -1;
                            if (!isOld)
                                res = oDoc.Add();
                            else
                                res = oDoc.Update();

                            if (res < 0)
                            {

                                oCompany.GetLastError(out res, out message);
                                models.Message = message;
                                models.isSuccess = false;
                                tran.Rollback();
                                return models;
                            }
                            else
                            {

                                string getWBSDocNum = @"select DocEntry from " + headerTable + " where U_WBS_DocNum =" + ID;
                                tbl_docRow docRowModel = new tbl_docRow();
                                using (var rdr3 = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, getWBSDocNum))
                                {
                                    while (rdr3.Read())
                                    {
                                        docRowModel.DocEntry = rdr3["DocEntry"].ToInt();

                                    }
                                }
                                if (docRowModel.DocEntry == null || docRowModel.DocEntry.ToString() == "")
                                {
                                    string getWBSDocNumApprvl = @"select DocEntry from ODRF where U_WBS_DocNum =" + ID + " and ElCoStatus=1 and ObjType=" + ObjectCode;
                                    docRowModel.DocEntry = SqlHelper.ExecuteScalar(tran, CommandType.Text, getWBSDocNumApprvl).ToInt();
                                }
                                if (docRowModel.DocEntry != null)
                                {
                                    #region Updating Table Row as Posted , Add Sap Base Entry
                                    string UpdateHeaderTable = @"Update " + headerTable + " set isPosted = 1,Sap_Ref_No = " + docRowModel.DocEntry + ",is_Edited = 0  where Id =" + ID;    //For Updating master table row as this data is posted to SAP
                                    int res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, UpdateHeaderTable).ToInt();
                                    if (res1 <= 0)
                                    {
                                        tran.Rollback();

                                        models.Message = "Document Posted but Error Occured while updating Document !";
                                        models.isSuccess = true;
                                        return models;
                                    }

                                    string UpdateRowTable = @"Update " + rowTable + " set Sap_Ref_No = " + docRowModel.DocEntry + " where Id =" + ID;
                                    res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, UpdateRowTable).ToInt();
                                    if (res1 <= 0)
                                    {
                                        tran.Rollback();

                                        models.Message = "Document Posted but Error Occured while updating Document !";
                                        models.isSuccess = true;
                                        return models;
                                    }
                                    #endregion
                                }
                                else
                                {
                                    tran.Rollback();

                                    models.Message = "Document Posted but Error Occured while updating Document !";
                                    models.isSuccess = true;
                                    return models;
                                }
                                tran.Commit();


                            }
                            #endregion

                        }


                        models.Message = "Goods Receipt Posted Successfully !!";
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

        public ResponseModels PostGoodIssue(string[] checkedIDs, int ObjectCode)
        {
            ResponseModels models = new ResponseModels();
            try
            {
                if (Connect())
                {
                    CommonDal dal = new CommonDal();

                    SAPbobsCOM.Documents oDoc = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(BoObjectTypes.oInventoryGenExit);
                    if (oDoc != null)
                    {
                        string headerTable = dal.GetMasterTable(ObjectCode);
                        string rowTable = dal.GetRowTable(ObjectCode);
                        string message = "";

                        SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
                        conn.Open();
                        SqlTransaction tran = conn.BeginTransaction();



                        foreach (var ID in checkedIDs)
                        {
                            string UDF = "";
                            bool isOld = false;

                            string headerQuery = @"select Id,Guid,MySeries,DocNum,Series,DocDate,GroupNum,TaxDate,DocTotal,Ref2,Comments,JrnlMemo,Sap_Ref_No " + UDF + " from " + headerTable + " where Id=" + ID + " and isPosted = 0";
                            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, headerQuery))
                            {
                                try
                                {


                                    while (rdr.Read())
                                    {
                                        #region Insert In Header
                                        isOld = oDoc.GetByKey(rdr["Sap_Ref_No"].ToInt());

                                        oDoc.Series = rdr["Series"].ToInt();

                                        oDoc.DocDate = rdr["DocDate"].ToString() == "" ? DateTime.Now : Convert.ToDateTime(rdr["DocDate"].ToString());
                                        oDoc.GroupNumber = rdr["GroupNum"].ToInt();
                                        oDoc.TaxDate = rdr["TaxDate"].ToString() == "" ? DateTime.Now : Convert.ToDateTime(rdr["TaxDate"].ToString());
                                        //oDoc.DocTotal = rdr["DocTotal"].ToDouble();
                                        oDoc.Reference2 = rdr["Ref2"].ToString();
                                        oDoc.Comments = rdr["Comments"].ToString();
                                        oDoc.JournalMemo = rdr["JrnlMemo"].ToString();
                                        oDoc.UserFields.Fields.Item("U_WBS_DocNum").Value = ID;

                                        #endregion

                                        #region Insert in Row
                                        string RowQuery = @"select Id,LineNum,ItemCode,Dscription,WhsCode,Quantity,Price,LineTotal,AcctCode,UomEntry from " + rowTable + " where Id = " + ID;
                                        using (var rdr2 = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, RowQuery))
                                        {
                                            try
                                            {


                                                while (rdr2.Read())
                                                {

                                                   // oDoc.Lines.UserFields.Fields.Item("U_WSB_BaseRef").Value = ID;
                                                    oDoc.Lines.ItemCode = rdr2["ItemCode"].ToString();
                                                    oDoc.Lines.ItemDescription = rdr2["Dscription"].ToString();
                                                    oDoc.Lines.WarehouseCode = rdr2["WhsCode"].ToString();
                                                    oDoc.Lines.Quantity = Convert.ToDouble(rdr2["Quantity"]);
                                                    if (rdr2["Price"].ToString() != "")
                                                        oDoc.Lines.Price = rdr2["Price"].ToDouble();
                                                    if (rdr2["LineTotal"].ToString() != "")
                                                        oDoc.Lines.LineTotal = rdr2["LineTotal"].ToDouble();
                                                   // oDoc.Lines.AccountCode = rdr2["AcctCode"].ToString();
                                                    oDoc.Lines.UoMEntry = rdr2["UomEntry"].ToInt();



                                                    try
                                                    {


                                                        string BatchQuery = @" select ITL1.ItemCode,ITL1.SysNumber,ITL1.Quantity,ITL1.AllocQty,OITL.CreateDate, OBTN.ExpDate,OBTN.DistNumber from OITL 
                                                                           inner join ITL1 on OITL.LogEntry = ITL1.LogEntry 
                                                                           inner join OBTQ on ITL1.MdAbsEntry = OBTQ.MdAbsEntry 
                                                                           inner join OBTN on OBTQ.MdAbsEntry = OBTN.AbsEntry
                                                                           where DocLine = '" + rdr2["LineNum"].ToString() + "' and DocNum = '" + rdr["Id"].ToString() + "' and DocType ="+ObjectCode;
                                                        using (var rdr3 = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, BatchQuery))
                                                        {
                                                            int i = 0;
                                                            while (rdr3.Read())
                                                            {
                                                                oDoc.Lines.BatchNumbers.BaseLineNumber = oDoc.Lines.LineNum;
                                                                oDoc.Lines.BatchNumbers.SetCurrentLine(i);
                                                                oDoc.Lines.BatchNumbers.ItemCode = rdr3["ItemCode"].ToString();
                                                                oDoc.Lines.BatchNumbers.BatchNumber = rdr3["DistNumber"].ToString();
                                                                oDoc.Lines.BatchNumbers.Quantity = Convert.ToDouble(Convert.ToDouble(rdr3["Quantity"]) > 0 ? Convert.ToDouble(rdr3["Quantity"]) : (-1 * Convert.ToDouble(rdr3["Quantity"])));
                                                                oDoc.Lines.BatchNumbers.AddmisionDate = rdr3["CreateDate"].ToString() == "" ? DateTime.Now : Convert.ToDateTime(rdr3["CreateDate"].ToString());
                                                                if (rdr3["ExpDate"].ToString() != "")
                                                                    oDoc.Lines.BatchNumbers.ExpiryDate = Convert.ToDateTime(rdr3["ExpDate"].ToString());
                                                                oDoc.Lines.BatchNumbers.Add();
                                                                i += 1;
                                                            }
                                                        }
                                                    }
                                                    catch (Exception)
                                                    {

                                                        throw;
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

                            #region Posting data to SAP
                            int res = -1;
                            if (!isOld)
                                res = oDoc.Add();
                            else
                                res = oDoc.Update();

                            if (res < 0)
                            {

                                oCompany.GetLastError(out res, out message);
                                models.Message = message;
                                models.isSuccess = false;
                                tran.Rollback();
                                return models;
                            }
                            else
                            {

                                string getWBSDocNum = @"select DocEntry from " + headerTable + " where U_WBS_DocNum =" + ID;
                                tbl_docRow docRowModel = new tbl_docRow();
                                using (var rdr3 = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, getWBSDocNum))
                                {
                                    while (rdr3.Read())
                                    {
                                        docRowModel.DocEntry = rdr3["DocEntry"].ToInt();

                                    }
                                }
                                if (docRowModel.DocEntry == null || docRowModel.DocEntry.ToString() == "")
                                {
                                    string getWBSDocNumApprvl = @"select DocEntry from ODRF where U_WBS_DocNum =" + ID + " and ElCoStatus=1 and ObjType=" + ObjectCode;
                                    docRowModel.DocEntry = SqlHelper.ExecuteScalar(tran, CommandType.Text, getWBSDocNumApprvl).ToInt();
                                }
                                if (docRowModel.DocEntry != null)
                                {
                                    #region Updating Table Row as Posted , Add Sap Base Entry
                                    string UpdateHeaderTable = @"Update " + headerTable + " set isPosted = 1,Sap_Ref_No = " + docRowModel.DocEntry + ",is_Edited = 0  where Id =" + ID;    //For Updating master table row as this data is posted to SAP
                                    int res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, UpdateHeaderTable).ToInt();
                                    if (res1 <= 0)
                                    {
                                        tran.Rollback();

                                        models.Message = "Document Posted but Error Occured while updating Document !";
                                        models.isSuccess = true;
                                        return models;
                                    }

                                    string UpdateRowTable = @"Update " + rowTable + " set Sap_Ref_No = " + docRowModel.DocEntry + " where Id =" + ID;
                                    res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, UpdateRowTable).ToInt();
                                    if (res1 <= 0)
                                    {
                                        tran.Rollback();

                                        models.Message = "Document Posted but Error Occured while updating Document !";
                                        models.isSuccess = true;
                                        return models;
                                    }
                                    #endregion
                                }
                                else
                                {
                                    tran.Rollback();

                                    models.Message = "Document Posted but Error Occured while updating Document !";
                                    models.isSuccess = true;
                                    return models;
                                }


                                tran.Commit();


                            }
                            #endregion

                        }


                        models.Message = "Goods Receipt Posted Successfully !!";
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

        public ResponseModels PostInventoryTransfer(string[] checkedIDs, int ObjectCode)
        {
            ResponseModels models = new ResponseModels();
            try
            {
                if (Connect())
                {
                    CommonDal dal = new CommonDal();

                    SAPbobsCOM.StockTransfer oDoc = (SAPbobsCOM.StockTransfer)oCompany.GetBusinessObject(BoObjectTypes.oStockTransfer);
                    if (oDoc != null)
                    {
                        string headerTable = dal.GetMasterTable(ObjectCode);
                        string rowTable = dal.GetRowTable(ObjectCode);
                        string message = "";

                        SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
                        conn.Open();
                        SqlTransaction tran = conn.BeginTransaction();



                        foreach (var ID in checkedIDs)
                        {
                            string UDF = "";
                            bool isOld = false;

                            string headerQuery = @"select Id,Guid,MySeries,DocNum,Series,DocDate,GroupNum,TaxDate,Address,CardName,CardCode,Name,Comments,JrnlMemo,Sap_Ref_No,Filler,ToWhsCode " + UDF + " from " + headerTable + " where Id=" + ID + " and isPosted = 0";
                            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, headerQuery))
                            {
                                try
                                {


                                    while (rdr.Read())
                                    {
                                        #region Insert In Header
                                        isOld = oDoc.GetByKey(rdr["Sap_Ref_No"].ToInt());

                                        oDoc.Series = rdr["Series"].ToInt();

                                        oDoc.DocDate = rdr["DocDate"].ToString() == "" ? DateTime.Now : Convert.ToDateTime(rdr["DocDate"].ToString());
                                        oDoc.PriceList = rdr["GroupNum"].ToInt();
                                        oDoc.TaxDate = rdr["TaxDate"].ToString() == "" ? DateTime.Now : Convert.ToDateTime(rdr["TaxDate"].ToString());
                                        oDoc.Address = rdr["Address"].ToString();
                                        oDoc.CardName = rdr["CardName"].ToString();
                                        oDoc.CardCode = rdr["CardCode"].ToString();
                                        //oDoc.BPLName = rdr["Name"].ToString();
                                        oDoc.Comments = rdr["Comments"].ToString();
                                        oDoc.JournalMemo = rdr["JrnlMemo"].ToString();
                                        oDoc.FromWarehouse = rdr["Filler"].ToString();
                                        oDoc.ToWarehouse = rdr["ToWhsCode"].ToString();
                                        oDoc.UserFields.Fields.Item("U_WBS_DocNum").Value = ID;

                                        #endregion

                                        #region Insert in Row
                                        string RowQuery = @"select Id,LineNum,BaseRef,BaseEntry,BaseLine,ItemCode,Dscription,WhsCode,FromWhsCod,Quantity,UomEntry,UomCode,BaseQty,OpenQty from " + rowTable + " where Id = " + ID;
                                        using (var rdr2 = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, RowQuery))
                                        {
                                            try
                                            {


                                                while (rdr2.Read())
                                                {

                                                    //oDoc.Lines.UserFields.Fields.Item("U_WSB_BaseRef").Value = ID;
                                                    oDoc.Lines.ItemCode = rdr2["ItemCode"].ToString();
                                                    oDoc.Lines.ItemDescription = rdr2["Dscription"].ToString();
                                                    oDoc.Lines.WarehouseCode = rdr2["WhsCode"].ToString();
                                                    oDoc.Lines.FromWarehouseCode = rdr2["FromWhsCod"].ToString();
                                                    oDoc.Lines.Quantity = Convert.ToDouble(rdr2["Quantity"]);
                                                    oDoc.Lines.UoMEntry = rdr2["UomEntry"].ToInt();
                                                    

                                                    try
                                                    {


                                                        string BatchQuery = @" select ITL1.ItemCode,ITL1.SysNumber,ITL1.Quantity,ITL1.AllocQty,OITL.CreateDate, OBTN.ExpDate,OBTN.DistNumber from OITL 
                                                                           inner join ITL1 on OITL.LogEntry = ITL1.LogEntry                                                                            
                                                                           join OBTN on ITL1.MdAbsEntry = OBTN.AbsEntry
                                                                           where DocLine = '" + rdr2["LineNum"].ToString() + "' and DocNum = '" + rdr["Id"].ToString() + "' and DocType =" + ObjectCode;
                                                        using (var rdr3 = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, BatchQuery))
                                                        {
                                                            int i = 0;
                                                            while (rdr3.Read())
                                                            {
                                                                if (Convert.ToDouble(rdr3["Quantity"]) > 0)
                                                                {

                                                                
                                                                oDoc.Lines.BatchNumbers.BaseLineNumber = oDoc.Lines.LineNum;
                                                                oDoc.Lines.BatchNumbers.SetCurrentLine(i);
                                                                oDoc.Lines.BatchNumbers.ItemCode = rdr3["ItemCode"].ToString();
                                                                oDoc.Lines.BatchNumbers.BatchNumber = rdr3["DistNumber"].ToString();
                                                                oDoc.Lines.BatchNumbers.Quantity = Convert.ToDouble(rdr3["Quantity"]);
                                                                oDoc.Lines.BatchNumbers.AddmisionDate = rdr3["CreateDate"].ToString() == "" ? DateTime.Now : Convert.ToDateTime(rdr3["CreateDate"].ToString());
                                                                if (rdr3["ExpDate"].ToString() != "")
                                                                    oDoc.Lines.BatchNumbers.ExpiryDate = Convert.ToDateTime(rdr3["ExpDate"].ToString());
                                                                oDoc.Lines.BatchNumbers.Add();
                                                                i += 1;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    catch (Exception)
                                                    {

                                                        throw;
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

                            #region Posting data to SAP
                            int res = -1;
                            if (!isOld)
                                res = oDoc.Add();
                            else
                                res = oDoc.Update();

                            if (res < 0)
                            {

                                oCompany.GetLastError(out res, out message);
                                models.Message = message;
                                models.isSuccess = false;
                                tran.Rollback();
                                return models;
                            }
                            else
                            {

                                string getWBSDocNum = @"select DocEntry from " + headerTable + " where U_WBS_DocNum =" + ID;
                                tbl_docRow docRowModel = new tbl_docRow();
                                using (var rdr3 = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, getWBSDocNum))
                                {
                                    while (rdr3.Read())
                                    {
                                        docRowModel.DocEntry = rdr3["DocEntry"].ToInt();

                                    }
                                }
                                if (docRowModel.DocEntry == null || docRowModel.DocEntry.ToString() == "")
                                {
                                    string getWBSDocNumApprvl = @"select DocEntry from ODRF where U_WBS_DocNum =" + ID + " and ElCoStatus=1 and ObjType=" + ObjectCode;
                                    docRowModel.DocEntry = SqlHelper.ExecuteScalar(tran, CommandType.Text, getWBSDocNumApprvl).ToInt();
                                }
                                if (docRowModel.DocEntry != null)
                                {
                                    #region Updating Table Row as Posted , Add Sap Base Entry
                                    string UpdateHeaderTable = @"Update " + headerTable + " set isPosted = 1,Sap_Ref_No = " + docRowModel.DocEntry + ",is_Edited = 0  where Id =" + ID;    //For Updating master table row as this data is posted to SAP
                                    int res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, UpdateHeaderTable).ToInt();
                                    if (res1 <= 0)
                                    {
                                        tran.Rollback();

                                        models.Message = "Document Posted but Error Occured while updating Document !";
                                        models.isSuccess = true;
                                        return models;
                                    }

                                    string UpdateRowTable = @"Update " + rowTable + " set Sap_Ref_No = " + docRowModel.DocEntry + " where Id =" + ID;
                                    res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, UpdateRowTable).ToInt();
                                    if (res1 <= 0)
                                    {
                                        tran.Rollback();

                                        models.Message = "Document Posted but Error Occured while updating Document !";
                                        models.isSuccess = true;
                                        return models;
                                    }

                                    #endregion
                                }
                                else
                                {
                                    tran.Rollback();

                                    models.Message = "Document Posted but Error Occured while updating Document !";
                                    models.isSuccess = true;
                                    return models;
                                }


                                tran.Commit();


                            }
                            #endregion

                        }


                        models.Message = "Inventory Transfer Posted Successfully !!";
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

        public ResponseModels PostInventoryTransferRequest(string[] checkedIDs, int ObjectCode)
        {
            ResponseModels models = new ResponseModels();
            try
            {
                if (Connect())
                {
                    CommonDal dal = new CommonDal();

                    SAPbobsCOM.StockTransfer oDoc = (SAPbobsCOM.StockTransfer)oCompany.GetBusinessObject(BoObjectTypes.oInventoryTransferRequest);
                    if (oDoc != null)
                    {
                        string headerTable = dal.GetMasterTable(ObjectCode);
                        string rowTable = dal.GetRowTable(ObjectCode);
                        string message = "";

                        SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
                        conn.Open();
                        SqlTransaction tran = conn.BeginTransaction();



                        foreach (var ID in checkedIDs)
                        {
                            string UDF = "";
                            bool isOld = false;

                            string headerQuery = @"select Id,Guid,MySeries,DocNum,Series,DocDate,DocDueDate,GroupNum,TaxDate,Address,CardName,CardCode,CntctCode,Comments,JrnlMemo,Sap_Ref_No,Filler,ToWhsCode " + UDF + " from " + headerTable + " where Id=" + ID + " and isPosted = 0";
                            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, headerQuery))
                            {
                                try
                                {


                                    while (rdr.Read())
                                    {
                                        #region Insert In Header
                                        isOld = oDoc.GetByKey(rdr["Sap_Ref_No"].ToInt());

                                        oDoc.Series = rdr["Series"].ToInt();

                                        oDoc.DocDate = rdr["DocDate"].ToString() == "" ? DateTime.Now : Convert.ToDateTime(rdr["DocDate"].ToString());
                                        oDoc.DueDate = rdr["DocDueDate"].ToString() == "" ? DateTime.Now : Convert.ToDateTime(rdr["DocDueDate"].ToString());
                                        oDoc.PriceList = rdr["GroupNum"].ToInt();
                                        oDoc.TaxDate = rdr["TaxDate"].ToString() == "" ? DateTime.Now : Convert.ToDateTime(rdr["TaxDate"].ToString());
                                        oDoc.Address = rdr["Address"].ToString();
                                        oDoc.CardName = rdr["CardName"].ToString();
                                        oDoc.CardCode = rdr["CardCode"].ToString();
                                        if (rdr["CntctCode"].ToString() != "")
                                        oDoc.ContactPerson = rdr["CntctCode"].ToInt();                                        
                                        //oDoc.BPLName = rdr["Name"].ToString();
                                        oDoc.Comments = rdr["Comments"].ToString();
                                        oDoc.JournalMemo = rdr["JrnlMemo"].ToString();
                                        oDoc.FromWarehouse = rdr["Filler"].ToString();
                                        oDoc.ToWarehouse = rdr["ToWhsCode"].ToString();
                                        oDoc.UserFields.Fields.Item("U_WBS_DocNum").Value = ID;

                                        #endregion

                                        #region Insert in Row
                                        string RowQuery = @"select Id,LineNum,BaseRef,BaseEntry,BaseLine,ItemCode,Dscription,WhsCode,FromWhsCod,Quantity,UomEntry,UomCode,BaseQty,OpenQty from " + rowTable + " where Id = " + ID;
                                        using (var rdr2 = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, RowQuery))
                                        {
                                            try
                                            {


                                                while (rdr2.Read())
                                                {

                                                    //oDoc.Lines.UserFields.Fields.Item("U_WSB_BaseRef").Value = ID;
                                                    oDoc.Lines.ItemCode = rdr2["ItemCode"].ToString();
                                                    oDoc.Lines.ItemDescription = rdr2["Dscription"].ToString();
                                                    oDoc.Lines.WarehouseCode = rdr2["WhsCode"].ToString();
                                                    oDoc.Lines.FromWarehouseCode = rdr2["FromWhsCod"].ToString();
                                                    oDoc.Lines.Quantity = Convert.ToDouble(rdr2["Quantity"]);
                                                    oDoc.Lines.UoMEntry = rdr2["UomEntry"].ToInt();


                                                    try
                                                    {


                                                        string BatchQuery = @" select ITL1.ItemCode,ITL1.SysNumber,ITL1.Quantity,ITL1.AllocQty,OITL.CreateDate, OBTN.ExpDate,OBTN.DistNumber from OITL 
                                                                           inner join ITL1 on OITL.LogEntry = ITL1.LogEntry                                                                            
                                                                           join OBTN on ITL1.MdAbsEntry = OBTN.AbsEntry
                                                                           where DocLine = '" + rdr2["LineNum"].ToString() + "' and DocNum = '" + rdr["Id"].ToString() + "' and DocType =" + ObjectCode;
                                                        using (var rdr3 = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, BatchQuery))
                                                        {
                                                            int i = 0;
                                                            while (rdr3.Read())
                                                            {
                                                                if (Convert.ToDouble(rdr3["AllocQty"]) > 0)
                                                                {


                                                                    oDoc.Lines.BatchNumbers.BaseLineNumber = oDoc.Lines.LineNum;
                                                                    oDoc.Lines.BatchNumbers.SetCurrentLine(i);
                                                                    oDoc.Lines.BatchNumbers.ItemCode = rdr3["ItemCode"].ToString();
                                                                    oDoc.Lines.BatchNumbers.BatchNumber = rdr3["DistNumber"].ToString();
                                                                    oDoc.Lines.BatchNumbers.Quantity = Convert.ToDouble(rdr3["AllocQty"]);
                                                                    oDoc.Lines.BatchNumbers.AddmisionDate = rdr3["CreateDate"].ToString() == "" ? DateTime.Now : Convert.ToDateTime(rdr3["CreateDate"].ToString());
                                                                    if (rdr3["ExpDate"].ToString() != "")
                                                                        oDoc.Lines.BatchNumbers.ExpiryDate = Convert.ToDateTime(rdr3["ExpDate"].ToString());
                                                                    oDoc.Lines.BatchNumbers.Add();
                                                                    i += 1;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    catch (Exception)
                                                    {

                                                        throw;
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

                            #region Posting data to SAP
                            int res = -1;
                            if (!isOld)
                                res = oDoc.Add();
                            else
                                res = oDoc.Update();

                            if (res < 0)
                            {

                                oCompany.GetLastError(out res, out message);
                                models.Message = message;
                                models.isSuccess = false;
                                tran.Rollback();
                                return models;
                            }
                            else
                            {

                                string getWBSDocNum = @"select DocEntry from " + headerTable + " where U_WBS_DocNum =" + ID;
                                tbl_docRow docRowModel = new tbl_docRow();
                                using (var rdr3 = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, getWBSDocNum))
                                {
                                    while (rdr3.Read())
                                    {
                                        docRowModel.DocEntry = rdr3["DocEntry"].ToInt();

                                    }
                                }
                                if (docRowModel.DocEntry == null || docRowModel.DocEntry.ToString() == "")
                                {
                                    string getWBSDocNumApprvl = @"select DocEntry from ODRF where U_WBS_DocNum =" + ID + " and ElCoStatus=1 and ObjType=" + ObjectCode;
                                    docRowModel.DocEntry = SqlHelper.ExecuteScalar(tran, CommandType.Text, getWBSDocNumApprvl).ToInt();
                                }
                                if (docRowModel.DocEntry != null)
                                {
                                    #region Updating Table Row as Posted , Add Sap Base Entry
                                    string UpdateHeaderTable = @"Update " + headerTable + " set isPosted = 1,Sap_Ref_No = " + docRowModel.DocEntry + ",is_Edited = 0  where Id =" + ID;    //For Updating master table row as this data is posted to SAP
                                    int res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, UpdateHeaderTable).ToInt();
                                    if (res1 <= 0)
                                    {
                                        tran.Rollback();

                                        models.Message = "Document Posted but Error Occured while updating Document !";
                                        models.isSuccess = true;
                                        return models;
                                    }

                                    string UpdateRowTable = @"Update " + rowTable + " set Sap_Ref_No = " + docRowModel.DocEntry + " where Id =" + ID;
                                    res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, UpdateRowTable).ToInt();
                                    if (res1 <= 0)
                                    {
                                        tran.Rollback();

                                        models.Message = "Document Posted but Error Occured while updating Document !";
                                        models.isSuccess = true;
                                        return models;
                                    }

                                    #endregion
                                }
                                else
                                {
                                    tran.Rollback();

                                    models.Message = "Document Posted but Error Occured while updating Document !";
                                    models.isSuccess = true;
                                    return models;
                                }
                                tran.Commit();


                            }
                            #endregion

                        }


                        models.Message = "Inventory Transfer Request Posted Successfully !!";
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

        public ResponseModels PostBusinnesPartner(string[] checkedIDs,int ObjectCode)
        {
            ResponseModels models = new ResponseModels();
            try
            {
                if (Connect())
                {
                    CommonDal dal = new CommonDal();

                    SAPbobsCOM.BusinessPartners oDoc = (SAPbobsCOM.BusinessPartners)oCompany.GetBusinessObject(BoObjectTypes.oBusinessPartners);
                    if (oDoc != null)
                    {
                        string headerTable = dal.GetMasterTable(ObjectCode);                        
                        string message = "";

                        SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
                        conn.Open();
                        SqlTransaction tran = conn.BeginTransaction();



                        foreach (var ID in checkedIDs)
                        {
                            string UDF = "";
                            bool isOld = false;

                            string headerQuery = @"select Id,Guid,MySeries,CardCode,CardType,Series,CardName,CardFName,GroupCode,Currency,LicTradNum,
                                            Phone1,CntctPrsn,Phone2,AddID,Cellular,VatIdUnCmp,Fax,RegNum,E_Mail,Notes,IntrntSite,ShipType,SlpCode,Password,Indicator,ProjectCod,ChannlBP,
                                            IndustryC,DfTcnician,CmpPrivate,Territory,AliasName,GlblLocNum,validFor,validFrom,validTo,frozenFor,frozenFrom,frozenTo,FrozenComm, 
                                            GroupNum ,Discount,CreditLine,BankCountr,BankCode,DflAccount,DflSwift,DflBranch,BankCtlKey,DflIBAN,MandateID,SignDate,
                                            QryGroup1, QryGroup2, QryGroup3, QryGroup4, QryGroup5, QryGroup6,QryGroup7,QryGroup8, QryGroup9, QryGroup10, QryGroup11, 
                                            QryGroup12, QryGroup13, QryGroup14, QryGroup15, QryGroup16, QryGroup17, QryGroup18, QryGroup19, QryGroup20,QryGroup21, QryGroup22,
                                            QryGroup23, QryGroup24, QryGroup25, QryGroup26, QryGroup27, QryGroup28, QryGroup29, QryGroup30, QryGroup31, QryGroup32, QryGroup33,
                                            QryGroup34, QryGroup35, QryGroup36, QryGroup37, QryGroup38, QryGroup39, QryGroup40, QryGroup41, QryGroup42, QryGroup43, QryGroup44, QryGroup45, QryGroup46, 
                                            QryGroup47, QryGroup48, QryGroup49, QryGroup50, QryGroup51, QryGroup52, QryGroup53, QryGroup54, QryGroup55, QryGroup56, QryGroup57, QryGroup58, QryGroup59, 
                                            QryGroup60, QryGroup61, QryGroup62, QryGroup63, QryGroup64,Sap_ItemCode from OCRD where Id =" + ID;
                            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, headerQuery))
                            {
                                try
                                {


                                    while (rdr.Read())
                                    {
                                        #region Insert In Header
                                        isOld = oDoc.GetByKey(rdr["Sap_Ref_No"].ToString());

                                        oDoc.Series = rdr["Series"].ToInt();

                                        oDoc.CardCode = rdr["CardCode"].ToString();
                                        oDoc.CardName = rdr["CardName"].ToString();
                                        oDoc.CardForeignName = rdr["CardFName"].ToString();
                                        oDoc.CardType = rdr["CardCode"].ToString() == "C" ? BoCardTypes.cCustomer : rdr["CardCode"].ToString() == "S" ? BoCardTypes.cSupplier : BoCardTypes.cLid;
                                        oDoc.GroupCode = rdr["GroupCode"].ToInt();
                                        oDoc.Currency = rdr["Currency"].ToString();
                                        oDoc.Phone1 = rdr["Phone1"].ToString();
                                        oDoc.ContactPerson = rdr["CntctPrsn"].ToString();
                                        oDoc.Phone2 = rdr["Phone2"].ToString();
                                        oDoc.AdditionalID = rdr["AddID"].ToString();
                                        oDoc.Cellular = rdr["Cellular"].ToString();
                                        oDoc.VatIDNum = rdr["VatIdUnCmp"].ToString();
                                        oDoc.Fax = rdr["Fax"].ToString();
                                        oDoc.CompanyRegistrationNumber = rdr["RegNum"].ToString();
                                        oDoc.EmailAddress = rdr["E_Mail"].ToString();
                                        oDoc.Notes = rdr["Notes"].ToString();
                                        oDoc.Website = rdr["IntrntSite"].ToString();
                                        oDoc.ShippingType = rdr["ShipType"].ToInt();
                                        oDoc.SalesPersonCode = rdr["SlpCode"].ToInt();
                                        oDoc.Password = rdr["Password"].ToString();
                                        oDoc.Indicator = rdr["Indicator"].ToString();
                                        oDoc.ProjectCode = rdr["ProjectCod"].ToString();
                                        oDoc.ChannelBP = rdr["ChannlBP"].ToString();
                                        oDoc.Industry = rdr["IndustryC"].ToInt();
                                        oDoc.DefaultTechnician = rdr["DfTcnician"].ToInt();
                                        oDoc.BusinessType = rdr["CmpPrivate"].ToString();
                                        oDoc.Territory = rdr["Territory"].ToInt();
                                        oDoc.AliasName = rdr["AliasName"].ToString();
                                        oDoc.GlobalLocationNumber = rdr["GlblLocNum"].ToString();

                                        if (rdr["validFor"].ToString() == "Y")
                                        {
                                            oDoc.ValidFrom = Convert.ToDateTime(rdr["validFrom"]);
                                            oDoc.ValidTo = Convert.ToDateTime(rdr["validTo"]);
                                            oDoc.Valid = BoYesNoEnum.tYES;
                                        }
                                        else
                                        {
                                            oDoc.Frozen = BoYesNoEnum.tYES;
                                            oDoc.FrozenFrom = Convert.ToDateTime(rdr["frozenFrom"]);
                                            oDoc.FrozenTo = Convert.ToDateTime(rdr["frozenTo"]);
                                        }
                                        oDoc.FrozenRemarks = rdr["FrozenComm"].ToString();
                                        oDoc.DiscountPercent = rdr["Discount"].ToDouble();
                                        oDoc.CreditLimit = rdr["CreditLine"].ToDouble();
                                        oDoc.BankCountry = rdr["BankCountr"].ToString();
                                        oDoc.DefaultBankCode = rdr["BankCode"].ToString();
                                        oDoc.DefaultAccount = rdr["DflAccount"].ToString();
                                        oDoc.BillofExchangeonCollection = rdr["DflSwift"].ToString();
                                        oDoc.DefaultBranch = rdr["DflBranch"].ToString();
                                        oDoc.InstructionKey = rdr["BankCtlKey"].ToString();
                                        oDoc.IBAN = rdr["DflIBAN"].ToString();



                                        oDoc.UserFields.Fields.Item("U_WBS_DocNum").Value = ID;

                                        #region Properties
                                        oDoc.Properties[1] = rdr["QryGroup1"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                        oDoc.Properties[2] = rdr["QryGroup2"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                        oDoc.Properties[3] = rdr["QryGroup3"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                        oDoc.Properties[4] = rdr["QryGroup4"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                        oDoc.Properties[5] = rdr["QryGroup5"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                        oDoc.Properties[6] = rdr["QryGroup6"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                        oDoc.Properties[7] = rdr["QryGroup7"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                        oDoc.Properties[8] = rdr["QryGroup8"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                        oDoc.Properties[9] = rdr["QryGroup9"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                        oDoc.Properties[10] = rdr["QryGroup10"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                        oDoc.Properties[11] = rdr["QryGroup11"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                        oDoc.Properties[12] = rdr["QryGroup12"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                        oDoc.Properties[13] = rdr["QryGroup13"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                        oDoc.Properties[14] = rdr["QryGroup14"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                        oDoc.Properties[15] = rdr["QryGroup15"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                        oDoc.Properties[16] = rdr["QryGroup16"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                        oDoc.Properties[17] = rdr["QryGroup17"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                        oDoc.Properties[18] = rdr["QryGroup18"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                        oDoc.Properties[19] = rdr["QryGroup19"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                        oDoc.Properties[20] = rdr["QryGroup20"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                        oDoc.Properties[21] = rdr["QryGroup21"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                        oDoc.Properties[22] = rdr["QryGroup22"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                        oDoc.Properties[23] = rdr["QryGroup23"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                        oDoc.Properties[24] = rdr["QryGroup24"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                        oDoc.Properties[25] = rdr["QryGroup25"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                        oDoc.Properties[26] = rdr["QryGroup26"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                        oDoc.Properties[27] = rdr["QryGroup27"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                        oDoc.Properties[28] = rdr["QryGroup28"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                        oDoc.Properties[29] = rdr["QryGroup29"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                        oDoc.Properties[30] = rdr["QryGroup30"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                        oDoc.Properties[31] = rdr["QryGroup31"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                        oDoc.Properties[32] = rdr["QryGroup32"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                        oDoc.Properties[33] = rdr["QryGroup33"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                        oDoc.Properties[34] = rdr["QryGroup34"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                        oDoc.Properties[35] = rdr["QryGroup35"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                        oDoc.Properties[36] = rdr["QryGroup36"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                        oDoc.Properties[37] = rdr["QryGroup37"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                        oDoc.Properties[38] = rdr["QryGroup38"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                        oDoc.Properties[39] = rdr["QryGroup39"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                        oDoc.Properties[40] = rdr["QryGroup40"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                        oDoc.Properties[41] = rdr["QryGroup41"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                        oDoc.Properties[42] = rdr["QryGroup42"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                        oDoc.Properties[43] = rdr["QryGroup43"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                        oDoc.Properties[44] = rdr["QryGroup44"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                        oDoc.Properties[45] = rdr["QryGroup45"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                        oDoc.Properties[46] = rdr["QryGroup46"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                        oDoc.Properties[47] = rdr["QryGroup47"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                        oDoc.Properties[48] = rdr["QryGroup48"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                        oDoc.Properties[49] = rdr["QryGroup49"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                        oDoc.Properties[50] = rdr["QryGroup50"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                        oDoc.Properties[51] = rdr["QryGroup51"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                        oDoc.Properties[52] = rdr["QryGroup52"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                        oDoc.Properties[53] = rdr["QryGroup53"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                        oDoc.Properties[54] = rdr["QryGroup54"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                        oDoc.Properties[55] = rdr["QryGroup55"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                        oDoc.Properties[56] = rdr["QryGroup56"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                        oDoc.Properties[57] = rdr["QryGroup57"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                        oDoc.Properties[58] = rdr["QryGroup58"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                        oDoc.Properties[59] = rdr["QryGroup59"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                        oDoc.Properties[60] = rdr["QryGroup60"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                        oDoc.Properties[61] = rdr["QryGroup61"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                        oDoc.Properties[62] = rdr["QryGroup62"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                        oDoc.Properties[63] = rdr["QryGroup63"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                                        oDoc.Properties[64] = rdr["QryGroup64"].ToString() == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;

                                        #endregion

                                        #endregion

                                        #region Insert Contact Persons

                                        ContactEmployees contactPerson = (ContactEmployees)oDoc.ContactEmployees;

                                        string ContactPersonsQuery = @"select id,CardCode,Name,FirstName,MiddleName,LastName,Title,Position,Address,Tel1,Tel2,Cellolar,Fax,E_MailL,EmlGrpCode,
                                                                       Pager,Notes1,Notes2,Password,BirthDate,Gender,Profession,BirthCity from OCPR where CardCode =" + rdr["CardCode"].ToString() + "'";

                                        using (var rdr1 = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, ContactPersonsQuery))
                                        {
                                            while (rdr1.Read())
                                            {
                                                contactPerson.Name = rdr["Name"].ToString();
                                                contactPerson.FirstName = rdr["FirstName"].ToString();
                                                contactPerson.MiddleName = rdr["MiddleName"].ToString();
                                                contactPerson.LastName = rdr["LastName"].ToString();
                                                contactPerson.Title = rdr["Title"].ToString();
                                                contactPerson.Position = rdr["Position"].ToString();
                                                contactPerson.Address = rdr["Address"].ToString();
                                                contactPerson.Phone1 = rdr["Tel1"].ToString();
                                                contactPerson.Phone2 = rdr["Tel2"].ToString();
                                                contactPerson.MobilePhone = rdr["Cellolar"].ToString();
                                                contactPerson.Fax = rdr["Fax"].ToString();
                                                contactPerson.E_Mail = rdr["E_MailL"].ToString();
                                                contactPerson.EmailGroupCode = rdr["EmlGrpCode"].ToString();
                                                contactPerson.Pager = rdr["Pager"].ToString();
                                                contactPerson.Remarks1 = rdr["Notes1"].ToString();
                                                contactPerson.Remarks2 = rdr["Notes2"].ToString();
                                                contactPerson.Password = rdr["Password"].ToString();
                                                if (rdr["BirthDate"].ToString() != "")
                                                    contactPerson.DateOfBirth = Convert.ToDateTime(rdr["BirthDate"]);
                                                contactPerson.Gender = rdr["Gender"].ToString() == "M" ? BoGenderTypes.gt_Male : rdr["Gender"].ToString() == "F" ? BoGenderTypes.gt_Female : BoGenderTypes.gt_Undefined;
                                                contactPerson.Profession = rdr["Profession"].ToString();
                                                contactPerson.CityOfBirth = rdr["BirthCity"].ToString();

                                                contactPerson.Add();

                                            }


                                        }

                                        #endregion

                                        #region Address Bill To

                                        SAPbobsCOM.BPAddresses billtoAddress = (BPAddresses)oDoc.Addresses;
                                        string BillToAddressQuery = @"id,LineNum,CardCode,Address,Address2,Address3,Street,Block,City,ZipCode,County,State,Country,StreetNo,
                                                                      Building,GlblLocNum,AdresType where id =" + ID + " and AdresType ='B'";

                                        using (var rdr2 = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, BillToAddressQuery))
                                        {
                                            while (rdr2.Read())
                                            {

                                                billtoAddress.AddressType = BoAddressType.bo_BillTo;
                                                billtoAddress.AddressName = rdr["Address"].ToString();
                                                billtoAddress.AddressName2 = rdr["Address2"].ToString();
                                                billtoAddress.AddressName3 = rdr["Address3"].ToString();
                                                billtoAddress.Street = rdr["Street"].ToString();
                                                billtoAddress.Block = rdr["Block"].ToString();
                                                billtoAddress.City = rdr["City"].ToString();
                                                billtoAddress.ZipCode = rdr["ZipCode"].ToString();
                                                billtoAddress.County = rdr["County"].ToString();
                                                billtoAddress.State = rdr["State"].ToString();
                                                billtoAddress.Country = rdr["Country"].ToString();
                                                billtoAddress.StreetNo = rdr["StreetNo"].ToString();
                                                billtoAddress.BuildingFloorRoom = rdr["Building"].ToString();
                                                billtoAddress.GlobalLocationNumber = rdr["GlblLocNum"].ToString();
                                        
                                        
                                                billtoAddress.Add();
                                            }
                                        }
                                        #endregion

                                        #region Address Shipp To

                                        SAPbobsCOM.BPAddresses ShiptoAddress = (BPAddresses)oDoc.Addresses;
                                        string ShipToAddressQuery = @"id,LineNum,CardCode,Address,Address2,Address3,Street,Block,City,ZipCode,County,State,Country,StreetNo,
                                                                      Building,GlblLocNum,AdresType where id =" + ID + " and AdresType ='S'";

                                        using (var rdr3 = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, ShipToAddressQuery))
                                        {
                                            while (rdr3.Read())
                                            {

                                                ShiptoAddress.AddressType = BoAddressType.bo_ShipTo;
                                                ShiptoAddress.AddressName = rdr["Address"].ToString();
                                                ShiptoAddress.AddressName2 = rdr["Address2"].ToString();
                                                ShiptoAddress.AddressName3 = rdr["Address3"].ToString();
                                                ShiptoAddress.Street = rdr["Street"].ToString();
                                                ShiptoAddress.Block = rdr["Block"].ToString();
                                                ShiptoAddress.City = rdr["City"].ToString();
                                                ShiptoAddress.ZipCode = rdr["ZipCode"].ToString();
                                                ShiptoAddress.County = rdr["County"].ToString();
                                                ShiptoAddress.State = rdr["State"].ToString();
                                                ShiptoAddress.Country = rdr["Country"].ToString();
                                                ShiptoAddress.StreetNo = rdr["StreetNo"].ToString();
                                                ShiptoAddress.BuildingFloorRoom = rdr["Building"].ToString();
                                                ShiptoAddress.GlobalLocationNumber = rdr["GlblLocNum"].ToString();


                                                ShiptoAddress.Add();
                                            }
                                        }
                                        #endregion
                                    }


                                }
                                catch (Exception e)
                                {
                                    models.Message = e.Message;
                                    models.isSuccess = false;
                                    return models;
                                    throw;
                                }
                            }

                            #region Posting data to SAP
                            int res = -1;
                            if (!isOld)
                                res = oDoc.Add();
                            else
                                res = oDoc.Update();

                            if (res < 0)
                            {

                                oCompany.GetLastError(out res, out message);
                                models.Message = message;
                                models.isSuccess = false;
                                tran.Rollback();
                                return models;
                            }
                            else
                            {

                                string getWBSDocNum = @"select DocEntry from " + headerTable + " where U_WBS_DocNum =" + ID;
                                tbl_docRow docRowModel = new tbl_docRow();
                                using (var rdr3 = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, getWBSDocNum))
                                {
                                    while (rdr3.Read())
                                    {
                                        docRowModel.DocEntry = rdr3["DocEntry"].ToInt();

                                    }
                                }
                                if (docRowModel.DocEntry == null || docRowModel.DocEntry.ToString() == "")
                                {
                                    string getWBSDocNumApprvl = @"select DocEntry from ODRF where U_WBS_DocNum =" + ID + " and ElCoStatus=1 and ObjType=" + ObjectCode;
                                    docRowModel.DocEntry = SqlHelper.ExecuteScalar(tran, CommandType.Text, getWBSDocNumApprvl).ToInt();
                                }
                                #region Updating Table Row as Posted , Add Sap Base Entry
                                if (docRowModel.DocEntry != null)
                                {

                                    string UpdateHeaderTable = @"Update OCRD set isPosted = 1,Sap_Ref_No = " + docRowModel.DocEntry + ",is_Edited = 0  where Id =" + ID;    //For Updating master table row as this data is posted to SAP
                                    int res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, UpdateHeaderTable).ToInt();
                                    if (res1 <= 0)
                                    {
                                        tran.Rollback();

                                        models.Message = "Document Posted but Error Occured while updating Document !";
                                        models.isSuccess = true;
                                        return models;
                                    }
                                }
                                else
                                {
                                    tran.Rollback();

                                    models.Message = "Document Posted but Error Occured while updating Document !";
                                    models.isSuccess = true;
                                    return models;
                                }


                                #endregion
                                tran.Commit();


                            }
                            #endregion

                        }


                        models.Message = "Business Partner Posted Successfully !!";
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