using iSOL_Enterprise.Common;
using iSOL_Enterprise.Models;
using iSOL_Enterprise.Models.sale;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using SAPbobsCOM;
using SqlHelperExtensions;
using System.Data;
using System.Data.SqlClient;

namespace iSOL_Enterprise.Dal.Sale
{
    public class DeliveryDal
    {


        public List<SalesQuotation_MasterModels> GetDeliveryData()
        {
            string GetQuery = "select * from ODLN order by id DESC";
            List<SalesQuotation_MasterModels> list = new List<SalesQuotation_MasterModels>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {

                    SalesQuotation_MasterModels models = new SalesQuotation_MasterModels();
                    models.DocStatus = CommonDal.Check_IsNotEditable("DLN1", rdr["Id"].ToInt()) == false ? "Open" : "Closed";
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

        public List<SalesQuotation_MasterModels> GetSalesOrderData(int cardcode)
        {
            string GetQuery = "select * from ORDR where CardCode =" + cardcode;


            List<SalesQuotation_MasterModels> list = new List<SalesQuotation_MasterModels>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {
                    SalesQuotation_MasterModels models = new SalesQuotation_MasterModels();

                    models.Id = rdr["Id"].ToInt();
                    models.DocDate = rdr["DocDueDate"].ToDateTime();
                    models.PostingDate = rdr["DocDate"].ToDateTime();
                    models.DocNum = rdr["DocNum"].ToString();
                    models.DocType = rdr["DocType"].ToString();
                    models.CardCode = rdr["CardCode"].ToString();
                    models.Guid = rdr["Guid"].ToString();
                    models.CardName = rdr["CardName"].ToString();
                    list.Add(models);
                }
            }
            return list;
        }
        public List<SalesQuotation_MasterModels> GetOrderType(int DocId)
        {
            string GetQuery = "select DocType,DocNum from ORDR where Id = " + DocId;
            List<SalesQuotation_MasterModels> list = new List<SalesQuotation_MasterModels>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {
                    SalesQuotation_MasterModels models = new SalesQuotation_MasterModels();


                    models.DocType = rdr["DocType"].ToString();
                    models.DocNum = rdr["DocNum"].ToString();
                    list.Add(models);
                }
            }
            return list;
        }
        public dynamic GetOrderItemServiceList(int DocId)
        {
            DataSet ds = new DataSet();
            SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
            SqlDataAdapter sda = new SqlDataAdapter("select Id,LineNum,ItemCode,Quantity,DiscPrcnt,VatGroup,UomCode,CountryOrg,Dscription,AcctCode,OpenQty from RDR1 where id = " + DocId + "", conn);
            sda.Fill(ds);
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(ds.Tables);
            return JSONString;

        }
        public dynamic GetDeliveryDetails(int id)
        {
            DataSet ds = new DataSet();
            SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
            SqlDataAdapter sda = new SqlDataAdapter("select * from ODLN where id = " + id + ";select * from DLN1 where id = " + id + "", conn);
            sda.Fill(ds);
            return ds;
        }

        public List<tbl_OWHS> GetWareHouseData()
        {
            string GetQuery = "select WhsCode , WhsName = WhsName + ' (' + WhsCode + ')' from OWHS order by WhsCode";


            List<tbl_OWHS> list = new List<tbl_OWHS>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {

                    list.Add(
                        new tbl_OWHS()
                        {
                            whscode = rdr["WhsCode"].ToString(),
                            whsname = rdr["WhsName"].ToString()

                        });

                }
            }

            return list;
        }
        public List<tbl_OBTN> GetBatchList(string itemcode, string warehouse)
        {
            try
            {


                string GetQuery = "select OBTN.DistNumber,OBTN.ExpDate,OBTQ.Quantity,OBTN.InDate,OBTQ.AbsEntry,OBTQ.SysNumber,OBTQ.ItemCode,OBTQ.WhsCode  from OBTQ Inner join OBTN on OBTN.AbsEntry = OBTQ.MdAbsEntry where OBTQ.ItemCode = '" + itemcode + "' and OBTQ.WhsCode = '" + warehouse + "' and OBTQ.Quantity <> 0";


                List<tbl_OBTN> list = new List<tbl_OBTN>();
                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
                {
                    while (rdr.Read())
                    {

                        list.Add(
                            new tbl_OBTN()
                            {
                                AbsEntry = Convert.ToInt32(rdr["AbsEntry"]),
                                DistNumber = rdr["DistNumber"].ToString(),
                                Quantity = rdr["Quantity"].ToString() == "" ? 0 : Convert.ToInt32(rdr["Quantity"]),
                                InDate = Convert.ToDateTime(rdr["InDate"]),
                                ExpDate = rdr["ExpDate"].ToString() == "" ? null : Convert.ToDateTime(rdr["ExpDate"]),
                                SysNumber = Convert.ToInt32(rdr["SysNumber"])
                            });

                    }
                }

                return list;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool AddDelivery(string formData)
        {
            try
            {
                var model = JsonConvert.DeserializeObject<dynamic>(formData);
                CommonDal dal = new CommonDal();
                string DocType = model.ListItems == null ? "S" : "I";


                SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                int res1 = 0;
                try
                {

                    int Id = CommonDal.getPrimaryKey(tran, "ODLN");
                    string DocNum = SqlHelper.getUpdatedDocumentNumberOnLoad(tran, "ODLN", "DV");
                    if (model.HeaderData != null)
                    {

                        model.HeaderData.PurchaseType = model.HeaderData.PurchaseType == "" ? "NULL" : Convert.ToDecimal(model.HeaderData.PurchaseType);
                        model.HeaderData.TypeDetail = model.HeaderData.TypeDetail == "" ? "NULL" : Convert.ToDecimal(model.HeaderData.TypeDetail);
                        model.HeaderData.ProductionOrderNo = model.HeaderData.ProductionOrderNo == "" ? "NULL" : Convert.ToDecimal(model.HeaderData.ProductionOrderNo);
                        model.HeaderData.ChallanNo = model.HeaderData.ChallanNo == "" ? "NULL" : Convert.ToDecimal(model.HeaderData.ChallanNo);
                        model.HeaderData.ContainerNo = model.HeaderData.ContainerNo == "" ? "NULL" : Convert.ToDecimal(model.HeaderData.ContainerNo);
                        model.HeaderData.ManualGatePassNo = model.HeaderData.ManualGatePassNo == "" ? "NULL" : Convert.ToDecimal(model.HeaderData.ManualGatePassNo);
                        model.HeaderData.SaleOrderNo = model.HeaderData.SaleOrderNo == "" ? "NULL" : Convert.ToInt32(model.HeaderData.SaleOrderNo);
                        model.HeaderData.Series = model.HeaderData.Series == null ? "NULL" : Convert.ToInt32(model.HeaderData.Series);
                        model.FooterData.Discount = model.FooterData.Discount == "" ? "NULL" : Convert.ToDecimal(model.FooterData.Discount);

                        string HeadQuery = @"insert into ODLN(Id,Series,DocType,Guid,CardCode,DocNum,CardName,CntctCode,DocDate,NumAtCard,DocDueDate,DocCur,TaxDate , GroupNum,DocTotal , SlpCode , DiscPrcnt,
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

                            item.BaseEntry = item.BaseEntry == "" ? "NULL" : Convert.ToInt32(item.BaseEntry);
                            item.BaseLine = item.BaseLine == "" ? "NULL" : Convert.ToInt32(item.BaseLine);
                            item.BaseQty = item.BaseQty == "" ? "NULL" : Convert.ToInt32(item.BaseQty);
                            item.DicPrc = item.DicPrc == "" ? "NULL" : Convert.ToDecimal(item.DicPrc);
                            item.BaseType = item.BaseType == "" ? "NULL" : Convert.ToInt32(item.BaseType);


                            int LogEntry = CommonDal.getPrimaryKey(tran, "LogEntry", "OITL");
                            //int QUT1Id = CommonDal.getPrimaryKey(tran, "DLN1");
                            #region UpdateWarehouse&GenerateLog


                            #region OITLLog
                            string LogQueryOITL = @"insert into OITL(LogEntry,CardCode,ItemCode,ItemName,CardName,DocEntry,DocLine,DocType,BaseType,DocNum,DocQty,DocDate) 
                                           values(" + LogEntry + ",'"
                                              //+ DocType + "','"
                                              + model.HeaderData.CardCode + "','"
                                              + item.ItemCode + "','"
                                              + item.ItemName + "','"
                                              + model.HeaderData.CardName + "',"
                                              + Id + ","
                                              + LineNo + ","
                                              + 15 + ","
                                              + item.BaseType + ","
                                              + Id + ","
                                              + -1 * (decimal)item.QTY + ",'"
                                              // + Convert.ToDateTime(DateTime.Now) + "','"
                                              + Convert.ToDateTime(model.HeaderData.DocDate) + "')";

                            res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, LogQueryOITL).ToInt();
                            if (res1 <= 0)
                            {
                                tran.Rollback();
                                return false;
                            }

                            #endregion


                            #region Bataches & Logs working

                            if (model.Batches != null)
                            {

                                foreach (var batch in model.Batches)
                                {

                                    foreach (var ii in batch)
                                    {

                                        if (ii.itemno == item.ItemCode)
                                        {
                                            int count = Convert.ToInt32(SqlHelper.ExecuteScalar(tran, CommandType.Text, "Select Count(*) from OBTQ Where AbsEntry = " + ii.AbsEntry));

                                            #region Record not found in OBTQ
                                            if (count == 0)
                                            {
                                                string GetQuery = @"select OBTN.AbsEntry,OBTN.SysNumber,OBTN.ExpDate,OBTN.Quantity,OBTN.DistNumber,OBTN.ExpDate,OBTN.InDate, OBTQ.Quantity as obtqQuantity , OBTQ.MdAbsEntry from OBTQ " +
                                                                   "Inner join OBTN on OBTN.AbsEntry = OBTQ.MdAbsEntry " +
                                                                   "where OBTQ.ItemCode = '" + ii.itemno + "' and OBTQ.WhsCode = '" + ii.whseno + "' and OBTQ.SysNumber = '" + ii.SysNumber + "'";
                                                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
                                                {
                                                    while (rdr.Read())
                                                    {

                                                        string? ExpDate = rdr["ExpDate"].ToString() == "" ? "" : Convert.ToDateTime(rdr["ExpDate"]).ToString();
                                                        string? InDate = rdr["InDate"].ToString() == "" ? "" : Convert.ToDateTime(rdr["InDate"]).ToString();

                                                        string InsertBatchQuery = @"insert into OBTN(AbsEntry,ItemCode,SysNumber,DistNumber,ExpDate,InDate,Quantity)
                                                                    values(" + Convert.ToInt32(rdr["AbsEntry"]) + ",'"
                                                                   + ii.itemno + "',"
                                                                   + Convert.ToInt32(rdr["SysNumber"]) + ",'"
                                                                   + ii.DistNumber + "','"
                                                                   + ExpDate + "','"
                                                                   + InDate + "',"
                                                                   + rdr["Quantity"].ToDecimal() + ");" +

                                                                   " insert into OBTQ(AbsEntry,ItemCode,SysNumber,WhsCode,Quantity,MdAbsEntry) " +
                                                                   "values (" + ii.AbsEntry + ",'"
                                                                   + ii.itemno + "',"
                                                                   + ii.SysNumber + ",'"
                                                                   + ii.whseno + "',"
                                                                   + ((decimal)ii.Quantity - (decimal)ii.selectqty) + ","
                                                                   + Convert.ToInt32(rdr["AbsEntry"]) + ")";


                                                        res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, InsertBatchQuery).ToInt();
                                                        if (res1 <= 0)
                                                        {
                                                            tran.Rollback();
                                                            return false;
                                                        }


                                                    }
                                                }

                                            }
                                            #endregion

                                            #region Record found in OBTQ
                                            else
                                            {
                                                string BatchQueryOBTN = @"Update OBTQ set Quantity = " + ((decimal)ii.Quantity - (decimal)ii.selectqty) + " WHERE AbsEntry = " + ii.AbsEntry + "";

                                                res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, BatchQueryOBTN).ToInt();
                                                if (res1 <= 0)
                                                {
                                                    tran.Rollback();
                                                    return false;
                                                }
                                            }
                                            #endregion

                                            #region ITL1 log
                                            string LogQueryITL1 = @"insert into ITL1(LogEntry,ItemCode,SysNumber,Quantity,AllocQty,MdAbsEntry) 
                                                   values(" + LogEntry + ",'"
                                                 + item.ItemCode + "','"
                                                 + ii.SysNumber + "',"
                                                 + -1 * (decimal)ii.selectqty + ","
                                                 + -1 * (decimal)ii.selectqty + ","
                                                 + ii.AbsEntry + ")";


                                            res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, LogQueryITL1).ToInt();
                                            if (res1 <= 0)
                                            {
                                                tran.Rollback();
                                                return false;
                                            }
                                            #endregion
                                        }
                                        else break;

                                    }

                                }
                            }
                            #endregion


                            #endregion


                            #region Update Base Documnet
                            if ((int)model.BaseType != -1 && item.BaseEntry.ToString() != "" && item.BaseLine.ToString() != "")
                            {
                                string table = dal.GetRowTable(Convert.ToInt32(model.BaseType));
                                string Updatequery = @"Update " + table + " set OpenQty = OpenQty - " + item.QTY + " where Id =" + item.BaseEntry + " and LineNum = " + item.BaseLine + "and ItemCode = '" + item.ItemCode + "'";
                                int res = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, Updatequery).ToInt();
                                if (res < 0)
                                {
                                    tran.Rollback();
                                    return false;
                                }
                            }
                            #endregion




                            #region Insert into Row 
                            string RowQueryItem = @"insert into DLN1(Id,WhsCode,LineNum,BaseRef,BaseEntry,BaseLine,BaseQty,BaseType,ItemName,Price,LineTotal,OpenQty,ItemCode,Quantity,DiscPrcnt,VatGroup , UomCode,UomEntry ,CountryOrg)
                                              values(" + Id + ",'"
                                                + item.Warehouse + "',"
                                                + LineNo + ",'"
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

                            #region Update OITW If Sap Integration is OFF

                            if (!SqlHelper.SAPIntegration)
                            {
                                string UpdateOITWQuery = @"Update OITW set onHand = onHand - @Quantity where WhsCode = '" + item.Warehouse + "' and ItemCode = '" + item.ItemCode + "'";
                                List<SqlParameter> param2 = new List<SqlParameter>();
                                param2.Add(dal.GetParameter("@Quantity", item.QTY, typeof(decimal)));
                                res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, UpdateOITWQuery, param2.ToArray()).ToInt();
                                if (res1 <= 0)
                                {
                                    tran.Rollback();
                                    return false;
                                }
                            }

                            #endregion

                            LineNo += 1;
                            #endregion
                        }



                    }
                    else if (model.ListService != null)
                    {

                        int LineNo = 0;
                        foreach (var item in model.ListService)
                        {
                            //int QUT1Id = CommonDal.getPrimaryKey(tran, "DLN1");
                            item.BaseEntry2 = item.BaseEntry2 == "" ? "NULL" : Convert.ToInt32(item.BaseEntry2);
                            item.BaseLine2 = item.BaseLine2 == "" ? "NULL" : Convert.ToInt32(item.BaseLine2);

                            string RowQueryService = @"insert into DLN1(Id,LineNum,BaseRef,BaseEntry,BaseLine,BaseType,LineTotal,Dscription,AcctCode,VatGroup)
                                                  values(" + Id + ","
                                                     + LineNo + ",'"
                                                     + item.BaseRef2 + "',"
                                                     + item.BaseEntry2 + ","
                                                     + item.BaseLine2 + ","
                                                     + model.BaseType + ",'"
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
                            else
                                break;
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





        public bool EditDelivery(string formData)
        {
            try
            {
                var model = JsonConvert.DeserializeObject<dynamic>(formData);
                string DocType = model.ListItems == null ? "S" : "I";
                CommonDal dal = new CommonDal();
                string mytable = "DLN1";
                SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                int res1 = 0;
                try
                {

                    var Status = CommonDal.Check_IsNotEditable(mytable, Convert.ToInt32(model.ID)) == false ? "Open" : "Closed";
                    if (Status == "Closed")
                    {
                        string HeadQuery = @" Update ODLN set NumAtCard = '" + model.HeaderData.NumAtCard + "'" +
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



                        //string DeleteI_Or_SQuery = "Delete from DLN1 Where id = " + model.ID;
                        //res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, DeleteI_Or_SQuery).ToInt();
                        //if (res1 <= 0)
                        //{
                        //    tran.Rollback();
                        //    return false;
                        //}


                        #endregion

                        //int Id = CommonDal.getPrimaryKey(tran, "ODLN");
                        if (model.HeaderData != null)
                        {
                            model.HeaderData.PurchaseType = model.HeaderData.PurchaseType == "" ? "NULL" : Convert.ToDecimal(model.HeaderData.PurchaseType);
                            model.HeaderData.TypeDetail = model.HeaderData.TypeDetail == "" ? "NULL" : Convert.ToDecimal(model.HeaderData.TypeDetail);
                            model.HeaderData.ProductionOrderNo = model.HeaderData.ProductionOrderNo == "" ? "NULL" : Convert.ToDecimal(model.HeaderData.ProductionOrderNo);
                            model.HeaderData.ChallanNo = model.HeaderData.ChallanNo == "" ? "NULL" : Convert.ToDecimal(model.HeaderData.ChallanNo);
                            model.HeaderData.ContainerNo = model.HeaderData.ContainerNo == "" ? "NULL" : Convert.ToDecimal(model.HeaderData.ContainerNo);
                            model.HeaderData.ManualGatePassNo = model.HeaderData.ManualGatePassNo == "" ? "NULL" : Convert.ToDecimal(model.HeaderData.ManualGatePassNo);
                            model.HeaderData.SaleOrderNo = model.HeaderData.SaleOrderNo == "" ? "NULL" : Convert.ToInt32(model.HeaderData.SaleOrderNo);
                            model.HeaderData.Series = model.HeaderData.Series == null ? "NULL" : Convert.ToInt32(model.HeaderData.Series);
                            model.FooterData.Discount = model.FooterData.Discount == "" ? "NULL" : Convert.ToDecimal(model.FooterData.Discount);

                            string HeadQuery = @" Update ODLN set 
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
                        if (model.ListItems != null)
                        {
                            foreach (var item in model.ListItems)
                            {
                                item.DicPrc = item.DicPrc == "" ? "NULL" : Convert.ToDecimal(item.DicPrc);

                                if (item.LineNum != "" && item.LineNum != null)
                                {
                                    decimal OpenQty = Convert.ToDecimal(SqlHelper.ExecuteScalar(SqlHelper.defaultDB, CommandType.Text, "select OpenQty from " + mytable + " where Id=" + model.ID + " and LineNum=" + item.LineNum + ""));
                                    if (OpenQty > 0)
                                    {
                                        string oldDataQuery = @"select BaseEntry,BaseType ,BaseLine,Quantity from DLN1 where Id=" + model.ID + " and LineNum=" + item.LineNum + " and OpenQty <> 0";

                                        tbl_docRow docRowModel = new tbl_docRow();
                                        using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, oldDataQuery))
                                        {
                                            while (rdr.Read())
                                            {


                                                docRowModel.BaseEntry = rdr["BaseEntry"].ToString() == "" ? null : Convert.ToDecimal(rdr["BaseEntry"]);
                                                docRowModel.BaseLine = rdr["BaseLine"].ToString() == "" ? null : Convert.ToDecimal(rdr["BaseLine"]);
                                                docRowModel.Quantity = rdr["Quantity"].ToString() == "" ? null : Convert.ToDecimal(rdr["Quantity"]);
                                                docRowModel.BaseType = rdr["BaseType"].ToString() == "" ? null : Convert.ToDecimal(rdr["BaseType"]);

                                            }
                                        }
                                        #region if doc contains base ref
                                        if (docRowModel.BaseEntry != null)
                                        {
                                            string table = dal.GetRowTable(Convert.ToInt32(docRowModel.BaseType));
                                            string Updatequery = @"Update " + table + " set OpenQty =(OpenQty + " + docRowModel.Quantity + ") - " + item.QTY + " where Id =" + docRowModel.BaseEntry + "and LineNum =" + docRowModel.BaseLine;
                                            int res = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, Updatequery).ToInt();
                                            if (res <= 0)
                                            {
                                                tran.Rollback();
                                                return false;
                                            }
                                        }
                                        #endregion
                                        //item.DicPrc = item.DicPrc == "" ? "null" : item.DicPrc;
                                        string UpdateQuery = @"update DLN1 set
                                                         ItemCode  = '" + item.ItemCode + "'" +
                                                                ",ItemName  = '" + item.ItemName + "'" +
                                                                ",UomCode   = '" + item.UomCode + "'" +
                                                                ",UomEntry   = " + item.UomEntry +
                                                                ",Quantity  = '" + item.QTY + "'" +
                                                                ",OpenQty   = OpenQty + (" + item.QTY + "- OpenQty)" +
                                                                ",Price     = '" + item.UPrc + "'" +
                                                                ",LineTotal = '" + item.TtlPrc + "'" +
                                                                ",DiscPrcnt = " + item.DicPrc +
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
                                #region New Row added
                                else
                                {
                                    int LineNo = CommonDal.getLineNumber(tran, "DLN1", model.ID.ToString());

                                    string RowQueryItem = @"insert into DLN1(Id,LineNum,ItemName,Price,LineTotal,ItemCode,Quantity,OpenQty,DiscPrcnt,VatGroup, UomCode ,UomEntry ,CountryOrg)
                                              values(" + model.ID + ","
                                                  + LineNo + ",'"
                                                  + item.ItemName + "',"
                                                  + item.UPrc + ","
                                                  + item.TtlPrc + ",'"
                                                  + item.ItemCode + "',"
                                                  + item.QTY + ","
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
                                    #region Update OITW If Sap Integration is OFF

                                    if (!SqlHelper.SAPIntegration)
                                    {
                                        string UpdateOITWQuery = @"Update OITW set onHand = onHand - @Quantity where WhsCode = '" + item.Warehouse + "' and ItemCode = '" + item.ItemCode + "'";
                                        List<SqlParameter> param2 = new List<SqlParameter>();
                                        param2.Add(dal.GetParameter("@Quantity", item.QTY, typeof(decimal)));
                                        res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, UpdateOITWQuery, param2.ToArray()).ToInt();
                                        if (res1 <= 0)
                                        {
                                            tran.Rollback();
                                            return false;
                                        }
                                    }

                                    #endregion

                                }
                                #endregion
                            }

                        }
                        else if (model.ListService != null)
                        {

                            int LineNo = 0;
                            foreach (var item in model.ListService)
                            {
                                //int QUT1Id = CommonDal.getPrimaryKey(tran, "DLN1");

                                string RowQueryService = @"insert into DLN1(Id,LineNum,LineTotal,Dscription,AcctCode,VatGroup)
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
