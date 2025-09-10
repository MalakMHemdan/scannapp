using BLL.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public interface IRegisterService
    {
        Task<string> RegisterAsync(RegisterDto model);
    }

    public class RegisterService : IRegisterService
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _config;

        public RegisterService(UserManager<User> userManager, IEmailSender emailSender, IConfiguration config)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _config = config;
        }

        public async Task<string> RegisterAsync(RegisterDto model)
        {
            var user = new User
            {
                UserName = model.Username,
                Email = model.Email,
                Firstname = model.Firstname,
                Lastname = model.Lastname
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return string.Join(", ", result.Errors.Select(e => e.Description));

            // Generate confirmation token
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = $"{_config["App:BaseUrl"]}/api/auth/confirmemail?userId={user.Id}&token={Uri.EscapeDataString(token)}";

            // Send confirmation email
            await _emailSender.SendEmailAsync(model.Email, "Confirm Your Email",
                $"<a href='{confirmationLink}'>Click here to confirm your account</a>");

            return "User registered successfully. Please check your email to confirm your account.";
        }
    }
}
