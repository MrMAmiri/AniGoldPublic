using System;
using System.Collections.Generic;
using System.Text;

namespace AniGoldShop.Application.Common.Exceptions.FileException
{
    public class FileMaximumLengthException : Exception
    {
        public FileMaximumLengthException(string message): base(message)
        {

        }
    }
}
