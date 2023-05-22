using iSOL_Enterprise.Common;
using iSOL_Enterprise.Models;
using iSOL_Enterprise.Models.Logs;
using iSOL_Enterprise.Models.sale;
using Newtonsoft.Json;
using SqlHelperExtensions;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace iSOL_Enterprise.Dal.Inventory_Transactions
{
    public class InventoryTransferDal
    {



        public List<SalesQuotation_MasterModels> GetData()
        {
            string GetQuery = "select * from OWTR order by id DESC";


            List<SalesQuotation_MasterModels> list = new List<SalesQuotation_MasterModels>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {
                    SalesQuotation_MasterModels models = new SalesQuotation_MasterModels();
                   // models.DocStatus = CommonDal.Check_IsNotEditable("PDN1", rdr["Id"].ToInt()) == false ? "Open" : "Closed";
                    models.DocStatus = "Open";
                    models.Id = rdr["Id"].ToInt();
                    models.DocDate = rdr["DocDate"].ToDateTime();
                    models.PostingDate = rdr["TaxDate"].ToDateTime();
                    models.DocNum = rdr["DocNum"].ToString();
                    models.CardCode = rdr["CardCode"].ToString();
                    models.Guid = rdr["Guid"].ToString();
                    models.CardName = rdr["CardName"].ToString();
                    models.IsPosted = rdr["isPosted"].ToString();
                    models.IsEdited = rdr["is_Edited"].ToString();
                    models.Sap_Ref_No = rdr["Sap_Ref_No"].ToString();
                    list.Add(models);
                }
            }
            return list;
        }
        public int GetId(string guid)
        {
            guid = HttpUtility.UrlDecode(guid);
            return Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.defaultDB, CommandType.Text, "select Id from OWTR where GUID ='" + guid.ToString() + "'"));

        }
        public dynamic GetHeaderOldData(int ItemID)
        {
            try
            {

                DataSet ds = new DataSet();
                SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
                string headerQuery = @"select MySeries,DocNum,Series,CardCode,CardName,DocDate,CntctCode,DocDueDate,TaxDate,Address,GroupNum,Filler,ToWhsCode,SlpCode,JrnlMemo,Comments,
                                       TypeDetail,ProductionOrderNo,ChallanNo,DONo,SaleOrderNo From OWTR where Id =" + ItemID;
                SqlDataAdapter sda = new SqlDataAdapter(headerQuery, conn);
                sda.Fill(ds);
                string JSONString = string.Empty;
                JSONString = Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables);
                return JSONString;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public dynamic GetRowOldData(int ItemID)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
                string headerQuery = @"select ItemCode,Dscription,FromWhsCod,WhsCode,Quantity,UomEntry,UomCode From WTR1 where Id =" + ItemID;
                SqlDataAdapter sda = new SqlDataAdapter(headerQuery, conn);
                sda.Fill(ds);
                string JSONString = string.Empty;
                JSONString = Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables);
                return JSONString;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public tbl_OBTN GetBatchList(SqlTransaction tran, string itemcode, string distnumber,string WhsCode)
        {
            try
            {


                string GetQuery = "select OBTQ.MdAbsEntry,OBTQ.SysNumber,OBTQ.AbsEntry  from OBTN inner join OBTQ on OBTQ.MdAbsEntry = OBTN.AbsEntry where OBTN.ItemCode ='" + itemcode + "' and OBTN.DistNumber = '" + distnumber + "' and OBTQ.WhsCode ='"+WhsCode+"'";


                tbl_OBTN model = new tbl_OBTN();
                using (var rdr = SqlHelper.ExecuteReader(tran, CommandType.Text, GetQuery))
                {
                    while (rdr.Read())
                    {


                        model.MdAbsEntry = Convert.ToInt32(rdr["MdAbsEntry"]);
                        model.AbsEntry = Convert.ToInt32(rdr["AbsEntry"]);
                        model.SysNumber = rdr["SysNumber"].ToInt();



                    }
                }

                return model;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ResponseModels AddInventoryTransfer(string formData)
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
                    int Id = CommonDal.getPrimaryKey(tran, "OWTR");

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
                        int count = SqlHelper.ExecuteScalar(tran, CommandType.Text, "select Count(*) from OWTR where DocNum ='" + model.HeaderData.DocNum.ToString() + "'");
                        if (count > 0)
                        {
                            tran.Rollback();
                            response.isSuccess = false;
                            response.Message = "Duplicate Document Number !";
                            return response;
                        }
                    }
                    string HeadQuery = @"insert into OWTR (Id,Guid,MySeries,DocNum,Series,DocDate,GroupNum,TaxDate,Address,ShipToCode,CardName,CardCode,Comments,
                                        JrnlMemo,Filler,ToWhsCode,CntctCode,PurchaseType,TypeDetail,ProductionOrderNo,ChallanNo,DONo,SaleOrderNo) 
                                        values(@Id,@Guid,@MySeries,@DocNum,@Series,@DocDate,@GroupNum,@TaxDate,@Address,@ShipToCode,@CardName,@CardCode,@Comments,
                                        @JrnlMemo,@Filler,@ToWhsCode,@CntctCode,@PurchaseType,@TypeDetail,@ProductionOrderNo,@ChallanNo,@DONo,@SaleOrderNo)";
                     
                    #region SqlParameters
                    #region Header data
                    param.Add(cdal.GetParameter("@MySeries", model.HeaderData.MySeries, typeof(int)));
                    param.Add(cdal.GetParameter("@DocNum", model.HeaderData.DocNum, typeof(string)));
                    param.Add(cdal.GetParameter("@Series", model.HeaderData.Series, typeof(int)));
                    param.Add(cdal.GetParameter("@DocDate", model.HeaderData.DocDate, typeof(DateTime)));
                    param.Add(cdal.GetParameter("@GroupNum", model.HeaderData.GroupNum, typeof(Int16)));
                    param.Add(cdal.GetParameter("@TaxDate", model.HeaderData.TaxDate, typeof(DateTime)));
                    param.Add(cdal.GetParameter("@CardCode", model.HeaderData.CardCode, typeof(string)));
                    param.Add(cdal.GetParameter("@CardName", model.HeaderData.CardName, typeof(string)));
                    param.Add(cdal.GetParameter("@Address", model.HeaderData.Address, typeof(string)));
                    param.Add(cdal.GetParameter("@ShipToCode", model.HeaderData.ShipToCode, typeof(string)));
                    param.Add(cdal.GetParameter("@Filler", model.HeaderData.Filler, typeof(string)));
                    param.Add(cdal.GetParameter("@ToWhsCode", model.HeaderData.ToWhsCode, typeof(string)));
                    param.Add(cdal.GetParameter("@CntctCode", model.HeaderData.CntcCode, typeof(int)));
                    #endregion

                    #region Footer Data
                    param.Add(cdal.GetParameter("@SlpCode", model.FooterData.JrnlMemo, typeof(string))); 
                    param.Add(cdal.GetParameter("@Comments", model.FooterData.Comments, typeof(string)));
                    param.Add(cdal.GetParameter("@JrnlMemo", model.FooterData.JrnlMemo, typeof(string)));
                    #endregion

                    #region UDFs
                    param.Add(cdal.GetParameter("@PurchaseType", model.HeaderData.PurchaseType, typeof(decimal)));
                    param.Add(cdal.GetParameter("@TypeDetail", model.HeaderData.TypeDetail, typeof(string)));
                    param.Add(cdal.GetParameter("@ProductionOrderNo", model.HeaderData.ProductionOrderNo, typeof(decimal)));
                    param.Add(cdal.GetParameter("@ChallanNo", model.HeaderData.ChallanNo, typeof(decimal)));
                    param.Add(cdal.GetParameter("@DONo", model.HeaderData.DONo, typeof(decimal)));
                    param.Add(cdal.GetParameter("@SaleOrderNo", model.HeaderData.SalesOrderCode, typeof(int)));

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

                            string RowQueryItem1 = @"insert into WTR1
                                (Id,LineNum,BaseRef,BaseEntry,BaseLine,ItemCode,Dscription,WhsCode,FromWhsCod,Quantity,UomEntry,UomCode,BaseQty,OpenQty)
                          values(@Id,@LineNum,@BaseRef,@BaseEntry,@BaseLine,@ItemCode,@Dscription,@WhsCode,@FromWhsCod,@Quantity,@UomEntry,@UomCode,@BaseQty,@OpenQty)";
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
                            param1.Add(cdal.GetParameter("@WhsCode", item.WhsCode, typeof(string)));
                            param1.Add(cdal.GetParameter("@FromWhsCod", item.Warehouse, typeof(string)));
                            param1.Add(cdal.GetParameter("@Quantity", item.QTY, typeof(decimal)));  
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
                            int LogEntry2 = LogEntry + 1;
                            //int LogEntry2 = CommonDal.getPrimaryKey(tran, "LogEntry", "OITL");

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
                            OITLModel.DocLine = LineNum;
                            OITLModel.DocType = 67;
                            OITLModel.BaseType = item.BaseType;
                            OITLModel.Quantity = -1 * ((Decimal)(item.QTY));
                            OITLModel.DocDate = Convert.ToDateTime(model.HeaderData.DocDate);

                            if (!cdal.OITLLog(tran, OITLModel))
                            {
                                tran.Rollback();
                                response.isSuccess = false;
                                response.Message = "An Error Occured";
                                return response;
                            }

                            OITL OITLModel2 = new OITL();
                            OITLModel2.LogEntry = LogEntry2;
                            OITLModel2.CardCode = model.HeaderData.CardCode.ToString();
                            OITLModel2.CardName = model.HeaderData.CardName.ToString();
                            OITLModel2.ItemCode = item.ItemCode.ToString();
                            OITLModel2.ItemName = item.ItemName.ToString();
                            OITLModel2.ID = Id;
                            OITLModel2.DocLine = LineNum;
                            OITLModel2.DocType = 67;
                            OITLModel2.BaseType = item.BaseType;
                            OITLModel2.Quantity = ((Decimal)(item.QTY));
                            OITLModel2.DocDate = Convert.ToDateTime(model.HeaderData.DocDate);

                            if (!cdal.OITLLog(tran, OITLModel2))
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

                                    bool responseBatch = cdal.OutBatches(tran, model.Batches, item.ItemCode.ToString(), LogEntry, item.Warehouse.ToString(), LineNum);
                                    if (!responseBatch)
                                    {
                                        tran.Rollback();
                                        response.isSuccess = false;
                                        response.Message = "An Error Occured";
                                        return response;
                                    }

                                    bool responseBatch2 = cdal.InBatches(tran, model.Batches, item.ItemCode.ToString(), LogEntry2, item.WhsCode.ToString(), LineNum , item.Warehouse.ToString());
                                    if (!responseBatch2)
                                    {
                                        tran.Rollback();
                                        response.isSuccess = false;
                                        response.Message = "An Error Occured";
                                        return response;
                                    }
                                
                                }
                            #endregion

                            #region Update OITW If Sap Integration is OFF

                            if (!SqlHelper.SAPIntegration)
                            {
                                string UpdateOITWQuery = @"Update OITW set onHand = onHand - @Quantity where WhsCode = '" + item.Warehouse.ToString() + "' and ItemCode = '" + item.ItemCode.ToString() + "';" +
                                                          "Update OITW set onHand = onHand + @Quantity where WhsCode = '" + item.WhsCode.ToString() + "' and ItemCode = '" + item.ItemCode.ToString() + "'";
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
                        #endregion


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
                    response.Message = "Item Added Successfully !";

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
