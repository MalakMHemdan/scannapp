using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public interface IConfirmEmailService
    {
        Task<string> ConfirmEmailAsync(int userId, string token);
    }

    public class ConfirmEmailService : IConfirmEmailService
    {
        private readonly UserManager<User> _userManager;

        public ConfirmEmailService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<string> ConfirmEmailAsync(int userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return "User not found";

            var result = await _userManager.ConfirmEmailAsync(user, token);
            return result.Succeeded ? "Email confirmed successfully" : "Invalid token";
        }
    }
}
