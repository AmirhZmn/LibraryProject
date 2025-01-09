using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModularPatternTraining.Modules.RoleManagement.Service;

namespace ModularPatternTraining.Modules.RoleManagement.Controller
{
    [Authorize(Policy = "SuperUserOnly")]
    [ApiVersion(1)]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly ILogger<RoleController> _logger;
        public RoleController(IRoleService roleService, ILogger<RoleController> logger)
        {
            _roleService = roleService;
            _logger = logger;
        }
        [HttpPost]
        [Route("AddRole")]
        public async Task<IActionResult> AddRole(string roleName)
        {
            var result = await _roleService.AddRole(roleName);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.ErrorMessage);
        }
        [HttpPost]
        [Route("RemoveRole")]
        public async Task<IActionResult> RemoveRole(string roleName)
        {
            var result = await _roleService.RemoveRole(roleName);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.ErrorMessage);
        }
        [HttpGet]
        [Route("GetRoles")]
        public async Task<IActionResult> GetRoles()
        {
            var result = await _roleService.GetRoles();
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }
        [HttpGet]
        [Route("GetRoleById")]
        public async Task<IActionResult> GetRoleById(string id)
        {
            var result = await _roleService.GetRoleById(id);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return NotFound(result.ErrorMessage);
        }
        [HttpGet]
        [Route("RoleExistsAsync")]
        public async Task<IActionResult> RoleExistsAsync(string roleName)
        {
            var result = await _roleService.RoleExistsAsync(roleName);
            return Ok(result);
        }
    }
}
