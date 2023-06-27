using iSOL_Enterprise.Common;
using iSOL_Enterprise.Models;
using SqlHelperExtensions;
using System.Data;
using System.Data.SqlClient;
namespace iSOL_Enterprise.Dal.Financials
{
    public class ChartOfAccountDal
    {

        public List<ListModel> GetDrawers()
        {


            string GetQuery = "select AcctCode,AcctName  from OACT where FatherNum is null";

            List<ListModel> list = new List<ListModel>();

            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, GetQuery))
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

            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text,query))
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
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultSapDB, CommandType.Text, query))
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
    }
}
