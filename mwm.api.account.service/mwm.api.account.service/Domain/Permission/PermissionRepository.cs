using Dapper;
using Phoenixnet.Extensions.Data.MySql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Account.Service.Domain.Permission
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly IDbFactory _dbFactory;

        public PermissionRepository(IDbFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }
        public async ValueTask<RoleEntity> GetRoleAsync(int role_id)
        {
            string tsql = @"select id, name, ctime, utime, state
                              from role
                             where id = @role_id";

            return await _dbFactory.OperateAsync(async a =>
            {
                return await a.QueryFirstOrDefaultAsync<RoleEntity>(tsql, new { role_id });
            });
        }

        public async ValueTask<RoleEntity> GetRoleByNameAsync(string role_name)
        {
            string tsql = @"select id, name, ctime, utime, state
                              from role
                             where name = @role_name";

            return await _dbFactory.OperateAsync(async a =>
            {
                return await a.QueryFirstOrDefaultAsync<RoleEntity>(tsql, new { role_name });
            });
        }

        public async ValueTask<IEnumerable<RoleEntity>> GetAllRoleAsync()
        {
            string tsql = @"select id, name, ctime, utime, state
                              from role";

            return await _dbFactory.OperateAsync(async a =>
            {
                return await a.QueryAsync<RoleEntity>(tsql);
            });
        }

        public async ValueTask<FunctionEntity> GetFunctionAsync(int code)
        {
            string tsql = @"select code, name, level, ctime, utime, state
                              from function
                             where code = @code";
            return await _dbFactory.OperateAsync(async a =>
            {
                return await a.QueryFirstOrDefaultAsync<FunctionEntity>(tsql, new { code });
            });
        }

        public async ValueTask<IEnumerable<FunctionEntity>> GetAllFunctionAsync()
        {
            string tsql = @"select code, name, level, ctime, utime, state
                              from function";

            return await _dbFactory.OperateAsync(async a =>
            {
                return await a.QueryAsync<FunctionEntity>(tsql);
            });
        }

        public async ValueTask<IEnumerable<RoleFunctionMapping>> GetRoleFunctionMappingsAsync(int role_id)
        {
            string tsql = @"select role_id, function_id, ctime, utime, state
                              from role_functions
                             where role_id = @role_id";
            return await _dbFactory.OperateAsync(async a =>
            {
                return await a.QueryAsync<RoleFunctionMapping>(tsql, new { role_id });
            });
        }

        public async ValueTask<int> CreateRoleAsync(RoleEntity role)
        {
            string tsql = @"INSERT INTO `role` (`id`, `name`, `ctime`, `utime`, `state`) 
                                        VALUES (0, @name, @ctime, @utime, @state)";

            return await _dbFactory.OperateAsync(async a =>
            {
                return await a.ExecuteAsync(tsql, role);
            });
        }

        public async ValueTask<int> UpdateRoleAsync(RoleEntity role)
        {
            string tsql = @"update role
                               set name = if(@name = '', name, @name),
                                   utime = @utime
                             where id = @id";

            return await _dbFactory.OperateAsync(async a =>
            {
                return await a.ExecuteAsync(tsql, role);
            });
        }

        public async ValueTask<int> DeleteRoleAsync(int role_id)
        {
            string tsql = @"delete from role where id = @role_id";

            return await _dbFactory.OperateAsync(async a =>
            {
                return await a.ExecuteAsync(tsql, new { role_id });
            });
        }

        public async ValueTask<int> CreateRoleFunctionMappingAsync(string filter)
        {
            string sql = @"INSERT INTO `role_functions` (`role_id`, `function_id`, `ctime`, `utime`, `state`) VALUES ";
            var tsql = $"{sql} {filter}";

            return await _dbFactory.OperateAsync(async a =>
            {
                return await a.ExecuteAsync(tsql);
            });
        }

        public async ValueTask<int> DeleteRoleFunctionMappingAsync(int role_id)
        {
            string tsql = @"delete from role_functions where role_id = @role_id";

            return await _dbFactory.OperateAsync(async a =>
            {
                return await a.ExecuteAsync(tsql, new { role_id });
            });
        }

    }
}
