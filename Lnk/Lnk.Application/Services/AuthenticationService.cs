using Lnk.Application.Abstracts;
using Lnk.Application.DTOs;
using Lnk.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Lnk.Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthenticationService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        public async Task<ResponseModel> CheckLogin(string username, string password, bool rememberMe)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return new ResponseModel
                {
                    Status = false,
                    Message = "Tên người dùng hoặc mật khẩu không đúng"
                };
            }

            var result = await _signInManager.PasswordSignInAsync(user, password, isPersistent: rememberMe, lockoutOnFailure: true);

            if (result.IsLockedOut)
            {
                var remainingLockout = user.LockoutEnd - DateTimeOffset.Now;

                return new ResponseModel
                {
                    Status = false,
                    Message = $"Tài khoản đã bị khóa. Vui lòng thử lại sau {Math.Round(remainingLockout?.TotalMinutes ?? 0, 0)} phút."
                };
            }

            if (result.Succeeded)
            {
                if (user.AccessFailedCount > 0)
                {
                    await _userManager.ResetAccessFailedCountAsync(user);
                }
                return new ResponseModel
                {
                    Status = true,
                    Message = "Đăng nhập thành công"
                };
            }

            return new ResponseModel
            {
                Status = false,
                Message = "Tên người dùng hoặc mật khẩu không đúng"
            };
        }

    }
}
