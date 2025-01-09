using ModularPatternTraining.Modules.RoleManagement.DataAccess;
using ModularPatternTraining.Modules.RoleManagement.Model;
using ModularPatternTraining.Shared.Models;

namespace ModularPatternTraining.Modules.RoleManagement.Service
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<Result<bool>> AddRole(string roleName)
        {
           var result = await _roleRepository.AddRole(roleName);
            return result.Succeeded ? Result<bool>.Success(true) : Result<bool>.Failure("Error While Adding Roles" , 400);
        }

        public async Task<Result<bool>> RemoveRole(string roleName)
        {
            var result = await _roleRepository.AddRole(roleName);
            return result.Succeeded ? Result<bool>.Success(true) : Result<bool>.Failure(result.Errors.FirstOrDefault().Description,400);
        }

        public async Task<List<Role>> GetRoles()
        {
           var roles = await _roleRepository.GetRoles();
           return roles;
        }

        public async Task<Result<Role>> GetRoleById(string id)
        {
            var role = await _roleRepository.GetRoleById(id);
            if (role== null)return Result<Role>.Failure("Role Not found" , 404);
            return Result<Role>.Success(new Role()
            {
                Id = role.Id,
                Name = role.Name
            });
        }

        public async Task<bool> RoleExistsAsync(string roleName)
        {
            var isExist = await _roleRepository.RoleExistsAsync(roleName);
            return isExist;

        }

        
    }
}
