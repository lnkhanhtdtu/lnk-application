using Lnk.Application.Abstracts;
using Lnk.Domain.Entities;
using Lnk.Application.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Lnk.Domain.Enums;

namespace Lnk.Application.Services
{
    public class UserServices : IUserServices
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserServices(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

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

        public async Task<ResponseModel> CreateUser(AccountDTO model)
        {
            string errors = string.Empty;

            var user = new ApplicationUser
            {
                FullName = model.Fullname,
                UserName = model.Username,
                IsActive = model.IsActive,
                Email = model.Email,
                PhoneNumber = model.Phone
                // Address = model.Address
            };

            if (string.IsNullOrEmpty(model.Id))
            {
                var identityResult = await _userManager.CreateAsync(user, model.Password);

                if (identityResult.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, model.RoleName);

                    return new ResponseModel
                    {
                        Status = true,
                        Message = "Tạo tài khoản thành công",
                        Action = ActionType.Insert
                    };
                }

                errors = string.Join("<br />", identityResult.Errors.Select(e => e.Description));
            }
            else
            {
                var identityResult = await _userManager.UpdateAsync(user);
                if (identityResult.Succeeded)
                {
                    return new ResponseModel
                    {
                        Status = true,
                        Message = "Cập nhật tài khoản thành công",
                        Action = ActionType.Update
                    };
                }
                errors = string.Join("<br />", identityResult.Errors.Select(e => e.Description));
            }

            return new ResponseModel
            {
                Action = string.IsNullOrEmpty(model.Id) ? ActionType.Insert : ActionType.Update,
                Status = false,
                Message = $"{(string.IsNullOrEmpty(model.Id) ? "Tạo tài khoản thất bại" : "Cập nhật tài khoản thất bại")} {errors}"
            };
        }
    }
}