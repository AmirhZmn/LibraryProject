using ModularPatternTraining.Modules.Authentication.Models;
using ModularPatternTraining.Modules.UserManagement.Model;
using ModularPatternTraining.Shared.Models;

namespace ModularPatternTraining.Modules.Authentication.Services
{
    public interface IAuthService
    {
        Task<Result<string>> GenerateJwtToken(string userId,IList<string> roleList);
        Task<Result<string>> GenerateRefreshToken();
        Task<Result<bool>> SaveRefreshToken(string userId, string refreshToken);
        Task<Result<RefreshToken>> GetRefreshToken(string token);
        Task<Result<ApplicationUser>> GetUserWithRefreshToken(string token);
        Task <Result<bool>> RevokeRefreshToken(string token);
        Task<CookieOptions> SendTokenOptions ();
        Task<Result<bool>> IsBlacklistedAsync(string jti);
        Task<Result<bool>> AddToBlacklistAsync(string jti, DateTime expiryDate);
    }
}
