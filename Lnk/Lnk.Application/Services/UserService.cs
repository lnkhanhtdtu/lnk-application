using AutoMapper;
using Lnk.Application.Abstracts;
using Lnk.Domain.Entities;
using Lnk.Application.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Lnk.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Lnk.Application.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;

        public UserService(UserManager<ApplicationUser> userManager, IMapper mapper, IImageService imageService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _imageService = imageService;
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

        public async Task<ResponseModel> Save(AccountDTO model)
        {
            string errors = string.Empty;
            IdentityResult identityResult;

            if (string.IsNullOrEmpty(model.Id))
            {
                var user = new ApplicationUser
                {
                    FullName = model.Fullname,
                    UserName = model.Username,
                    IsActive = model.IsActive,
                    Email = model.Email,
                    PhoneNumber = model.Phone
                    // Address = model.Address
                };

                identityResult = await _userManager.CreateAsync(user, model.Password);

                if (identityResult.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, model.RoleName);

                    await _imageService.SaveImage(new List<IFormFile>() { model.Avatar }, "images/user",
                        $"{user.Id}.png");

                    return new ResponseModel
                    {
                        Status = true,
                        Message = "Tạo tài khoản thành công",
                        Action = ActionType.Insert
                    };
                }
            }
            else
            {
                var user = await _userManager.FindByIdAsync(model.Id);

                user.FullName = model.Fullname;
                user.Email = model.Email;
                // user.MobilePhone = model.MobilePhone;
                user.PhoneNumber = model.Phone;
                // user.Address = model.Address;
                user.IsActive = model.IsActive;

                identityResult = await _userManager.UpdateAsync(user);

                if (identityResult.Succeeded)
                {
                    await _imageService.SaveImage(new List<IFormFile>() { model.Avatar }, "images/user", $"{user.Id}.png");

                    var hasRole = await _userManager.IsInRoleAsync(user, model.RoleName);

                    if (!hasRole)
                    {
                        var oldRoleName = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

                        var removeResult = await _userManager.RemoveFromRoleAsync(user, oldRoleName);

                        if (removeResult.Succeeded)
                        {
                            await _userManager.AddToRoleAsync(user, model.RoleName);
                        }
                    }

                    return new ResponseModel
                    {
                        Status = true,
                        Message = "Cập nhật thành công.",
                        Action = ActionType.Update
                    };
                }
            }

            errors = string.Join("<br />", identityResult.Errors.Select(e => e.Description));

            return new ResponseModel
            {
                Action = string.IsNullOrEmpty(model.Id) ? ActionType.Insert : ActionType.Update,
                Status = false,
                Message = $"{(string.IsNullOrEmpty(model.Id) ? "Tạo tài khoản thất bại" : "Cập nhật tài khoản thất bại")} {errors}"
            };
        }

        public async Task<AccountDTO> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return _mapper.Map<AccountDTO>(user);
        }
    }
}