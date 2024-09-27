using System;

namespace Lnk.Application.DTOs;

public class AccountDTO
{
    public string RoleName { get; set; }

    public string Username { get; set; }

    public string Fullname { get; set; }

    public string Password { get; set; }

    public string Email { get; set; }

    public string Phone { get; set; }

    public string MobilePhone { get; set; }

    public string Address { get; set; }

    public bool IsActive { get; set; }

    public Microsoft.AspNetCore.Http.IFormFile Avatar { get; set; }
}
