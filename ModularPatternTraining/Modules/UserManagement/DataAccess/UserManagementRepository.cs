using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ModularPatternTraining.Data.AppDbContext;
using ModularPatternTraining.Modules.UserManagement.Dto;
using ModularPatternTraining.Modules.UserManagement.Model;

namespace ModularPatternTraining.Modules.UserManagement.DataAccess
{
    public class UserManagementRepository : IUserManagementRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDbContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        
        public UserManagementRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, AppDbContext dbContext, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _dbContext = dbContext;
            _roleManager = roleManager;
          
        }

      
        public async Task<ApplicationUser> GetByNameAsync(string name)
        {
            return await _userManager.FindByNameAsync(name);
        }

        public async Task<ApplicationUser> GetUserByUsernameAsync(string UserName)
        {
            return await _userManager.FindByNameAsync(UserName);
        }

        public async Task<ResponseUserDto> GetUserImportantById(string id)
        {
            return await _userManager.Users.AsNoTracking()
                .Where(x => x.Id == id)
                .Select(u => DtosToAppUserMap.ToDto(u))
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllAsync()
        {
            return await _userManager.Users.AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<ResponseUserDto>> GetAllImportantAsync()
        {
            return await _userManager.Users.AsNoTracking()
                .Select(u => DtosToAppUserMap.ToDto(u)).ToListAsync();
        }

        public async Task<IdentityResult> AddAsync(ApplicationUser entity , string password)
        {
            return await _userManager.CreateAsync(entity,password);
        }
        public async Task<IdentityResult> UpdateUserAsync(ApplicationUser usr)
        {
            var user = await _userManager.FindByNameAsync(usr.UserName);
            if (user == null)return IdentityResult.Failed(new IdentityError()
            {
                Code = "404",
                Description = "User Not Found"
            } );

                user.UserName = usr.UserName;
                user.FirstName = usr.LastName;
                user.LastName = usr.LastName;
                user.Email = usr.Email;
                user.PhoneNumber = usr.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);
            return result;
        }

        public async Task<SignInResult> LoginUser(string username, string password)
        {
            var user = await GetUserByUsernameAsync(username);
            return await _signInManager.CheckPasswordSignInAsync(user, password, false);
        }

        public async Task<bool> IsExistAsync(string name)
        {
            var user = await _userManager.FindByNameAsync(name);

            return user != null;
        }

        public async Task<bool> IsExistById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return user != null;
        }

        public async Task<bool> IsLockout(ApplicationUser entity)
        {
            return await _userManager.IsLockedOutAsync(entity);
        }

        public async Task<IdentityResult> AssignRoleToUser(string userId, string roleName)
        {

            
            var role = await _roleManager.RoleExistsAsync(roleName);
            if (!role) return IdentityResult.Failed(new IdentityError()
            {
                Code = "404",
                Description = "Role Not Found"
            });
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return IdentityResult.Failed(new IdentityError()
            {
                Code = "404",
                Description = "User Not Found"
            });

            var inRole = await IsUserInRole(userId, roleName);
            if (inRole.Succeeded)
            {
                return IdentityResult.Failed(new IdentityError()
                {
                    Code = "400",
                    Description = "User Already In Role"
                });
            }
            return await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task<IdentityResult> RemoveRoleFromUser(string userId, string roleName)
        {
            var role = await _roleManager.RoleExistsAsync(roleName);
            if (role) return IdentityResult.Failed(new IdentityError()
            {
                Code = "404",
                Description = "Role Not Found"
            });
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return IdentityResult.Failed(new IdentityError()
            {
                Code = "404",
                Description = "User Not Found"
            });
           
            return await _userManager.RemoveFromRoleAsync(user, roleName);
        }

        public async Task<IdentityResult> IsUserInRole(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return IdentityResult.Failed(new IdentityError
            {
                Code = "404",
                Description = "User Not Found"
            });
            var role = await _roleManager.RoleExistsAsync(roleName);
            if (role) return IdentityResult.Failed(new IdentityError
            {
                Code = "404",
                Description = "Role Not Found"
            });
            var isInRole = await _userManager.IsInRoleAsync(user, roleName);
            return isInRole ? IdentityResult.Success : IdentityResult.Failed(new IdentityError
            {
                Code = "404",
                Description ="Role Was Not Found for User"
            });
        }

        public async Task<IList<string>> GetRoleByUser(ApplicationUser user)
        {
            var role = await _dbContext.UserRoles.Where(x => x.UserId == user.Id).Select(x=>x.RoleId).ToListAsync();
            return role;
        }

        
    }
}
