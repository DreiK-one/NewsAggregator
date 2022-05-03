using System.ComponentModel.DataAnnotations;

namespace NewsAggregator.WebAPI.Models.Requests
{
    public class RegisterRequest
    {
        public string Email { get; set; }
        public string Nickname { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
