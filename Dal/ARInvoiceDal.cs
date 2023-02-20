using iSOL_Enterprise.Common;
using iSOL_Enterprise.Models;
using iSOL_Enterprise.Models.sale;
using Newtonsoft.Json;
using SqlHelperExtensions;
using System.Data;
using System.Data.SqlClient;

namespace iSOL_Enterprise.Dal
{
    public class ARInvoiceDal
    {

        public List<SalesQuotation_MasterModels> GetARInvoiceData()
        {
            string GetQuery = "select * from OINV";
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
                    models.CardCode = rdr["CardCode"].ToString();
                    models.Guid = rdr["Guid"].ToString();
                    models.CardName = rdr["CardName"].ToString();
                    list.Add(models);
                }
            }
            return list;
        }

        public List<SalesQuotation_MasterModels> GetDeliveryData(int cardcode)
        {
            string GetQuery = "select * from ODLN where CardCode =" + cardcode;


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
        public List<SalesQuotation_MasterModels> GetDeliveryType(int DocId)
        {
            string GetQuery = "select DocType,DocNum from ODLN where Id = " + DocId;
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
        public dynamic GetDeliveryItemServiceList(int DocId)
        {
            DataSet ds = new DataSet();
            SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
            SqlDataAdapter sda = new SqlDataAdapter("select Id,LineNum,ItemCode,Quantity,DiscPrcnt,VatGroup ,UomCode,CountryOrg,Dscription,AcctCode,OpenQty from DLN1 where id = " + DocId + "", conn);
            sda.Fill(ds);
            string JSONString = string.Empty;
            JSONString = Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables);
            return JSONString;

        }

        public dynamic GetARInvoiceDetails(int id)
        {
            DataSet ds = new DataSet();
            SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
            SqlDataAdapter sda = new SqlDataAdapter("select * from OINV where id = " + id + ";select * from INV1 where id = " + id + "", conn);
            sda.Fill(ds);
            return ds;
        }



        public bool AddARInvoice(string formData)
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
                    int Id = CommonDal.getPrimaryKey(tran, "OINV");
                    string DocNum = SqlHelper.getUpdatedDocumentNumberOnLoad(tran, "OINV", "AR");
                    if (model.HeaderData != null)
                    {


                        string HeadQuery = @"insert into OINV(Id,DocType,Guid,CardCode,DocNum,CardName,CntctCode,DocDate,NumAtCard,DocDueDate,DocCur,TaxDate , GroupNum , SlpCode , Comments) 
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
                    }
                    if (model.ListItems != null)
                    {
                            int LineNo = 1;
                        foreach (var item in model.ListItems)
                        {
                            //int QUT1Id = CommonDal.getPrimaryKey(tran, "INV1");
                            if (item.BaseEntry != "" && item.BaseEntry != null)
                            {

                                string Updatequery = @"Update DLN1 set OpenQty =OpenQty - " + item.QTY + " where Id =" + item.BaseEntry + "and LineNum =" + item.BaseLine;
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

                            string RowQueryItem = @"insert into INV1(Id,LineNum,BaseRef,BaseEntry,BaseLine,BaseQty,ItemName,Price,LineTotal,OpenQty,ItemCode,Quantity,DiscPrcnt,VatGroup , UomCode,UomEntry ,CountryOrg)
                                              values(" + Id + ","
                                                + LineNo + ",'"
                                                + item.BaseRef + "',"
                                                + item.BaseEntry + ","
                                                + item.BaseLine + ","
                                                + item.BaseQty + ",'"
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
                            //int QUT1Id = CommonDal.getPrimaryKey(tran, "INV1");
                            item.BaseEntry2 = item.BaseEntry2 == "" ? "NULL" : Convert.ToInt32(item.BaseEntry2);
                            item.BaseLine2 = item.BaseLine2 == "" ? "NULL" : Convert.ToInt32(item.BaseLine2);

                            string RowQueryService = @"insert into INV1(Id,LineNum,BaseRef,BaseEntry,BaseLine,LineTotal,Dscription,AcctCode,VatGroup)
                                                  values(" + Id + ","
                                                     + LineNo + ",'"
                                                     + item.BaseRef2 + "',"
                                                     + item.BaseEntry2 + ","
                                                     + item.BaseLine2 + ","
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





        public bool EditARInvoice(string formData)
        {
            try
            {
               
                var model = JsonConvert.DeserializeObject<dynamic>(formData);
                string DocType = model.ListItems == null ? "S" : "I";


                SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                //string GetDocNum = CommonDal.getDocType(tran, "OINV", model.ID.ToString());
                int res1 = 0;
                try
                {
                    #region Deleting Items/List



                    //string DeleteI_Or_SQuery = "Delete from INV1 Where id = " + model.ID;
                    //res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, DeleteI_Or_SQuery).ToInt();
                    //if (res1 <= 0)
                    //{
                    //    tran.Rollback();
                    //    return false;
                    //}


                    #endregion

                    if (model.HeaderData != null)
                    {
                        //string HeadQuery = @"insert into OINV(Id,DocType,Guid,CardCode,DocNum,CardName,CntctCode,DocDate,NumAtCard,DocDueDate,DocCur,TaxDate , GroupNum , SlpCode , Comments) 
                        //                   values(" + model.ID + ",'"
                        //                       + DocType + "','"
                        //                   + CommonDal.generatedGuid() + "','"
                        //                        + model.HeaderData.CardCode + "','"
                        //                        + model.HeaderData.DocNum + "','"
                        //                        + model.HeaderData.CardName + "','"
                        //                        + model.HeaderData.CntctCode + "','"
                        //                        + Convert.ToDateTime(model.HeaderData.DocDate) + "','"
                        //                        + model.HeaderData.NumAtCard + "','"
                        //                        + Convert.ToDateTime(model.HeaderData.DocDueDate) + "','"
                        //                        + model.HeaderData.DocCur + "','"
                        //                        + Convert.ToDateTime(model.HeaderData.TaxDate) + "','"
                        //                        + model.ListAccouting.GroupNum + "',"
                        //                        + Convert.ToInt32(model.FooterData.SlpCode) + ",'"
                        //                        + model.FooterData.Comments + "')";

                        string HeadQuery = @" Update OINV set 
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




                    //var GetDocNum = SqlHelper.ExecuteScalar(tran, CommandType.Text, "Select DocType from ORDR where Id = " + model.Id + " ");


                     


                        if (model.ListItems != null)
                        {

                            foreach (var item in model.ListItems)
                            {
                            item.DicPrc = item.DicPrc == "" ? "NULL" : Convert.ToDecimal(item.DicPrc);
                            if (item.LineNum != "" && item.LineNum != null)
                                {


                                string oldDataQuery = @"select BaseEntry,BaseLine,Quantity from INV1 where Id=" + model.ID + " and LineNum=" + item.LineNum + " and OpenQty <> 0";

                                tbl_docRow docRowModel = new tbl_docRow();
                                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, oldDataQuery))
                                {
                                    while (rdr.Read())
                                    {


                                        docRowModel.BaseEntry = rdr["BaseEntry"].ToInt();
                                        docRowModel.BaseLine = rdr["BaseLine"].ToInt();
                                        docRowModel.Quantity = rdr["Quantity"].ToInt();


                                    }
                                }
                                #region if doc contains base ref
                                if (docRowModel.BaseEntry != null)
                                {
                                    string Updatequery = @"Update DLN1 set OpenQty =(OpenQty + " + docRowModel.Quantity + ") - " + item.QTY + " where Id =" + docRowModel.BaseEntry + "and LineNum =" + docRowModel.BaseLine;
                                    int res = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, Updatequery).ToInt();
                                    if (res <= 0)
                                    {
                                        tran.Rollback();
                                        return false;
                                    }
                                }
                                #endregion


                                string UpdateQuery = @"update INV1 set
                                                              ItemCode  = '" + item.ItemCode + "'" +
                                                            ",ItemName  = '" + item.ItemName + "'" +
                                                            ",UomCode   = '" + item.UomCode + "'" +
                                                            ",UomEntry   = " + item.UomEntry  +
                                                            ",Quantity  = " + item.QTY + "" +
                                                            ",OpenQty  =  " + item.QTY + "" +
                                                            ",Price     = '" + item.UPrc + "'" +
                                                            ",LineTotal = '" + item.TtlPrc + "'" +
                                                            ",DiscPrcnt = " + item.DicPrc  +
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
                                    int LineNo = CommonDal.getLineNumber(tran, "INV1", (model.ID).ToString());
                                    string RowQueryItem = @"insert into RDR1(Id,LineNum,ItemName,Price,LineTotal,ItemCode,Quantity,OpenQty,DiscPrcnt,VatGroup, UomCode,UomEntry ,CountryOrg)
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
                            }
                        #endregion


                    }
                    else if (model.ListService != null)
                        {
                            int LineNo = 1;

                            foreach (var item in model.ListService)
                            {
                                //int QUT1Id = CommonDal.getPrimaryKey(tran, "QUT1");
                                string RowQueryService = @"insert into INV1(Id,LineNum,LineTotal,Dscription,AcctCode,VatGroup)
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
