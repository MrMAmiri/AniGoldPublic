using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AniGoldShop.Application.Common.FluentValidator
{
    public static class FluentValidatorHelper
    {
        public static bool IsMobilePhoneNumber(this string phoneNumber)
        {
            var countPlus = phoneNumber.Count(s => s == '+');
            if (countPlus > 1) return false;

            // +1 12 345 67890 , +98 12 345 6789
            if (countPlus == 1 && (phoneNumber.Length < 12 || phoneNumber.Length > 13)) return false;

            // 0913 123 4567 , 913 123 4567
            if (countPlus == 0)
            {
                // 0913 123 4567 
                if (phoneNumber.StartsWith('0') && phoneNumber.Length != 11) return false;

                // 913 123 4567
                if (!phoneNumber.StartsWith('0') && phoneNumber.Length != 10) return false;
            }

            return true;
        }
    }
}
