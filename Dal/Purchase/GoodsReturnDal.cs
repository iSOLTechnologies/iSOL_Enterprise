using iSOL_Enterprise.Common;
using iSOL_Enterprise.Models;
using iSOL_Enterprise.Models.Logs;
using iSOL_Enterprise.Models.sale;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using SqlHelperExtensions;
using System.Data;
using System.Data.SqlClient;

namespace iSOL_Enterprise.Dal.Purchase
{
    public class GoodsReturnDal
    {


        public List<SalesQuotation_MasterModels> GetGoodsReturnData()
        {
            string GetQuery = "select * from ORPD order by id DESC";
            List<SalesQuotation_MasterModels> list = new List<SalesQuotation_MasterModels>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {

                    SalesQuotation_MasterModels models = new SalesQuotation_MasterModels();

                    models.DocStatus = CommonDal.Check_IsNotEditable("RPD1", rdr["Id"].ToInt()) == false ? "Open" : "Closed";
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


        public dynamic GetGoodsReturnDetails(int id)
        {
            DataSet ds = new DataSet();
            SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
            SqlDataAdapter sda = new SqlDataAdapter("select * from ORPD where id = " + id + ";select * from RPD1 where id = " + id + "", conn);
            sda.Fill(ds);
            return ds;
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


                string GetQuery = "select OBTN.DistNumber,OBTN.Quantity,OBTN.InDate,OBTN.AbsEntry,OBTN.SysNumber  from OBTW Inner join OBTN on OBTN.AbsEntry = OBTW.AbsEntry where OBTW.ItemCode = '" + itemcode + "' and OBTW.WhsCode = '" + warehouse + "'";


                List<tbl_OBTN> list = new List<tbl_OBTN>();
                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, GetQuery))
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

        public bool AddGoodsReturn(string formData)
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

                    int Id = CommonDal.getPrimaryKey(tran, "ORPD");
                    string DocNum = SqlHelper.getUpdatedDocumentNumberOnLoad(tran, "ORPD", "GRT");
                    if (model.HeaderData != null)
                    {

                        model.HeaderData.PurchaseType = model.HeaderData.PurchaseType == "" ? "NULL" : Convert.ToDecimal(model.HeaderData.PurchaseType);
                        model.HeaderData.TypeDetail = model.HeaderData.TypeDetail == "" || model.HeaderData.TypeDetail == null ? "NULL" : Convert.ToDecimal(model.HeaderData.TypeDetail);
                        model.HeaderData.ProductionOrderNo = model.HeaderData.ProductionOrderNo == "" ? "NULL" : Convert.ToDecimal(model.HeaderData.ProductionOrderNo);
                        model.HeaderData.ChallanNo = model.HeaderData.ChallanNo == "" ? "NULL" : Convert.ToDecimal(model.HeaderData.ChallanNo);
                        model.HeaderData.DONo = model.HeaderData.DONo == "" ? "NULL" : Convert.ToDecimal(model.HeaderData.DONo);
                        model.HeaderData.SaleOrderNo = model.HeaderData.SaleOrderNo == "" ? "NULL" : Convert.ToInt32(model.HeaderData.SaleOrderNo);
                        model.HeaderData.Series = model.HeaderData.Series == null ? "NULL" : Convert.ToInt32(model.HeaderData.Series);
                        model.FooterData.Discount = model.FooterData.Discount == "" ? "NULL" : Convert.ToDecimal(model.FooterData.Discount);
                        string HeadQuery = @"insert into ORPD(Id,Series,DocType,Guid,CardCode,DocNum,CardName,CntctCode,DocDate,NumAtCard,DocDueDate,DocCur,TaxDate , GroupNum ,DocTotal, SlpCode ,DiscPrcnt,
                                            PurchaseType,TypeDetail,ProductionOrderNo,ChallanNo,DONo,SaleOrderNo, Comments)
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
                                                + model.HeaderData.DONo + ","
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
                        CommonDal dal = new CommonDal();
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
                            OITL OITLModel = new OITL();
                            OITLModel.LogEntry = LogEntry;
                            OITLModel.CardCode = model.HeaderData.CardCode.ToString();
                            OITLModel.CardName = model.HeaderData.CardName.ToString();
                            OITLModel.ItemCode = item.ItemCode.ToString();
                            OITLModel.ItemName = item.ItemName.ToString();
                            OITLModel.ID = Id;
                            OITLModel.DocLine = LineNo;
                            OITLModel.DocType = 21;
                            OITLModel.BaseType = item.BaseType;
                            OITLModel.Quantity = -1 * (decimal)item.QTY;
                            OITLModel.DocDate = Convert.ToDateTime(model.HeaderData.DocDate);

                            if (!dal.OITLLog(tran, OITLModel))
                                return false;


                            #endregion

                            #region Bataches & Logs working

                            if (model.Batches != null)
                            {
                                bool response = dal.OutBatches(tran, model.Batches, item.ItemCode.ToString(), LogEntry, item.Warehouse.ToString(), LineNo);
                                if (!response)
                                {
                                    return false;
                                }
                                
                            }
                            #endregion


                            #endregion


                            #region If Doc copied data from other Doc then get data from Goods Receipt  then   Update in Purchase Order &  Goods Receipt
                            if ((int)model.BaseType != -1 && item.BaseEntry.ToString() != "" && item.BaseLine.ToString() != "")
                            {
                                string table = dal.GetRowTable(Convert.ToInt32(model.BaseType));
                                string getFromDeliveryQuery = "select BaseEntry,BaseLine,ItemCode from " + table + " where Id =" + item.BaseEntry + "and LineNum =" + item.BaseLine + "and ItemCode = '" + item.ItemCode + "'";

                                try
                                {
                                    tbl_docRow docRowModel = new tbl_docRow();
                                    using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, getFromDeliveryQuery))
                                    {
                                        while (rdr.Read())
                                        {


                                            docRowModel.BaseEntry = Convert.ToInt32(rdr["BaseEntry"]);
                                            docRowModel.BaseLine = Convert.ToInt32(rdr["BaseLine"]);
                                            docRowModel.ItemCode = rdr["ItemCode"].ToString();

                                            string UpdateDLQuery = @"Update " + table + " set Quantity =Quantity - " + item.QTY + " , OpenQty = OpenQty - " + item.QTY + " where Id =" + item.BaseEntry + "and LineNum =" + item.BaseLine + "and ItemCode = '" + item.ItemCode + "'";
                                            int res = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, UpdateDLQuery).ToInt();
                                            if (res <= 0)
                                            {
                                                tran.Rollback();
                                                return false;
                                            }

                                            string UpdatePOQuery = @"Update POR1 set OpenQty =OpenQty + " + item.QTY + " where Id =" + docRowModel.BaseEntry + "and LineNum =" + docRowModel.BaseLine + "and ItemCode = '" + docRowModel.ItemCode + "'";
                                            res = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, UpdatePOQuery).ToInt();
                                            if (res <= 0)
                                            {
                                                tran.Rollback();
                                                return false;
                                            }

                                        }
                                    }

                                }
                                catch (Exception)
                                {
                                    tran.Rollback();
                                    return false;
                                    throw;
                                }
                            }
                            #endregion




                            #region Insert into Row 
                            string RowQueryItem = @"insert into RPD1(Id,LineNum,WhsCode,BaseRef,BaseEntry,BaseLine,BaseQty,BaseType,ItemName,Price,LineTotal,OpenQty,ItemCode,Quantity,DiscPrcnt,VatGroup , UomCode,UomEntry ,CountryOrg)
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
                        }



                    }
                    else if (model.ListService != null)
                    {

                        int LineNo = 0;
                        foreach (var item in model.ListService)
                        {
                            //int QUT1Id = CommonDal.getPrimaryKey(tran, "INV1");
                            item.BaseEntry2 = item.BaseEntry2 == "" ? "NULL" : Convert.ToInt32(item.BaseEntry2);
                            item.BaseLine2 = item.BaseLine2 == "" ? "NULL" : Convert.ToInt32(item.BaseLine2);

                            string RowQueryService = @"insert into RPD1(Id,LineNum,BaseRef,BaseEntry,BaseLine,BaseType,LineTotal,Dscription,AcctCode,VatGroup)
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





        public bool EditGoodsReturn(string formData)
        {
            try
            {
                var model = JsonConvert.DeserializeObject<dynamic>(formData);
                string DocType = model.ListItems == null ? "S" : "I";
                CommonDal dal = new CommonDal();
                string mytable = "RPD1";
                SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                int res1 = 0;
                try
                {

                    var Status = CommonDal.Check_IsNotEditable(mytable, Convert.ToInt32(model.ID)) == false ? "Open" : "Closed";
                    if (Status == "Closed")
                    {
                        string HeadQuery = @" Update ORPD set NumAtCard = '" + model.HeaderData.NumAtCard + "'" +
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


                            model.HeaderData.PurchaseType = model.HeaderData.PurchaseType == "" ? "NULL" : Convert.ToInt32(model.HeaderData.PurchaseType);
                            model.HeaderData.TypeDetail = model.HeaderData.TypeDetail == "" || model.HeaderData.TypeDetail == null ? "NULL" : Convert.ToDecimal(model.HeaderData.TypeDetail);
                            model.HeaderData.ProductionOrderNo = model.HeaderData.ProductionOrderNo == "" ? "NULL" : Convert.ToDecimal(model.HeaderData.ProductionOrderNo);
                            model.HeaderData.ChallanNo = model.HeaderData.ChallanNo == "" ? "NULL" : Convert.ToDecimal(model.HeaderData.ChallanNo);
                            model.HeaderData.DONo = model.HeaderData.DONo == "" ? "NULL" : Convert.ToDecimal(model.HeaderData.DONo);
                            model.HeaderData.SaleOrderNo = model.HeaderData.SaleOrderNo == "" ? "NULL" : Convert.ToInt32(model.HeaderData.SaleOrderNo);
                            model.HeaderData.Series = model.HeaderData.Series == null ? "NULL" : Convert.ToInt32(model.HeaderData.Series);
                            model.FooterData.Discount = model.FooterData.Discount == "" ? "NULL" : Convert.ToDecimal(model.FooterData.Discount);
                            string HeadQuery = @" Update ORPD set 
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
                                                        ",DONo = " + model.HeaderData.DONo + "" +
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
                            int index = 0;
                            foreach (var item in model.ListItems)
                            {



                                if (item.LineNum != "" && item.LineNum != null)
                                {
                                    decimal OpenQty = Convert.ToDecimal(SqlHelper.ExecuteScalar(SqlHelper.defaultDB, CommandType.Text, "select OpenQty from " + mytable + " where Id=" + model.ID + " and LineNum=" + item.LineNum + ""));
                                    if (OpenQty > 0)
                                    {

                                        string oldDataQuery = @"select BaseEntry,BaseType,BaseLine,Quantity from RPD1 where Id=" + model.ID + " and LineNum=" + item.LineNum + "and ItemCode = '" + item.ItemCode + "'";

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
                                            string getFromDeliveryQuery = "select BaseEntry,BaseLine,ItemCode from " + table + " where Id =" + docRowModel.BaseEntry + "and LineNum =" + docRowModel.BaseLine + "and ItemCode = '" + item.ItemCode + "'";

                                            try
                                            {
                                                tbl_docRow docRowModel2 = new tbl_docRow();
                                                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, getFromDeliveryQuery))
                                                {
                                                    while (rdr.Read())
                                                    {


                                                        docRowModel2.BaseEntry = Convert.ToInt32(rdr["BaseEntry"]);
                                                        docRowModel2.BaseLine = Convert.ToInt32(rdr["BaseLine"]);
                                                        docRowModel2.ItemCode = rdr["ItemCode"].ToString();

                                                        string UpdateDLQuery = @"Update " + table + " set Quantity =(Quantity + " + docRowModel.Quantity + ") - " + item.QTY + " , OpenQty = (OpenQty + " + docRowModel.Quantity + ") - " + item.QTY + " where Id =" + docRowModel.BaseEntry + "and LineNum =" + docRowModel.BaseLine + "and ItemCode = '" + item.ItemCode + "'";
                                                        int res = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, UpdateDLQuery).ToInt();
                                                        if (res <= 0)
                                                        {
                                                            tran.Rollback();
                                                            return false;
                                                        }

                                                        string UpdatePOQuery = @"Update POR1 set OpenQty =(OpenQty - " + docRowModel.Quantity + ") + " + item.QTY + " where Id =" + docRowModel2.BaseEntry + "and LineNum =" + docRowModel2.BaseLine + "and ItemCode = '" + docRowModel2.ItemCode + "'";
                                                        res = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, UpdatePOQuery).ToInt();
                                                        if (res <= 0)
                                                        {
                                                            tran.Rollback();
                                                            return false;
                                                        }

                                                    }
                                                }

                                            }
                                            catch (Exception)
                                            {
                                                tran.Rollback();
                                                return false;
                                                throw;
                                            }
                                        }
                                        #endregion

                                        item.DicPrc = item.DicPrc == "" ? "NULL" : Convert.ToDecimal(item.DicPrc);
                                        string UpdateQuery = @"update RPD1 set
                                                             ItemCode  = '" + item.ItemCode + "'" +
                                                                    ",ItemName  = '" + item.ItemName + "'" +
                                                                    ",UomEntry  =  " + item.UomEntry + "" +
                                                                    ",UomCode   = '" + item.UomCode + "'" +
                                                                    ",Quantity  = '" + item.QTY + "'" +
                                                                    ",OpenQty   = OpenQty + (" + item.QTY + "- OpenQty)" +
                                                                    ",Price     = '" + item.UPrc + "'" +
                                                                    ",LineTotal = " + item.TtlPrc + "" +
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
                                        #region If Item is Batch Type Generate Log
                                        else
                                        {


                                            if (Convert.ToDecimal(item.QTY) != Convert.ToDecimal(item.OldQty))
                                            {
                                                ResponseModels ItemData = dal.GetItemData(item.ItemCode.ToString(), "P");
                                                if (ItemData.Data.ManBtchNum == "Y")
                                                {


                                                    if (dal.ReverseOutTransaction(tran, Convert.ToInt32(model.ID), Convert.ToInt32(item.LineNum), 21))
                                                    {

                                                        int LogEntry1 = CommonDal.getPrimaryKey(tran, "LogEntry", "OITL");

                                                        #region OITLLog
                                                        OITL OITLModel = new OITL();
                                                        OITLModel.LogEntry = LogEntry1;
                                                        OITLModel.CardCode = model.HeaderData.CardCode.ToString();
                                                        OITLModel.CardName = model.HeaderData.CardName.ToString();
                                                        OITLModel.ItemCode = item.ItemCode.ToString();
                                                        OITLModel.ItemName = item.ItemName.ToString();
                                                        OITLModel.ID = Convert.ToInt32(model.ID);
                                                        OITLModel.DocLine = Convert.ToInt32(item.LineNum);
                                                        OITLModel.DocType = 21;
                                                        OITLModel.BaseType = item.BaseType;
                                                        OITLModel.Quantity = -1 * (decimal)item.QTY;
                                                        OITLModel.DocDate = Convert.ToDateTime(model.HeaderData.DocDate);

                                                        if (!dal.OITLLog(tran, OITLModel))
                                                            return false;

                                                        if (!dal.OutBatches(tran, model.Batches, item.ItemCode.ToString(), LogEntry1, item.Warehouse.ToString(), index))
                                                            return false;
                                                        #endregion


                                                    }

                                                }

                                            }

                                        }
                                        #endregion
                                        #region Update OITW If Sap Integration is OFF

                                        if (!SqlHelper.SAPIntegration)
                                        {
                                            string OldQtyQuery = @"select DocQty from OITL where DocEntry=" + model.ID + " and DocLine = " + item.LineNum + "and DocType =21";
                                            decimal OldQty = SqlHelper.ExecuteScalar(tran, CommandType.Text, OldQtyQuery).ToDecimal();

                                            string UpdateOITWQuery = @"Update OITW set onHand = onHand - (" + OldQty + ") - @Quantity where WhsCode = '" + item.Warehouse + "' and ItemCode = '" + item.ItemCode + "'";
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

                                }

                                #region New Row added
                                else
                                {
                                    int LineNo = CommonDal.getLineNumber(tran, "RPD1", model.ID.ToString());
                                    item.DicPrc = item.DicPrc == "" ? "NULL" : Convert.ToDecimal(item.DicPrc);

                                    string RowQueryItem = @"insert into RPD1(Id,LineNum,WhsCode,ItemName,Price,LineTotal,OpenQty,ItemCode,Quantity,DiscPrcnt,VatGroup , UomCode,UomEntry ,CountryOrg)
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


                                    int LogEntry = CommonDal.getPrimaryKey(tran, "LogEntry", "OITL");

                                    #region OITLLog

                                    OITL OITLModel = new OITL();
                                    OITLModel.LogEntry = LogEntry;
                                    OITLModel.CardCode = model.HeaderData.CardCode.ToString();
                                    OITLModel.CardName = model.HeaderData.CardName.ToString();
                                    OITLModel.ItemCode = item.ItemCode.ToString();
                                    OITLModel.ItemName = item.ItemName.ToString();
                                    OITLModel.ID = Convert.ToInt32(model.ID);
                                    OITLModel.DocLine = Convert.ToInt32(item.LineNum);
                                    OITLModel.DocType = 21;
                                    OITLModel.BaseType = item.BaseType;
                                    OITLModel.Quantity = -1 * (decimal)item.QTY;
                                    OITLModel.DocDate = Convert.ToDateTime(model.HeaderData.DocDate);

                                    if (!dal.OITLLog(tran, OITLModel))
                                        return false;

                                        #region Bataches & Logs working

                                        if (model.Batches != null)
                                        {
                                            bool response = dal.OutBatches(tran, model.Batches, item.ItemCode.ToString(), LogEntry, item.Warehouse.ToString(), index);
                                            if (!response)
                                            {
                                                return false;
                                            }

                                        }
                                        #endregion
                                    #endregion
                                }
                                #endregion
                            ++index;
                            }


                        }
                        else if (model.ListService != null)
                        {

                            int LineNo = 0;
                            foreach (var item in model.ListService)
                            {
                                //int QUT1Id = CommonDal.getPrimaryKey(tran, "DLN1");

                                string RowQueryService = @"insert into RPD1(Id,LineNum,LineTotal,Dscription,AcctCode,VatGroup)
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
