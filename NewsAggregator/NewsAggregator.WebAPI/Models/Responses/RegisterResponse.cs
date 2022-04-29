using NewsAggregator.Core.DTOs;
using System.Text.Json.Serialization;

namespace NewsAggregator.WebAPI.Models.Responses
{
    public class RegisterResponse
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Nickname { get; set; }
        public DateTime RegistrationDate { get; set; }
        public List<UserRoleDto> UserRoles { get; set; }
    }
}
