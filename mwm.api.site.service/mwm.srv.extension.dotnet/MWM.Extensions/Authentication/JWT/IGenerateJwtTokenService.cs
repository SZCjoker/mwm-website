using System.Collections.Generic;
using System.Security.Claims;

namespace MWM.Extensions.Authentication.JWT
{
public     interface IGenerateJwtTokenService
    {
        string GenerateJwtToken(IEnumerable<Claim> claims, int? timeWindow);

    }
}