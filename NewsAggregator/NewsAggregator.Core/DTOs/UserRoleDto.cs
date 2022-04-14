using NewsAggregator.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Core.DTOs
{
    public class UserRoleDto
    {
        public Guid UserId { get; set; }
        public RoleDto Role{ get; set; }
    }
}
