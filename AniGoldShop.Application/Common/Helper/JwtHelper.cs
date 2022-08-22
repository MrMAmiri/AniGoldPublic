using AniGoldShop.Application.Common.Exceptions;
using AniGoldShop.Application.Common.ViewModel;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AniGoldShop.Application
{
    public static class JwtHelper
    {
        
        public static string generateJwtToken(JWTSettings jwtSettings, Guid userId, string name, string timezoneId,string role)
        {
            var key = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,name),
                    new Claim(ClaimTypes.NameIdentifier,userId.ToString()),
                    new Claim(ClaimTypes.Locality,timezoneId),
                    new Claim(ClaimTypes.Role,role)
                }),
                Expires = DateTime.UtcNow.AddDays(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescription);
            return tokenHandler.WriteToken(token);
        }

        public static string getClaimTimeZone(ClaimsPrincipal claims)
        {
            var timezon = claims.Claims.Where(e => e.Type == ClaimTypes.Locality);

            if (!timezon.Any())
            {
                throw new InvalidJwtTokenException();
            }

            return timezon
                .ToList()
                .First()
                .Value;
        }

        public static string getClaimUserId(ClaimsPrincipal claims)
        {
            var userId = claims.Claims
                .Where(e => e.Type == ClaimTypes.NameIdentifier.ToString());

            if (!userId.Any())
            {
                throw new InvalidJwtTokenException();
            }

            return userId
                .ToList()
                .First()
                .Value;
        }
    }
}
