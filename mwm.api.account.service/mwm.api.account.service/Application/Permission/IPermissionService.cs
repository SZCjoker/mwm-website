using Phoenixnet.Extensions.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Account.Service.Application.Permission
{
    public interface IPermissionService
    {
        ValueTask<BasicResponse> CreateOrUpdateRoleFunctionMapping(RoleFuntionRequest request);
        ValueTask<BasicResponse<int>> CreateRoleAsync(RoleRequest request);
        ValueTask<BasicResponse> DeleteRoleAsync(int role_id);
        ValueTask<BasicResponse<RoleResponse>> GetRoleAsync(int role_id);
        ValueTask<BasicResponse<IEnumerable<RoleResponse>>> GetRoleListAsync();
        ValueTask<BasicResponse> UpdateRoleAsync(RoleRequest request);
    }
}
