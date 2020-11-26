using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace MWM.Extensions.Authentication.JWT
{
    public class GenerateJwttokenService:IGenerateJwtTokenService
    {
        private readonly string _jwtIssuer;
        private readonly string _jwtKey;
        private readonly int _jwtExpireMinutes;
      
        public GenerateJwttokenService(string jwtIssuer, string jwtKey, int jwtExpireMinutes)
        {
            this._jwtIssuer = jwtIssuer;
            _jwtKey = jwtKey;
            _jwtExpireMinutes = jwtExpireMinutes;
        }
        public string GenerateJwtToken(IEnumerable<Claim> claims, int? timeWindow)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTimeOffset.Now.AddMinutes(timeWindow ?? this._jwtExpireMinutes);

            var token = new JwtSecurityToken(
                this._jwtKey,
                this._jwtIssuer,
                claims,
                expires: expires.DateTime,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}