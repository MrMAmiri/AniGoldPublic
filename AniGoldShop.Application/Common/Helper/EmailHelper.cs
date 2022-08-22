using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniGoldShop.Application.Common.Helper
{
    public class EmailHelper
    {
        public static string SetTemplatevariables(string template,Dictionary<string,dynamic> paramters)
        {
            if (template == null) return null;

            foreach (var param in paramters)
            {
                template = template.Replace($"@{param.Key}", Convert.ToString(param.Value));
            }

            return template;
        }
    }
}
