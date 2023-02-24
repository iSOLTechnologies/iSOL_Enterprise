using iSOL_Enterprise.Common;
using iSOL_Enterprise.Models;
using iSOL_Enterprise.Models.sale;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using SAPbobsCOM;
using SqlHelperExtensions;
using System.Data;
using System.Data.SqlClient;

namespace iSOL_Enterprise.Dal
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
                    models.DocStatus = CommonDal.Check_IsEditable("INV1", rdr["Id"].ToInt()) == false ? "Open" : "Closed";
                    models.Id = rdr["Id"].ToInt();
                    models.DocDate = rdr["DocDueDate"].ToDateTime();
                    models.PostingDate = rdr["DocDate"].ToDateTime();
                    models.DocNum = rdr["DocNum"].ToString();
                    models.CardCode = rdr["CardCode"].ToString();
                    models.Guid = rdr["Guid"].ToString();
                    models.CardName = rdr["CardName"].ToString();
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
            JSONString = Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables);
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
            string GetQuery = "select WhsCode , WhsName = WhsName + ' (' + WhsCode + ')' from OWHS";


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

            
            string GetQuery = "select OBTN.DistNumber,OBTN.ExpDate,OBTN.Quantity,OBTN.InDate,OBTN.AbsEntry,OBTN.SysNumber  from OBTW Inner join OBTN on OBTN.AbsEntry = OBTW.AbsEntry where OBTW.ItemCode = '" + itemcode+"' and OBTW.WhsCode = '"+warehouse+"'";


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
                            InDate = Convert.ToDateTime( rdr["InDate"]),
                            ExpDate = rdr["ExpDate"].ToString() == "" ? null : Convert.ToDateTime(rdr["ExpDate"]) ,
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
                        

                        string HeadQuery = @"insert into ODLN(Id,DocType,Guid,CardCode,DocNum,CardName,CntctCode,DocDate,NumAtCard,DocDueDate,DocCur,TaxDate , GroupNum , SlpCode , Comments) 
                                           values(" + Id + ",'"
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
                                                + Convert.ToInt32(model.FooterData.SlpCode) + ",'"
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
                        int LineNo = 1;
                        
                        foreach (var item in model.ListItems)
                        {
                        int LogEntry = CommonDal.getPrimaryKey(tran, "LogEntry", "OITL");
                            //int QUT1Id = CommonDal.getPrimaryKey(tran, "DLN1");
                            #region UpdateWarehouse&GenerateLog


                            #region OITLLog
                            string LogQueryOITL = @"insert into OITL(LogEntry,CardCode,ItemCode,ItemName,CardName,DocEntry,DocLine,DocType,DocNum,DocQty,DocDate) 
                                           values(" + LogEntry + ",'"
                                              //+ DocType + "','"
                                              + model.HeaderData.CardCode + "','"
                                              + item.ItemCode + "','"
                                              + item.ItemName + "','"
                                              + model.HeaderData.CardName + "',"
                                              + Id + ","
                                              + LineNo + ","
                                              + 0 + ", "
                                              + Id + " ,"
                                              + -1 * ((Decimal)(item.QTY)) + ",'"
                                              // + Convert.ToDateTime(DateTime.Now) + "','"
                                              + Convert.ToDateTime(model.HeaderData.DocDate) + "')";

                            res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, LogQueryOITL).ToInt();
                            if (res1 <= 0)
                            {
                                tran.Rollback();
                                return false;
                            }

                            #endregion


                            #region Update_OBTN & ITL1Log


                            if (model.Batches != null)
                            {

                                foreach (var batch in model.Batches)
                                {

                                    foreach (var ii in batch)
                                    {
                                       
                                        if (ii.itemno == item.ItemCode)
                                        {
                                            int count = Convert.ToInt32(SqlHelper.ExecuteScalar(tran, CommandType.Text, "Select Count(*) from OBTN Where AbsEntry = "+ ii.AbsEntry));
                                            string BatchQueryOBTN = count == 0 ?
                                                 @"insert into OBTN(AbsEntry,ItemCode,SysNumber,DistNumber,ExpDate,Quantity)
                                                                    values(" + ii.AbsEntry + ",'"
                                                                    + ii.itemno + "',"
                                                                    + ii.SysNumber + ",'"
                                                                    + ii.DistNumber + "','"
                                                                    + Convert.ToDateTime(ii.ExpDate) + "',"
                                                                    + ((Decimal)(ii.Quantity) - (Decimal)(ii.selectqty)) + "); insert into OBTW(AbsEntry,ItemCode,SysNumber,WhsCode,DataSource) values(" + ii.AbsEntry + ",'" + ii.itemno + "'," + ii.SysNumber + ",'" + ii.whseno + "','" + "N')" 
                                                : @"Update OBTN set Quantity = " + ((Decimal)(ii.Quantity) - (Decimal)(ii.selectqty)) + " WHERE AbsEntry = " + ii.AbsEntry + "";

                                            //string BatchQueryOBTN = @" Update OBTN set Quantity = " + ((Decimal)(ii.Quantity) - (Decimal)(ii.selectqty)) + " WHERE AbsEntry = " + ii.AbsEntry + "";


                                            res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, BatchQueryOBTN).ToInt();
                                            if (res1 <= 0)
                                            {
                                                tran.Rollback();
                                                return false;
                                            }

                                            string LogQueryITL1 = @"insert into ITL1(LogEntry,ItemCode,SysNumber,Quantity,AllocQty,MdAbsEntry) 
                                                   values(" + LogEntry + ",'"
                                                 + item.ItemCode + "','"
                                                 + ii.SysNumber + "',"
                                                 + ii.Quantity + ","
                                                 + -1 * ((Decimal)(ii.selectqty)) + ","
                                                 + ii.AbsEntry + ")";


                                            res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, LogQueryITL1).ToInt();
                                            if (res1 <= 0)
                                            {
                                                tran.Rollback();
                                                return false;
                                            } 
                                        }
                                        else  break;

                                    }

                                    }
                            }
                            #endregion


                            #endregion



                            if (model.BaseType != -1)
                            {
                                string table = dal.GetRowTable(Convert.ToInt32(model.BaseType));
                                string Updatequery = @"Update "+table+" set OpenQty =OpenQty - " + item.QTY + " where Id ="+item.BaseEntry + "and LineNum ="+item.BaseLine;
                                int res = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, Updatequery).ToInt();
                                if (res <= 0)
                                {
                                    tran.Rollback();
                                    return false;
                                }
                            }
                            item.BaseEntry = item.BaseEntry == "" ? "NULL" : Convert.ToInt32(item.BaseEntry);
                            item.BaseLine = item.BaseLine == "" ? "NULL" : Convert.ToInt32(item.BaseLine);
                            item.BaseQty = item.BaseQty == "" ? "NULL" : Convert.ToInt32(item.BaseQty);
                            item.DicPrc = item.DicPrc == "" ? "NULL" : Convert.ToInt32(item.DicPrc);

                            string RowQueryItem = @"insert into DLN1(Id,LineNum,BaseRef,BaseEntry,BaseLine,BaseQty,BaseType,ItemName,Price,LineTotal,OpenQty,ItemCode,Quantity,DiscPrcnt,VatGroup , UomCode,UomEntry ,CountryOrg)
                                              values(" + Id + ","
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
                            LineNo += 1;
                        }



                    }
                    else if (model.ListService != null)
                    {

                        int LineNo = 1;
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


                        int LineNo = 1;
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

                SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                int res1 = 0;
                try
                {
                    var Status = CommonDal.Check_IsEditable("INV1", Convert.ToInt32(model.ID)) == false ? "Open" : "Closed";
                    if (Status == "Closed")
                    {
                        tran.Rollback();
                        return false;
                    }
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
                                                       ",SlpCode = " + model.FooterData.SlpCode + "" +
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
                                    string Updatequery = @"Update "+table+" set OpenQty =(OpenQty + " + docRowModel.Quantity + ") - " + item.QTY + " where Id =" + docRowModel.BaseEntry + "and LineNum =" + docRowModel.BaseLine;
                                    int res = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, Updatequery).ToInt();
                                    if (res <= 0)
                                    {
                                        tran.Rollback();
                                        return false;
                                    }
                                }
                                #endregion

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
                                                        " where Id=" + model.ID + " and LineNum=" + item.LineNum + " and OpenQty <> 0";
                                int res2 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, UpdateQuery).ToInt();
                                if (res2 <= 0)
                                {
                                    tran.Rollback();
                                    return false;
                                }

                            }
                            #region New Row added
                            else
                            {
                                int LineNo = CommonDal.getLineNumber(tran, "DLN1", (model.ID).ToString());

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

                            }
                            #endregion
                        }

                    }
                    else if (model.ListService != null)
                    {

                        int LineNo = 1;
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


                        int LineNo = 1;
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
