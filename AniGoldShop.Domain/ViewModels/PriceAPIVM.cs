using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniGoldShop.Domain.ViewModels
{
    public sealed class PriceAPIVM
    {
        public decimal price { get; set; }
        public string slug { get; set; }
        public decimal min { get; set; }
        public decimal max { get; set; }
        public string diff { get; set; }
        public string diff_percent { get; set; }
        public string direction { get; set; }
        public string api_updated_at { get; set; }
    }
}
