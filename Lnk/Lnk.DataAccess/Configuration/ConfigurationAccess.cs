using Lnk.DataAccess.DataAccess;
using Lnk.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lnk.DataAccess.Configuration
{
    public static class ConfigurationAccess
    {
        /// <summary>
        /// Đăng ký db context
        /// </summary>
        /// <param name="service"></param>
        /// <param name="config"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void ConfigureIdentity(this IServiceCollection service, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("DefaultConnection")
                                   ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            service.AddDbContext<LnkDbContext>(options => options.UseSqlServer(connectionString));

            // Đăng ký identity
            service.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<LnkDbContext>();

            service.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "LnkCookie"; // Tên cookie
                options.ExpireTimeSpan = TimeSpan.FromMinutes(10); // Thời gian tồn tại của cookie
                options.LoginPath = "/Admin/Authentication/Login"; // Đường dẫn đăng nhập
                options.AccessDeniedPath = "/Admin/Authentication/AccessDenied"; // Đường dẫn trang cấm truy cập
                options.SlidingExpiration = true; // Thời gian tồn tại của cookie (Sẽ hiện rõ thời gian tồn tại của cookie)
            });

            service.Configure<IdentityOptions>(options =>
            {
                options.Lockout.AllowedForNewUsers = true; // Cho phép khóa tài khoản mới
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1); // Thời gian khóa tài khoản
                options.Lockout.MaxFailedAccessAttempts = 3; // Số lần thất bại đăng nhập tối đa trước khi tài khoản bị khóa
            });
        }
    }
}
