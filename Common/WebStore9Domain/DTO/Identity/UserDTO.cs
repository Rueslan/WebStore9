﻿using Microsoft.AspNetCore.Identity;
using WebStore9Domain.Entities.Identity;

namespace WebStore9Domain.DTO.Identity
{
    public abstract class UserDTO
    {
        public User User { get; set; }
    }

    public class AddLoginDTO : UserDTO
    {
        public UserLoginInfo UserLoginInfo { get; set; }
    }

    public class PasswordHashDTO : UserDTO
    {
        public string Hash { get; set; }
    }

    public class SetLockoutDTO : UserDTO
    {
        public DateTimeOffset? LockoutEnd { get; set; }
    }

}
