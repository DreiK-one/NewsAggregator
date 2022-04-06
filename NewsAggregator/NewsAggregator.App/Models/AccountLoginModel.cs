using System.ComponentModel.DataAnnotations;

namespace NewsAggregator.App.Models
{
    public class AccountLoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
