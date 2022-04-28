using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQS.Models.Commands
{
    public class RateArticleCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public float Rate { get; set; }
    }
}
