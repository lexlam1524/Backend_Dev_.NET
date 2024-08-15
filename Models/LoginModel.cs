using System;
using System.Collections.Generic;

namespace robot_controller_api.Models;

public partial class LoginModel
{
    public LoginModel() { }

    public LoginModel(string email, string password)
    {
        this.Email = email;
        this.Password = password;
    }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;
}