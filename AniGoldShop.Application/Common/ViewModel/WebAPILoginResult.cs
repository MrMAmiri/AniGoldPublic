using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniGoldShop.Application.Common.ViewModel
{
    public class WebAPILoginResult
    {
        public bool is_success { get; set; }
        public string status { get; set; }
        public string message { get; set; }
        public APILoginData entity { get; set; }
    }

    public class APILoginData
    {
        public long id { get; set; }
        public string name { get; set; }
        public string family { get; set; }
        public string mobile { get; set; }
        public string full_name { get; set; }
    }
}
