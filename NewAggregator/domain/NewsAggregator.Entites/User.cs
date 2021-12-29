using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Entites
{
    internal class User
    {
        public Guid Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        [Required]
        public string? Login { get; set; }

        [Required]
        public string? PasswordHash { get; set; }

        [Required]
        public string? Email { get; set; }

        public int RoleId { get; set; }

        public virtual Role? Role { get; set; }

        public virtual IEnumerable<Comment>? Comments { get; set; }

        public virtual IEnumerable<UserActivity>? UserActivities { get; set; }
    }
}
