using BLL.DTOs;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public interface IChangePasswordService
    {
        Task<string> ChangePasswordAsync(ChangePasswordDto model, string userId);
    }

    public class ChangePasswordService : IChangePasswordService
    {
        private readonly UserManager<User> _userManager;

        public ChangePasswordService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<string> ChangePasswordAsync(ChangePasswordDto model, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return "User not found";

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            return result.Succeeded
                ? "Password changed successfully"
                : string.Join(", ", result.Errors.Select(e => e.Description));
        }
    }
}
