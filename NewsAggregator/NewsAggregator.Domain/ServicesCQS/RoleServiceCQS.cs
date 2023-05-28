using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Helpers;
using NewsAggregator.Core.Interfaces.InterfacesCQS;
using NewsAggregetor.CQS.Models.Queries.RoleQueries;


namespace NewsAggregator.Domain.ServicesCQS
{
    public class RoleServiceCQS : IRoleServiceCQS
    {
        private readonly IMapper _mapper;
        private readonly ILogger<RoleServiceCQS> _logger;
        private readonly IMediator _mediator;
        private readonly IAccountServiceCQS _accountServiceCQS;

        public RoleServiceCQS(IMapper mapper,
            ILogger<RoleServiceCQS> logger,
            IMediator mediator,
            IAccountServiceCQS accountServiceCQS)
        {
            _mapper = mapper;
            _logger = logger;
            _mediator = mediator;
            _accountServiceCQS = accountServiceCQS;
        }

        public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
        {
            try
            {
                return await _mediator.Send(new GetAllRolesAsyncQuery(), 
                    new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public async Task<RoleDto> GetRoleAsync(Guid id)
        {
            try
            {
                return await _mediator.Send(new GetRoleAsyncQuery(id), 
                    new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public async Task<Guid> GetRoleIdByNameAsync(string name)
        {
            try
            {
                return await _mediator.Send(new GetRoleIdByNameAsyncQuery(name), 
                    new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public async Task<Guid> GetRoleIdByUserIdAsync(Guid id)
        {
            try
            {
                return await _mediator.Send(new GetRoleIdByUserIdAsyncQuery(id), 
                    new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public async Task<string> GetRoleNameByIdAsync(Guid id)
        {
            try
            {
                return await _mediator.Send(new GetRoleNameByIdAsyncQuery(id), 
                    new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public Task<int?> CreateAsync(RoleDto roleDto)
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

        public Task<Guid> CreateRole(string name)
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

        public Task<int?> ChangeUserRole(UserRoleDto dto)
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

        public Task<int?> UpdateAsync(RoleDto roleDto)
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
