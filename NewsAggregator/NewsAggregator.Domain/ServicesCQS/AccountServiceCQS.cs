using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Interfaces.InterfacesCQS;

namespace NewsAggregator.Domain.ServicesCQS
{
    public class AccountServiceCQS : IAccountServiceCQS
    {
        private readonly IMapper _mapper;
        private readonly ILogger<CommentServiceCQS> _logger;
        private readonly IMediator _mediator;

        public AccountServiceCQS(IMapper mapper, 
            ILogger<CommentServiceCQS> logger, 
            IMediator mediator)
        {
            _mapper = mapper;
            _logger = logger;
            _mediator = mediator;
        }
    }
}
