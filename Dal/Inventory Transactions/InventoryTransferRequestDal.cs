using iSOL_Enterprise.Common;
using iSOL_Enterprise.Models;
using iSOL_Enterprise.Models.sale;
using Newtonsoft.Json;
using SqlHelperExtensions;
using System.Data;
using System.Data.SqlClient;

namespace iSOL_Enterprise.Dal.Inventory_Transactions
{
    public class InventoryTransferRequestDal
    {



        public List<SalesQuotation_MasterModels> GetData()
        {
            string GetQuery = "select * from OWTQ order by id DESC";


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
                    models.IsPosted = rdr["isPosted"].ToString(); models.IsEdited = rdr["is_Edited"].ToString();
                    list.Add(models);
                }
            }
            return list;
        }
        public dynamic GetHeaderOldData(int ItemID)
        {
            try
            {

                DataSet ds = new DataSet();
                SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
                string headerQuery = @"select MySeries,DocNum,Series,CardCode,CardName,DocDate,CntctCode,DocDueDate,TaxDate,Address,GroupNum,Filler,ToWhsCode,SlpCode,JrnlMemo,Comments From OWTQ where Id =" + ItemID;
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
                string headerQuery = @"select ItemCode,Dscription,FromWhsCod,WhsCode,Quantity,UomEntry,UomCode From WTQ1 where Id =" + ItemID;
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
        public ResponseModels AddInventoryTransferRequest(string formData)
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
                    int Id = CommonDal.getPrimaryKey(tran, "OWTQ");

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
                        int count = SqlHelper.ExecuteScalar(tran, CommandType.Text, "select Count(*) from OWTQ where DocNum ='" + model.HeaderData.DocNum.ToString() + "'");
                        if (count > 0)
                        {
                            tran.Rollback();
                            response.isSuccess = false;
                            response.Message = "Duplicate Document Number !";
                            return response;
                        }
                    }
                    string HeadQuery = @"insert into OWTQ (Id,Guid,MySeries,DocNum,Series,DocDate,CntctCode,DocDueDate,GroupNum,TaxDate,Address,ShipToCode,CardName,CardCode,Comments,JrnlMemo,Filler,ToWhsCode) 
                                        values(@Id,@Guid,@MySeries,@DocNum,@Series,@DocDate,@CntctCode,@DocDueDate,@GroupNum,@TaxDate,@Address,@ShipToCode,@CardName,@CardCode,@Comments,@JrnlMemo,@Filler,@ToWhsCode)";



                    #region SqlParameters

                    #region Header data
                    param.Add(cdal.GetParameter("@MySeries", model.HeaderData.MySeries, typeof(int)));
                    param.Add(cdal.GetParameter("@DocNum", model.HeaderData.DocNum, typeof(string)));
                    param.Add(cdal.GetParameter("@Series", model.HeaderData.Series, typeof(int)));
                    param.Add(cdal.GetParameter("@DocDate", model.HeaderData.DocDate, typeof(DateTime)));
                    param.Add(cdal.GetParameter("@CntctCode", model.HeaderData.CntcCode, typeof(int)));
                    param.Add(cdal.GetParameter("@DocDueDate", model.HeaderData.DocDueDate, typeof(DateTime)));
                    param.Add(cdal.GetParameter("@GroupNum", model.HeaderData.GroupNum, typeof(Int16)));
                    param.Add(cdal.GetParameter("@TaxDate", model.HeaderData.TaxDate, typeof(DateTime)));
                    param.Add(cdal.GetParameter("@CardCode", model.HeaderData.CardCode, typeof(string)));
                    param.Add(cdal.GetParameter("@CardName", model.HeaderData.CardName, typeof(string)));
                   // param.Add(cdal.GetParameter("@Name", model.HeaderData.CntcCode, typeof(string)));
                    param.Add(cdal.GetParameter("@Address", model.HeaderData.Address, typeof(string)));
                    param.Add(cdal.GetParameter("@ShipToCode", model.HeaderData.ShipToCode, typeof(string)));
                    param.Add(cdal.GetParameter("@Filler", model.HeaderData.Filler, typeof(string)));
                    param.Add(cdal.GetParameter("@ToWhsCode", model.HeaderData.ToWhsCode, typeof(string)));
                    #endregion

                    #region Footer Data
                    param.Add(cdal.GetParameter("@SlpCode", model.FooterData.JrnlMemo, typeof(string))); 
                    param.Add(cdal.GetParameter("@Comments", model.FooterData.Comments, typeof(string)));
                    param.Add(cdal.GetParameter("@JrnlMemo", model.FooterData.JrnlMemo, typeof(string))); 
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

                            string RowQueryItem1 = @"insert into WTQ1
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
                            //int LogEntry2 = LogEntry + 1;
                            //int LogEntry2 = CommonDal.getPrimaryKey(tran, "LogEntry", "OITL");

                            //int QUT1Id = CommonDal.getPrimaryKey(tran, "DLN1");
                            
                            #region UpdateWarehouse&GenerateLog
                                
                                #region OITLLog

                                item.BaseType = item.BaseType == "" ? "NULL" : Convert.ToInt32(item.BaseType);
                            //First query is For From & Second Query is for To

                            string LogQueryOITL = @"insert into OITL(LogEntry,CardCode,ItemCode,CardName,ItemName,DocEntry,DocLine,DocType,BaseType,DocNum,DocQty,DocDate) 
                                                    values(" + LogEntry + ",'"
                                              + model.HeaderData.CardCode + "','"
                                              + item.ItemCode + "','"
                                              + item.ItemName + "','"
                                              + model.HeaderData.CardName + "',"
                                              + Id + ","
                                              + LineNum + ","
                                              + 1250000001 + ","
                                              + item.BaseType + ","
                                              + Id + ","
                                              + ((Decimal)(item.QTY)) + ",'"
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

                            LineNum += 1;
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
                                                int SysNumber = CommonDal.getSysNumber(tran, item.ItemCode.ToString());
                                                int AbsEntry = CommonDal.getPrimaryKey(tran, "AbsEntry", "OBTN");   //Primary Key
                                                int ObtqAbsEntry = CommonDal.getPrimaryKey(tran, "AbsEntry", "OBTQ"); //Primary Key
                                                tbl_OBTN OldBatchData = GetBatchList(tran,item.ItemCode.ToString(), ii.DistNumber.ToString(), item.Warehouse.ToString());
                                                #region From WareHouse Working

                                           

                                                    #region Record Found in OBTQ For From Whs
                                                    if (OldBatchData.AbsEntry > 0)
                                                    {
                                                        #region Update OBTQ

                                                        string BatchQueryOBTN = @"Update OBTQ set CommitQty = CommitQty +" + (Decimal)(ii.selectqty) + " WHERE AbsEntry = " + OldBatchData.AbsEntry;

                                                        res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, BatchQueryOBTN).ToInt();
                                                        if (res1 <= 0)
                                                        {
                                                            tran.Rollback();
                                                            response.isSuccess = false;
                                                            response.Message = "An Error Occured";
                                                            return response;
                                                        }
                                                        SysNumber = OldBatchData.SysNumber;
                                                        AbsEntry = OldBatchData.MdAbsEntry;
                                                        #endregion
                                                    }
                                                    #endregion

                                                    #region Record Not Found in OBTQ For From Whs
                                                    else
                                                    {
                                                        batch[0].ExpDate = batch[0].ExpDate.ToString() == "" ? null : Convert.ToDateTime(batch[0].ExpDate);
                                                        #region Insert in OBTN
                                                        string BatchQueryOBTN = @"insert into OBTN(AbsEntry,ItemCode,SysNumber,DistNumber,InDate,ExpDate)
                                                                                                    values(" + AbsEntry + ",'"
                                                                                + item.ItemCode + "',"
                                                                                + SysNumber + ",'"
                                                                                + ii.DistNumber + "','"
                                                                                + DateTime.Now + "','"
                                                                                + batch[0].ExpDate + "')";


                                                        res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, BatchQueryOBTN).ToInt();
                                                        if (res1 <= 0)
                                                        {
                                                            tran.Rollback();
                                                            response.isSuccess = false;
                                                            response.Message = "An Error Occured";
                                                            return response;
                                                        }
                                                        #endregion


                                                        #region Insert in OBTQ
                                                        
                                                        string BatchQueryOBTQ = @"insert into OBTQ(AbsEntry,MdAbsEntry,ItemCode,SysNumber,WhsCode,Quantity,CommitQty)
                                                                                                    values(" + ObtqAbsEntry + ","
                                                                                + AbsEntry + ",'"
                                                                                + item.ItemCode + "',"
                                                                                + SysNumber + ",'"
                                                                                + item.Warehouse + "',"
                                                                                + ((Decimal)(ii.Quantity)  + ","
                                                                                + (Decimal)(ii.selectqty)) + ")";

                                                        res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, BatchQueryOBTQ).ToInt();
                                                        if (res1 <= 0)
                                                        {
                                                            tran.Rollback();
                                                            response.isSuccess = false;
                                                            response.Message = "An Error Occured";
                                                            return response;
                                                        }
                                                        #endregion
                                                    }




                                                    #endregion

                                                #endregion
                                            
                                                #region To WareHouse Working

                                                //tbl_OBTN OldBatchData1 = GetBatchList(tran,item.ItemCode.ToString(), ii.DistNumber.ToString() , item.WhsCode.ToString());

                                                    #region Record Found in OBTQ For To Whs
                                                        //if (OldBatchData1.AbsEntry > 0)
                                                        //{
                                                        //    #region Update OBTQ

                                                        //    //string BatchQueryOBTN = @"Update OBTQ set Quantity = Quantity +" + ((Decimal)(ii.selectqty)) + " WHERE AbsEntry = " + OldBatchData1.AbsEntry;

                                                        //    //res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, BatchQueryOBTN).ToInt();
                                                        //    //if (res1 <= 0)
                                                        //    //{
                                                        //    //    tran.Rollback();
                                                        //    //    response.isSuccess = false;
                                                        //    //    response.Message = "An Error Occured";
                                                        //    //    return response;
                                                        //    //}
                                                        //    //SysNumber = OldBatchData1.SysNumber;
                                                        //    //AbsEntry = OldBatchData1.MdAbsEntry;
                                                        //    #endregion
                                                        //}
                                                    #endregion

                                                    #region Record Not Found in OBTQ For To Whs
                                                        //else
                                                        //{

                                                            #region Insert in OBTN
                                                            //string BatchQueryOBTN = @"insert into OBTN(AbsEntry,ItemCode,SysNumber,DistNumber,InDate,ExpDate)
                                                            //                                values(" + AbsEntry + ",'"
                                                            //                        + itemno + "',"
                                                            //                        + SysNumber + ",'"
                                                            //                        + ii.DistNumber + "','"
                                                            //                        + DateTime.Now + "','"
                                                            //                        + Convert.ToDateTime(batch[0].ExpDate) + "')";


                                                            //res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, BatchQueryOBTN).ToInt();
                                                            //if (res1 <= 0)
                                                            //{
                                                            //    tran.Rollback();
                                                            //    response.isSuccess = false;
                                                            //    response.Message = "An Error Occured";
                                                            //    return response;
                                                            //}
                                                            #endregion


                                                            #region Insert in OBTQ
                                                            //int ObtqAbsEntry1 = CommonDal.getPrimaryKey(tran, "AbsEntry", "OBTQ");
                                                            //string BatchQueryOBTQ = @"insert into OBTQ(AbsEntry,MdAbsEntry,ItemCode,SysNumber,WhsCode,Quantity)
                                                            //                                values(" + ObtqAbsEntry1 + ","
                                                            //                        + AbsEntry + ",'"
                                                            //                        + item.ItemCode + "',"
                                                            //                        + SysNumber + ",'"
                                                            //                        + item.WhsCode + "',"
                                                            //                        + ii.selectqty + ")";

                                                            //res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, BatchQueryOBTQ).ToInt();
                                                            //if (res1 <= 0)
                                                            //{
                                                            //    tran.Rollback();
                                                            //    response.isSuccess = false;
                                                            //    response.Message = "An Error Occured";
                                                            //    return response;
                                                            //}
                                                            #endregion
                                                        //}
                                                    #endregion

                                                #endregion

                                            #region ITL1 log
                                            string LogQueryITL1 = @"insert into ITL1(LogEntry,ItemCode,SysNumber,Quantity,AllocQty,MdAbsEntry) 
                                                                           values(" + LogEntry + ",'"
                                                                     + item.ItemCode + "','"
                                                                     + SysNumber + "',"
                                                                     + "00,"
                                                                     + ((Decimal)(ii.selectqty)) + ","
                                                                     + AbsEntry + ")";


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
            #endregion
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
