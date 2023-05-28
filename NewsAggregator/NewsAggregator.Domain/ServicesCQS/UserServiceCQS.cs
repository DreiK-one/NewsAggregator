using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Helpers;
using NewsAggregator.Core.Interfaces.InterfacesCQS;
using NewsAggregator.Core.InterfacesCQS;
using NewsAggregetor.CQS.Models.Queries.UserQueries;


namespace NewsAggregator.Domain.ServicesCQS
{
    public class UserServiceCQS : IUserServiceCQS
    {
        private readonly IMapper _mapper;
        private readonly ILogger<UserServiceCQS> _logger;
        private readonly IMediator _mediator;
        private readonly IAccountServiceCQS _accountServiceCQS;

        public UserServiceCQS(IMapper mapper,
            ILogger<UserServiceCQS> logger,
            IMediator mediator,
            IAccountServiceCQS accountServiceCQS)
        {
            _mapper = mapper;
            _logger = logger;
            _mediator = mediator;
            _accountServiceCQS = accountServiceCQS;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersWithAllInfoAsync()
        {
            try
            {
                return await _mediator.Send(new GetAllUsersWithAllInfoAsyncQuery(), 
                    new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public async Task<UserDto> GetUserByIdAsync(Guid id)
        {
            try
            {
                return await _mediator.Send(new GetUserByIdAsyncQuery(id), 
                    new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public Task<int?> CreateAsync(CreateOrEditUserDto userDto)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public Task<int?> UpdateAsync(CreateOrEditUserDto userDto)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public Task<int?> DeleteAsync(Guid id)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        } 
    }
}
