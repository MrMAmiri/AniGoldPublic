using System;
using System.Collections.Generic;
using System.Text;

namespace AniGoldShop.Domain.Common.Exceptions.SMSService
{
    public class SMSServiceAPIKeyEmptyException : ArgumentNullException
    {
        public SMSServiceAPIKeyEmptyException(string message) : base(message)
        {
        }
    }
}
