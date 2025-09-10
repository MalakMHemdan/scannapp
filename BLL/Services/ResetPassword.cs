using BLL.DTOs;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public interface IResetPasswordService
    {
        Task<string> ResetPasswordAsync(ResetPasswordDto model);
    }

    public class ResetPasswordService : IResetPasswordService
    {
        private readonly UserManager<User> _userManager;

        public ResetPasswordService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<string> ResetPasswordAsync(ResetPasswordDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) return "User not found";

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            return result.Succeeded
                ? "Password reset successfully"
                : string.Join(", ", result.Errors.Select(e => e.Description));
        }
    }
}
