using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MWM.API.Account.Service.Application.Permission;

namespace MWM.API.Account.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _service;
        private readonly ILogger<PermissionController> _logger;
        private readonly IHttpContextAccessor _contextAccessor;



        public PermissionController(IPermissionService service, ILogger<PermissionController> logger, IHttpContextAccessor contextAccessor)
        {
            _service = service;
            _logger = logger;
            _contextAccessor = contextAccessor;
        }

        [HttpGet]
        public async ValueTask<IActionResult> Get()
        {
            return Ok("OK");
        }

        /// <summary>
        /// 取得單一角色訊息
        /// </summary>
        /// <param name="role_id"></param>
        /// <returns></returns>
        [HttpGet("role/{role_id}")]
        public async ValueTask<IActionResult> GetRole(int role_id)
        {
            var result = await _service.GetRoleAsync(role_id);
            return Ok(result);
        }

        /// <summary>
        /// 取得所有角色清單
        /// </summary>
        /// <returns></returns>
        [HttpGet("role/all")]
        public async ValueTask<IActionResult> GetRoleList()
        {
            var result = await _service.GetRoleListAsync();
            return Ok(result);
        }

        /// <summary>
        /// 建立角色
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("role")]
        public async ValueTask<IActionResult> CreateRole(RoleRequest request)
        {
            var result = await _service.CreateRoleAsync(request);
            return Ok(result);
        }

        /// <summary>
        /// 更新角色訊息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("role")]
        public async ValueTask<IActionResult> UpdateRole(RoleRequest request)
        {
            var result = await _service.UpdateRoleAsync(request);
            return Ok(result);
        }

        /// <summary>
        /// 刪除角色
        /// </summary>
        /// <param name="role_id"></param>
        /// <returns></returns>
        [HttpDelete("role/{role_id}")]
        public async ValueTask<IActionResult> DeleteRole(int role_id)
        {
            var result = await _service.DeleteRoleAsync(role_id);
            return Ok(result);
        }

        /// <summary>
        /// 建立、更新角色權限
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("role/function")]
        public async ValueTask<IActionResult> CreateOrUpdateRoleFunctionMapping(RoleFuntionRequest request)
        {
            var result = await _service.CreateOrUpdateRoleFunctionMapping(request);
            return Ok(result);
        }
    }
}