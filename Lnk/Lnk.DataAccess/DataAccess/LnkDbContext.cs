using Lnk.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Lnk.DataAccess.DataAccess
{
    public class LnkDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public LnkDbContext(DbContextOptions<LnkDbContext> options) : base(options) { }
    }
}