using Microsoft.AspNetCore.Mvc.Rendering;


namespace NewsAggregator.App.Models
{
    public class EditUserViewModel : BaseModel
    {
        public string Email { get; set; }
        public string Nickname { get; set; }
        public DateTime RegistrationDate { get; set; }
        public Guid RoleId { get; set; }

        public IEnumerable<SelectListItem>? Roles { get; set; }
    }
}
