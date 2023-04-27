using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iSOL_Enterprise.Common
{
    public class Helper
    {
        private static string connectionstring;
        public static string ConnectionString { get { return connectionstring; } set { connectionstring = value; } }
    }
}
