﻿using System.ComponentModel.DataAnnotations;

namespace TransactionRecordApp.Models
{
    /// <summary>
    /// View model to pass view username and password to account controller.
    /// </summary>
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Please enter a username.")]
        [StringLength(255)]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Please enter a password.")]
        [StringLength(255)]
        public string? Password { get; set; }

        public string? ReturnUrl { get; set; }

        public bool RememberMe { get; set; }
    }
}