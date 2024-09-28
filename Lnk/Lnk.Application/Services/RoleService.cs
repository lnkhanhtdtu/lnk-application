using Lnk.Application.Abstracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Lnk.Application.Services;

public class RoleService : IRoleService
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public RoleService(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<IEnumerable<SelectListItem>> GetRoleForDropdownList()
    {
        var roles = await _roleManager.Roles.ToListAsync();
        return roles.Select(x => new SelectListItem { Value = x.Name, Text = x.Name });
    }
}