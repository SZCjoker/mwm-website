using MWM.API.Account.Service.Domain.Permission;
using Phoenixnet.Extensions.Handler;
using Phoenixnet.Extensions.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWM.API.Account.Service.Application.Permission
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _repository;

        public PermissionService(IPermissionRepository repository)
        {
            _repository = repository;
        }

        public async ValueTask<BasicResponse<RoleResponse>> GetRoleAsync(int role_id)
        {
            var role = await _repository.GetRoleAsync(role_id);
            var mapping = await _repository.GetRoleFunctionMappingsAsync(role_id);

            var response = new RoleResponse()
            {
                id = role.id,
                role_name = role.name,
                code = mapping.Select(a => a.function_id).DefaultIfEmpty()
            };

            return StateCodeHandler.ForRecord(response);
        }

        public async ValueTask<BasicResponse<IEnumerable<RoleResponse>>> GetRoleListAsync()
        {
            var allRoles = await _repository.GetAllRoleAsync();

            var response = allRoles.Select(a => new RoleResponse()
            {
                id = a.id,
                role_name = a.name,
                code = null
            });

            return StateCodeHandler.ForRecord(response);
        }

        public async ValueTask<BasicResponse<int>> CreateRoleAsync(RoleRequest request)
        {

            var roles = await _repository.GetAllRoleAsync();

            var roleCount = roles.Where(role => role.name == request.role_name);

            if (roleCount.Count() > 0)
            {
                return new BasicResponse<int>() { code = 305, desc = "名稱重複", data = 0 };
            }

            var time = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var entity = new RoleEntity()
            {
                id = 0,
                name = request.role_name,
                ctime = time,
                utime = time,
                state = 1
            };

            var result = await _repository.CreateRoleAsync(entity);

            if (result > 0)
            {
                var role = await _repository.GetRoleByNameAsync(request.role_name);
                return StateCodeHandler.ForRecord(role.id);
            }

            return new BasicResponse<int>() { code = 300, desc = "新增失敗", data = 0 };
        }

        public async ValueTask<BasicResponse> UpdateRoleAsync(RoleRequest request)
        {
            var time = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds();



            var entity = new RoleEntity()
            {
                id = request.role_id,
                name = request.role_name,
                utime = time
            };

            var result = await _repository.UpdateRoleAsync(entity);

            return StateCodeHandler.ForCount(result);
        }

        public async ValueTask<BasicResponse> DeleteRoleAsync(int role_id)
        {
            var result = await _repository.DeleteRoleAsync(role_id);
            return StateCodeHandler.ForCount(result);
        }

        public async ValueTask<BasicResponse> CreateOrUpdateRoleFunctionMapping(RoleFuntionRequest request)
        {
            if (request.code == null || request.code.Count() == 0)
                return StateCodeHandler.ForBool(false);
            var deleteResult = await _repository.DeleteRoleFunctionMappingAsync(request.role_id);

            var time = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var filter = new StringBuilder();

            foreach (var code in request.code)
            {
                filter.Append($" ({request.role_id}, {code}, {time}, {time}, 1)");
                filter.Append(" ,");
            }
            filter.Remove(filter.Length - 1, 1);

            var result = await _repository.CreateRoleFunctionMappingAsync(filter.ToString());

            return StateCodeHandler.ForCount(result);
        }
    }
}
