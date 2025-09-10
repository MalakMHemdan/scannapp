using BLL.DTOs;
using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BLL.Services
{
    public interface ILoginService
    {
        Task<TokenResponseDto> LoginAsync(LoginDto model);
    }

    public class LoginService : ILoginService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;

        public LoginService(UserManager<User> userManager, IConfiguration config, AppDbContext context)
        {
            _userManager = userManager;
            _config = config;
            _context = context;
        }

        public async Task<TokenResponseDto> LoginAsync(LoginDto model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                return null;

            var roles = await _userManager.GetRolesAsync(user);

            //  Claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            // Access Token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var accessToken = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["Jwt:DurationInMinutes"])),
                signingCredentials: creds
            );

            var accessTokenString = new JwtSecurityTokenHandler().WriteToken(accessToken);

            // Generate Refresh Token
            var refreshToken = new RefreshToken
            {
                UserId = user.Id,
                Token = Guid.NewGuid().ToString(),
                ExpiryDate = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            };

            //  Save Refresh Token
            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            return new TokenResponseDto
            {
                AccessToken = accessTokenString,
                RefreshToken = refreshToken.Token
            };
        }
    }
}
