using NewsAggregator.Core.DTOs;
using System.Text.Json.Serialization;


namespace NewsAggregator.WebAPI.Models.Responses
{
    public class AuthenticateResponse
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public List<UserRoleDto> UserRoles { get; set; }
        public string JwtToken { get; set; }

        [JsonIgnore]
        public string RefreshToken { get; set; }
    }
}
