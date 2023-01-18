using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iSOL_Enterprise.Models
{
    public class RequestModels
    {
        public string search { get; set; }
        public int? start { get; set; }
        public int? length { get; set; }

        public int? Offset { get { return start; } set { start = value; } }
        public int? PageSize { get { return length; } set { length = value; } }
        public int SortingCols { get; set; }
        public string searchCols { get; set; }
        public string Guid { get; set; }
        public string Role { get; set; }
        public int? UserId { get; set; }
        public string FieldId { get; set; }
        public string ItemCategoryCode { get; set; }
        public string VclCode { get; set; }
        public string ItemCode { get; set; }
        public string WarehouseCode { get; set; }
        public List<Order> order { get; set; }
        public List<Column> columns { get; set; }

        public string WhereClause { get; set; }
        public string[] WhereCla { get; set; }
        public List<string> ListWhereClause { get; set; }
        public string LookupCode { get; set; }
        public string PageId { get; set; }
        public List<AdvanceSearchModels> AdvSrch { get; set; }

        public class Order
        {
            public int column { get; set; }
            public string dir { get; set; }
        }
        public class Column
        {
            public string data { get; set; }
            public string name { get; set; }
            public bool searchable { get; set; }
            public bool orderable { get; set; }
            public Search search { get; set; }
        }
        public class Search
        {
            public string value { get; set; }
            public string regex { get; set; }
        }
        public List<whereClauseData> whereClauses { get; set; }
        public class whereClauseData
        {
            public string Data { get; set; }
            public string Parameter { get; set; }
        }
        public RequestModels()
        {
            whereClauses = new List<whereClauseData>();
            ListWhereClause = new List<string>();

        }
    }
}
