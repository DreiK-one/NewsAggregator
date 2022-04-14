using Microsoft.AspNetCore.Mvc.Rendering;

namespace NewsAggregator.App.Models
{
    public class CreateOrEditUserViewModel : BaseModel
    {
        public string Email { get; set; }
        public string? NormalizedEmail { get; set; }
        public string Nickname { get; set; }
        public string NormalizedNickname { get; set; }
        public DateTime RegistrationDate { get; set; }
        public Guid RoleId { get; set; }

        public List<CommentModel> Comments { get; set; }
        public List<UserRoleModel> UserRoles { get; set; }

        public IEnumerable<SelectListItem> Roles { get; set; }
    }
}
