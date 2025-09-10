using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IAuthService
    {
        Task<string> ConfirmEmailAsync(int userId, string token);
        Task<string> ForgotPasswordAsync(string email);
        Task<string> ResetPasswordAsync(ResetPassword model);
        Task<string> ChangePasswordAsync(ChangePassword model, string userId);
    }
}
