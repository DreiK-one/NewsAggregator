using System.ComponentModel.DataAnnotations;

namespace NewsAggregator.App.Models
{
    public class AccountRegisterModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string? ReturnUrl { get; set; }
    }
}
