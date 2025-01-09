using Microsoft.AspNetCore.Identity;
using System.Data;
using Microsoft.EntityFrameworkCore;
using ModularPatternTraining.Modules.RoleManagement.Model;
using ModularPatternTraining.Modules.UserManagement.Service;

namespace ModularPatternTraining.Modules.RoleManagement.DataAccess
{
    public class RoleRepository : IRoleRepository
    {
        private readonly RoleManager<IdentityRole> _roleManager;
      

        public RoleRepository(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            
        }

        public async Task<IdentityResult> AddRole(string roleName)
        {
            return await _roleManager.CreateAsync(new IdentityRole(roleName));
             
        }

        public async Task<IdentityResult> RemoveRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return IdentityResult.Failed(new IdentityError() { Code = "RoleNotFound", Description = "Role not found" });
            }

            return await _roleManager.DeleteAsync(role);
        }

        
      

        public async Task<List<Role>> GetRoles()
        {
           return await _roleManager.Roles.Select(r => new Role()
           {
               Id = r.Id,
               Name = r.Name
           }).ToListAsync();
        }

        public async Task<IdentityRole> GetRoleById(string id)
        {
            return await _roleManager.FindByIdAsync(id);
        }

        public async Task<bool> RoleExistsAsync(string roleName)
        {
           return await _roleManager.RoleExistsAsync(roleName);
        }
    }
}
