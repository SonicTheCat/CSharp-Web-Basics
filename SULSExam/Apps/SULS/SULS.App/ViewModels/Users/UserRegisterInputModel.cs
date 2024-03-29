﻿using SIS.MvcFramework.Attributes.Validation;

namespace SULS.App.ViewModels.Users
{
    public class UserRegisterInputModel
    {
        private const string UsernameErrorMessage = "Invalid username length! Must be between 5 and 20 symbols!";
        private const string PasswordErrorMessage = "Invalid password length!";

        [RequiredSis]
        [StringLengthSis(5, 20, UsernameErrorMessage)]
        public string Username { get; set; }

        [RequiredSis]
        [StringLengthSis(6, 20, UsernameErrorMessage)]
        public string Password { get; set; }

        [RequiredSis]
        public string ConfirmPassword { get; set; }

        [RequiredSis]
        [EmailSis]
        public string Email { get; set; }
    }
}