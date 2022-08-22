using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniGoldShop.Application.Common.Exceptions
{
    public class InvalidJwtTokenException : UnauthorizedAccessException
    {
        public InvalidJwtTokenException()
            : base("Bearer token is invalid or expired")
        {

        }
    }
}
