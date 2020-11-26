using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace MWM.Extensions.Authentication.JWT
{
    public class GetJwtTokenInfoService : IGetJwtTokenInfoService
    {
        private readonly IHttpContextAccessor _ctx;

        public GetJwtTokenInfoService(IHttpContextAccessor ctx)
        {
            _ctx = ctx;
        }

        /// <summary>
        /// 取得使用者ID
        /// </summary>
        public int UserId
        {
            get => Convert.ToInt32(_ctx.HttpContext.User.Claims.FirstOrDefault(o => o.Type.Equals(JwtRegisteredClaimNames.Sid))?.Value);
            // ReSharper disable once ValueParameterNotUsed
            protected set { }
        }
        
        /// <summary>
        /// 取得登入名稱
        /// </summary>
        public string LoginName
        {
            get => _ctx.HttpContext.User.Claims.FirstOrDefault(o => o.Type.Equals(ClaimTypes.Name))?.Value;
            // ReSharper disable once ValueParameterNotUsed
            protected set { }
        }

        public string SecretId
        {
            get => _ctx.HttpContext.User.Claims.FirstOrDefault(o => o.Type.Equals(ClaimTypes.PrimarySid))?.Value;
            // ReSharper disable once ValueParameterNotUsed
            protected set { }
        }

        /// <summary>
        /// 使用者權限
        /// </summary>
        public IEnumerable<string> Permission
        {
            get => _ctx.HttpContext.User.Claims.Where(o => o.Type.Equals(ClaimTypes.Role)).Select(o => o.Value);
            // ReSharper disable once ValueParameterNotUsed
            protected set { }
        }
    }
}