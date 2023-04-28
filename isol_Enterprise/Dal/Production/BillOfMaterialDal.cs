﻿using iSOL_Enterprise.Models;
using Newtonsoft.Json;
using SqlHelperExtensions;
using System.Data.SqlClient;
using System.Data;
using iSOL_Enterprise.Common;
using System.Reflection;

namespace iSOL_Enterprise.Dal.Production
{
    public class BillOfMaterialDal
    {

        public List<SalesQuotation_MasterModels> GetData()
        {
            string GetQuery = "select Id,Code,TreeType,CreateDate,Qauntity,Guid,ToWH,isPosted,is_Edited from OITT order by id DESC";


            List<SalesQuotation_MasterModels> list = new List<SalesQuotation_MasterModels>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {

                    SalesQuotation_MasterModels models = new SalesQuotation_MasterModels();                    
                    
                    models.DocStatus = "Open";
                    models.Id = rdr["Id"].ToInt();
                    models.CardCode = rdr["Code"].ToString();
                    models.CardName = GetTreeType(Convert.ToChar( rdr["TreeType"]));
                    models.DocDate = Convert.ToDateTime(rdr["CreateDate"]);
                    models.Quanity = Convert.ToDecimal(rdr["Qauntity"]);
                    models.Guid = rdr["Guid"].ToString();
                    models.Warehouse = rdr["ToWH"].ToString();
                    models.IsPosted = rdr["isPosted"].ToString(); 
                    models.IsEdited = rdr["is_Edited"].ToString();
                    list.Add(models);
                }
            }
            return list;
        }
        public string GetTreeType(char TreeType)
        {
            switch(TreeType)
            {
               case  'A':
                return "Assemble";
               case  'S'      :
                return "Sales";
                case  'P' :
                return "Production";
                case 'T'   :
                return "Template";
                default:
                return "";
            }
        }
        public ResponseModels AddUpdateBillOfMaterial(string formData)
        {
            ResponseModels response = new ResponseModels();

            try
            {


                var model = JsonConvert.DeserializeObject<dynamic>(formData);
                if (model.OldId != null)
                {
                   // response = EditBillOfMaterial(model);
                }
                else
                {
                    response = AddBillOfMaterial(model);
                }

                return response;


            }
            catch (Exception e)
            {

                response.isSuccess = false;
                response.Message = e.Message;
                return response;
            }

        }
        public ResponseModels AddBillOfMaterial(dynamic model)
        {
            ResponseModels response = new ResponseModels();
            CommonDal cdal = new CommonDal();
            SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
            conn.Open();            
            SqlTransaction tran = conn.BeginTransaction();
            int res1 = 0;
            try
            {

                if (model.HeaderData != null)
                {
                    List<SqlParameter> param = new List<SqlParameter>();
                    int Id = CommonDal.getPrimaryKey(tran, "OITT");

                    param.Add(cdal.GetParameter("@Id", Id, typeof(int)));
                    param.Add(cdal.GetParameter("@Guid", CommonDal.generatedGuid(), typeof(string)));

                    string TabHeader = "Id,Guid,Code,Qauntity,ToWH,Name,PriceList,TreeType,OcrCode,Project,PlAvgSize,CreateDate";
                    string TabHeaderP = "@Id,@Guid,@Code,@Quantity,@ToWH,@Name,@PriceList,@TreeType,@OcrCode,@Project,@PlAvgSize,@CreateDate";
                    
                    string HeadQuery = @"insert into OITT (" + TabHeader + ") " +
                                        "values("+TabHeaderP+")";



                    #region SqlParameters

                    #region Header data
                    param.Add(cdal.GetParameter("@Code", model.HeaderData.ItemmCode, typeof(string)));
                    param.Add(cdal.GetParameter("@Quantity", model.HeaderData.Quantity, typeof(decimal)));
                    param.Add(cdal.GetParameter("@ToWH", model.HeaderData.ToWH, typeof(string)));
                    param.Add(cdal.GetParameter("@Name", model.HeaderData.ItemDescription, typeof(string)));
                    param.Add(cdal.GetParameter("@PriceList", model.HeaderData.PriceList, typeof(string)));
                    param.Add(cdal.GetParameter("@TreeType", model.HeaderData.TreeType, typeof(char)));
                    param.Add(cdal.GetParameter("@OcrCode", model.HeaderData.OcrCode, typeof(string)));
                    param.Add(cdal.GetParameter("@Project", model.HeaderData.Project, typeof(string)));
                    param.Add(cdal.GetParameter("@PlAvgSize", model.HeaderData.PlAvgSize, typeof(decimal)));
                    param.Add(cdal.GetParameter("@CreateDate",DateTime.Now, typeof(DateTime)));
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
                        int ChildNum = 0;
                        foreach (var item in model.ListItems)
                        {
                            param.Clear();
                            

                            string Tabitem = "Id,Father,ChildNum,VisOrder,Type,Code,ItemName,Quantity,Uom,Warehouse,IssueMthd,PriceList,Price,LineTotal,Comment";
                            string TabitemP = "@Id,@Father,@ChildNum,@VisOrder,@Type,@Code,@ItemName,@Quantity,@Uom,@Warehouse,@IssueMthd,@PriceList,@Price,@LineTotal,@Comment";
                            string ITT1_Query = @"insert into ITT1 ("+Tabitem+") "+
                                                 "values("+TabitemP+")";

                            #region ListItems data
                            param.Add(cdal.GetParameter("@id", Id, typeof(int)));
                            param.Add(cdal.GetParameter("@Father", model.HeaderData.ItemmCode, typeof(string)));
                            param.Add(cdal.GetParameter("@ChildNum", ChildNum, typeof(int)));
                            param.Add(cdal.GetParameter("@VisOrder", ChildNum, typeof(int)));
                            param.Add(cdal.GetParameter("@Type", item.MaterialType, typeof(int)));
                            param.Add(cdal.GetParameter("@Code", item.ItemCode, typeof(string)));
                            param.Add(cdal.GetParameter("@ItemName", item.ItemName, typeof(string)));
                            param.Add(cdal.GetParameter("@Quantity", item.QTY, typeof(decimal)));
                            param.Add(cdal.GetParameter("@Uom", item.BuyUnitMsr, typeof(string)));
                            param.Add(cdal.GetParameter("@Warehouse", item.Warehouse, typeof(string)));
                            param.Add(cdal.GetParameter("@IssueMthd", item.IssueMthd, typeof(char)));
                            param.Add(cdal.GetParameter("@PriceList", item.PriceList1, typeof(int)));
                            param.Add(cdal.GetParameter("@Price", item.UPrc, typeof(decimal)));
                            param.Add(cdal.GetParameter("@LineTotal", item.TtlPrc, typeof(decimal)));
                            param.Add(cdal.GetParameter("@Comment", item.Comment, typeof(string)));                            
                            #endregion

                            res1 = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, ITT1_Query, param.ToArray()).ToInt();
                            if (res1 <= 0)
                            {
                                tran.Rollback();
                                response.isSuccess = false;
                                response.Message = "An Error Occured";
                                return response;

                            }
                            ChildNum += 1;
                        }
                    }

                    
                }
                if (res1 > 0)
                {
                    tran.Commit();
                    response.isSuccess = true;
                    response.Message = "Bill Of Material Added Successfully !";

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