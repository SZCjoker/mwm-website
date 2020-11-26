using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MWM.Extensions.Authentication.JWT;
using Phoenixnet.Extensions;

namespace MWM.API.Advert.Service.ModuleConfig
{
    /// <summary>
    /// JWT驗證設定
    /// </summary>
    public class AuthJwtConfig : IAbstractModule
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public void Load(IServiceCollection services, IConfiguration configuration)
        {
            //JWT 驗證啟用
            var key = configuration.GetValue<string>("Jwt:JwtKey");
            var issuer = configuration.GetValue<string>("Jwt:JwtIssuer");
            services.UseJWTAuth(issuer, key);
            services.AddScoped<IGetJwtTokenInfoService, GetJwtTokenInfoService>();
        }
    }
}
