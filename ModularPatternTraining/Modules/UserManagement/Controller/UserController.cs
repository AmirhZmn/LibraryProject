using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModularPatternTraining.Modules.UserManagement.Dto;
using ModularPatternTraining.Modules.UserManagement.Service;

namespace ModularPatternTraining.Modules.UserManagement.Controller
{
    [Authorize(Policy = "AdminOrSuperUser")]
    [ApiVersion(1)]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManagementService _userManagement;

        public UserController(IUserManagementService userManagement)
        {
            _userManagement = userManagement;
        }

        [HttpGet("User")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userManagement.GetMainsById(id);
            return  user.IsSuccess ? Ok(user.Data) : NotFound(user.ErrorMessage);
        }

        [HttpGet("Users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManagement.GetAllMainsAsync();
            return users.IsSuccess ? Ok(users.Data) : NotFound(users.ErrorMessage);
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto entity)
        {
           var result = await _userManagement.AddAsync(entity);
           return result.IsSuccess? Ok(result.Data) : BadRequest(result.ErrorMessage);

        }

        [HttpPost("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto entity)
        {
            var result = await _userManagement.UpdateAsync(entity);
            return result.IsSuccess ? Ok(result.Data) : BadRequest(result.ErrorMessage);

        }

        [HttpPost("AddRoleToUser")]
        public async Task<IActionResult> AddRoleToUser(string userId, string roleName)
        {
            var result = await _userManagement.AssignRoleToUser(userId, roleName);
            return result.IsSuccess ? Ok(result.Data) : BadRequest(result.ErrorMessage);
        }

        [HttpPost("RemoveRoleFromUser")]
        public async Task<IActionResult> RemoveRoleFromUser(string userId, string roleName)
        {
            var result = await _userManagement.RemoveRoleFromUser(userId, roleName);
            return result.IsSuccess ? Ok(result.Data) : BadRequest(result.ErrorMessage);
        }

    }
}
