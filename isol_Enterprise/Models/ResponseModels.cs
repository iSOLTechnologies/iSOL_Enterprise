using System;
using System.Collections.Generic;
using System.Linq;

namespace iSOL_Enterprise.Models
{
    public class ResponseModels
    {
        public int Id { get; set; }

        public bool isInserted { get; set; }

        public bool isEdited { get; set; }

        public bool isDeleted { get; set; }

        public bool isDuplicate { get; set; }
        public bool isError { get; set; }
        public bool isLogin { get; set; }
        public bool isAuthorized { get; set; }
        public bool isQc { get; set; }

        public bool isAdjustable { get; set; }

        public string Message { get; set; }

        public dynamic Data { get; set; }
        public dynamic DataColumns { get; set; }

        public int draw { get; set; }

        public int? recordsTotal { get; set; }

        public int? recordsFiltered { get; set; }

        public bool isInfo { get; set; }
        public bool isSuccess { get; set; }
        public bool isWarning { get; set; } = false;
        public bool isLoggedIn { get; set; }
        public bool? isLogOut { get; set; }
        public string LookupCode { get; set; }

        public List<string> ListWhereClause { get; set; }
        public List<string> DuplicateMessage { get; set; }

        public ResponseModels()
        {
            DuplicateMessage = new List<string>();
        }
    }
}
