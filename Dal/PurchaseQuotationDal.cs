using iSOL_Enterprise.Common;
using iSOL_Enterprise.Models;
using iSOL_Enterprise.Models.sale;
using Newtonsoft.Json;
using SqlHelperExtensions;
using System.Data;
using System.Data.SqlClient;
using static iSOL_Enterprise.Dal.DashboardDal;

namespace iSOL_Enterprise.Dal
{
	public class PurchaseQuotationDal
	{
        public dynamic GetPurchaseOrderEditDetails(int id)
        {
            DataSet ds = new DataSet();
            SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
            SqlDataAdapter sda = new SqlDataAdapter("select * from OPQT where id = " + id + ";select * from PQT1 where id = " + id + "", conn);
            sda.Fill(ds);
            return ds;
        }



        public List<SalesQuotation_MasterModels> GetData()
		{
			string GetQuery = "select * from OPQT order by id DESC";


			List<SalesQuotation_MasterModels> list = new List<SalesQuotation_MasterModels>();
			using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, GetQuery))
			{
				while (rdr.Read())
				{
                    
                    SalesQuotation_MasterModels models = new SalesQuotation_MasterModels();
                    models.DocStatus = CommonDal.Check_IsEditable("POR1", rdr["Id"].ToInt()) == false ? "Open" : "Closed";
                    models.Id = rdr["Id"].ToInt();
                    models.DocDate = rdr["DocDueDate"].ToDateTime();
					models.PostingDate = rdr["DocDate"].ToDateTime();
					models.DocNum = rdr["DocNum"].ToString();
					models.CardCode = rdr["CardCode"].ToString();
					models.Guid = rdr["Guid"].ToString();
					models.CardName = rdr["CardName"].ToString();
                    models.IsPosted = rdr["isPosted"].ToString();
                    list.Add(models);
				}
			}
			return list;
		}
        public dynamic GetPurchaseQuotationDetails(int id)
        {
            DataSet ds = new DataSet();
            SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
            SqlDataAdapter sda = new SqlDataAdapter("select * from OPQT where id = " + id + ";select * from PQT1 where id = " + id + "", conn);
            sda.Fill(ds);
            return ds;
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

		public List<tbl_customer> GetVendors()
		{
			string GetQuery = "select CardCode,CardName,Currency,Balance from OCRD Where CardType = 'S'";


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


        public bool AddPurchaseQoutation(string formData)
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
                    int Id = CommonDal.getPrimaryKey(tran, "OPQT");
                    string DocNum = SqlHelper.getUpdatedDocumentNumberOnLoad(tran, "OPQT", "PQ");
                    if (model.HeaderData != null)
                    {


                        string HeadQuery = @"insert into OPQT(Id,Series,DocType,Guid,CardCode,DocNum,CardName,CntctCode,DocDate,NumAtCard,DocDueDate,DocCur,TaxDate ,PQTGrpNum,ReqDate, GroupNum ,DocTotal, SlpCode , Comments) 
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
                                                + Convert.ToDateTime(model.HeaderData.TaxDate) + "',"
                                                + model.HeaderData.PQTGrpNum + ",'"
                                                + Convert.ToDateTime(model.HeaderData.ReqDate) + "','"
                                                + model.ListAccouting.GroupNum + "',"
                                                + model.FooterData.Total + ","
                                                + Convert.ToInt32(model.FooterData.SlpCode) + ",'"
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
                        int LineNo = 1;
                        foreach (var item in model.ListItems)
                        {


                            item.BaseEntry = item.BaseEntry == "" ? "null" : item.BaseEntry;
                            item.BaseLine = item.BaseLine == "" ? "null" : item.BaseLine;
                            item.BaseQty = item.BaseQty == "" ? "null" : item.BaseQty;
                            item.DicPrc = item.DicPrc == "" ? "NULL" : Convert.ToDecimal(item.DicPrc);

                            string RowQueryItem = @"insert into PQT1(Id,LineNum,WhsCode,ItemName,Price,LineTotal,ItemCode,PQTReqDate,ShipDate,PQTReqQty,OpenQty,Quantity,DiscPrcnt,VatGroup , UomCode,UomEntry ,CountryOrg)
                                              values(" + Id + ","
                                              + LineNo + ",'"
                                              + item.Warehouse + "','"
                                              + item.ItemName + "',"
                                              + item.UPrc + ","
                                              + item.TtlPrc + ",'"
                                              + item.ItemCode + "','"
                                              + Convert.ToDateTime(item.PQTReqDate) + "','"
                                              + Convert.ToDateTime(item.ShipDate) + "',"
                                              + item.PQTReqQty + ","
                                              + item.QTY + ","
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
                        int LineNo = 1;

                        foreach (var item in model.ListService)
                        {

                            item.BaseEntry = item.BaseEntry == "" ? "null" : item.BaseEntry;
                            item.BaseLine = item.BaseLine == "" ? "null" : item.BaseLine;
                            item.BaseQty = item.BaseQty == "" ? "null" : item.BaseQty;
                            //int QUT1Id = CommonDal.getPrimaryKey(tran, "QUT1");
                            string RowQueryService = @"insert into PQT1(Id,LineNum,LineTotal,Dscription,PQTReqDate,ShipDate,AcctCode,VatGroup)
                                                  values(" + Id + ","
                                                    + LineNo + ","
                                                    + item.TotalLC + ",'"
                                                    + item.Dscription + "','"
                                                    + Convert.ToDateTime(item.PQTReqDate) + "','"
                                                    + Convert.ToDateTime(item.ShipDate) + "','"
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



        public bool EditPurchaseQoutation(string formData)
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
                    var Status = CommonDal.Check_IsEditable("POR1", Convert.ToInt32(model.ID)) == false ? "Open" : "Closed";
                    if (Status == "Closed")
                    {
                        tran.Rollback();
                        return false;
                    }
                    #region Deleting Items/List

                    //string DeleteI_Or_SQuery = "Delete from PQT1 Where id = " + model.ID;
                    // res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, DeleteI_Or_SQuery).ToInt();
                    //if (res1 <= 0)
                    //{
                    //    tran.Rollback();
                    //    return false;
                    //}


                    #endregion



                    // int Id = CommonDal.getPrimaryKey(tran, "OPQT");

                    if (model.HeaderData != null)
					{


						//string HeadQuery = @"insert into OPQT(Id,DocType,Guid,CardCode,DocNum,CardName,CntctCode,DocDate,NumAtCard,DocDueDate,DocCur,TaxDate ,PQTGrpNum,ReqDate, GroupNum , SlpCode , Comments) 
						//                     values(" + Id + ",'"
						//						+ DocType + "','"
						//						+ CommonDal.generatedGuid() + "','"
						//						+ model.HeaderData.CardCode + "','"
						//						+ model.HeaderData.DocNum + "','"
						//						+ model.HeaderData.CardName + "','"
						//						+ model.HeaderData.CntctCode + "','"
						//						+ Convert.ToDateTime(model.HeaderData.DocDate) + "','"
						//						+ model.HeaderData.NumAtCard + "','"
						//						+ Convert.ToDateTime(model.HeaderData.DocDueDate) + "','"
						//						+ model.HeaderData.DocCur + "','"
						//						+ Convert.ToDateTime(model.HeaderData.TaxDate) + "',"
						//						+ model.HeaderData.PQTGrpNum + ",'"
						//						+ Convert.ToDateTime(model.HeaderData.ReqDate) + "','"
						//						+ model.ListAccouting.GroupNum + "',"
						//						+ Convert.ToInt32(model.FooterData.SlpCode) + ",'"
						//						+ model.FooterData.Comments + "')";

                        string HeadQuery = @" Update OPQT set 
                                                          DocType = '" + DocType + "'" +
                                                       ",PQTGrpNum = '" + model.HeaderData.PQTGrpNum + "'" +
                                                       ",CardName = '" + model.HeaderData.CardName + "'" +
                                                       ",CntctCode = '" + model.HeaderData.CntcCode + "'" +
                                                       ",DocDate = '" + Convert.ToDateTime(model.HeaderData.DocDate) + "'" +
                                                       ",DocDueDate = '" + Convert.ToDateTime(model.HeaderData.DocDueDate) + "'" +
                                                       ",ReqDate = '" + Convert.ToDateTime(model.HeaderData.ReqDate) + "'" +
                                                       ",TaxDate = '" + Convert.ToDateTime(model.HeaderData.TaxDate) + "'" +
                                                       ",NumAtCard = '" + model.HeaderData.NumAtCard + "'" +
                                                       ",DocCur = '" + model.HeaderData.DocCur + "'" +
                                                       ",GroupNum = '" + model.ListAccouting.GroupNum + "'" +
                                                       ",SlpCode = " + model.FooterData.SlpCode + "" +
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




                    //var GetDocNum = SqlHelper.ExecuteScalar(tran, CommandType.Text, "Select DocType from ORDR where Id = " + model.Id + " ");
                     
                        if (model.ListItems != null)
                        {

                            foreach (var item in model.ListItems)
                            {

							item.DicPrc = item.DicPrc == "" ? "null" : item.DicPrc;
							//int QUT1Id = CommonDal.getPrimaryKey(tran, "QUT1");
							if (item.LineNum != "" && item.LineNum != null)
                                {
                                    string UpdateQuery = @"update PQT1 set
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
                                                            ",PQTReqDate= '" + Convert.ToDateTime(item.PQTReqDate) + "'" +
                                                            ",ShipDate= '" + Convert.ToDateTime(item.ShipDate) + "'" +
                                                            ",PQTReqQty= " + item.PQTReqQty + "" +
                                                            " where Id=" + model.ID + " and LineNum=" + item.LineNum + " and OpenQty <> 0";
                                    int res2 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, UpdateQuery).ToInt();
                                    if (res2 <= 0)
                                    {
                                        tran.Rollback();
                                        return false;
                                    }

                                }
                                else
                                {
                                    int LineNo = CommonDal.getLineNumber(tran, "PQT1", (model.ID).ToString());
                                    string RowQueryItem = @"insert into PQT1(Id,LineNum,ItemName,Price,LineTotal,ItemCode,PQTReqDate,ShipDate,PQTReqQty,Quantity,DiscPrcnt,VatGroup , UomCode ,CountryOrg)
                                              values(" + model.ID + ","
                                                  + LineNo + ",'"
                                               + item.ItemName + "',"
                                               + item.UPrc + ","
                                               + item.TtlPrc + ",'"
                                                 + item.ItemCode + "','"
                                                 + Convert.ToDateTime(item.PQTReqDate) + "','"
                                                 + Convert.ToDateTime(item.ShipDate) + "',"
                                                 + item.PQTReqQty + ","
                                                 + item.QTY + ","
                                                 + item.DicPrc + ",'"
                                                 + item.VatGroup + "','"
                                                 + item.UomCode + "','"
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
                            int LineNo = 1;

                            foreach (var item in model.ListService)
                            {
                                //int QUT1Id = CommonDal.getPrimaryKey(tran, "QUT1");
                                string RowQueryService = @"insert into PQT1(Id,LineNum,LineTotal,Dscription,AcctCode,VatGroup)
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
