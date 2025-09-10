using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public interface IForgotPasswordService
    {
        Task<string> ForgotPasswordAsync(string email);
    }

    public class ForgotPasswordService : IForgotPasswordService
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _config;

        public ForgotPasswordService(UserManager<User> userManager, IEmailSender emailSender, IConfiguration config)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _config = config;
        }

        public async Task<string> ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return "User not found";

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var link = $"{_config["App:BaseUrl"]}/resetpassword?email={email}&token={Uri.EscapeDataString(token)}";

            await _emailSender.SendEmailAsync(email, "Reset Password", $"<a href='{link}'>Click to reset</a>");

            return "Password reset link sent to email.";
        }
    }
}
