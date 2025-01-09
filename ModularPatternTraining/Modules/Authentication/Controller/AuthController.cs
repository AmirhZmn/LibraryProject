using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModularPatternTraining.Modules.Authentication.Dtos;
using ModularPatternTraining.Modules.Authentication.Services;
using ModularPatternTraining.Modules.UserManagement.Service;
using System.IdentityModel.Tokens.Jwt;
using ModularPatternTraining.Modules.RoleManagement.Service;

namespace ModularPatternTraining.Modules.Authentication.Controller
{
   
    [ApiVersion(1)]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserManagementService _userManagementService;
        private readonly IAuthService _authService;
        private readonly IRoleService _roleService;

        public AuthController(IUserManagementService userManagementService, IAuthService authService, IRoleService roleService)
        {
            _userManagementService = userManagementService;
            _authService = authService;
            _roleService = roleService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto entityDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Try Again");
            }

            var user = await _userManagementService.LoginUser(entityDto.UserName, entityDto.Password);
            if (!user.IsSuccess) return Unauthorized(new { message = "Invalid username or password" });

            var myUser = await _userManagementService.GetUserByName(user.Data.Name);
            var userRole = await _userManagementService.GetRoleByUser(myUser.Data);
            var token =await _authService.GenerateJwtToken(user.Data.Id,userRole);
            var refreshToken =await _authService.GenerateRefreshToken();
            await _authService.SaveRefreshToken(user.Data.Id, refreshToken.Data);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("RefreshToken", refreshToken.Data, cookieOptions);

            return Ok(new { accessToken = token.Data });

        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            
            var refreshToken = Request.Cookies["RefreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized(new { error = "Refresh token is missing" });
            }

            var userResult = await _authService.GetUserWithRefreshToken(refreshToken);

            if (!userResult.IsSuccess)
            {
                return Unauthorized(new { error = userResult.ErrorMessage });
            }

            var user = userResult.Data;
            var userRole = await _userManagementService.GetRoleByUser(user);
            var newAccessToken =await _authService.GenerateJwtToken(user.Id,userRole );
            var newRefreshToken = await _authService.GenerateRefreshToken();
            var cookieOptions =await  _authService.SendTokenOptions();
            await _authService.RevokeRefreshToken(refreshToken);
            await _authService.SaveRefreshToken(user.Id, newRefreshToken.Data);
            Response.Cookies.Append("RefreshToken", newRefreshToken.Data, cookieOptions);
            return Ok(new
            {
                accessToken = newAccessToken.Data
            });
        }

        [Authorize]
        [HttpPost("LogOut")]
        public async Task<IActionResult> LogOut()
        {
           
            var refreshToken = Request.Cookies["RefreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
            {
                return BadRequest("Refresh token is missing");
            }

            
            var accessToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
            if (string.IsNullOrEmpty(accessToken))
            {
                return BadRequest("Access token is missing");
            }

            
            var resultRefreshToken = await _authService.RevokeRefreshToken(refreshToken);
            if (!resultRefreshToken.IsSuccess)
            {
                return BadRequest("Invalid refresh token");
            }

            
            var jwtHandler = new JwtSecurityTokenHandler();
            if (!jwtHandler.CanReadToken(accessToken))
            {
                return BadRequest("Invalid access token");
            }

            var jwtToken = jwtHandler.ReadJwtToken(accessToken);
            var jti = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
            var expiryDateUnix = long.Parse(jwtToken.Claims.First(c => c.Type == JwtRegisteredClaimNames.Exp).Value);
            var expiryDate = DateTimeOffset.FromUnixTimeSeconds(expiryDateUnix).UtcDateTime;
            
            if (string.IsNullOrEmpty(jti))
            {
                return BadRequest("Invalid JTI in access token");
            }

            var resultAccessToken = await _authService.AddToBlacklistAsync(jti, expiryDate);
            if (!resultAccessToken.IsSuccess)
            {
                return BadRequest("Failed to blacklist access token");
            }



            Response.Cookies.Delete("RefreshToken");

           
            return Ok("Logged out successfully");
        }



    }
}
