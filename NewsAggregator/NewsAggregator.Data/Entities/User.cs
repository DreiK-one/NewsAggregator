using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Entities
{
    public class User
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public DateTime RegistrationDate { get; set; }

        
        public virtual IEnumerable<Comment> Comments { get; set; }

        public virtual IEnumerable<UserActivity> UserActivities { get; set; }

        public virtual IEnumerable<UserRole> UserRoles { get; set; }
    }
}
