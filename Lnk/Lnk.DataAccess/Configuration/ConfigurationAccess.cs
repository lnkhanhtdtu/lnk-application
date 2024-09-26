using Lnk.DataAccess.DataAccess;
using Lnk.Domain.Entities;
using Microsoft.AspNetCore.Identity;
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
        public static void RegisterDb(this IServiceCollection service, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("DefaultConnection")
                                   ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            service.AddDbContext<LnkDbContext>(options => options.UseSqlServer(connectionString));

            service.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<LnkDbContext>();
        }
    }
}
