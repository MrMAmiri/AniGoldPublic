using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniGoldShop.Application.Common.Enum
{
    public enum OrderStatus
    {
        PreFactor=0,
        New=1,
        ReadyForProduction=2,
        Producing=3,
        Produced=4,
        SentToStorage=5,
        SentToCustomer=6,
        Changed=7,
        Canceled=8,
        Stored=9
    }
}
