using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Core.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Nickname { get; set; }
        public string NormalizedNickname { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
