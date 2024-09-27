using Lnk.Domain.Entities;
using Lnk.Application.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Lnk.Application.Services
{
    public class UserServices : IUserServices
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserServices(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
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

        //GetAllUser
        public async Task<ResponseDatatable<UserModel>> GetUserPagination(RequestDataTable request)
        {
            var users = await _userManager.Users
                .Where(x => string.IsNullOrEmpty(request.Keyword)
                            || x.UserName.Contains(request.Keyword)
                            || x.FullName.Contains(request.Keyword)
                            || x.Email.Contains(request.Keyword)
                            || x.PhoneNumber.Contains(request.Keyword))
                .Select(user => new UserModel
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Fullname = user.FullName,
                    Email = user.Email,
                    Phone = user.PhoneNumber,
                    IsActive = user.IsActive ? "Có" : "Không"
                }).ToListAsync();

            int totalRecords = users.Count;
            var result = users.Skip(request.SkipItems).Take(request.PageSize).ToList();

            return new ResponseDatatable<UserModel>
            {
                Draw = request.Draw,
                RecordsFiltered = totalRecords,
                RecordsTotal = totalRecords,
                Data = result
            };
        }
    }
}