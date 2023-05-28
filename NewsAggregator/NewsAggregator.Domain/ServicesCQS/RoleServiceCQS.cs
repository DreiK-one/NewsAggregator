using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Helpers;
using NewsAggregator.Core.Interfaces.InterfacesCQS;
using NewsAggregetor.CQS.Models.Commands.RoleCommands;
using NewsAggregetor.CQS.Models.Commands.UserRoleCommands;
using NewsAggregetor.CQS.Models.Queries.RoleQueries;
using NewsAggregetor.CQS.Models.Queries.UserRoleQueries;


namespace NewsAggregator.Domain.ServicesCQS
{
    public class RoleServiceCQS : IRoleServiceCQS
    {
        private readonly IMapper _mapper;
        private readonly ILogger<RoleServiceCQS> _logger;
        private readonly IMediator _mediator;

        public RoleServiceCQS(IMapper mapper,
            ILogger<RoleServiceCQS> logger,
            IMediator mediator)
        {
            _mapper = mapper;
            _logger = logger;
            _mediator = mediator;
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

        public async Task<int?> CreateAsync(RoleDto roleDto)
        {
            try
            {
                if (roleDto != null)
                {
                    var existRole = GetAllRolesAsync().Result
                        .Select(c => c.Name.ToLower() == roleDto.Name.ToLower());

                    if (!existRole.Any())
                    {
                        var command = _mapper.Map<CreateRoleAsyncCommand>(roleDto);

                        return await _mediator.Send(command,
                            new CancellationToken());
                    }
                    else
                    {
                        throw new NullReferenceException();
                    }
                }
                else
                {
                    throw new NullReferenceException();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public async Task<Guid> CreateRole(string name)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new NullReferenceException();
                }

                return await _mediator.Send(new CreateRoleCommand(name), 
                    new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public async Task<int?> ChangeUserRole(UserRoleDto dto)
        {
            try
            {
                if (dto != null)
                {
                    var userRole = await _mediator.Send(new GetUserRoleByUserIdQuery(dto.UserId), 
                        new CancellationToken());

                    if (userRole == null)
                    {
                        return null;
                    }

                    var command = _mapper.Map<UpdateUserRoleCommand>(dto);

                    return await _mediator.Send(command, 
                        new CancellationToken());
                }
                else
                {
                    throw new NullReferenceException();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public async Task<int?> UpdateAsync(RoleDto roleDto)
        {
            try
            {
                if (roleDto != null)
                {
                    var command = _mapper.Map<UpdateRoleCommand>(roleDto);

                    return await _mediator.Send(command,
                        new CancellationToken());
                }
                else
                {
                    throw new NullReferenceException();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public async Task<int?> DeleteAsync(Guid id)
        {
            try
            {
                if (GetRoleAsync(id) != null)
                {
                    return await _mediator.Send(new DeleteRoleCommand(id),
                        new CancellationToken());
                }
                else
                {
                    throw new NullReferenceException();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }      
    }
}
