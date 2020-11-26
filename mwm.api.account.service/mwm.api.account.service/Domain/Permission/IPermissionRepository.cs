using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Account.Service.Domain.Permission
{
    public interface IPermissionRepository
    {
        ValueTask<int> CreateRoleAsync(RoleEntity role);
        ValueTask<int> CreateRoleFunctionMappingAsync(string filter);
        ValueTask<int> DeleteRoleAsync(int role_id);
        ValueTask<int> DeleteRoleFunctionMappingAsync(int role_id);
        ValueTask<IEnumerable<FunctionEntity>> GetAllFunctionAsync();
        ValueTask<IEnumerable<RoleEntity>> GetAllRoleAsync();
        ValueTask<FunctionEntity> GetFunctionAsync(int function_id);
        ValueTask<RoleEntity> GetRoleAsync(int role_id);
        ValueTask<RoleEntity> GetRoleByNameAsync(string role_name);
        ValueTask<IEnumerable<RoleFunctionMapping>> GetRoleFunctionMappingsAsync(int role_id);
        ValueTask<int> UpdateRoleAsync(RoleEntity role);
    }
}
