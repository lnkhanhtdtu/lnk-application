using Microsoft.AspNetCore.Mvc.Rendering;

namespace Lnk.Application.Abstracts;
public interface IRoleService
{
    Task<IEnumerable<SelectListItem>> GetRoleForDropdownList();
}