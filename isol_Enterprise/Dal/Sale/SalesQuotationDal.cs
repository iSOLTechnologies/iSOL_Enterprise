﻿using iSOL_Enterprise.Common;
using iSOL_Enterprise.Models;
using iSOL_Enterprise.Models.sale;
using iSOL_Enterprise.Models.Sale;
using Newtonsoft.Json;
using SAPbobsCOM;
using SqlHelperExtensions;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Intrinsics.Arm;
using static iSOL_Enterprise.Dal.DashboardDal;

namespace iSOL_Enterprise.Dal.Sale
{
    public class SalesQuotationDal
    {
        public List<SalesQuotation_MasterModels> GetData()
        {
            string GetQuery = "select * from OQUT order by id DESC";


            List<SalesQuotation_MasterModels> list = new List<SalesQuotation_MasterModels>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {

                    SalesQuotation_MasterModels models = new SalesQuotation_MasterModels();
                    models.DocStatus = CommonDal.Check_IsNotEditable("QUT1", rdr["Id"].ToInt()) == false ? "Open" : "Closed";
                    models.Id = rdr["Id"].ToInt();
                    models.DocDate = rdr["DocDueDate"].ToDateTime();
                    models.PostingDate = rdr["DocDate"].ToDateTime();
                    models.DocNum = rdr["DocNum"].ToString();
                    models.CardCode = rdr["CardCode"].ToString();
                    models.Guid = rdr["Guid"].ToString();
                    models.CardName = rdr["CardName"].ToString();
                    models.IsPosted = rdr["isPosted"].ToString(); models.IsEdited = rdr["is_Edited"].ToString();
                    list.Add(models);
                }
            }
            return list;
        }

        public List<tbl_item> GetItemsData(string DocModule)
        {
            List<tbl_item> list = new List<tbl_item>();
            string GetQuery = "";
            if (DocModule == "S")
            {
                GetQuery = "select ItemCode,ItemName,OnHand,ManBtchNum from OITM where SellItem = 'Y' and FrozenFor='N'";
            }
            else if (DocModule == "P")
            {
                GetQuery = "select ItemCode,ItemName,OnHand,ManBtchNum from OITM where PrchseItem = 'Y' and FrozenFor='N'";
            }
            else if (DocModule == "I")
            {
                GetQuery = "select ItemCode,ItemName,OnHand,ManBtchNum from OITM where InvntItem = 'Y' and FrozenFor='N'";
            }
            else
            {
                return list;
            }


            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {

                    list.Add(
                        new tbl_item()
                        {
                            ItemCode = rdr["ItemCode"].ToString(),
                            ItemName = rdr["ItemName"].ToString(),
                            ManBtchNum = rdr["ManBtchNum"].ToString(),
                            OnHand = (decimal)rdr["OnHand"],
                        });

                }
            }

            return list;
        }

        public List<tbl_account> GetGLAccountData()
        {
            string GetQuery = "select * from OACT";


            List<tbl_account> list = new List<tbl_account>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {

                    list.Add(
                        new tbl_account()
                        {
                            AcctCode = rdr["AcctCode"].ToString(),
                            AcctName = rdr["AcctName"].ToString(),
                            CurrTotal = (decimal)rdr["CurrTotal"],
                        });

                }
            }

            return list;
        }


        public List<tbl_OCTG> GetPaymentTerms()
        {
            string GetQuery = "select GroupNum,PymntGroup from OCTG";


            List<tbl_OCTG> list = new List<tbl_OCTG>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {

                    list.Add(
                        new tbl_OCTG()
                        {
                            GroupNum = rdr["GroupNum"].ToInt(),
                            PymntGroup = rdr["PymntGroup"].ToString()
                        });

                }
            }

            return list;
        }
        public List<tbl_OSLP> GetSalesEmployee()
        {
            string GetQuery = "select SlpCode,SlpName from OSLP";


            List<tbl_OSLP> list = new List<tbl_OSLP>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {

                    list.Add(
                        new tbl_OSLP()
                        {
                            SlpCode = rdr["SlpCode"].ToInt(),
                            SlpName = rdr["SlpName"].ToString()
                        });

                }
            }

            return list;
        }


        public List<tbl_OVTG> GetVatGroupData(string DocModule)
        {
            string GetQuery = "";
            if (DocModule == "S")
            {
                GetQuery = "select code = Code, vatName = Code+' - ' +Name , Rate from OVTG Where Category = 'O'";
            }
            else if (DocModule == "P")
            {
                GetQuery = "select code = Code, vatName = Code+' - ' +Name , Rate from OVTG  Where Category = 'I'";
            }


            List<tbl_OVTG> list = new List<tbl_OVTG>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {

                    list.Add(
                        new tbl_OVTG()
                        {
                            code = rdr["code"].ToString(),
                            vatName = rdr["vatName"].ToString(),
                            Rate = (decimal)rdr["Rate"]
                        });

                }
            }

            return list;
        }

        public List<ListModel> GetContactPersons(string cardCode)
        {
            string GetQuery = "select OCRD.CardCode,OCPR.Name from ocrd join ocpr on ocrd.CardCode = OCPR.CardCode where ocrd.CardCode = '" + cardCode + "'";


            List<ListModel> list = new List<ListModel>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {

                    list.Add(
                        new ListModel()
                        {
                            Value = rdr["CardCode"].ToInt(),
                            Text = rdr["Name"].ToString()
                        });

                }
            }

            return list;
        }



        public dynamic GetSaleQuotationEditDetails(int id)
        {
            DataSet ds = new DataSet();
            SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
            SqlDataAdapter sda = new SqlDataAdapter("select * from OQUT where id = " + id + ";select * from QUT1 where id = " + id + "", conn);
            sda.Fill(ds);
            return ds;
        }

        public List<tbl_OITM> GetOITMRowData(string ItemCode)
        {

            List<tbl_OITM> list = new List<tbl_OITM>();

            string UGPEntry = @"select ItemCode,UgpEntry,SUoMEntry,IUoMEntry,PUoMEntry from OITM where ItemCode ='" + ItemCode + "'";
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, UGPEntry))
            {
                while (rdr.Read())
                {
                    list.Add(
                       new tbl_OITM()
                       {
                           ItemCode = rdr["ItemCode"].ToString(),
                           UgpEntry = rdr["UgpEntry"].ToInt(),
                           SUoMEntry = rdr["SUoMEntry"].ToInt(),
                           IUoMEntry = rdr["IUoMEntry"].ToInt(),
                           PUoMEntry = rdr["PUoMEntry"].ToInt(),

                       });

                }
            }
            return list;
        }
        public List<tbl_OUOM> GetOUOMList(int UgpEntry)
        {
            List<tbl_OUOM> list = new List<tbl_OUOM>();

            string OUOMQuery = @"select OUOM.UomEntry,UomCode  from OUGP
	                            Inner Join UGP1 on OUGP.UgpEntry = UGP1.UgpEntry
	                            Inner Join OUOM on UGP1.UomEntry = OUOM.UomEntry
	                            where OUGP.UgpEntry = " + UgpEntry;

            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, OUOMQuery))
            {
                while (rdr.Read())
                {
                    list.Add(
                       new tbl_OUOM()
                       {
                           UomEntry = rdr["UomEntry"].ToInt(),
                           UomName = rdr["UomCode"].ToString()

                       });

                }
            }

            return list;
        }


        public bool AddSalesQoutation(string formData)
        {
            try
            {
                var model = JsonConvert.DeserializeObject<dynamic>(formData);
                string DocType = model.ListItems == null ? "S" : "I";


                SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                int res1 = 0;
                try
                {
                    int Id = CommonDal.getPrimaryKey(tran, "OQUT");
                    string DocNum = SqlHelper.getUpdatedDocumentNumberOnLoad(tran, "OQUT", "SQ");
                    if (model.HeaderData != null)
                    {

                        model.HeaderData.PurchaseType = model.HeaderData.PurchaseType == "" ? "NULL" : Convert.ToDecimal(model.HeaderData.PurchaseType);
                        model.HeaderData.TypeDetail = model.HeaderData.TypeDetail == "" || model.HeaderData.TypeDetail == null ? "NULL" : Convert.ToDecimal(model.HeaderData.TypeDetail);
                        model.HeaderData.ProductionOrderNo = model.HeaderData.ProductionOrderNo == "" ? "NULL" : Convert.ToDecimal(model.HeaderData.ProductionOrderNo);
                        model.HeaderData.ChallanNo = model.HeaderData.ChallanNo == "" ? "NULL" : Convert.ToDecimal(model.HeaderData.ChallanNo);
                        model.HeaderData.ContainerNo = model.HeaderData.ContainerNo == "" ? "NULL" : Convert.ToDecimal(model.HeaderData.ContainerNo);
                        model.HeaderData.ManualGatePassNo = model.HeaderData.ManualGatePassNo == "" ? "NULL" : Convert.ToDecimal(model.HeaderData.ManualGatePassNo);
                        model.HeaderData.SaleOrderNo = model.HeaderData.SaleOrderNo == "" ? "NULL" : Convert.ToInt32(model.HeaderData.SaleOrderNo);
                        model.HeaderData.Series = model.HeaderData.Series == null ? "NULL" : Convert.ToInt32(model.HeaderData.Series);
                        model.FooterData.Discount = model.FooterData.Discount == "" ? "NULL" : Convert.ToDecimal(model.FooterData.Discount);

                        string HeadQuery = @"insert into OQUT(Id,Series,DocType,Guid,CardCode,DocNum,CardName,CntctCode,DocDate,NumAtCard,DocDueDate,DocCur,TaxDate , GroupNum ,DocTotal, SlpCode,DiscPrcnt,
                                            PurchaseType,TypeDetail,ProductionOrderNo,ChallanNo,ContainerNo,ManualGatePassNo,SaleOrderNo, Comments) 
                                           values(" + Id + ","
                                                + model.HeaderData.Series + ",'"
                                                + DocType + "','"
                                                + CommonDal.generatedGuid() + "','"
                                                + model.HeaderData.CardCode + "','"
                                                + DocNum + "','"
                                                + model.HeaderData.CardName + "','"
                                                + model.HeaderData.CntctCode + "','"
                                                + Convert.ToDateTime(model.HeaderData.DocDate) + "','"
                                                + model.HeaderData.NumAtCard + "','"
                                                + Convert.ToDateTime(model.HeaderData.DocDueDate) + "','"
                                                + model.HeaderData.DocCur + "','"
                                                + Convert.ToDateTime(model.HeaderData.TaxDate) + "','"
                                                + model.ListAccouting.GroupNum + "',"
                                                + model.FooterData.Total + ","
                                                + Convert.ToInt32(model.FooterData.SlpCode) + ","
                                                + model.FooterData.Discount + ","
                                                + model.HeaderData.PurchaseType + ","
                                                + model.HeaderData.TypeDetail + ","
                                                + model.HeaderData.ProductionOrderNo + ","
                                                + model.HeaderData.ChallanNo + ","
                                                + model.HeaderData.ContainerNo + ","
                                                + model.HeaderData.ManualGatePassNo + ","
                                                + model.HeaderData.SaleOrderNo + ",'"
                                                + model.FooterData.Comments + "')";

                        #region SqlParameters
                        //List<SqlParameter> param = new List<SqlParameter>   
                        //        {
                        //            new SqlParameter("@id",Id),
                        //            new SqlParameter("@Guid", CommonDal.generatedGuid()),
                        //            new SqlParameter("@CardCode",model.HeaderData.CardCode.ToString()),
                        //            new SqlParameter("@DocNum",model.HeaderData.DocNum.ToString()),
                        //            new SqlParameter("@CardName",model.HeaderData.CardName.ToString()),
                        //            new SqlParameter("@CntctCode",model.HeaderData.CntctCode == null ? null : model.HeaderData.CntctCode.ToInt()),
                        //            new SqlParameter("@DocDate",Convert.ToDateTime( model.HeaderData.DocDate)),
                        //            new SqlParameter("@NumAtCard",model.HeaderData.NumAtCard.ToString()),
                        //            new SqlParameter("@DocDueDate",Convert.ToDateTime(model.HeaderData.DocDueDate)),
                        //            new SqlParameter("@DocCur",model.HeaderData.DocCur.ToString()),
                        //            new SqlParameter("@TaxDate",Convert.ToDateTime(model.HeaderData.TaxDate)),
                        //            new SqlParameter("@GroupNum",model.ListAccouting.GroupNum == null ?  "" : Convert.ToInt32(model.ListAccouting.GroupNum)),
                        //            new SqlParameter("@SlpCode",Convert.ToInt32(model.FooterData.SlpCode)),
                        //            new SqlParameter("@Comments",model.FooterData.Comments == null ? "" : model.FooterData.Comments.ToString()),
                        //        };
                        #endregion
                        res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, HeadQuery).ToInt();
                        if (res1 <= 0)
                        {
                            tran.Rollback();
                            return false;
                        }
                    }
                    if (model.ListItems != null)
                    {
                        int LineNo = 0;
                        foreach (var item in model.ListItems)
                        {
                            //int QUT1Id = CommonDal.getPrimaryKey(tran, "QUT1");
                            item.DicPrc = item.DicPrc == "" ? "NULL" : Convert.ToDecimal(item.DicPrc);

                            string RowQueryItem = @"insert into QUT1(Id,LineNum,WhsCode,ItemName,Price,LineTotal,OpenQty,ItemCode,Quantity,DiscPrcnt,VatGroup,UomCode,UomEntry,CountryOrg)
                                              values(" + Id + ","
                                              + LineNo + ",'"
                                              + item.Warehouse + "','"
                                              + item.ItemName + "',"
                                              + item.UPrc + ","
                                              + item.TtlPrc + ","
                                              + item.QTY + ",'"
                                              + item.ItemCode + "',"
                                              + item.QTY + ","
                                              + item.DicPrc + ",'"
                                              + item.VatGroup + "','"
                                              + item.UomCode + "',"
                                              + item.UomEntry + ",'"
                                              + item.CountryOrg + "')";

                            #region sqlparam
                            //List<SqlParameter> param2 = new List<SqlParameter>
                            //        {
                            //            new SqlParameter("@id",QUT1Id),
                            //            new SqlParameter("@ItemCode",item.ItemCode),
                            //            new SqlParameter("@Quantity",item.Quantity),
                            //            new SqlParameter("@DiscPrcnt",item.DiscPrcnt),
                            //            new SqlParameter("@VatGroup",item.VatGroup),
                            //            new SqlParameter("@UomCode",item.UomCode),
                            //            new SqlParameter("@CountryOrg",item.CountryOrg),
                            //        };
                            #endregion

                            int res2 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, RowQueryItem).ToInt();
                            if (res2 <= 0)
                            {
                                tran.Rollback();
                                return false;
                            }
                            LineNo += 1;
                        }



                    }
                    else if (model.ListService != null)
                    {
                        int LineNo = 0;

                        foreach (var item in model.ListService)
                        {
                            //int QUT1Id = CommonDal.getPrimaryKey(tran, "QUT1");
                            string RowQueryService = @"insert into QUT1(Id,LineNum,LineTotal,OpenQty,Dscription,AcctCode,VatGroup)
                                                   values(" + Id + ","
                                                    + LineNo + ","
                                                     + item.TotalLC + ","
                                                     + item.TotalLC + ",'"
                                                    + item.Dscription + "','"
                                                    + item.AcctCode + "','"
                                                    + item.VatGroup2 + "')";



                            #region sqlparam
                            //List<SqlParameter> param3 = new List<SqlParameter>
                            //            {
                            //                new SqlParameter("@id",QUT1Id),
                            //                new SqlParameter("@Dscription",item.Dscription),
                            //                new SqlParameter("@AcctCode",item.AcctCode),
                            //                new SqlParameter("@VatGroup",item.VatGroup),
                            //            };
                            #endregion


                            int res3 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, RowQueryService).ToInt();
                            if (res3 <= 0)
                            {
                                tran.Rollback();
                                return false;
                            }
                            LineNo += 1;
                        }
                    }
                    if (model.ListAttachment != null)
                    {


                        int LineNo = 0;
                        int ATC1Id = CommonDal.getPrimaryKey(tran, "AbsEntry", "ATC1");
                        foreach (var item in model.ListAttachment)
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
                                int res4 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, RowQueryAttachment).ToInt();
                                if (res4 <= 0)
                                {
                                    tran.Rollback();
                                    return false;

                                }
                                LineNo += 1;
                            }
                        }



                    }
                    if (res1 > 0)
                    {
                        tran.Commit();
                    }

                }
                catch (Exception)
                {
                    tran.Rollback();
                    return false;
                }

                return res1 > 0 ? true : false;

            }
            catch (Exception)
            {

                return false;
            }
        }






        public bool EditSalesQoutation(string formData)
        {
            try
            {
                var model = JsonConvert.DeserializeObject<dynamic>(formData);
                string DocType = model.ListItems == null ? "S" : "I";
                string mytable = "QUT1";

                SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();

                //string GetDocNum = CommonDal.getDocType(tran, "OQUT", model.ID.ToString());
                int res1 = 0;
                try
                {


                    var Status = CommonDal.Check_IsNotEditable(mytable, Convert.ToInt32(model.ID)) == false ? "Open" : "Closed";
                    if (Status == "Closed")
                    {
                        string HeadQuery = @" Update OQUT set NumAtCard = '" + model.HeaderData.NumAtCard + "'" +
                                                      ",Comments = '" + model.FooterData.Comments + "' " +
                                                      "WHERE Id = '" + model.ID + "'";

                        res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, HeadQuery).ToInt();
                        if (res1 <= 0)
                        {
                            tran.Rollback();
                            return false;
                        }
                    }
                    else
                    {

                        #region Deleting Items/List

                        //string DeleteI_Or_SQuery = "Delete from QUT1 Where id = "+model.ID;
                        //res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, DeleteI_Or_SQuery).ToInt();
                        //if (res1 <= 0)
                        //{
                        //    tran.Rollback();
                        //    return false;
                        //}                  
                        #endregion







                        //int Id = model.ID.ToInt();

                        if (model.HeaderData != null)
                        {

                            model.HeaderData.PurchaseType = model.HeaderData.PurchaseType == "" ? "NULL" : Convert.ToDecimal(model.HeaderData.PurchaseType);
                            model.HeaderData.TypeDetail = model.HeaderData.TypeDetail == "" || model.HeaderData.TypeDetail == null ? "NULL" : Convert.ToDecimal(model.HeaderData.TypeDetail);
                            model.HeaderData.ProductionOrderNo = model.HeaderData.ProductionOrderNo == "" ? "NULL" : Convert.ToDecimal(model.HeaderData.ProductionOrderNo);
                            model.HeaderData.ChallanNo = model.HeaderData.ChallanNo == "" ? "NULL" : Convert.ToDecimal(model.HeaderData.ChallanNo);
                            model.HeaderData.ContainerNo = model.HeaderData.ContainerNo == "" ? "NULL" : Convert.ToDecimal(model.HeaderData.ContainerNo);
                            model.HeaderData.ManualGatePassNo = model.HeaderData.ManualGatePassNo == "" ? "NULL" : Convert.ToDecimal(model.HeaderData.ManualGatePassNo);
                            model.HeaderData.SaleOrderNo = model.HeaderData.SaleOrderNo == "" ? "NULL" : Convert.ToInt32(model.HeaderData.SaleOrderNo);
                            model.HeaderData.Series = model.HeaderData.Series == null ? "NULL" : Convert.ToInt32(model.HeaderData.Series);
                            model.FooterData.Discount = model.FooterData.Discount == "" ? "NULL" : Convert.ToDecimal(model.FooterData.Discount);
                            string HeadQuery = @" Update OQUT set 
                                                          DocType = '" + DocType + "'" +
                                                        ",CardName = '" + model.HeaderData.CardName + "'" +
                                                        ",CntctCode = '" + model.HeaderData.CntcCode + "'" +
                                                        ",DocDate = '" + Convert.ToDateTime(model.HeaderData.DocDate) + "'" +
                                                        ",DocDueDate = '" + Convert.ToDateTime(model.HeaderData.DocDueDate) + "'" +
                                                        ",TaxDate = '" + Convert.ToDateTime(model.HeaderData.TaxDate) + "'" +
                                                        ",NumAtCard = '" + model.HeaderData.NumAtCard + "'" +
                                                        ",DocCur = '" + model.HeaderData.DocCur + "'" +
                                                        ",GroupNum = '" + model.ListAccouting.GroupNum + "'" +
                                                        ",SlpCode = " + model.FooterData.SlpCode + " , is_Edited = 1" +
                                                        ",Series = " + model.HeaderData.Series + "" +
                                                        ",DiscPrcnt = " + model.FooterData.Discount + "" +
                                                        ",PurchaseType = " + model.HeaderData.PurchaseType + "" +
                                                        ",TypeDetail = " + model.HeaderData.TypeDetail + "" +
                                                        ",ProductionOrderNo = " + model.HeaderData.ProductionOrderNo + "" +
                                                        ",ChallanNo = " + model.HeaderData.ChallanNo + "" +
                                                        ",ContainerNo = " + model.HeaderData.ContainerNo + "" +
                                                        ",ManualGatePassNo = " + model.HeaderData.ManualGatePassNo + "" +
                                                        ",SaleOrderNo = " + model.HeaderData.SaleOrderNo + "" +
                                                        ",Comments = '" + model.FooterData.Comments + "' " +
                                                        "WHERE Id = '" + model.ID + "'";

                            res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, HeadQuery).ToInt();
                            if (res1 <= 0)
                            {
                                tran.Rollback();
                                return false;
                            }
                        }




                        //var GetDocNum = SqlHelper.ExecuteScalar(tran, CommandType.Text, "Select DocType from ORDR where Id = " + model.Id + " ");





                        if (model.ListItems != null)
                        {

                            foreach (var item in model.ListItems)
                            {

                                //int QUT1Id = CommonDal.getPrimaryKey(tran, "QUT1");
                                if (item.LineNum != "" && item.LineNum != null)
                                {
                                    if (item.LineNum != "" && item.LineNum != null)
                                    {
                                        decimal OpenQty = Convert.ToDecimal(SqlHelper.ExecuteScalar(SqlHelper.defaultDB, CommandType.Text, "select OpenQty from " + mytable + " where Id=" + model.ID + " and LineNum=" + item.LineNum + ""));
                                        if (OpenQty > 0)
                                        {
                                            item.DicPrc = item.DicPrc == "" ? "NUll" : item.DicPrc;
                                            string UpdateQuery = @"update QUT1 set
                                                                      ItemCode  = '" + item.ItemCode + "'" +
                                                            ",ItemName  = '" + item.ItemName + "'" +
                                                            ",UomCode   = '" + item.UomCode + "'" +
                                                            ",Quantity  = " + item.QTY + "" +
                                                            ",OpenQty  =  " + item.QTY + "" +
                                                            ",Price     = '" + item.UPrc + "'" +
                                                            ",LineTotal = '" + item.TtlPrc + "'" +
                                                            ",DiscPrcnt = " + item.DicPrc + "" +
                                                            ",VatGroup  = '" + item.VatGroup + "'" +
                                                            ",CountryOrg= '" + item.CountryOrg + "'" +
                                                            " where Id=" + model.ID + " and LineNum=" + item.LineNum + " and OpenQty = Quantity";
                                            int res2 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, UpdateQuery).ToInt();
                                            if (res2 < 0)
                                            {
                                                tran.Rollback();
                                                return false;
                                            }
                                        }
                                    }

                                }
                                else
                                {
                                    int LineNo = CommonDal.getLineNumber(tran, "QUT1", model.ID.ToString());
                                    
                                    item.DicPrc = item.DicPrc == "" ? "NULL" : Convert.ToDecimal(item.DicPrc);

                                    string RowQueryItem = @"insert into QUT1(Id,LineNum,WhsCode,ItemName,Price,LineTotal,OpenQty,ItemCode,Quantity,DiscPrcnt,VatGroup,UomCode,UomEntry,CountryOrg)
                                              values(" + model.ID + ","
                                                      + LineNo + ",'"
                                                      + item.Warehouse + "','"
                                                      + item.ItemName + "',"
                                                      + item.UPrc + ","
                                                      + item.TtlPrc + ","
                                                      + item.QTY + ",'"
                                                      + item.ItemCode + "',"
                                                      + item.QTY + ","
                                                      + item.DicPrc + ",'"
                                                      + item.VatGroup + "','"
                                                      + item.UomCode + "',"
                                                      + item.UomEntry + ",'"
                                                      + item.CountryOrg + "')";

                                    int res2 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, RowQueryItem).ToInt();
                                    if (res2 <= 0)
                                    {
                                        tran.Rollback();
                                        return false;
                                    }

                                }
                            }



                        }
                        else if (model.ListService != null)
                        {
                            int LineNo = 0;

                            foreach (var item in model.ListService)
                            {
                                //int QUT1Id = CommonDal.getPrimaryKey(tran, "QUT1");
                                string RowQueryService = @"insert into QUT1(Id,LineNum,LineTotal,Dscription,AcctCode,VatGroup)
                                                  values(" + model.ID + ","
                                                       + LineNo + ","
                                                        + item.TotalLC + ",'"
                                                       + item.Dscription + "','"
                                                       + item.AcctCode + "','"
                                                       + item.VatGroup2 + "')";



                                int res3 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, RowQueryService).ToInt();
                                if (res3 <= 0)
                                {
                                    tran.Rollback();
                                    return false;

                                }
                                LineNo += 1;
                            }



                        }









                        if (model.ListAttachment != null)
                        {


                            int LineNo = 0;
                            int ATC1Id = CommonDal.getPrimaryKey(tran, "AbsEntry", "ATC1");
                            foreach (var item in model.ListAttachment)
                            {
                                if (item.selectedFilePath != "" && item.selectedFileName != "" && item.selectedFileDate != "")
                                {


                                    string RowQueryAttachment = @"insert into ATC1(AbsEntry,Line,trgtPath,FileName,Date)
                                                  values(" + ATC1Id + ","
                                                            + LineNo + ",'"
                                                            + item.selectedFilePath + "','"
                                                            + item.selectedFileName + "','"
                                                            + Convert.ToDateTime(item.selectedFileDate) + "')";

                                    int res4 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, RowQueryAttachment).ToInt();
                                    if (res4 <= 0)
                                    {
                                        tran.Rollback();
                                        return false;

                                    }
                                    LineNo += 1;
                                }
                            }



                        }

                    }
                    if (res1 > 0)
                    {
                        tran.Commit();
                    }
                }
                catch (Exception)
                {
                    tran.Rollback();
                    return false;
                }

                return res1 > 0 ? true : false;

            }
            catch (Exception)
            {

                return false;
            }

        }

    }
}