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
    public interface IRefreshTokenService
    {
        Task<TokenResponseDto> RefreshTokenAsync(string refreshToken);
    }

    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _config;

        public RefreshTokenService(AppDbContext context, UserManager<User> userManager, IConfiguration config)
        {
            _context = context;
            _userManager = userManager;
            _config = config;
        }

        public async Task<TokenResponseDto> RefreshTokenAsync(string refreshToken)
        {
            var storedToken = await _context.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken && !rt.IsRevoked);

            if (storedToken == null || storedToken.ExpiryDate < DateTime.UtcNow)
                return null;

            // Revoke Old Token
            storedToken.IsRevoked = true;
            await _context.SaveChangesAsync();

            var user = storedToken.User;
            var roles = await _userManager.GetRolesAsync(user);

            // Claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // New Access Token
            var newAccessToken = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["Jwt:DurationInMinutes"])),
                signingCredentials: creds
            );

            // New Refresh Token
            var newRefreshToken = new RefreshToken
            {
                UserId = user.Id,
                Token = Guid.NewGuid().ToString(),
                ExpiryDate = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            };

            _context.RefreshTokens.Add(newRefreshToken);
            await _context.SaveChangesAsync();

            return new TokenResponseDto
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                RefreshToken = newRefreshToken.Token
            };
        }
    }
}
