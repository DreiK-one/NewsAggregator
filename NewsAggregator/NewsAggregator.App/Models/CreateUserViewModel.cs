using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace NewsAggregator.App.Models
{
    public class CreateUserViewModel : BaseModel
    {
        public string Email { get; set; }
        public string Nickname { get; set; }
        public DateTime RegistrationDate { get; set; }
        public Guid RoleId { get; set; }

        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public IEnumerable<SelectListItem> Roles { get; set; }
    }
}
