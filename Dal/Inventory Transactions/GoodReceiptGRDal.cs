using iSOL_Enterprise.Models.sale;
using SqlHelperExtensions;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;
using iSOL_Enterprise.Common;

namespace iSOL_Enterprise.Dal.Inventory_Transactions
{
    public class GoodReceiptGRDal
    {

        public bool AddGoodReceiptGR(string formData)
        {
            try
            {
                var model = JsonConvert.DeserializeObject<dynamic>(formData);
                

                SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                int res1 = 0;
                try
                {
                    int Id = CommonDal.getPrimaryKey(tran, "OPDN");
                    string DocNum = SqlHelper.getUpdatedDocumentNumberOnLoad(tran, "OPDN", "GR");
                    if (model.HeaderData != null)
                    {

                        model.HeaderData.PurchaseType = model.HeaderData.PurchaseType == "" ? "NULL" : Convert.ToDecimal(model.HeaderData.PurchaseType);
                        model.HeaderData.TypeDetail = model.HeaderData.TypeDetail == "" ? "NULL" : Convert.ToDecimal(model.HeaderData.TypeDetail);
                        model.HeaderData.ProductionOrderNo = model.HeaderData.ProductionOrderNo == "" ? "NULL" : Convert.ToDecimal(model.HeaderData.ProductionOrderNo);
                        model.HeaderData.ChallanNo = model.HeaderData.ChallanNo == "" ? "NULL" : Convert.ToDecimal(model.HeaderData.ChallanNo);
                        model.HeaderData.DONo = model.HeaderData.DONo == "" ? "NULL" : Convert.ToDecimal(model.HeaderData.DONo);
                        model.HeaderData.SaleOrderNo = model.HeaderData.SaleOrderNo == "" ? "NULL" : Convert.ToInt32(model.HeaderData.SaleOrderNo);
                        model.HeaderData.Series = model.HeaderData.Series == null ? "NULL" : Convert.ToInt32(model.HeaderData.Series);

                        string HeadQuery = @"insert into OPDN(Id,Series,DocType,Guid,CardCode,DocNum,CardName,CntctCode,DocDate,NumAtCard,DocDueDate,DocCur,TaxDate , GroupNum,DocTotal , SlpCode ,
                                            PurchaseType,TypeDetail,ProductionOrderNo,ChallanNo,DONo,SaleOrderNo, Comments)
                                           values(" + Id + ","
                                                + model.HeaderData.Series + ",'"
                                                + model.HeaderData.DocType + "','"
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
                                                + model.HeaderData.PurchaseType + ","
                                                + model.HeaderData.TypeDetail + ","
                                                + model.HeaderData.ProductionOrderNo + ","
                                                + model.HeaderData.ChallanNo + ","
                                                + model.HeaderData.DONo + ","
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
                        CommonDal dal = new CommonDal();
                        int LineNo = 0;
                        foreach (var item in model.ListItems)
                        {



                            #region If Doc copied data from other Doc
                            if (model.BaseType != -1 && item.BaseEntry != "" && item.BaseLine != "")
                            {
                                string table = dal.GetRowTable(Convert.ToInt32(model.BaseType));
                                string Updatequery = @"Update " + table + " set OpenQty =OpenQty - " + item.QTY + " where Id =" + item.BaseEntry + "and LineNum =" + item.BaseLine + "and ItemCode = '" + item.ItemCode + "'";
                                int res = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, Updatequery).ToInt();
                                if (res < 0)
                                {
                                    tran.Rollback();
                                    return false;
                                }
                            }
                            #endregion

                            item.BaseEntry = item.BaseEntry == "" ? "NULL" : Convert.ToInt32(item.BaseEntry);
                            item.BaseLine = item.BaseLine == "" ? "NULL" : Convert.ToInt32(item.BaseLine);
                            item.BaseQty = item.BaseQty == "" ? "NULL" : Convert.ToInt32(item.BaseQty);
                            item.DicPrc = item.DicPrc == "" ? "NULL" : Convert.ToDecimal(item.DicPrc);

                            #region Insert in Rows
                            string RowQueryItem = @"insert into PDN1(Id,LineNum,WhsCode,BaseRef,BaseEntry,BaseLine,BaseQty,BaseType,ItemName,Price,LineTotal,OpenQty,ItemCode,Quantity,DiscPrcnt,VatGroup , UomCode,UomEntry ,CountryOrg)
                                              values(" + Id + ","
                                           + LineNo + ",'"
                                            + item.Warehouse + "','"
                                           + item.BaseRef + "',"
                                           + item.BaseEntry + ","
                                           + item.BaseLine + ","
                                           + item.BaseQty + ","
                                           + model.BaseType + ",'"
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
                            #endregion

                            #region OITL Log
                            int LogEntry = CommonDal.getPrimaryKey(tran, "LogEntry", "OITL");   //Primary Key

                            string LogQueryOITL = @"insert into OITL(LogEntry,CardCode,ItemCode,ItemName,CardName,DocEntry,DocLine,DocType,DocNum,DocQty,DocDate) 
                                           values(" + LogEntry + ",'"
                                              + model.HeaderData.CardCode + "','"
                                              + item.ItemCode + "','"
                                              + item.ItemName + "','"
                                              + model.HeaderData.CardName + "',"
                                              + Id + ","
                                              + LineNo + ","
                                              + 0 + ", "
                                              + Id + " ,"
                                              + ((Decimal)(item.QTY)) + ",'"
                                              + Convert.ToDateTime(model.HeaderData.DocDate) + "')";

                            res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, LogQueryOITL).ToInt();
                            if (res1 <= 0)
                            {
                                tran.Rollback();
                                return false;
                            }

                            #endregion

                            #region Batches & Log Working

                            if (model.Batches != null)
                            {

                                foreach (var batch in model.Batches)
                                {

                                    if (batch[0].itemno == item.ItemCode)
                                    {


                                        foreach (var ii in batch)
                                        {


                                            string itemno = ii.itemno;
                                            int SysNumber = CommonDal.getSysNumber(tran, itemno);
                                            int AbsEntry = CommonDal.getPrimaryKey(tran, "AbsEntry", "OBTN");   //Primary Key
                                           // tbl_OBTN OldBatchData = GetBatchList(itemno, ii.DistNumber);
                                            //if (OldBatchData.AbsEntry > 0)
                                            //{
                                            //    #region Update OBTQ

                                            //    string BatchQueryOBTN = @"Update OBTQ set Quantity = Quantity +" + ((Decimal)(ii.BQuantity)) + " WHERE AbsEntry = " + OldBatchData.AbsEntry;

                                            //    res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, BatchQueryOBTN).ToInt();
                                            //    if (res1 <= 0)
                                            //    {
                                            //        tran.Rollback();
                                            //        return false;
                                            //    }
                                            //    SysNumber = OldBatchData.SysNumber;
                                            //    AbsEntry = OldBatchData.MdAbsEntry;
                                            //    #endregion
                                            //}
                                            //else
                                            //{

                                            //    #region Insert in OBTN
                                            //    string BatchQueryOBTN = @"insert into OBTN(AbsEntry,ItemCode,SysNumber,DistNumber,InDate,ExpDate)
                                            //                            values(" + AbsEntry + ",'"
                                            //                            + itemno + "',"
                                            //                            + SysNumber + ",'"
                                            //                            + ii.DistNumber + "','"
                                            //                            + DateTime.Now + "','"
                                            //                            + Convert.ToDateTime(batch[0].ExpDate) + "')";


                                            //    res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, BatchQueryOBTN).ToInt();
                                            //    if (res1 <= 0)
                                            //    {
                                            //        tran.Rollback();
                                            //        return false;
                                            //    }
                                            //    #endregion


                                            //    #region Insert in OBTQ
                                            //    int ObtqAbsEntry = CommonDal.getPrimaryKey(tran, "AbsEntry", "OBTQ");
                                            //    string BatchQueryOBTQ = @"insert into OBTQ(AbsEntry,MdAbsEntry,ItemCode,SysNumber,WhsCode,Quantity)
                                            //                            values(" + ObtqAbsEntry + ","
                                            //                            + AbsEntry + ",'"
                                            //                            + ii.itemno + "',"
                                            //                            + SysNumber + ",'"
                                            //                            + ii.whseno + "',"
                                            //                            + ii.BQuantity + ")";

                                            //    res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, BatchQueryOBTQ).ToInt();
                                            //    if (res1 <= 0)
                                            //    {
                                            //        tran.Rollback();
                                            //        return false;
                                            //    }
                                            //    #endregion
                                            //}

                                            #region Insert in ITL1
                                            string LogQueryITL1 = @"insert into ITL1(LogEntry,ItemCode,SysNumber,Quantity,OrderedQty,MdAbsEntry) 
                                                   values(" + LogEntry + ",'"
                                                 + ii.itemno + "','"
                                                 + SysNumber + "',"
                                                 + ii.BQuantity + ","
                                                 + ((Decimal)(ii.BQuantity)) + ","
                                                 + AbsEntry + ")";


                                            res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, LogQueryITL1).ToInt();
                                            if (res1 <= 0)
                                            {
                                                tran.Rollback();
                                                return false;
                                            }
                                            #endregion
                                        }

                                    }
                                    else
                                        break;

                                }
                            }
                            #endregion



                            LineNo += 1;
                        }



                    }
                    else if (model.ListService != null)
                    {
                        int LineNo = 0;

                        foreach (var item in model.ListService)
                        {

                            item.BaseEntry = item.BaseEntry == "" ? "null" : item.BaseEntry;
                            item.BaseLine = item.BaseLine == "" ? "null" : item.BaseLine;
                            item.BaseQty = item.BaseQty == "" ? "null" : item.BaseQty;
                            //int QUT1Id = CommonDal.getPrimaryKey(tran, "QUT1");
                            string RowQueryService = @"insert into PDN1(Id,LineNum,BaseRef,BaseEntry,BaseLine,BaseType,LineTotal,OpenQty,Dscription,AcctCode,VatGroup)
                                                  values(" + Id + ","
                                                    + LineNo + ",'"
                                                    + item.BaseRef2 + "',"
                                                    + item.BaseEntry2 + ","
                                                    + item.BaseLine2 + ","
                                                    + model.BaseType + ",'"
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
    }
}
