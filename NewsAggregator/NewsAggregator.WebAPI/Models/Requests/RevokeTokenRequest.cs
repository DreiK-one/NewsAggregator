using System.ComponentModel.DataAnnotations;

namespace NewsAggregator.WebAPI.Models.Requests
{
    public class RevokeTokenRequest
    {
        public string Token { get; set; }
    }
}
