using Microsoft.AspNetCore.Identity;

namespace Lnk.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public bool IsActive { get; set; }
    }
}
