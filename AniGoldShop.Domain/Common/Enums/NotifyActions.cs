using System;
using System.Collections.Generic;
using System.Text;

namespace AniGoldShop.Domain.Common.Enums
{
    [Flags]
    public enum NotifyActions
    {
        None = 0,
        SMS = 1,
        Email = 2,
        WebsiteNotify = 4
    }
}
