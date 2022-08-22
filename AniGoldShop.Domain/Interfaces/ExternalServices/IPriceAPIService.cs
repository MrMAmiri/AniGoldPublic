using AniGoldShop.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniGoldShop.Domain.Interfaces.ExternalServices
{
    public interface IPriceAPIService
    {
        Task<IEnumerable<PriceAPIVM>> GetNewPrices();
    }
}
