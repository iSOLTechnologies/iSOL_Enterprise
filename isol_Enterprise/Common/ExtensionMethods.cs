using SqlHelperExtensions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
namespace iSOL_Enterprise.Common
{
    public static class ExtensionMethods
    {
        public static string ToString(this object val)
        {
            if (val == null) return String.Empty;
            return Convert.ToString(val);
        }

        public static int ToInt(this object val)
        {
            if (val == null) return 0;
            int number = 0;
            bool success = Int32.TryParse(val.ToString(), out number);
            if (success)
                return number;
            else
                return 0;
        }

        public static decimal ToDecimal(this object val)
        {
            if (val == null) return 0;
            decimal number = 0;
            bool success = Decimal.TryParse(val.ToString(), out number);
            if (success)
                return number;
            else
                return 0;
        }

        public static double ToDouble(this object val)
        {
            if (val == null) return 0;
            double number = 0;
            bool success = Double.TryParse(val.ToString(), out number);
            if (success)
                return number;
            else
                return 0;
        }

        public static bool ToBool(this object val)
        {
            if (val == null) return false;
            Boolean result = false;
            bool success = Boolean.TryParse(val.ToString(), out result);
            if (success)
                return result;
            else
                return false;
        }

        public static bool ToBoolean(this object val)
        {
            if (val == null) return false;           
            else
            {
                if (val.ToInt() >= 1) return true;
                else return false;
            };
        }

        public static DateTime? ToDateTime(this object val)
        {
            if (val == null) return null;
            DateTime dt;
            bool success = DateTime.TryParse(val.ToString(), out dt);
            if (success)
                return dt;
            else
                return null;
        }

        public static string ToZeroOrOne(this string val)
        {
            if (val == null) return "";
            if (val.Equals("t") || val.Equals("tr") || val.Equals("tru") || val.Equals("true"))
                return "1";
            else if (val.Equals("f") || val.Equals("fa") || val.Equals("fal") || val.Equals("fals") || val.Equals("false"))
                return "0";
            else
                return String.Empty;
        }       

        public static bool hasDuplicates<T>(this List<T> myList)
        {
            if (myList == null) return false;
            if ((myList.Count==0) || (myList.Count == 1))  return false;

            var hs = new HashSet<T>();
            for (var i = 0; i < myList.Count; ++i)
            {
                if (!hs.Add(myList[i])) return true;
            }
            return false;
        }
        public static int  GetApprovalStatus(this object val,SqlTransaction tran)
        {
            try
            {
                if (val == null) return 1;

                bool Approve = false;
                string HeadQuery = @" Select Approve from Pages WHERE ObjectCode = " + val;

                Approve = SqlHelper.ExecuteScalar(tran, CommandType.Text, HeadQuery).ToBool();
                
                return Approve == true ?  0 : 1;
            }
            catch (Exception)
            {
                return 1;
                throw;
            }

        }
        //public static string GetErrorMessages(this ModelStateDictionary modelState)
        //{
        //    var messages = new List<string>();

        //    foreach (var entry in modelState)
        //    {
        //        foreach (var error in entry.Value.Errors)
        //            messages.Add(entry.Key + ": " + error.ErrorMessage);
        //    }

        //    return String.Join(" ", messages);
        //}


        //public static DateTime ToDateTime(this object val)
        //{
        //    if (val == null) return String.Empty;
        //    return Convert.ToString(val);
        //}

    }
}
