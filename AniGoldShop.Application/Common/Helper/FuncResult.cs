using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniGoldShop.Application
{
    public class FuncResult
    {
        public bool Successful { get; set; }

        public string Message { get; set; }

        public object Data { get; set; }

        public T GetData<T>()
        {
            return (T)Data;
        }
    }
}
