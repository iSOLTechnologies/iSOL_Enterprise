using iSOL_Enterprise.Models.sale;
using SqlHelperExtensions;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;
using iSOL_Enterprise.Common;
using iSOL_Enterprise.Models;
using System.Reflection.Emit;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json.Linq;

namespace iSOL_Enterprise.Dal.Inventory_Transactions
{
    public class GoodReceiptGRDal
    { 
          public ResponseModels AddGoodReceiptGR(string formData)
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
                    int Id = CommonDal.getPrimaryKey(tran, "OIGN");

                    param.Add(cdal.GetParameter("@Id", Id, typeof(int)));
                    param.Add(cdal.GetParameter("@Guid", CommonDal.generatedGuid(), typeof(string)));
                     
                    #region BackendCheck For Series
                    string? DocNum = SqlHelper.MySeriesUpdate_GetItemCode(MySeries, tran);
                    if (DocNum == null)
                    {
                        tran.Rollback();
                        response.isSuccess = false;
                        response.Message = "An Error Occured";
                        return response;
                    }
                    model.HeaderData.DocNum = DocNum;
                    #endregion

                    string HeadQuery = @"insert into OIGN (Id,Guid,MySeries,DocNum,Series,DocDate,GroupNum,TaxDate,Ref2) 
                                        values(@Id,@Guid,@MySeries,@DocNum,@Series,@DocDate,@GroupNum,@TaxDate,@Ref2)";



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

                            string RowQueryItem1 = @"insert into IGN1
                                (Id,LineNum,BaseRef,BaseEntry,BaseLine,ItemCode,Dscription,WhsCode,Quantity,Price,LineTotal,AcctCode,ItemCost,UomEntry,UomCode,BaseQty,OpenQty)
                          values(@Id,@LineNum,@BaseRef,@BaseEntry,@BaseLine,@ItemCode,@Dscription,@WhsCode,@Quantity,@Price,@LineTotal,@AcctCode,@ItemCost,@UomEntry,@UomCode,@BaseQty,@OpenQty)";

                            #region sqlparam
                            List<SqlParameter> param1 = new List<SqlParameter>();
                            param1.Add(cdal.GetParameter("@Id", Id, typeof(int)));
                            param1.Add(cdal.GetParameter("@LineNum", LineNum, typeof(int)));
                            param1.Add(cdal.GetParameter("@BaseRef", model.item.BaseRef == "" ? "null" : model.item.BaseRef, typeof(string)));
                            param1.Add(cdal.GetParameter("@BaseEntry", model.item.BaseEntry , typeof(int)));
                            param1.Add(cdal.GetParameter("@BaseLine", model.item.BaseLine, typeof(int)));
                            param1.Add(cdal.GetParameter("@ItemCode", model.item.ItemCode, typeof(string)));
                            param1.Add(cdal.GetParameter("@Dscription", model.item.ItemName, typeof(string)));
                            param1.Add(cdal.GetParameter("@WhsCode", model.item.WhsCode, typeof(string)));
                            param1.Add(cdal.GetParameter("@Quantity", model.item.QTY, typeof(decimal)));
                            param1.Add(cdal.GetParameter("@Price", model.item.UPrc, typeof(decimal)));
                            param1.Add(cdal.GetParameter("@LineTotal", model.item.TtlPrc, typeof(decimal)));
                            param1.Add(cdal.GetParameter("@AcctCode", model.item.AcctCode, typeof(string)));
                            param1.Add(cdal.GetParameter("@ItemCost", model.item.ItemCost, typeof(string)));
                            param1.Add(cdal.GetParameter("@UomEntry", model.item.UomEntry, typeof(int)));
                            param1.Add(cdal.GetParameter("@UomCode", model.item.UomCode, typeof(string)));
                            param1.Add(cdal.GetParameter("@BaseQty", model.item.BaseQty, typeof(string)));
                            param1.Add(cdal.GetParameter("@OpenQty", model.item.QTY, typeof(decimal)));                             

                            #endregion

                            res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, RowQueryItem1, param1.ToArray()).ToInt();
                            if (res1 <= 0)
                            {
                                tran.Rollback();
                                response.isSuccess = false;
                                response.Message = "An Error Occured";
                                return response;
                            }
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
