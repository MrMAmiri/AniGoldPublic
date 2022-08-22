using System;
using System.Collections.Generic;
using System.Text;

namespace AniGoldShop.Domain.Common.Exceptions.SMSService
{
    public class SMSServiceException : Exception
    {
        public SMSServiceException(string message) : base(message)
        {

        }
    }
}
