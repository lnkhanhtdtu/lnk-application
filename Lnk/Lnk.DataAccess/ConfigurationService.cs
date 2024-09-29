using Lnk.Application.Abstracts;
using Lnk.DataAccess.DataAccess;
using Lnk.Domain.Entities;
using Lnk.Application.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;

namespace Lnk.DataAccess
{
    public static class ConfigurationService
    {
        /// <summary>
        /// Tự động update database
        /// </summary>
        /// <param name="webApplication"></param>
        public static void AutoMigration(this WebApplication webApplication)
        {
            using var scope = webApplication.Services.CreateScope();
            var appContext = scope.ServiceProvider.GetRequiredService<LnkDbContext>();
            appContext.Database.MigrateAsync().Wait();
        }

        /// <summary>
        /// Tạo data mẫu
        /// </summary>
        /// <param name="webApplication"></param>
        /// <returns></returns>
        public static async Task SeedData(this WebApplication webApplication)
        {
            using var scope = webApplication.Services.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Tạo role
            var isAdminRoleExist = await roleManager.RoleExistsAsync("Admin");

            if (!isAdminRoleExist)
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            // Tạo user
            var adminUser = new ApplicationUser
            {
                UserName = "admin",
                FullName = "Lê Nhựt Khánh",
                IsActive = true,
                AccessFailedCount = 0
            };

            var identityResult = await userManager.CreateAsync(adminUser, "Admin@123");
            if (identityResult.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }

        public static void AddDependencyInjection(this IServiceCollection services)

        {
            services.AddTransient<PasswordHasher<ApplicationUser>>();
            services.AddTransient<IUserServices, UserServices>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IAuthenticationService, AuthenticationService>();
        }

        public static void AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}
