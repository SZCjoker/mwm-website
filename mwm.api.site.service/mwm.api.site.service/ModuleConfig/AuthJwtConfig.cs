using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MWM.Extensions.Authentication.JWT;
using Phoenixnet.Extensions;

namespace MWM.API.Site.Service.ModuleConfig
{
    public class AuthJwtConfig : IAbstractModule
    {
        public void Load(IServiceCollection services, IConfiguration configuration)
        {
            var key = configuration.GetValue<string>("Jwt:JwtKey");
            var issuer = configuration.GetValue<string>("Jwt:JwtIssuer");
            services.UseJWTAuth(issuer, key);
            services.AddTransient<MWM.Extensions.Authentication.JWT.IGetJwtTokenInfoService, MWM.Extensions.Authentication.JWT.GetJwtTokenInfoService>();
        }
    }
}