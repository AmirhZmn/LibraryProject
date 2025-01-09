using Microsoft.EntityFrameworkCore;
using ModularPatternTraining.Data.AppDbContext;
using ModularPatternTraining.Modules.Authentication.Models;
using ModularPatternTraining.Shared.Models;
using System.Security.Cryptography;
using ModularPatternTraining.Modules.UserManagement.Model;

namespace ModularPatternTraining.Modules.Authentication.DataAccess
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AppDbContext _appDbContext;

        public AuthRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Result<bool>> SaveRefreshToken(string userId, string refreshToken)
        {
            await _appDbContext.RefreshTokens.AddAsync(new RefreshToken()
            {
                Token = refreshToken,
                UserId = userId,
                ExpiryDate = DateTime.Now.AddDays(7),
                IsRevoked = false
            });

            await _appDbContext.SaveChangesAsync();

            return Result<bool>.Success(true);
        }

        public async Task<Result<string>> GenerateRefreshToken()
        {
            var randomNumber = new byte[100];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
            }
            return Result<string>.Success(Convert.ToBase64String(randomNumber));
        }

        public async Task<Result<RefreshToken>> GetRefreshToken(string token)
        {
           var myToken = await _appDbContext.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token);
           if (myToken == null) return Result<RefreshToken>.Failure("Invalid refresh token.", 404);
           if (IsTokenExpired(myToken.ExpiryDate)) return Result<RefreshToken>.Failure("Refresh token has expired.Login Again", 400);

           return Result<RefreshToken>.Success(myToken);

        }

        public async Task<Result<ApplicationUser>> GetUserWithRefreshToken(string token)
        {
            var refreshToken = await _appDbContext.RefreshTokens
                    .Include(rt => rt.User) 
                    .FirstOrDefaultAsync(rt => rt.Token == token && rt.IsRevoked==false);
            if (refreshToken == null) return Result<ApplicationUser>.Failure("Invalid refresh token.", 404);
            if (IsTokenExpired(refreshToken.ExpiryDate)) return Result<ApplicationUser>.Failure("Refresh token has expired.Login Again", 400);
            

            return Result<ApplicationUser>.Success(refreshToken.User);
        }
        private static bool IsTokenExpired(DateTime expiryDate)
        {
            return expiryDate < DateTime.Now;
        }


        public async Task<Result<bool>> RevokeRefreshToken(string token)
        {
           var myToken =  await _appDbContext.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token);
           if (myToken==null)return Result<bool>.Failure("Token Not Found" , 404);
           
           myToken.IsRevoked = true;
           await _appDbContext.SaveChangesAsync();

           return Result<bool>.Success(true);
        }
        public async Task<Result<bool>> IsBlacklistedAsync(string jti)
        {
            var now = DateTime.UtcNow;
            var isLogedOut= await _appDbContext.TokenBlackList
                .AnyAsync(t => t.jti == jti && t.ExpiryDate > now);
            return Result<bool>.Success(isLogedOut);
        }

        public async Task<Result<bool>> AddToBlacklistAsync(string jti, DateTime expiryDate)
        {
            var token = new TokenBlackListModel()
            {
                jti = jti,
                ExpiryDate = expiryDate
            };
            _appDbContext.TokenBlackList.Add(token);
            await _appDbContext.SaveChangesAsync();
            return Result<bool>.Success(true);
        }
    }
}
