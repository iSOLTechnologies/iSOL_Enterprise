using iSOL_Enterprise.Common;
using iSOL_Enterprise.Models;
using iSOL_Enterprise.Models.Inventory;
using iSOL_Enterprise.Models.sale;
using Newtonsoft.Json;
using SqlHelperExtensions;
using System.Data;
using System.Data.SqlClient;

namespace iSOL_Enterprise.Dal.Business
{
    public class BusinessPartnerMasterDataDal
    {

        public List<ListModel> GetGroups()
        {
            string GetQuery = "select GroupCode,GroupName from OCRG  where GroupType = 'C' ORDER BY GroupCode";

            List<ListModel> list = new List<ListModel>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {
                    list.Add(new ListModel()
                    {
                        Value = rdr["GroupCode"].ToInt(),
                        Text = rdr["GroupName"].ToString()

                    });
                }
            }
            return list;
        }
        public List<tbl_OSLP> GetEmailGroup()
        { 
            string GetQuery = "select EmlGrpCode,EmlGrpName From OEGP";


            List<tbl_OSLP> list = new List<tbl_OSLP>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {

                    list.Add(
                        new tbl_OSLP()
                        {
                            SlpCode = Convert.ToInt32(rdr["EmlGrpCode"]),
                            SlpName = rdr["EmlGrpName"].ToString()
                        });

                }
            }

            return list;
        }
        public List<tbl_OSLP> GetStateCode()
        { 
            string GetQuery = "select Code,[Name] From OCST";


            List<tbl_OSLP> list = new List<tbl_OSLP>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {

                    list.Add(
                        new tbl_OSLP()
                        { 
                            Code = rdr["Code"].ToString(),
                            SlpName = rdr["Name"].ToString()
                        });

                }
            }

            return list;
        }
        public List<ListModel> GetShipTypes()
        {
            string GetQuery = "select TrnspCode,TrnspName from OSHP  where Active = 'Y' ORDER BY TrnspCode";

            List<ListModel> list = new List<ListModel>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {
                    list.Add(new ListModel()
                    {
                        Value = rdr["TrnspCode"].ToInt(),
                        Text = rdr["TrnspName"].ToString()

                    });
                }
            }
            return list;
        }
        public List<ListModel> GetNames()
        {
            string GetQuery = "select Code,Name from OIDC  ORDER BY Code";

            List<ListModel> list = new List<ListModel>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {
                    list.Add(new ListModel()
                    {
                        Value = rdr["Code"].ToInt(),
                        Text = rdr["Name"].ToString()

                    });
                }
            }
            return list;
        }
        public List<ListModel> GetProjectCodes()
        {
            string GetQuery = "select PrjCode,PrjName from OPRJ  ORDER BY PrjCode";

            List<ListModel> list = new List<ListModel>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {
                    list.Add(new ListModel()
                    {
                        Value = rdr["PrjCode"].ToInt(),
                        Text = rdr["PrjName"].ToString()

                    });
                }
            }
            return list;
        }
        public List<ListModel> GetBusinessPartners()
        {
            string GetQuery = "select CardCode,CardName,Balance from OCRD where CardType = 'C'  ORDER BY CardCode";

            List<ListModel> list = new List<ListModel>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {
                    list.Add(new ListModel()
                    {
                        Value = rdr["CardCode"].ToInt(),
                        Text = rdr["CardName"].ToString() + " -- " + rdr["Balance"].ToString()

                    });
                }
            }
            return list;
        }
        public List<ListModel> GetIndustries()
        {
            string GetQuery = "select IndCode,IndName from OOND  ORDER BY IndCode";

            List<ListModel> list = new List<ListModel>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {
                    list.Add(new ListModel()
                    {
                        Value = rdr["IndCode"].ToInt(),
                        Text = rdr["IndName"].ToString() 

                    });
                }
            }
            return list;
        }
        public List<ListModel> GetTechnicians()
        {
            string GetQuery = "select empID,LastName,firstName from OHEM  ORDER BY empID";

            List<ListModel> list = new List<ListModel>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {
                    list.Add(new ListModel()
                    {
                        Value = rdr["empID"].ToInt(),
                        Text = rdr["LastName"].ToString() +" "+ rdr["firstName"].ToString()

                    });
                }
            }
            return list;
        }
        public List<ListModel> GetTerritories()
        {
            string GetQuery = "select territryID,descript from OTER where inactive = 'N'";

            List<ListModel> list = new List<ListModel>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {
                    list.Add(new ListModel()
                    {
                        Value = rdr["territryID"].ToInt(),
                        Text = rdr["descript"].ToString() 

                    });
                }
            }
            return list;
        }
        public List<tbl_OITG> GetProperties()
        {
            string GetQuery = "select GroupCode,GroupName from OCQG ORDER BY GroupCode";

            List<tbl_OITG> list = new List<tbl_OITG>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {
                    list.Add(new tbl_OITG()
                    {
                        ItmsTypCod = rdr["GroupCode"].ToInt(),
                        ItmsGrpNam = rdr["GroupName"].ToString()

                    });
                }
            }
            return list;
        }











        public ResponseModels AddBusinessMasterData(string formData)
        {
            var model = JsonConvert.DeserializeObject<dynamic>(formData);
            ResponseModels response = new ResponseModels();
            CommonDal cdal = new CommonDal();
            SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
            conn.Open();
            int MySeries = Convert.ToInt32(model.HeaderData.MySeries);
            SqlTransaction tran = conn.BeginTransaction();
            int res1 = 0;
            try
            {
                //int Id = CommonDal.getPrimaryKey(tran, "OITM");
                //string DocNum = SqlHelper.getUpdatedDocumentNumberOnLoad(tran, "OQUT", "SQ");
                if (model.HeaderData != null)
                {
                    List<SqlParameter> param = new List<SqlParameter>();
                    int Id = CommonDal.getPrimaryKey(tran, "OIGE");

                    param.Add(cdal.GetParameter("@Id", Id, typeof(int)));
                    param.Add(cdal.GetParameter("@Guid", CommonDal.generatedGuid(), typeof(string)));

                    #region BackendCheck For Series
                    if (MySeries != -1)
                    {
                        string? DocNum = SqlHelper.MySeriesUpdate_GetItemCode(MySeries, tran);
                        if (DocNum == null)
                        {
                            tran.Rollback();
                            response.isSuccess = false;
                            response.Message = "An Error Occured";
                            return response;
                        }
                        model.HeaderData.DocNum = DocNum;
                    }
                    #endregion
                    else
                    {
                        int count = SqlHelper.ExecuteScalar(tran, CommandType.Text, "select Count(*) from OIGE where DocNum ='" + model.HeaderData.DocNum.ToString() + "'");
                        if (count > 0)
                        {
                            tran.Rollback();
                            response.isSuccess = false;
                            response.Message = "Duplicate Document Number !";
                            return response;
                        }
                    }
                    string HeadQuery = @"insert into OCRD (Id,Guid,MySeries,DocNum,Series,DocDate,GroupNum,TaxDate,Ref2,Comments,JrnlMemo,DocTotal) 
                                        values(@Id,@Guid,@MySeries,@DocNum,@Series,@DocDate,@GroupNum,@TaxDate,@Ref2,@Comments,@JrnlMemo,@DocTotal)";



                    #region SqlParameters

                    #region Header data
                    param.Add(cdal.GetParameter("@MySeries", model.HeaderData.MySeries, typeof(int)));
                    param.Add(cdal.GetParameter("@DocNum", model.HeaderData.DocNum, typeof(string)));
                    param.Add(cdal.GetParameter("@Series", model.HeaderData.Series, typeof(int)));
                    param.Add(cdal.GetParameter("@DocDate", model.HeaderData.DocDate, typeof(DateTime)));
                    param.Add(cdal.GetParameter("@GroupNum", model.HeaderData.GroupNum, typeof(Int16)));
                    param.Add(cdal.GetParameter("@TaxDate", model.HeaderData.TaxDate, typeof(DateTime)));
                    param.Add(cdal.GetParameter("@Ref2", model.HeaderData.Ref2, typeof(string)));
                    #endregion

                    #region Footer Data
                    param.Add(cdal.GetParameter("@Comments", model.FooterData.Comments, typeof(string)));
                    param.Add(cdal.GetParameter("@JrnlMemo", model.FooterData.JrnlMemo, typeof(string)));
                    param.Add(cdal.GetParameter("@DocTotal", model.FooterData.TotalBeforeDiscount, typeof(decimal)));
                    #endregion




                    #endregion

                    res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, HeadQuery, param.ToArray()).ToInt();
                    if (res1 <= 0)
                    {
                        tran.Rollback();
                        response.isSuccess = false;
                        response.Message = "An Error Occured";
                        return response;
                    }

                    if (model.ListItems != null)
                    {
                        int LineNum = 0;
                        foreach (var item in model.ListItems)
                        {

                            string RowQueryItem1 = @"insert into IGE1
                                (Id,LineNum,BaseRef,BaseEntry,BaseLine,ItemCode,Dscription,WhsCode,Quantity,Price,LineTotal,AcctCode,UomEntry,UomCode,BaseQty,OpenQty)
                          values(@Id,@LineNum,@BaseRef,@BaseEntry,@BaseLine,@ItemCode,@Dscription,@WhsCode,@Quantity,@Price,@LineTotal,@AcctCode,@UomEntry,@UomCode,@BaseQty,@OpenQty)";
                            var BaseRef = item.BaseRef;
                            #region sqlparam
                            List<SqlParameter> param1 = new List<SqlParameter>();
                            param1.Add(cdal.GetParameter("@Id", Id, typeof(int)));
                            param1.Add(cdal.GetParameter("@LineNum", LineNum, typeof(int)));
                            param1.Add(cdal.GetParameter("@BaseRef", item.BaseRef, typeof(string)));
                            param1.Add(cdal.GetParameter("@BaseEntry", item.BaseEntry, typeof(int)));
                            param1.Add(cdal.GetParameter("@BaseLine", item.BaseLine, typeof(int)));
                            param1.Add(cdal.GetParameter("@ItemCode", item.ItemCode, typeof(string)));
                            param1.Add(cdal.GetParameter("@Dscription", item.ItemName, typeof(string)));
                            param1.Add(cdal.GetParameter("@WhsCode", item.Warehouse, typeof(string)));
                            param1.Add(cdal.GetParameter("@Quantity", item.QTY, typeof(decimal)));
                            param1.Add(cdal.GetParameter("@Price", item.UPrc, typeof(decimal)));
                            param1.Add(cdal.GetParameter("@LineTotal", item.TtlPrc, typeof(decimal)));
                            param1.Add(cdal.GetParameter("@AcctCode", item.AcctCode, typeof(string)));
                            //param1.Add(cdal.GetParameter("@ItemCost", item.ItemCost, typeof(string)));
                            param1.Add(cdal.GetParameter("@UomEntry", item.UomEntry, typeof(int)));
                            param1.Add(cdal.GetParameter("@UomCode", item.UomCode, typeof(string)));
                            param1.Add(cdal.GetParameter("@BaseQty", item.BaseQty, typeof(string)));
                            param1.Add(cdal.GetParameter("@OpenQty", item.QTY, typeof(decimal)));

                            #endregion

                            res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, RowQueryItem1, param1.ToArray()).ToInt();
                            if (res1 <= 0)
                            {
                                tran.Rollback();
                                response.isSuccess = false;
                                response.Message = "An Error Occured";
                                return response;
                            }
                            int LogEntry = CommonDal.getPrimaryKey(tran, "LogEntry", "OITL");
                            //int QUT1Id = CommonDal.getPrimaryKey(tran, "DLN1");
                            #region UpdateWarehouse&GenerateLog
                            #region OITLLog

                            item.BaseType = item.BaseType == "" ? "NULL" : Convert.ToInt32(item.BaseType);

                            string LogQueryOITL = @"insert into OITL(LogEntry,ItemCode,ItemName,DocEntry,DocLine,DocType,BaseType,DocNum,DocQty,DocDate) 
                                           values(" + LogEntry + ",'"
                                              //+ model.HeaderData.CardCode + "','"
                                              + item.ItemCode + "','"
                                              + item.ItemName + "',"
                                              //+ model.HeaderData.CardName + "',"
                                              + Id + ","
                                              + LineNum + ","
                                              + 60 + ","
                                              + item.BaseType + ","
                                              + Id + ","
                                              + -1 * ((Decimal)(item.QTY)) + ",'"
                                              + Convert.ToDateTime(model.HeaderData.DocDate) + "')";

                            res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, LogQueryOITL).ToInt();
                            if (res1 <= 0)
                            {
                                tran.Rollback();
                                response.isSuccess = false;
                                response.Message = "An Error Occured";
                                return response;
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


                                                        string? ExpDate = rdr["ExpDate"].ToString() == "" ? "" : (Convert.ToDateTime(rdr["ExpDate"]).ToString());
                                                        string? InDate = rdr["InDate"].ToString() == "" ? "" : (Convert.ToDateTime(rdr["InDate"]).ToString());

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
                                                                   + ((Decimal)(ii.Quantity) - (Decimal)(ii.selectqty)) + ","
                                                                   + Convert.ToInt32(rdr["AbsEntry"]) + ")";


                                                        res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, InsertBatchQuery).ToInt();
                                                        if (res1 <= 0)
                                                        {
                                                            tran.Rollback();
                                                            response.isSuccess = false;
                                                            response.Message = "An Error Occured";
                                                            return response;
                                                        }


                                                    }
                                                }

                                            }
                                            #endregion

                                            #region Record found in OBTQ
                                            else
                                            {
                                                string BatchQueryOBTN = @"Update OBTQ set Quantity = " + ((Decimal)(ii.Quantity) - (Decimal)(ii.selectqty)) + " WHERE AbsEntry = " + ii.AbsEntry + "";

                                                res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, BatchQueryOBTN).ToInt();
                                                if (res1 <= 0)
                                                {
                                                    tran.Rollback();
                                                    response.isSuccess = false;
                                                    response.Message = "An Error Occured";
                                                    return response;
                                                }
                                            }
                                            #endregion

                                            #region ITL1 log
                                            string LogQueryITL1 = @"insert into ITL1(LogEntry,ItemCode,SysNumber,Quantity,AllocQty,MdAbsEntry) 
                                                   values(" + LogEntry + ",'"
                                                 + item.ItemCode + "','"
                                                 + ii.SysNumber + "',"
                                                 + -1 * ((Decimal)(ii.selectqty)) + ","
                                                 + -1 * ((Decimal)(ii.selectqty)) + ","
                                                 + ii.AbsEntry + ")";


                                            res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, LogQueryITL1).ToInt();
                                            if (res1 <= 0)
                                            {
                                                tran.Rollback();
                                                response.isSuccess = false;
                                                response.Message = "An Error Occured";
                                                return response;
                                            }
                                            #endregion
                                        }
                                        else break;
                                    }
                                }
                            }
                            #endregion
                            #endregion
                            #region Update OITW If Sap Integration is OFF

                            if (!SqlHelper.SAPIntegration)
                            {
                                string UpdateOITWQuery = @"Update OITW set onHand = onHand - @Quantity where WhsCode = '" + item.Warehouse + "' and ItemCode = '" + item.ItemCode + "'";
                                List<SqlParameter> param2 = new List<SqlParameter>();
                                param2.Add(cdal.GetParameter("@Quantity", item.QTY, typeof(decimal)));
                                res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, UpdateOITWQuery, param2.ToArray()).ToInt();
                                if (res1 <= 0)
                                {
                                    tran.Rollback();
                                    response.isSuccess = false;
                                    response.Message = "An Error Occured";
                                    return response;
                                }
                            }

                            #endregion

                            LineNum += 1;
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

                                res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, RowQueryAttachment).ToInt();
                                if (res1 <= 0)
                                {
                                    tran.Rollback();
                                    response.isSuccess = false;
                                    response.Message = "An Error Occured";
                                    return response;

                                }
                                LineNo += 1;
                            }
                        }
                    }
                }
                if (res1 > 0)
                {
                    tran.Commit();
                    response.isSuccess = true;
                    response.Message = "Goods Issue Added Successfully !";

                }

            }

            catch (Exception e)
            {
                tran.Rollback();
                response.isSuccess = false;
                response.Message = e.Message;
                return response;
            }
            return response;

        }













    }
}
