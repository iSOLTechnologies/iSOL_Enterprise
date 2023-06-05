using iSOL_Enterprise.Common;
using iSOL_Enterprise.Models;
using iSOL_Enterprise.Models.sale;
using Newtonsoft.Json;
using SqlHelperExtensions;
using System.Data;
using System.Data.SqlClient;
using static iSOL_Enterprise.Dal.DashboardDal;

namespace iSOL_Enterprise.Dal.Purchase
{
    public class PurchaseOrderDal
    {



        public List<SalesQuotation_MasterModels> GetData()
        {
            string GetQuery = "select * from OPOR order by id DESC";


            List<SalesQuotation_MasterModels> list = new List<SalesQuotation_MasterModels>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {

                    SalesQuotation_MasterModels models = new SalesQuotation_MasterModels();
                    models.DocStatus = CommonDal.Check_IsNotEditable("POR1", rdr["Id"].ToInt()) == false ? "Open" : "Closed";
                    models.Id = rdr["Id"].ToInt();
                    models.DocDate = rdr["DocDueDate"].ToDateTime();
                    models.PostingDate = rdr["DocDate"].ToDateTime();
                    models.DocNum = rdr["DocNum"].ToString();
                    models.CardCode = rdr["CardCode"].ToString();
                    models.Guid = rdr["Guid"].ToString();
                    models.CardName = rdr["CardName"].ToString();
                    models.IsPosted = rdr["isPosted"].ToString(); models.IsEdited = rdr["is_Edited"].ToString();
                    models.isApproved = rdr["isApproved"].ToBool();
                    models.apprSeen = rdr["apprSeen"].ToBool();
                    models.Sap_Ref_No = rdr["Sap_Ref_No"].ToString();

                    list.Add(models);
                }
            }
            return list;
        }

        public dynamic GetPurchaseOrderEditDetails(int id)
        {
            DataSet ds = new DataSet();
            SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
            SqlDataAdapter sda = new SqlDataAdapter("select * from OPOR where id = " + id + ";select * from POR1 where id = " + id + "", conn);
            sda.Fill(ds);
            return ds;
        }

        public List<SalesQuotation_MasterModels> GetPurchaseQuotationData(string cardcode)
        {
            string GetQuery = "select * from OPQT where CardCode ='" + cardcode + "'";


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

        public List<SalesQuotation_MasterModels> GetQuotationType(int DocId)
        {
            string GetQuery = "select DocType,DocNum from OPQT where Id = " + DocId;
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
        public dynamic GetQuotationItemServiceList(int DocId)
        {
            DataSet ds = new DataSet();
            SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
            SqlDataAdapter sda = new SqlDataAdapter("select Id,LineNum,ItemCode,Quantity,DiscPrcnt,VatGroup ,UomCode,CountryOrg,Dscription,AcctCode,OpenQty from PQT1 where id = " + DocId + "", conn);
            sda.Fill(ds);
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(ds.Tables);
            return JSONString;

        }



        public List<tbl_item> GetItemsData()
        {
            string GetQuery = "select * from OITM";


            List<tbl_item> list = new List<tbl_item>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {

                    list.Add(
                        new tbl_item()
                        {
                            ItemCode = rdr["ItemCode"].ToString(),
                            ItemName = rdr["ItemName"].ToString(),
                            OnHand = (decimal)rdr["OnHand"],
                        });

                }
            }

            return list;
        }

        public List<tbl_customer> GetCustomerData()
        {
            string GetQuery = "select * from OCRD Where CardType = 'C'";


            List<tbl_customer> list = new List<tbl_customer>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {

                    list.Add(
                        new tbl_customer()
                        {
                            CardCode = rdr["CardCode"].ToString(),
                            CardName = rdr["CardName"].ToString(),
                            Currency = rdr["Currency"].ToString(),
                            Balance = (decimal)rdr["Balance"],
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

        public List<tbl_OVTG> GetVatGroupData()
        {
            string GetQuery = "select vatName = Code+' - ' +Name , Rate from OVTG ";


            List<tbl_OVTG> list = new List<tbl_OVTG>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {

                    list.Add(
                        new tbl_OVTG()
                        {
                            vatName = rdr["vatName"].ToString(),
                            Rate = (decimal)rdr["Rate"]
                        });

                }
            }

            return list;
        }
        public List<ListModel> GetContactPersons(int cardCode)
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
        public bool AddPurchaseOrder(string formData)
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
                    int Id = CommonDal.getPrimaryKey(tran, "OPOR");
                    string DocNum = SqlHelper.getUpdatedDocumentNumberOnLoad(tran, "OPOR", "PO");
                    string Guid = CommonDal.generatedGuid();
                    if (model.HeaderData != null)
                    {

                        model.HeaderData.PurchaseType = model.HeaderData.PurchaseType == "" ? "NULL" : Convert.ToDecimal(model.HeaderData.PurchaseType);
                        model.HeaderData.TypeDetail = model.HeaderData.TypeDetail == "" || model.HeaderData.TypeDetail == null ? "NULL" : Convert.ToDecimal(model.HeaderData.TypeDetail);
                        model.HeaderData.ProductionOrderNo = model.HeaderData.ProductionOrderNo == "" ? "NULL" : Convert.ToDecimal(model.HeaderData.ProductionOrderNo);
                        model.HeaderData.ChallanNo = model.HeaderData.ChallanNo == "" ? "NULL" : Convert.ToDecimal(model.HeaderData.ChallanNo);
                        model.HeaderData.DONo = model.HeaderData.DONo == "" ? "NULL" : Convert.ToDecimal(model.HeaderData.DONo);
                        model.HeaderData.SalesOrderCode = model.HeaderData.SalesOrderCode == "" ? "NULL" : Convert.ToInt32(model.HeaderData.SalesOrderCode);
                        model.HeaderData.Series = model.HeaderData.Series == null ? "NULL" : Convert.ToInt32(model.HeaderData.Series);
                        model.FooterData.Discount = model.FooterData.Discount == "" ? "NULL" : Convert.ToDecimal(model.FooterData.Discount);

                        int ObjectCode = 22;
                        int isApproved = ObjectCode.GetApprovalStatus(tran);
                        #region Insert in Approval Table

                        if (isApproved == 0)
                        {
                            ApprovalModel approvalModel = new()
                            {
                                Id = CommonDal.getPrimaryKey(tran, "tbl_DocumentsApprovals"),
                                ObjectCode = ObjectCode,
                                DocEntry = Id,
                                DocNum = DocNum,
                                Guid = Guid

                            };
                            bool response = dal.AddApproval(tran, approvalModel);
                            if (!response)
                                return false;
                        }

                        #endregion

                        string HeadQuery = @"insert into OPOR(Id,Series,DocType,Guid,CardCode,DocNum,Segment,CardName,CntctCode,DocDate,NumAtCard,DocDueDate,DocCur,TaxDate , GroupNum,DocTotal , SlpCode,DiscPrcnt,
                                            PurchaseType,TypeDetail,ProductionOrderNo,ChallanNo,DONo,SaleOrderNo,isApproved, Comments) 
                                           values(" + Id + ","
                                                 + model.HeaderData.Series + ",'"
                                                + DocType + "','"
                                                + Guid + "','"
                                                + model.HeaderData.CardCode + "','"
                                                + DocNum + "','"
                                                + model.HeaderData.Segment + "','"
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
                                                + model.HeaderData.SalesOrderCode + ","
                                                + isApproved + ",'"
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

                            if ((int)model.BaseType != -1 && item.BaseEntry.ToString() != "" && item.BaseLine.ToString() != "")
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

                            item.BaseEntry = item.BaseEntry == "" ? "NULL" : item.BaseEntry;
                            item.BaseLine = item.BaseLine == "" ? "NULL" : item.BaseLine;
                            item.BaseQty = item.BaseQty == "" ? "NULL" : item.BaseQty;
                            item.DicPrc = item.DicPrc == "" ? "NULL" : Convert.ToDecimal(item.DicPrc);

                            string RowQueryItem = @"insert into POR1(Id,LineNum,WhsCode,BaseRef,BaseEntry,BaseLine,BaseQty,BaseType,ItemName,Price,LineTotal,OpenQty,ItemCode,Quantity,DiscPrcnt,VatGroup , UomCode,
                                                    UomEntry,SaleOrderCode,SaleOrderDocNo,PreCostingTowelCode,AccessoriesType,ProductionOrder,ProductionOrderProdName,CountryOrg)
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
                                              + item.UomEntry + ",@SaleOrderCode,@SaleOrderDocNo,@PreCostingTowelCode,@AccessoriesType,@ProductionOrder,@ProductionOrderProdName,'"
                                              + item.CountryOrg + "')";

                            #region sqlparam
                            List<SqlParameter> param1 = new List<SqlParameter>();
                            param1.Add(dal.GetParameter("@SaleOrderCode", item.SaleOrderCode, typeof(int)));
                            param1.Add(dal.GetParameter("@SaleOrderDocNo", item.SaleOrderDocNo, typeof(string)));
                            param1.Add(dal.GetParameter("@PreCostingTowelCode", item.PreCostingTowelCode, typeof(int)));
                            param1.Add(dal.GetParameter("@AccessoriesType", item.AccessoriesType, typeof(string)));
                            param1.Add(dal.GetParameter("@ProductionOrder", item.ProductionOrder, typeof(int)));
                            param1.Add(dal.GetParameter("@ProductionOrderProdName", item.ProductionOrderProdName, typeof(string)));
                            #endregion

                            int res2 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, RowQueryItem, param1.ToArray()).ToInt();
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

                            item.BaseEntry = item.BaseEntry == "" ? "null" : item.BaseEntry;
                            item.BaseLine = item.BaseLine == "" ? "null" : item.BaseLine;
                            item.BaseQty = item.BaseQty == "" ? "null" : item.BaseQty;
                            //int QUT1Id = CommonDal.getPrimaryKey(tran, "QUT1");
                            string RowQueryService = @"insert into POR1(Id,LineNum,BaseRef,BaseEntry,BaseLine,BaseType,LineTotal,OpenQty,Dscription,AcctCode,VatGroup)
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










        public bool EditPurchaseOrder(string formData)
        {
            try
            {
                var model = JsonConvert.DeserializeObject<dynamic>(formData);
                string DocType = model.ListItems == null ? "S" : "I";
                CommonDal dal = new CommonDal();
                string mytable = "POR1";
                SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                int res1 = 0;
                try
                {


                    var Status = CommonDal.Check_IsNotEditable(mytable, Convert.ToInt32(model.ID)) == false ? "Open" : "Closed";
                    if (Status == "Closed")
                    {
                        string HeadQuery = @" Update OPOR set NumAtCard = '" + model.HeaderData.NumAtCard + "'" +
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



                        //string DeleteI_Or_SQuery = "Delete from POR1 Where id = " + model.ID;
                        //res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, DeleteI_Or_SQuery).ToInt();
                        //if (res1 <= 0)
                        //{
                        //    tran.Rollback();
                        //    return false;
                        //}


                        #endregion


                        // int Id = CommonDal.getPrimaryKey(tran, "OPOR");

                        if (model.HeaderData != null)
                        {

                            model.HeaderData.PurchaseType = model.HeaderData.PurchaseType == "" ? "NULL" : Convert.ToInt32(model.HeaderData.PurchaseType);
                            model.HeaderData.TypeDetail = model.HeaderData.TypeDetail == "" || model.HeaderData.TypeDetail == null ? "NULL" : Convert.ToDecimal(model.HeaderData.TypeDetail);
                            model.HeaderData.ProductionOrderNo = model.HeaderData.ProductionOrderNo == "" ? "NULL" : Convert.ToDecimal(model.HeaderData.ProductionOrderNo);
                            model.HeaderData.ChallanNo = model.HeaderData.ChallanNo == "" ? "NULL" : Convert.ToDecimal(model.HeaderData.ChallanNo);
                            model.HeaderData.DONo = model.HeaderData.DONo == "" ? "NULL" : Convert.ToDecimal(model.HeaderData.DONo);
                            model.HeaderData.SalesOrderCode = model.HeaderData.SalesOrderCode == "" ? "NULL" : Convert.ToInt32(model.HeaderData.SalesOrderCode);
                            model.HeaderData.Series = model.HeaderData.Series == null ? "NULL" : Convert.ToInt32(model.HeaderData.Series);
                            model.FooterData.Discount = model.FooterData.Discount == "" ? "NULL" : Convert.ToDecimal(model.FooterData.Discount);

                            int ObjectCode = 22;
                            int isApproved = ObjectCode.GetApprovalStatus(tran);
                            #region Insert in Approval Table

                            if (isApproved == 0)
                            {
                                ApprovalModel approvalModel = new()
                                {
                                    Id = CommonDal.getPrimaryKey(tran, "tbl_DocumentsApprovals"),
                                    ObjectCode = ObjectCode,
                                    DocEntry = model.ID,
                                    DocNum = SqlHelper.ExecuteScalar(tran, CommandType.Text, @"select DocNum from OPOR where id=" + model.ID).ToString(),
                                    Guid = SqlHelper.ExecuteScalar(tran, CommandType.Text, @"select GUID from OPOR where id=" + model.ID).ToString()
                                };
                                bool response = dal.AddApproval(tran, approvalModel);
                                if (!response)
                                    return false;
                            }

                            #endregion

                            string HeadQuery = @" Update OPOR set 
                                                          DocType = '" + DocType + "'" +
                                                       ",Segment = '" + model.HeaderData.Segment + "'" +
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
                                                        ",SaleOrderNo = " + model.HeaderData.SalesOrderCode + "" +
                                                        ",isApproved = " + isApproved + "" +
                                                        ",apprSeen = " + 0 + "" +
                                                       ",Comments = '" + model.FooterData.Comments + "' " +
                                                       "WHERE Id = '" + model.ID + "'";

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

                            foreach (var item in model.ListItems)
                            {
                                item.DicPrc = item.DicPrc == "" ? "NULL" : Convert.ToDecimal(item.DicPrc);

                                #region sqlparam
                                List<SqlParameter> param1 = new List<SqlParameter>();
                                param1.Add(dal.GetParameter("@SaleOrderCode", item.SaleOrderCode, typeof(int)));
                                param1.Add(dal.GetParameter("@SaleOrderDocNo", item.SaleOrderDocNo, typeof(string)));
                                param1.Add(dal.GetParameter("@PreCostingTowelCode", item.PreCostingTowelCode, typeof(int)));
                                param1.Add(dal.GetParameter("@AccessoriesType", item.AccessoriesType, typeof(string)));
                                param1.Add(dal.GetParameter("@ProductionOrder", item.ProductionOrder, typeof(int)));
                                param1.Add(dal.GetParameter("@ProductionOrderProdName", item.ProductionOrderProdName, typeof(string)));
                                #endregion


                                if (item.LineNum != "" && item.LineNum != null)
                                {
                                    decimal OpenQty = Convert.ToDecimal(SqlHelper.ExecuteScalar(tran, CommandType.Text, "select OpenQty from " + mytable + " where Id=" + model.ID + " and LineNum=" + item.LineNum + ""));
                                    if (OpenQty > 0)
                                    {

                                        string oldDataQuery = @"select BaseEntry,BaseLine,BaseType,Quantity from POR1 where Id=" + model.ID + " and LineNum=" + item.LineNum + " and OpenQty <> 0";

                                        tbl_docRow docRowModel = new tbl_docRow();
                                        using (var rdr = SqlHelper.ExecuteReader(tran, CommandType.Text, oldDataQuery))
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
                                        
                                        string UpdateQuery = @"update POR1 set
                                                                  ItemCode  = '" + item.ItemCode + "'" +
                                                                ",ItemName  = '" + item.ItemName + "'" +
                                                                ",UomCode   = '" + item.UomCode + "'" +
                                                                ",UomEntry   = " + item.UomEntry +
                                                                ",Quantity  = '" + item.QTY + "'" +
                                                                ",OpenQty   = OpenQty + (" + item.QTY + "- OpenQty)" +
                                                                ",Price     = '" + item.UPrc + "',WhsCode ='" + item.Warehouse + "'" +
                                                                ",LineTotal = '" + item.TtlPrc + "'" +
                                                                ",DiscPrcnt =  " + item.DicPrc +
                                                                ",VatGroup  = '" + item.VatGroup + "'" +
                                                                ",CountryOrg= '" + item.CountryOrg + "',SaleOrderCode=@SaleOrderCode,SaleOrderDocNo=@SaleOrderDocNo,PreCostingTowelCode=@PreCostingTowelCode,AccessoriesType=@AccessoriesType,ProductionOrder=@ProductionOrder,ProductionOrderProdName=@ProductionOrderProdName " +
                                                                " where Id=" + model.ID + " and LineNum=" + item.LineNum + " and OpenQty = Quantity";
                                        int res2 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, UpdateQuery,param1.ToArray()).ToInt();
                                        if (res2 < 0)
                                        {
                                            tran.Rollback();
                                            return false;
                                        }

                                    }
                                }
                                else
                                {
                                    int LineNo = CommonDal.getLineNumber(tran, "POR1", model.ID.ToString());                                  
                                    //item.DicPrc = item.DicPrc == "" ? "NULL" : Convert.ToDecimal(item.DicPrc);

                                    string RowQueryItem = @"insert into POR1(Id,LineNum,WhsCode,ItemName,Price,LineTotal,OpenQty,ItemCode,Quantity,DiscPrcnt,VatGroup , UomCode,
                                                           UomEntry,SaleOrderCode,SaleOrderDocNo,PreCostingTowelCode,AccessoriesType,ProductionOrder,ProductionOrderProdName ,CountryOrg)
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
                                                      + item.UomEntry + ",@SaleOrderCode,@SaleOrderDocNo,@PreCostingTowelCode,@AccessoriesType,@ProductionOrder,@ProductionOrderProdName'"
                                                      + item.CountryOrg + "')";

                                    int res2 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, RowQueryItem,param1.ToArray()).ToInt();
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
                                string RowQueryService = @"insert into POR1(Id,LineNum,LineTotal,Dscription,AcctCode,VatGroup)
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
