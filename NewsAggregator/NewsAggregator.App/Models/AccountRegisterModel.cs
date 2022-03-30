﻿using System.ComponentModel.DataAnnotations;

namespace NewsAggregator.App.Models
{
    public class AccountRegisterModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
