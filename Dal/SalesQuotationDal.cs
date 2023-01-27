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
    public class SalesQuotationDal
    {
        public List<SalesQuotation_MasterModels> GetData()
        {
            string GetQuery = "select * from OQUT";


            List<SalesQuotation_MasterModels> list =  new List<SalesQuotation_MasterModels>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {
                    SalesQuotation_MasterModels models = new SalesQuotation_MasterModels();
                    models.DocDate = rdr["DocDueDate"].ToDateTime();
                    models.PostingDate = rdr["DocDate"].ToDateTime();
                    models.DocNum = rdr["DocNum"].ToString();
                    models.CardCode = rdr["CardCode"].ToString();
                    models.CardName = rdr["CardName"].ToString();
                    list.Add(models);
                }
            }

            return list;
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
                            PymntGroup= rdr["PymntGroup"].ToString()
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
                            SlpName= rdr["SlpName"].ToString()
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
                            Rate = (decimal) rdr["Rate"]
                        });

                }
            }

            return list;
        }

        public bool AddSalesQoutation(string formData)
        {
            try
            {
                var model = JsonConvert.DeserializeObject<dynamic>(formData);



                SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                int res1 = 0;
                try
                {

                    if (model.HeaderData != null)
                    {

                        string HeadQuery = @"insert into OQUT(Id,Guid,CardCode,DocNum,CardName,CntctCode,DocDate,NumAtCard,DocDueDate,DocCur,TaxDate , GroupNum , SlpCode , Comments) 
                                           values(@Id,@Guid,@CardCode,@DocNum,@CardName,@CntctCode,@DocDate,@NumAtCard,@DocDueDate,@DocCur,@TaxDate,@GroupNum @SlpCode ,@Comments)";

                        int Id = CommonDal.getPrimaryKey(tran, "OQUT");

                        List<SqlParameter> param = new List<SqlParameter>
                                {
                                    new SqlParameter("@id",Id),
                                    new SqlParameter("@Guid", CommonDal.generatedGuid()),
                                    new SqlParameter("@CardCode",model.HeaderData.CardCode),
                                    new SqlParameter("@DocNum",model.HeaderData.DocNum),
                                    new SqlParameter("@CardName",model.HeaderData.CardName),
                                    new SqlParameter("@CntctCode",model.HeaderData.CntctCode),
                                    new SqlParameter("@DocDate",model.HeaderData.DocDate),
                                    new SqlParameter("@NumAtCard",model.HeaderData.NumAtCard),
                                    new SqlParameter("@DocDueDate",model.HeaderData.DocDueDate),
                                    new SqlParameter("@DocCur",model.HeaderData.DocCur),
                                    new SqlParameter("@TaxDate",model.HeaderData.TaxDate),
                                    new SqlParameter("@GroupNum",model.PaymentTerms.GroupNum),
                                    new SqlParameter("@SlpCode",model.FooterData.SlpCode),
                                    new SqlParameter("@Comments",model.FooterData.Comments == null ? "" : model.FooterData.Comments),
                                };

                        res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, HeadQuery, param.ToArray()).ToInt();
                    }
                    if (model.ListItems != null)
                    {
                        string RowQueryItem = @"insert into QUT1(Id,ItemCode,Quantity,DiscPrcnt,VatGroup , UomCode ,CountryOrg)
                                            values(@Id,@ItemCode,@Quantity,@DiscPrcnt,@VatGroup, @UomCode, @CountryOrg)";

                        foreach (var item in model.ListItems)
                        {
                            int QUT1Id = CommonDal.getPrimaryKey(tran, "QUT1");
                            List<SqlParameter> param2 = new List<SqlParameter>
                                    {
                                        new SqlParameter("@id",QUT1Id),
                                        new SqlParameter("@ItemCode",item.ItemCode),
                                        new SqlParameter("@Quantity",item.Quantity),
                                        new SqlParameter("@DiscPrcnt",item.DiscPrcnt),
                                        new SqlParameter("@VatGroup",item.VatGroup),
                                        new SqlParameter("@UomCode",item.UomCode),
                                        new SqlParameter("@CountryOrg",item.CountryOrg),

                                    };
                            int res2 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, RowQueryItem, param2.ToArray()).ToInt();
                            if (res2 <= 0)
                            {
                                tran.Rollback();
                                return false;
                            }

                        }



                    }
                    else if (model.ListService != null)
                    {

                        string RowQueryService = @"insert into QUT1(Id,Dscription,AcctCode,VatGroup)
                                                  values(@Id,@Dscription,@AcctCode,@VatGroup)";
                        foreach (var item in model.ListService)
                        {
                            int QUT1Id = CommonDal.getPrimaryKey(tran, "UserRolePageActivity");

                            List<SqlParameter> param3 = new List<SqlParameter>
                                        {
                                            new SqlParameter("@id",QUT1Id),
                                            new SqlParameter("@Dscription",item.Dscription),
                                            new SqlParameter("@AcctCode",item.AcctCode),
                                            new SqlParameter("@VatGroup",item.VatGroup),


                                        };

                            int res3 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, RowQueryService, param3.ToArray()).ToInt();
                            if (res3 <= 0)
                            {
                                tran.Rollback();
                                return false;

                            }

                        }



                    }
                    if (model.ListAttachment != null)
                    {

                        string RowQueryAttachment = @"insert into ATC1(Id,trgtPath,FileName,Date)
                                                  values(@Id,@trgtPath,@FileName,@Date)";
                        foreach (var item in model.ListAttachment)
                        {
                            int ATC1Id = CommonDal.getPrimaryKey(tran, "UserRolePageActivity");

                            List<SqlParameter> param3 = new List<SqlParameter>
                                        {
                                            new SqlParameter("@id",ATC1Id),
                                            new SqlParameter("@trgtPath",item.trgtPath),
                                            new SqlParameter("@FileName",item.FileName),
                                            new SqlParameter("@Date",item.Date),


                                        };

                            int res4 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, RowQueryAttachment, param3.ToArray()).ToInt();
                            if (res4 <= 0)
                            {
                                tran.Rollback();
                                return false;

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
