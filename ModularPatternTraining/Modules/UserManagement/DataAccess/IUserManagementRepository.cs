using Microsoft.AspNetCore.Identity;
using ModularPatternTraining.Modules.UserManagement.Dto;
using ModularPatternTraining.Modules.UserManagement.Model;

namespace ModularPatternTraining.Modules.UserManagement.DataAccess
{
    public interface IUserManagementRepository
    {
        Task<ApplicationUser> GetByNameAsync(string name);
        Task<ApplicationUser> GetUserByUsernameAsync(string UserName);
        Task<ResponseUserDto> GetUserImportantById(string id);
        Task<IEnumerable<ApplicationUser>> GetAllAsync();
        Task<IEnumerable<ResponseUserDto>> GetAllImportantAsync();
        Task<IdentityResult> AddAsync(ApplicationUser entity , string password);
        Task<IdentityResult> UpdateUserAsync(ApplicationUser user);

        Task<SignInResult> LoginUser(string username, string password);
        Task<bool> IsExistAsync(string name);
        Task<bool> IsExistById(string id);

        Task<bool> IsLockout(ApplicationUser entity);
        Task<IdentityResult> AssignRoleToUser(string userId, string roleName);
        Task<IdentityResult> RemoveRoleFromUser(string userId, string roleName);
        Task<IdentityResult> IsUserInRole(string userId, string roleName);
        Task<IList<string>> GetRoleByUser(ApplicationUser user);
    }
}
