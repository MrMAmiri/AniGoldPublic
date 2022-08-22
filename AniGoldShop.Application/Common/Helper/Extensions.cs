using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniGoldShop.Application.Common.Helper
{
    public static class Extensions
    {
        public static string[] CSplit(this string val)
        {
            if (string.IsNullOrEmpty(val))
                return null;
            else
                return val.Split(",");
        }
        public static string CJoin(this string[] val)
        {
            if (val==null || val.Length<=0)
                return null;
            else
                return String.Join(",", val);   
        }
    }
}
