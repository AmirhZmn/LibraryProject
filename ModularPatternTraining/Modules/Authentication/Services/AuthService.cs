using Azure;
using ModularPatternTraining.Modules.Authentication.DataAccess;
using ModularPatternTraining.Modules.Authentication.Models;
using ModularPatternTraining.Modules.UserManagement.Model;
using ModularPatternTraining.Shared.Models;
using ModularPatternTraining.Shared.Services.JWTService;

namespace ModularPatternTraining.Modules.Authentication.Services
{
    public class AuthService:IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IAuthRepository authRepository, IConfiguration configuration)
        {
            _authRepository = authRepository;
            _configuration = configuration;
        }

        public async Task<Result<string>> GenerateJwtToken(string userId, IList<string> roleList)
        {
           
            var token = JwtService.GenerateJwtToken(userId,roleList, _configuration);
            return Result<string>.Success(token);
        }

        public async Task<Result<string>> GenerateRefreshToken()
        {
           return await _authRepository.GenerateRefreshToken();
            
        }

        public async Task<Result<bool>> SaveRefreshToken(string userId, string refreshToken)
        {
            var result = await _authRepository.SaveRefreshToken(userId, refreshToken);

            return result;
        }

        public async Task<Result<RefreshToken>> GetRefreshToken(string token)
        {
            return await _authRepository.GetRefreshToken(token);

        }

        public async Task<Result<ApplicationUser>> GetUserWithRefreshToken(string token)
        {
            var user = await _authRepository.GetUserWithRefreshToken(token);
            return user.IsSuccess
                ? Result<ApplicationUser>.Success(user.Data)
                : Result<ApplicationUser>.Failure(user.ErrorMessage, user.StatusCode);
        }

        public async Task<Result<bool>> RevokeRefreshToken(string token)
        {
            return await _authRepository.RevokeRefreshToken(token);
        }

        public async Task<CookieOptions> SendTokenOptions()
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            return cookieOptions;
        }

        public async Task<Result<bool>> IsBlacklistedAsync(string jti)
        {
            var result = await _authRepository.IsBlacklistedAsync(jti);
            return result.Data ?  Result<bool>.Success(true) : Result<bool>.Failure("Is Not Black listed",404) ;
        }

        public Task<Result<bool>> AddToBlacklistAsync(string jti, DateTime expiryDate)
        {
            var result = _authRepository.AddToBlacklistAsync(jti, expiryDate);
            return result;
        }
    }
}
