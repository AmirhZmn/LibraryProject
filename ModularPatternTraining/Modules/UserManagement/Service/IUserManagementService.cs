using ModularPatternTraining.Modules.UserManagement.Dto;
using ModularPatternTraining.Modules.UserManagement.Model;
using ModularPatternTraining.Shared.Models;

namespace ModularPatternTraining.Modules.UserManagement.Service
{
    public interface IUserManagementService
    {
        Task<Result<ResponseUserDto>> GetMainsById(string id);
        Task<Result<ApplicationUser>> GetUserByName(string name);
        Task<Result<ResponseUserDto>> GetMainsByUsername(string username);
        Task<Result<IEnumerable<ResponseUserDto>>> GetAllMainsAsync();
        Task<Result<bool>> AddAsync(CreateUserDto entity);
        Task<Result<ResponseUserDto>> LoginUser(string username, string password);
        Task<Result<bool>> UpdateAsync(UpdateUserDto entity);
        Task<Result<bool>> IsLockedOut(ResponseUserDto entity);
        Task<Result<bool>> AssignRoleToUser(string userId, string roleName);
        Task<Result<bool>> RemoveRoleFromUser(string userId, string roleName);
        Task<Result<bool>> IsUserInRole(string userId, string roleName);
        Task<IList<string>> GetRoleByUser(ApplicationUser user);
    }
}
