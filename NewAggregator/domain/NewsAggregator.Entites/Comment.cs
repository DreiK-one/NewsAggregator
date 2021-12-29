using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Entites
{
    internal class Comment
    {
        public int Id { get; set; }

        public string? Text { get; set; }

        public DateTime DateOfCreation { get; set; }

        public Guid UserId { get; set; }

        public virtual User? User { get; set; }

        public Guid NewsId { get; set; }

        public virtual News? News { get; set; }
    }
}
