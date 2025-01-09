using ModularPatternTraining.Modules.RoleManagement.Model;
using ModularPatternTraining.Shared.Models;

namespace ModularPatternTraining.Modules.RoleManagement.Service
{
    public interface IRoleService
    {
        Task<Result<bool>> AddRole(string roleName);
        Task<Result<bool>> RemoveRole(string roleName);


        Task<List<Role>> GetRoles();
        Task<Result<Role>> GetRoleById(string id);
        Task<bool> RoleExistsAsync(string roleName);
    }
}
