using iSOL_Enterprise.Common;
using iSOL_Enterprise.Models;
using iSOL_Enterprise.Models.sale;
using iSOL_Enterprise.Models.Sale;
using Newtonsoft.Json;
using SAPbobsCOM;
using SqlHelperExtensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace iSOL_Enterprise.Dal
{
    public class CommonDal
    {
        public static string API_Url;


               public static string DiAPI_Server = "DESKTOP-AJM6HM8\\SQLSERVER19";
               public static string DiAPI_companydb = "SAPDB";
               public static SAPbobsCOM.BoDataServerTypes dst_HANADB = BoDataServerTypes.dst_MSSQL2019;
               public static string DiAPI_dbusername = "sa";
               public static string DiAPI_dbpassword = "n5210567";
               public static string DiAPI_username = "manager";
               public static string DiAPI_password = "Bilal@123";
               public static SAPbobsCOM.BoSuppLangs DiAPI_langauge = BoSuppLangs.ln_English_Gb;
               public static bool DiAPI_UserTrusted = false;
        //public static string DiAPI_SldServer;
        //public static string ln_English;
        public decimal GetSelectedWareHouseData(string ItemCode, string WhsCode)
        {
            decimal res = Convert.ToDecimal(SqlHelper.ExecuteScalar(SqlHelper.defaultSapDB, CommandType.Text, "select onHand from OITW where ItemCode = '"+ItemCode+"' and WhsCode = '" + WhsCode + "'"));
            return res;
        }


        public List<tbl_customer> GetBusinessPartners(string DocModule)
        {
            string GetQuery = "";
            if (DocModule == "S")
            {
               GetQuery = "select CardCode,CardName,Currency,Balance from OCRD Where CardType = 'C'";
            }else if (DocModule == "I")
            {
              GetQuery = "select CardCode,CardName,Currency,Balance from OCRD";
            }


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

        public SqlParameter GetParameter(string name, dynamic? value, Type type)
        {
            SqlParameter param = new SqlParameter();
            if (type == typeof(int))
            {
                int? value1 = value.ToString() == "" ? null : (int)value;
                
                param = new SqlParameter(name, value1);
            }
            else if (type == typeof(string))
            {
                string? value1 = value == "" ? null : Convert.ToString(value);
                param = new SqlParameter(name, value1);
            }
            else if (type == typeof(DateTime))
            {
                DateTime? value1 = value == "" ? null : Convert.ToDateTime(value);
                param = new SqlParameter(name, value1);
            }
            else if (type == typeof(char))
            {
                char value1 = value == "" ? null : Convert.ToChar(value);
                param = new SqlParameter(name, value1);
            }
            else if (type == typeof(decimal))
            {
                decimal value1 = value == "" ? null : Convert.ToDecimal(value);
                param = new SqlParameter(name, value1);
            }
            else if (type == typeof(Int16))
            {
                int value1 = Convert.ToInt16(value);
                param = new SqlParameter(name, value1);
            }
            // param = new SqlParameter(name,value);

            return param;
        }

        public static int Count(string tblName)
        {
            int count = SqlHelper.ExecuteScalar(SqlHelper.defaultDB, CommandType.Text, "select count(*) from Line_One_ProductionData where windowsService=1 and cast(timestamp as date) = cast(getdate() as date)").ToInt();
            return count;
        }

        public static bool Count(string tblName,string FieldName,string input)
        {
            int count = SqlHelper.ExecuteScalar(SqlHelper.defaultDB, CommandType.Text, "select count(*) from " + tblName + " where "+ FieldName + "= '"+ input + "'").ToInt();

            return count > 0 ? true : false ;
        }

        public static bool CountOnEdit(string tblName, string FieldName, string input, string Guid)
        {
            int count = SqlHelper.ExecuteScalar(SqlHelper.defaultDB, CommandType.Text, "select count(*) from " + tblName + " where Guid <>'"+Guid+"' and " + FieldName + "= '" + input + "'").ToInt();

            return count > 0 ? true : false;
        }

        public List<UserRolePageActivityModels> GetUserPageActivity(string RoleCode,string Guid)
        {
            string Query = @"select ur.RoleActivityCode From  UserRolePageActivity ur
inner join Pages p on p.PageId=ur.PageId
where ur.RoleCode=@RoleCode and p.Guid=@Guid ";
            List<UserRolePageActivityModels> models = new List<UserRolePageActivityModels>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, Query, new SqlParameter("RoleCode", RoleCode),new SqlParameter("@Guid",Guid)))
            {
                while (rdr.Read())
                {
                    UserRolePageActivityModels detail = new UserRolePageActivityModels();
                    detail.RoleActivityCode = rdr["RoleActivityCode"].ToString();
     //               detail.Guid = rdr["Guid"].ToString();
                    models.Add(detail);
                }
            }
            return models;
        }

        public DataTable GetAdvanceSearch(string Guid)
        {
            string Query = @"select a.SearchColumn,a.FieldText From AdvanceSearch a
inner join Pages p on p.PageId=a.PageId
where a.IsActive=1 and p.IsActive=1 and p.RowStatus=1 and p.Guid=@Guid";

            DataTable dt = SqlHelper.GetData(Query, new SqlParameter("Guid", Guid));
            return dt;
        }


        public static string _PageId;
        public static int? _UserId;
        public static bool CheckPage(string pageId, int? UserId)
        {
            string query = @"select count(*) From Users u 
inner join UserRoles ur on ur.RoleCode=u.RoleCode
inner join Pages p on p.PageId=ur.PageId
where p.Guid=@Guid and u.Id=@UserId";
            _PageId = pageId;
            _UserId = UserId;
            //int count = SqlHelper.ExecuteScalar(SqlHelper.defaultDB, CommandType.Text, query, new SqlParameter("@Guid", pageId), new SqlParameter("@UserId", UserId)).ToInt();
            //if (count > 0)
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
            DataTable dt = SqlHelper.GetData(@"select count(*) From Users u 
inner join UserRoles ur on ur.RoleCode=u.RoleCode
inner join Pages p on p.PageId=ur.PageId
where p.Guid=@Guid and u.Id=@UserId", new SqlParameter("@Guid", pageId), new SqlParameter("@UserId", UserId));
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public List<Setup_PageListViewQueryDetailModels> GetColumnNames(RequestModels request)
        {
            List<Setup_PageListViewQueryDetailModels> model = new List<Setup_PageListViewQueryDetailModels>();
            StringBuilder sb = new StringBuilder();
            request.Offset = 0;
            request.PageSize = 10;

            List<SqlParameter> param = new List<SqlParameter>
            {
                new SqlParameter("@Offset",request.Offset),
                new SqlParameter("@PageSize",request.PageSize),
                new SqlParameter("@Guid",request.Guid)
            };

            string HdrQry = @"select d.columnName,d.DataType,d.Status From Setup_PageListViewQueryMaster s
inner join Setup_PageListViewQueryDetail d on d.MasterId=s.Id
inner join Pages p on p.PageId=s.PageId
where s.Status=1 and d.status=1 and p.Guid=@Guid";

            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, HdrQry, param.ToArray()))
            {
                while (rdr.Read())
                {
                    Setup_PageListViewQueryDetailModels detail = new Setup_PageListViewQueryDetailModels();
                    detail.ColumnName = rdr["ColumnName"].ToString();
                    detail.DataType = rdr["DataType"].ToString();
                    model.Add(detail);
                }
            }
            return model;
        }
        public ResponseModels GetLookupData(RequestModels request,string Role,int? UserId, List<AdvanceSearchModels> AdvSrch)
        {
            ResponseModels response = new ResponseModels();
            string queryAppend = "";
            DataTable dt = null;
            StringBuilder sb = new StringBuilder();
            StringBuilder xb = new StringBuilder();
            StringBuilder adv = new StringBuilder();
            List<SqlParameter> param = new List<SqlParameter>
            {
                new SqlParameter("@Offset",request.Offset),
                new SqlParameter("@PageSize",request.PageSize),
                new SqlParameter("@Guid",request.Guid)
            };

            string HdrQry = @"select s.Id,s.PageId,s.Query,s.SearchColumns,StaffClause From Setup_PageListViewQueryMaster s
inner join Pages p on p.PageId=s.PageId
where s.Status=1 and p.Guid=@Guid";

            DataTable dtL_Query = SqlHelper.GetData(HdrQry, param.ToArray());


           
           
            string DtQuery = dtL_Query.Rows[0]["Query"].ToString();
            string SearchColumns = dtL_Query.Rows[0]["SearchColumns"].ToString();
            string StaffClause = dtL_Query.Rows[0]["StaffClause"].ToString();

            string[] col = SearchColumns.Split(',');
            string[] ret;
            if (DtQuery.Contains("where") && Role == "STF")
            {
                if (!string.IsNullOrEmpty(StaffClause))
                {
                    param.Add(new SqlParameter("@StaffClause", UserId));
                    xb.Append(" and " + StaffClause + "=@StaffClause");
  //                  queryAppend = String.Format(DtQuery, xb.ToString(), sb.ToString());
                }
        

            }

            if (AdvSrch != null)
            {
                if (DtQuery.Contains("where"))
                {
                    for (int i = 0; i < AdvSrch.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(AdvSrch[i].Value))
                        {
                            param.Add(new SqlParameter("@" +i+"adv",AdvSrch[i].Value));
                            adv.Append(" and " + AdvSrch[i].SearchColumn + "= @" + i + "adv");
                        }
                      
                    }
                }

            }

            if (request.search != "" && !string.IsNullOrEmpty(SearchColumns))
            {
                if (col.Length >= 0)
                {
                    if (DtQuery.Contains("where"))
                    {

                    }
                    else
                    {
                        sb.Append(" where ");
                    }

                }
                for (int i = 0; i < col.Length; i++)
                {
                    if (i == 0 && DtQuery.Contains("where"))
                    {
                        sb.Append(" and ");
                    }
                    if (i > 0)
                    {
                        sb.Append(" OR ");
                    }
                    // ret = col[i].Split('=');
                    ret = col[i].Split(',');

                    param.Add(new SqlParameter("@" + i, "%" + request.search + "%"));
                    sb.Append(col[i] + " Like " + "@" + i);

                }
             


                queryAppend = String.Format(DtQuery, xb.ToString(), sb.ToString(),adv.ToString());


                dt = SqlHelper.GetData(queryAppend, param.ToArray());

            }
            else
            if (!string.IsNullOrEmpty(SearchColumns))
            {
                queryAppend = String.Format(DtQuery, sb.ToString(),xb.ToString(), adv.ToString());
                dt = SqlHelper.GetData(queryAppend, param.ToArray());
            }


            string[] split = queryAppend.Split(new string[] { "OFFSET" }, StringSplitOptions.None);
            DataTable dt1 = SqlHelper.GetData(split[0], param.ToArray());

            response.Data = dt;
            response.recordsFiltered = dt1.Rows.Count;
            response.recordsTotal = dt.Rows.Count;
            return response;
        }

        public static int getPrimaryKey(SqlTransaction tran, string tblName)
        {
            string query = "select max(id) from " + tblName + " (TABLOCKX)";
            int id = SqlHelper.ExecuteScalar(tran, CommandType.Text, query).ToInt();
            id = id + 1;
            return id;
        }
        public static int getPrimaryKeyNoLock(SqlTransaction tran, string tblid, string tblName)
        {
            string query = "select max(" + tblid + ") from " + tblName;
            int id = SqlHelper.ExecuteScalar(tran, CommandType.Text, query).ToInt();
            id = id + 1;
            return id;
        }
         
            public static bool Check_IsNotEditable(string table, int id)
            {


            string query = "select OpenQty from " + table + " where Id = " + id + "";
            DataSet ds = new DataSet();
            SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
            SqlDataAdapter sda = new SqlDataAdapter(query, conn);
            sda.Fill(ds);
            foreach (DataRow item in ds.Tables[0].Rows)
            {
                if (Convert.ToDecimal(item["OpenQty"]) > 0)
                {
                    return false; //Status is Open
                }
            }
            return true; //Status is Closed  

              
                //int count = SqlHelper.ExecuteScalar(SqlHelper.defaultDB, CommandType.Text, query).ToInt();
                //if (count > 0) { return true; } //Closed
                //return false; //Open

            }

         


        public static string getDocType(SqlTransaction tran, string tblName , string id)
        {
            string query   = "select DocType from " + tblName + " where Id =" +id;
            //string query   = "select DocType from ORDR where Id = 1 " ;
            string data = SqlHelper.ExecuteScalar(tran, CommandType.Text, query).ToString(); 
            return data;
        }
        public static int getLineNumber(SqlTransaction tran, string tblName , string id)
        {
            string query = "select max(LineNum) from " + tblName + " where Id =" +id;
            int lineNum = SqlHelper.ExecuteScalar(tran, CommandType.Text, query).ToInt();
            lineNum = lineNum + 1;
            return lineNum;
        }
        public static int getPrimaryKey(SqlTransaction tran,string tblid, string tblName)
        {
            string query = "select max("+ tblid + ") from " + tblName + " (TABLOCKX)";
            int id = SqlHelper.ExecuteScalar(tran, CommandType.Text, query).ToInt();
            id = id + 1;
            return id;
        }
        public static int getSysNumber(SqlTransaction tran,string itemcode)
        {
            string query = "select max(SysNumber) from  OBTN where ItemCode ='"+itemcode + "'";
            int sysnumber = SqlHelper.ExecuteScalar(tran, CommandType.Text, query).ToInt();
            sysnumber = sysnumber + 1;
            return sysnumber;
        }
        public static int GetTicketCode(SqlTransaction tran)
        {
            string query = "select max(ticketCode) from Tickets_Master (TABLOCKX)";
            int id = SqlHelper.ExecuteScalar(tran, CommandType.Text, query).ToInt();
            id = id + 1;
            return id;
        }
        public static string GetItemCode(int ItemID)
        {
            string query = "select ItemCode from OITM Where id ="+ItemID;
            string? ItemCode = SqlHelper.ExecuteScalar(SqlHelper.defaultDB, CommandType.Text, query).ToString();
           
            return ItemCode;
        }
        public static string generatedGuid()
        {
            string guid = Guid.NewGuid().ToString().Replace("-", Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 5)) + Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 5) + DateTime.Now.ToString("ssmmddhhyyyymm");
            guid = guid.Replace("+", "");
            return guid;
        }

        public static string SaveFileFromBase64(string base64, string basepath, string Name)
        {
            string ext = getExtensionFromFile(base64);
            base64 = base64.Replace('-', '+');
            base64 = base64.Replace('_', '/');
            if (ext == "rpt")
            {
                base64 = base64.Replace("data:application/octet+stream;base64,", String.Empty);
            }
            else
                if (ext == "pdf")
            {
                base64 = base64.Replace("data:application/pdf;base64,", String.Empty);
            }
            else
                if (ext == "xlsx")
            {
       
                base64 = base64.Replace("data:application/vnd.openxmlformats+officedocument.spreadsheetml.sheet;base64,", String.Empty);
            }
            else
                if (ext == "xls")
            {
       
                base64 = base64.Replace("data:application/vnd.ms+excel;base64,", String.Empty);
            }
            else
            {
                base64 = base64.Replace("data:application/" + ext + ";base64,", String.Empty);
                base64 = base64.Replace("data:image/" + ext + ";base64,", String.Empty);
            }
            base64 = base64.Replace('_', '/');
            byte[] bytes = Convert.FromBase64String(base64);
            string path = Path.Combine(basepath, "Attachments\\" + Name);
            System.IO.FileStream stream = new FileStream(path, FileMode.CreateNew);
            System.IO.BinaryWriter writer = new BinaryWriter(stream);
            writer.Write(bytes, 0, bytes.Length);
            writer.Close();
            return "Attachments\\" + Name;
        }

        public static string getExtensionFromFile(string base64)
        {
            string extension = null;

            if (base64.Contains("image/png"))
            {
                extension = "png";
            }
            if (base64.Contains("image/jpg"))
            {
                extension = "jpg";
            }
            if (base64.Contains("image/jpeg"))
            {
                extension = "jpeg";
            }
            if (base64.Contains("application/pdf"))
            {
                extension = "pdf";
            }
            if (base64.Contains("application/octet+stream"))
            {
                extension = "rpt";
            }
             if (base64.Contains("data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"))
            {
                extension = "xlsx";
            }
            if (base64.Contains("data:application/vnd.ms-excel;base64,"))
            {
                extension = "xls";
            }


            return extension;
        }

        public static bool Delete(string tblName, KeyValuePair<string, string> WhereClause)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@"+ WhereClause.Key, WhereClause.Value)
            };
            int res = SqlHelper.ExecuteNonQuery("Update " + tblName + " set Rowstatus=0 where " + WhereClause.Key + "=@" + WhereClause.Key, param);
            return res > 0 ? true : false;
        }

        public string GetMasterTable(int Basetype)
        {
            string table = "";
            switch (Basetype)
            {
                case 13:
                table = "OINV"; //A/R Invoice
                break;
                case 15:
                table = "ODLN"; //Delivery
                break;
                case 16:
                table = "ORDN"; //Return
                break;
                case 17:
                table = "ORDR"; //Sales Order
                break;
                case 18:
                table = "OPCH"; //A/P Invoice
                break;
                case 19:
                table = "ORPC"; //A/P Credit Memo
                break;
                case 20:
                table = "OPDN"; //Goods Receipt PO
                break;
                case 21:
                table = "ORPD"; //Goods Return
                break;
                case 22:
                table = "OPOR"; //Purchase Order
                break;
                case 23:
                table = "OQUT"; //Sales Quotation
                break;
                case 540000006:
                table = "OPQT"; //Purchase Quotation
                break;
                case 14: //AR Credit Memo
                table = "ORIN";
                break;
                case 59: //Goods Receipt GR Item Transaction
                table = "OIGN";
                break;
                case 60: //Goods Issue Item Transaction
                table = "OIGE";
                break;
                case 67: //Inventory Transfer
                table = "OWTR";
                break;
                default:
                return table;
            }
            return table;
        }

        public string GetRowTable(int Basetype)
        {
            string table = "";
            switch (Basetype)
            {
                case 13:
                table = "INV1"; //A/R Invoice
                break;
                case 15:
                table = "DLN1"; //Delivery
                break;
                case 16:
                table = "RDN1"; //Return
                break;
                case 17:
                table = "RDR1"; //Sales Order
                break;
                case 18:
                table = "PCH1"; //A/P Invoice
                break;
                case 19:
                table = "RPC1"; //A/P Credit Memo
                break;
                case 20:
                table = "PDN1"; //Goods Receipt PO
                break;
                case 21:
                table = "RPD1"; //Goods Return
                break;
                case 22:
                table = "POR1"; //Purchase Order
                break;
                case 23:
                table = "QUT1"; //Sales Quotation
                break;
                case 540000006:
                table = "PQT1"; //Purchase Quotation
                break;
                case 14:
                table = "RIN1";
                break;
                case 59: //Goods Receipt GR Item Transaction
                table = "IGN1";
                break;
                case 60: //Goods Issue Item Transaction
                table = "IGE1";
                break;
                case 67: //Inventory Transfer
                table = "WTR1";
                break;
                default:
                return table;
            }
            return table;
        }
        public SAPbobsCOM.Documents getDocObj(int ObjectCode, Company oCompany)
        {
            SAPbobsCOM.Documents? oDoc = null;
            switch (ObjectCode)
            {
                
                case 23: //Sales Quotation
                oDoc = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations);
                break;
                case 17: //Sales Order
                oDoc = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);
                break;
                case 15: //Delivery
                oDoc = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDeliveryNotes);
                break;
                case 16: //Return
                oDoc = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oReturns); 
                break;
                case 13: //A/R Invoice
                oDoc = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);
                break;
                case 14: //AR Credit Memo
                oDoc = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oCreditNotes);
                break;
                case 540000006: //Purchase Quotation
                oDoc = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseQuotations);
                break;
                case 22: //Purchase Order
                oDoc = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseOrders);
                break;
                case 20: //Goods Receipt PO
                oDoc = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseDeliveryNotes);
                break;
                case 21: //Goods Return
                oDoc = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseReturns);
                break;
                case 18: //A/P Invoice
                oDoc = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseInvoices); 
                break;
                case 19: //A/P Credit Memo
                oDoc = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseCreditNotes); 
                break;
                
                
                
               
                
            }
            return oDoc;
        }

        public List<SalesQuotation_MasterModels> GetBaseDocData(string cardcode, int BaseType)
        {
            string table = GetMasterTable(BaseType);
            string rowTable = GetRowTable(BaseType);
            string GetQuery = "select * from " + table + " where CardCode ='" + cardcode + "' order by Id desc"; /*isPosted = 1*/
            
            List<SalesQuotation_MasterModels> list = new List<SalesQuotation_MasterModels>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {
                    SalesQuotation_MasterModels models = new SalesQuotation_MasterModels();
                    if (!(CommonDal.Check_IsNotEditable(rowTable, rdr["Id"].ToInt())))
                    {                    
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
            }
            return list;
        }

        public List<SalesQuotation_MasterModels> GetBaseDocType(string DocId, int BaseType)
        {
            string table = GetMasterTable(BaseType);
            string GetQuery = "select DocType,DocNum from "+table+" where Id = '" + DocId +"'";
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
        public dynamic GetBaseDocItemServiceList(string DocId , int BaseType)
        {
            try
            {

            
            string table = GetRowTable(BaseType);
            DataSet ds = new DataSet();
            SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
            SqlDataAdapter sda = new SqlDataAdapter("select Id,WhsCode,LineNum,ItemCode,ItemName,Quantity,DiscPrcnt,Price,VatGroup,UomCode,CountryOrg,Dscription,AcctCode,OpenQty,LineTotal from " + table+" where id = '" + DocId + "' and OpenQty <> 0", conn);
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
        public dynamic GetBaseDocItemServiceList_Return(string DocId , int BaseType)
        {
            try
            {

            
            string table = GetRowTable(BaseType);
            DataSet ds = new DataSet();
            SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
            SqlDataAdapter sda = new SqlDataAdapter("select Id,WhsCode,LineNum,ItemCode,ItemName,Quantity,DiscPrcnt,Price,VatGroup,UomCode,CountryOrg,Dscription,AcctCode,OpenQty,LineTotal from " + table+" where id = '" + DocId+"'", conn);
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

        public List<tbl_country> GetCountries()
        {
            string GetQuery = "select * from tbl_country ";


            List<tbl_country> list = new List<tbl_country>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {

                    list.Add(
                        new tbl_country()
                        {
                            country_code = rdr["country_code"].ToString(),
                            country_name = rdr["country_name"].ToString()
                        });

                }
            }

            return list;
        }

        public List<tbl_currency> GetCurrencydata()
        {
            string GetQuery = "select CurrCode, CurrCode +' - '  +CurrName  as CurrName from OCRN order by CurrCode";


            List<tbl_currency> list = new List<tbl_currency>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {

                    list.Add(
                        new tbl_currency()
                        {
                            curr_code = rdr["CurrCode"].ToString(),
                            curr_name = rdr["CurrName"].ToString()
                        });

                }
            }

            return list;
        }

        public  ResponseModels GetCustomerData(string cardcode,string DocModule)
        {
            string GetQuery = "";
            if (DocModule == "S")
            {
                GetQuery = "select CardCode,CardName,Currency from OCRD Where CardType = 'C' and CardCode ='" + cardcode + "'";
            }
            else if (DocModule == "P")
            {
                GetQuery = "select CardCode,CardName,Currency from OCRD Where CardType = 'S' and CardCode ='" + cardcode + "'";
            }

           
            ResponseModels model = new ResponseModels();

            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {
                    tbl_customer customer = new tbl_customer();

                    customer.CardCode = rdr["CardCode"].ToString();
                    customer.CardName = rdr["CardName"].ToString();
                    customer.Currency = rdr["Currency"].ToString();

                    model.Data = customer;
                    model.isSuccess = true;
                    return model;
                }
                model.Data = "";
                model.isSuccess = false;
                return model;
            }
            

        }
        public ResponseModels GetItemData(string itemcode, string DocModule)
        {
            string GetQuery = "";
            
            if (DocModule == "S")
            {
                GetQuery = "select ItemCode,ItemName,OnHand,ManBtchNum from OITM where SellItem = 'Y'  and ItemCode='" + itemcode+"'";
            }
            else if (DocModule == "P")
            {
                GetQuery = "select ItemCode,ItemName,OnHand,ManBtchNum from OITM where PrchseItem = 'Y' and ItemCode='" + itemcode+"'";
            }
            else if (DocModule == "I")
            {
                GetQuery = "select ItemCode,ItemName,OnHand,ManBtchNum from OITM where InvntItem = 'Y' and ItemCode='" + itemcode+"'";
            }

          
            ResponseModels model = new ResponseModels();

            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {
                    tbl_item item = new tbl_item();

                    item.ItemCode = rdr["ItemCode"].ToString();
                    item.ItemName = rdr["ItemName"].ToString();
                    item.ManBtchNum = rdr["ManBtchNum"].ToString();

                    model.Data = item;
                    model.isSuccess = true;
                    return model;
                }
                model.Data = "";
                model.isSuccess = false;
                return model;
            }


        }
        public List<SalesQuotation_MasterModels> GetSaleOrders()
        {
            string GetQuery = "select DocEntry,DocNum from ORDR";


            List<SalesQuotation_MasterModels> list = new List<SalesQuotation_MasterModels>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {

                    list.Add(
                        new SalesQuotation_MasterModels()
                        {
                            Id = rdr["DocEntry"].ToInt(),
                            DocNum = rdr["DocNum"].ToString()
                        });

                }
            }

            return list;
        }
        public static List<tbl_OWHS> GetWareHouseList(string ItemCode)
        {

            string GetQuery = "select WhsCode , WhsName ,MinStock,MaxStock,MinOrder,Locked from OITW  where ItemCode ='" +ItemCode + "' order by WhsCode";


            List<tbl_OWHS> list = new List<tbl_OWHS>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {

                    list.Add(
                        new tbl_OWHS()
                        {
                            whscode = rdr["WhsCode"].ToString(),
                            whsname = rdr["WhsName"].ToString(),
                            MinStock = rdr["MinStock"].ToString() == "" ? 0 : rdr["MinStock"].ToDouble(),
                            MaxStock = rdr["MaxStock"].ToString() == "" ? 0 : rdr["MaxStock"].ToDouble(),
                            MinOrder = rdr["MinOrder"].ToString() == "" ? 0 : rdr["MinOrder"].ToDouble(),
                            Locked =   rdr["Locked"].ToString() == "" ? 'N' : Convert.ToChar( rdr["Locked"]),
                            isEditable = IsWareHouseTransactionAdded(rdr["WhsCode"].ToString(), ItemCode) 
                        });

                }
            }

            return list;

        }
        public static bool IsWareHouseTransactionAdded(string? WhsCode , string ItemCode)
        {
            if (WhsCode != null)
            {

            
                    string GetCount = @"select COUNT(*) from OBTQ Inner join OBTN on OBTN.AbsEntry = OBTQ.MdAbsEntry inner join ITL1 on ITL1.SysNumber = OBTN.SysNumber where OBTQ.WhsCode = '"+WhsCode+ "' and OBTQ.ItemCode = '"+ ItemCode + "'";
                    int count = SqlHelper.ExecuteScalar(SqlHelper.defaultDB, CommandType.Text, GetCount).ToInt();
                    if (count > 0)
                    {
                        return false;
                    }
            }

            return true;
        }


        //public static Array GetTables()
        //{

        //    "OINV",                 
        //    "ODLN",                
        //    "ORDN",                            
        //    "ORDR",           
        //    "OPCH",              
        //    "ORPC",            
        //    "OPDN",            
        //    "ORPD",            
        //    "OPOR",                
        //    "OQUT",           
        //    "OPQT"

        //               "ORIN"
        //    string[] tablesArray = { };
        //}
    }
}
