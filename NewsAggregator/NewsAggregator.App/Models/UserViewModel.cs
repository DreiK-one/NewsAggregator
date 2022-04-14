using NewsAggregator.Core.DTOs;
using NewsAggregator.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.App.Models
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Nickname { get; set; }
        public DateTime RegistrationDate { get; set; }

        public List<CommentModel> Comments { get; set; }

        public List<UserRoleModel> UserRoles { get; set; }
    }
}