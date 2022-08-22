using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniGoldShop.Application.Common
{
    public class BaseRequest
    {
        public Guid? Modifier { get; set; }
        public int? PageNumber { get; set; } = 1;
        public int? PageSize { get; set; } = 50;
        public int? Status { get; set; } = null;
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public bool? ForSelect { get; set; } = false;
        public string SortName { get; set; }
        public bool SortDesc { get; set; } = false;
        public bool IsCustomer { get; set; } = false;
        public Guid? ReqUserId { get; set; }
        public long? Priority { get; set; }
    }
}
