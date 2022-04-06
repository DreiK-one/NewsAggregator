using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Data.Entities
{
    public class User : BaseEntity
    {
        public string Email { get; set; }
        public string? NormalizedEmail { get; set; }
        public string? PasswordHash { get; set; }
        public string Nickname { get; set; }
        public string NormalizedNickname { get; set; }
        public DateTime? RegistrationDate { get; set; }


        public virtual IEnumerable<Comment> Comments { get; set; }
        public virtual IEnumerable<UserActivity> UserActivities { get; set; }
        public virtual IEnumerable<UserRole> UserRoles { get; set; }
    }
}
