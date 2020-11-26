using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace MWM.Extensions.Authentication.JWT
{
    public static class JWTAuthExtension
    {
        /// <summary>
        /// 使用JWT驗證
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="jwtIssuer"></param>
        /// <param name="jwtKey"></param>
        public static void UseJWTAuth(this IServiceCollection serviceCollection, string jwtIssuer, string jwtKey)
        {
            
            serviceCollection
                .AddAuthentication(
                    opt =>
                    {
                        opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                        opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    })
                .AddJwtBearer(
                    opt =>
                    {
                        opt.RequireHttpsMetadata = false;
                        opt.SaveToken = true;
                        opt.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateAudience = false,
                            ValidateIssuer = false,
                            ValidIssuer = jwtIssuer,
                            ValidAudience = jwtIssuer,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                            ClockSkew = TimeSpan.Zero // remove delay of token when expire
                        };
                    });
        }
    }
}