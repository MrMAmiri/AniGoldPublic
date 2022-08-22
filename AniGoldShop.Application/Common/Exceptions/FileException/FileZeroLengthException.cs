using System;
using System.Collections.Generic;
using System.Text;

namespace AniGoldShop.Application.Common.Exceptions.FileException
{
    public class FileZeroLengthException : Exception
    {
        public FileZeroLengthException(string message) : base(message){}
    }
}
