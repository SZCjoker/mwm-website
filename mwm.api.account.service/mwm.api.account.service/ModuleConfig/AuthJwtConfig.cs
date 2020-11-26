using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MWM.Extensions.Authentication.JWT;
using Phoenixnet.Extensions;

namespace MWM.API.Account.Service.ModuleConfig
{
    public class AuthJwtConfig : IAbstractModule
    {
        public void Load(IServiceCollection services, IConfiguration configuration)
        {
            var key = configuration.GetValue<string>("Jwt:JwtKey");
            var issuer = configuration.GetValue<string>("Jwt:JwtIssuer");
            var expt = configuration.GetValue<int>("Jwt:JwtExpireMinutes");
            //JwtExpireMinutes
            services.UseJWTAuth(issuer, key);
            services.AddTransient<MWM.Extensions.Authentication.JWT.IGetJwtTokenInfoService, MWM.Extensions.Authentication.JWT.GetJwtTokenInfoService>();
            services.AddTransient<IGenerateJwtTokenService>(s=> new GenerateJwttokenService(issuer, key, expt));
        }
    }
}