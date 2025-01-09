using ModularPatternTraining.Modules.UserManagement.DataAccess;
using ModularPatternTraining.Modules.UserManagement.Dto;
using ModularPatternTraining.Modules.UserManagement.Model;
using ModularPatternTraining.Shared.Models;

namespace ModularPatternTraining.Modules.UserManagement.Service
{
    public class UserManagementService : IUserManagementService
    {
        private readonly IUserManagementRepository _userManagementRepository;


        public UserManagementService(IUserManagementRepository userManagementRepository)
        {
            _userManagementRepository = userManagementRepository;
        }

        public async Task<Result<ResponseUserDto>> GetMainsById(string id)
        {
            var user = await _userManagementRepository.GetUserImportantById(id);
            return user!=null ? Result<ResponseUserDto>.Success(user) : Result<ResponseUserDto>.Failure("User not found", 404);
        }

        public async Task<Result<ApplicationUser>> GetUserByName(string name)
        {
            var user = await _userManagementRepository.GetByNameAsync(name);

            return user != null
                ? Result<ApplicationUser>.Success(user)
                : Result<ApplicationUser>.Failure("User Not Found", 404);
        }

        public async Task<Result<ResponseUserDto>> GetMainsByUsername(string username)
        {
            var user = await _userManagementRepository.GetUserByUsernameAsync(username);
            return user != null
                ? Result<ResponseUserDto>.Success(DtosToAppUserMap.ToDto(user))
                : Result<ResponseUserDto>.Failure("User Not Found", 404);
        }

        public async Task<Result<IEnumerable<ResponseUserDto>>> GetAllMainsAsync()
        {
            var users = await _userManagementRepository.GetAllImportantAsync();
            return users.Any() ? Result<IEnumerable<ResponseUserDto>>.Success(users) : Result<IEnumerable<ResponseUserDto>>.Failure("No Users Found" , 404);
        }

        public async Task<Result<bool>> AddAsync(CreateUserDto entity)
        {
            var adding = await _userManagementRepository.AddAsync(DtosToAppUserMap.ToAppUser(entity),entity.Password);
            if (adding.Succeeded)
            {
                return Result<bool>.Success(true);
            }
            var error = adding.Errors.Aggregate(string.Empty, (current, err) => current + err.Description);

            return Result<bool>.Failure(error ,400 );
        }

        public async Task<Result<ResponseUserDto>> LoginUser(string username, string password)
        {
            var result = await _userManagementRepository.LoginUser(username, password);
            if (!result.Succeeded) return Result<ResponseUserDto>.Failure("Invalid username or password", 401);
            
            var user = await _userManagementRepository.GetUserByUsernameAsync(username);

            return Result<ResponseUserDto>.Success(DtosToAppUserMap.ToDto(user));


        }

        public async Task<Result<bool>> UpdateAsync(UpdateUserDto entity)
        {
            var result = await _userManagementRepository.UpdateUserAsync(DtosToAppUserMap.ToAppUser(entity));
            if (result.Succeeded)
            {
              return  Result<bool>.Success(true);
            }

            var error = result.Errors.Aggregate(string.Empty, (current, err) => current + err.Description);

            return Result<bool>.Failure(error, 400);
        }

        public async Task<Result<bool>> IsLockedOut(ResponseUserDto entity)
        {
            var islocked = await _userManagementRepository.IsLockout(DtosToAppUserMap.ToAppUser(entity));
            
            return Result<bool>.Success(islocked);
        }

        public async Task<Result<bool>> AssignRoleToUser(string userId, string roleName)
        {
            var result = await _userManagementRepository.AssignRoleToUser(userId, roleName);
            return result.Succeeded
                ? Result<bool>.Success(true)
                : Result<bool>.Failure(result.Errors.FirstOrDefault().Description , 400 );
        }

        public async Task<Result<bool>> RemoveRoleFromUser(string userId, string roleName)
        {
            var result = await _userManagementRepository.RemoveRoleFromUser(userId, roleName);
            return result.Succeeded
                ? Result<bool>.Success(true)
                : Result<bool>.Failure(result.Errors.FirstOrDefault().Description, 400);
        }

        public async Task<Result<bool>> IsUserInRole(string userId, string roleName)
        {
            var result = await _userManagementRepository.IsUserInRole(userId, roleName);
            return result.Succeeded ? Result<bool>.Success(true) : Result<bool>.Failure(result.Errors.FirstOrDefault().Description, 400);
        }

        public async Task<IList<string>> GetRoleByUser(ApplicationUser user)
        {
            var roles = await _userManagementRepository.GetRoleByUser(user);
            
            return roles;
        }

        
    }
}
