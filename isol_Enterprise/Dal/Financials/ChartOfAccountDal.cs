using iSOL_Enterprise.Common;
using iSOL_Enterprise.Models;
using iSOL_Enterprise.Models.ChartOfAccount;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using SqlHelperExtensions;
using System.Data;
using System.Data.SqlClient;
namespace iSOL_Enterprise.Dal.Financials
{
    public class ChartOfAccountDal
    {
        public List<ChartOfAccountMasterModel> GetChartOfAccounts()
        {
            string GetQuery = "select DocEntry,ActId,FatherNum,AcctCode,AcctName,AccntntCod,ActCurr,Levels,isApproved,is_Edited,isApproved,apprSeen from OACT order by DocEntry DESC";


            List<ChartOfAccountMasterModel> list = new List<ChartOfAccountMasterModel>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {

                    ChartOfAccountMasterModel models = new ()
                    {

                        DocEntry = rdr["DocEntry"].ToInt(),
                        FatherNum = rdr["FatherNum"].ToString(),
                        AcctCode = rdr["AcctCode"].ToString(),
                        AcctName = rdr["AcctName"].ToString(),
                        AccntntCod = rdr["AccntntCod"].ToString(),
                        ActCurr = rdr["ActCurr"].ToString(),
                        Levels = rdr["Levels"].ToInt(),
                        isApproved = rdr["isApproved"].ToBool(),
                        apprSeen = rdr["apprSeen"].ToBool(),
                        IsEdited = rdr["is_Edited"].ToString()
                    };
                    
                    list.Add(models);
                }
            }
            return list;
        }
        public List<ListModel> GetDrawers()
        {


            string GetQuery = "select AcctCode,AcctName  from OACT where FatherNum is null";

            List<ListModel> list = new List<ListModel>();

            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {
                    list.Add(new ListModel()
                    {
                        ValueString = rdr["AcctCode"].ToString(),
                        Text = rdr["AcctName"].ToString()

                    });
                }
            }
            return list;
        }

        public List<TreeModel> GetLevels(string drawer = null)
        {

            CommonDal cdal = new();

            List<TreeModel> Pages = new List<TreeModel>();
            string query = @"select AcctCode,AcctName,Levels,Postable from OACT where FatherNum =" + drawer;

            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text,query))
            {
                while (rdr.Read())
                {
                    TreeModel model = new TreeModel();
                    string AcctCode = rdr["AcctCode"].ToString();
                    int Levels = rdr["Levels"].ToInt();
                    string Postable = rdr["Postable"].ToString();

                    model.id = AcctCode;
                    model.text = "<span class='text-success act' act-level= "+Levels+" act-postable= "+ Postable + " >" + AcctCode + " - " + rdr["AcctName"].ToString() + "</span>";
                    model.@checked = false;
                    model.population -= null;
                    model.flagUrl = null;                    
                    model.children = GetLevelChilds(AcctCode);
                    Pages.Add(model);
                }
            }
            return Pages;
        }

        private List<TreeModel> GetLevelChilds(string AcctCode)
        {
            CommonDal cdal = new();

            
            List<TreeModel> Pages = new List<TreeModel>();
            string query = @"select AcctCode,AcctName,Levels,Postable from OACT where FatherNum ='" + AcctCode + "'";
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, query))
            {
                while (rdr.Read())
                {
                    TreeModel model = new TreeModel();
                    string AcctCode1 = rdr["AcctCode"].ToString();
                    int Levels = rdr["Levels"].ToInt();
                    string Postable = rdr["Postable"].ToString();

                    model.id = AcctCode1;
                    model.text = "<span class='act' act-level= " + Levels+" act-postable= "+ Postable + ">" + AcctCode1 + " - " + rdr["AcctName"].ToString() + "</span>";
                    model.@checked = false;
                    model.population -= null;
                    model.flagUrl = null;
                    model.children = GetLevelChilds(AcctCode1);
                    if (model.children.Count > 0)
                        model.text = "<span class='text-success act' act-level= " + Levels+" act-postable= "+ Postable + ">" + AcctCode1 + " - " + rdr["AcctName"].ToString() + "</span>";
                    
                    Pages.Add(model);
                }
            }
            return Pages;
        }

        public string GetUpdatedAcctCode(string FatherNum)
        {

            string query = @"select Top(1) AcctCode,ROW_NUMBER() OVER (ORDER BY AcctCode) AS row_number from OACT where FatherNum = '"+ FatherNum + "' order by row_number desc";
            string AcctCode = "";
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, query))
            {
                while (rdr.Read())
                {
                    AcctCode = rdr["AcctCode"].ToString();

                }
            }
            if (AcctCode == "" || string.IsNullOrWhiteSpace(AcctCode))
            {
                AcctCode = FatherNum + "01";
            }
            else
            {
                int number = int.Parse( AcctCode.Substring(AcctCode.Length - 2)) + 1;
                AcctCode = FatherNum + "0" + number;

            }

                    return AcctCode;
        }
        public ResponseModels Add(string formData)
        {
            ResponseModels response = new ResponseModels();
            CommonDal cdal = new CommonDal();
            SqlConnection conn = new SqlConnection(SqlHelper.defaultDB);
            conn.Open();
            SqlTransaction tran = conn.BeginTransaction();
            int res1 = 0;
            var model = JsonConvert.DeserializeObject<dynamic>(formData);

            try
            {
                int count = SqlHelper.ExecuteScalar(tran, CommandType.Text, "select count(*) from OACT where AcctCode='" + (model.HeaderData.AcctCode).ToString() + "'");
                if (count == 0)
                {


                    if (model.HeaderData != null)
                    {
                        List<SqlParameter> param = new List<SqlParameter>();
                        string AcctCode = GetUpdatedAcctCode(model.HeaderData.FatherNum.ToString());
                        string Guid = CommonDal.generatedGuid();
                        int DocEntry = CommonDal.getPrimaryKey(tran, "DocEntry" , "OACT");
                        int Levels = Convert.ToInt32( model.HeaderData.Levels ) + 1;
                        int ObjectCode = 1;
                        int isApproved = ObjectCode.GetApprovalStatus(tran);
                        #region Insert in Approval Table

                        if (isApproved == 0)
                        {
                            ApprovalModel approvalModel = new()
                            {
                                Id = CommonDal.getPrimaryKey(tran, "tbl_DocumentsApprovals"),
                                ObjectCode = ObjectCode,
                                DocEntry = DocEntry,
                                DocNum = (model.HeaderData.AcctCode).ToString(),
                                Guid = Guid

                            };
                            bool resp = cdal.AddApproval(tran, approvalModel);
                            if (!resp)
                            {
                                response.isSuccess = false;
                                response.Message = "An Error Occured";
                                return response;
                            }
                        }

                        #endregion

                        string TabHeader = "DocEntry,ActId,FatherNum,AcctCode,AcctName,AccntntCod,ActCurr,Levels,isApproved";
                        string TabHeaderP = "@DocEntry,@ActId,@FatherNum,@AcctCode,@AcctName,@AccntntCod,@ActCurr,@Levels,@isApproved";

                        if (model.HeaderData.postable == "Y")
                        {
                            TabHeader = TabHeader + ",Protected,ActType,LocManTran,Finanse,RevalMatch,BlocManPos,CfwRlvnt,PrjRelvnt,Project";
                            TabHeaderP = TabHeaderP + ",@Protected,@ActType,@LocManTran,@Finanse,@RevalMatch,@BlocManPos,@CfwRlvnt,@PrjRelvnt,@Project";
                        }
                        if (model.AccountDetail != null)
                        {
                            if (model.AccountDetail.validFor == "Y")
                            {
                                TabHeader = TabHeader + ",ValidFrom,ValidTo,ValidComm";
                                TabHeaderP = TabHeaderP + ",@ValidFrom,@ValidTo,@ValidComm";
                            }
                            else if (model.AccountDetail.validFor == "N")
                            {
                                TabHeader = TabHeader + ",FrozenFrom,FrozenTo,FrozenComm";
                                TabHeaderP = TabHeaderP + ",@FrozenFrom,@FrozenTo,@FrozenComm";
                            }
                            TabHeader = TabHeader + ",VatChange";
                            TabHeaderP = TabHeaderP + ",@VatChange";
                        }

                        string HeadQuery = @"insert into OACT (" + TabHeader + ") " +
                                            "values(" + TabHeaderP + ")";



                        #region SqlParameters

                        #region Header data
                        param.Add(cdal.GetParameter("@DocEntry", DocEntry, typeof(int)));
                        param.Add(cdal.GetParameter("@ActId", DocEntry, typeof(int)));
                        param.Add(cdal.GetParameter("@FatherNum", model.HeaderData.FatherNum, typeof(string)));
                        param.Add(cdal.GetParameter("@AcctCode", AcctCode, typeof(string)));
                        param.Add(cdal.GetParameter("@AcctName", model.HeaderData.AcctName, typeof(string)));
                        param.Add(cdal.GetParameter("@AccntntCod", model.HeaderData.AccntntCod, typeof(string)));
                        param.Add(cdal.GetParameter("@ActCurr", model.HeaderData.ActCurr, typeof(string)));
                        param.Add(cdal.GetParameter("@Levels", Levels, typeof(int)));
                        param.Add(cdal.GetParameter("@Protected", model.FooterData.Protected, typeof(char)));
                        param.Add(cdal.GetParameter("@ActType", model.FooterData.ActType, typeof(char)));
                        param.Add(cdal.GetParameter("@LocManTran", model.FooterData.LocManTran, typeof(char)));
                        param.Add(cdal.GetParameter("@Finanse", model.FooterData.Finanse, typeof(char)));
                        param.Add(cdal.GetParameter("@RevalMatch", model.FooterData.RevalMatch, typeof(char)));
                        param.Add(cdal.GetParameter("@BlocManPos", model.FooterData.BlocManPos, typeof(char)));
                        param.Add(cdal.GetParameter("@CfwRlvnt", model.FooterData.CfwRlvnt, typeof(char)));
                        param.Add(cdal.GetParameter("@PrjRelvnt", model.FooterData.PrjRelvnt, typeof(char)));
                        param.Add(cdal.GetParameter("@Project", model.FooterData.Project, typeof(string)));
                        
                        param.Add(cdal.GetParameter("@isApproved", isApproved, typeof(int)));

                        if (model.AccountDetail != null)
                        {
                            if (model.AccountDetail.validFor == "Y")
                            {
                                param.Add(cdal.GetParameter("@ValidFrom", model.AccountDetail.validFrom, typeof(DateTime)));
                                param.Add(cdal.GetParameter("@ValidTo", model.AccountDetail.validTo, typeof(DateTime)));
                                param.Add(cdal.GetParameter("@ValidComm", model.AccountDetail.validComm, typeof(string)));

                            }
                            else if (model.AccountDetail.validFor == "N")
                            {
                                param.Add(cdal.GetParameter("@FrozenFrom", model.AccountDetail.frozenFrom, typeof(DateTime)));
                                param.Add(cdal.GetParameter("@FrozenTo", model.AccountDetail.frozenTo, typeof(DateTime)));
                                param.Add(cdal.GetParameter("@FrozenComm", model.AccountDetail.frozenComm, typeof(string)));
                            }
                                param.Add(cdal.GetParameter("@VatChange", model.AccountDetail.VatChange, typeof(string)));
                        }
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

                        


                    }
                    if (res1 > 0)
                    {
                        tran.Commit();
                        response.isSuccess = true;
                        response.Message = "G/L Account Added Successfully !";

                    }
                }
                else
                {
                    response.isSuccess = false;
                    response.Message = "G/L Account Code already exists !";
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
