using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Web;

namespace Module_Vcc.DbContext
{
    public struct PrimaryKey
    {
        public string Name;
        public object Value;
    }

    public  class IEntity<T> where T : IEntity<T>
    {

        #region BaseColoumnsForAllModels
        public string client { get; set; }
        public string cocode { get; set; }
        public string isupdate { get; set; }
        public bool isactive { get; set; }
        public string delind { get; set; }
        public int version { get; set; }
        public string IpAddress { get; set; }
        public string ApiToken { get; set; }
        public string LoginToken { get; set; }
        public string reftype { get; set; }
        public string reftypeno { get; set; }
        public string UserId { get; set; }
        public string GetByFilter { get; set; }
        //ReftypeNo
        public string createdBy_No { get; set; }
        //Reftype
        public string createdBy_Type { get; set; }

        [ScaffoldColumn(false)]

        [DataType(DataType.Date)] // DateTime on MySqlServer
        public string createdat { get; set; } = DateTime.Now.ToString();

        [ScaffoldColumn(false)]
        [DataType(DataType.Date)] // DateTime on MySqlServer
        public string updatedat { get; set; } = DateTime.Now.ToString();

        [ScaffoldColumn(false)]
        [DataType(DataType.Date)] // DateTime on MySqlServer
        public string serverupdatedat { get; set; }

        [ScaffoldColumn(false)]
        [DataType(DataType.Date)] // DateTime on MySqlServer
        public string servercreatedat { get; set; }
        #endregion
    }
}