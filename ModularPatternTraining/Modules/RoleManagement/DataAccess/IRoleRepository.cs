using Microsoft.AspNetCore.Identity;
using ModularPatternTraining.Modules.RoleManagement.Model;

namespace ModularPatternTraining.Modules.RoleManagement.DataAccess
{
    public interface IRoleRepository
    {
        Task<IdentityResult> AddRole(string roleName);
        Task<IdentityResult> RemoveRole(string roleName);
      
       
        Task<List<Role>> GetRoles();
        Task<IdentityRole> GetRoleById(string id);
        Task<bool> RoleExistsAsync(string roleName);
    }
}
