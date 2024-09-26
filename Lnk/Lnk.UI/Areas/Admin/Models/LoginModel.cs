using System;

namespace Lnk.UI.Areas.Admin.Models;

public class LoginModel
{
    // Username
    public string Username { get; set; }
    // Password
    public string Password { get; set; }
    // RememberMe
    public bool RememberMe { get; set; }
}
