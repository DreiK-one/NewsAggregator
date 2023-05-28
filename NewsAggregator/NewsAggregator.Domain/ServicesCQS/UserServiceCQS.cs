using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Helpers;
using NewsAggregator.Core.Interfaces.InterfacesCQS;
using NewsAggregator.Core.InterfacesCQS;
using NewsAggregetor.CQS.Models.Commands.UserCommands;
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

        public async Task<int?> CreateAsync(CreateOrEditUserDto userDto)
        {
            try
            {
                if (userDto != null)
                {
                    var userRoleCommand = new CreateUserRoleCommand
                    {
                        Id = Guid.NewGuid(),
                        UserId = userDto.Id,
                        RoleId = userDto.RoleId
                    };

                    await _mediator.Send(userRoleCommand, new CancellationToken());

                    var userCommand = new UpdateUserCommand
                    {
                        Id = userDto.Id,
                        Email = userDto.Email,
                        NormalizedEmail = userDto.Email.ToUpperInvariant(),
                        Nickname = userDto.Nickname,
                        NormalizedNickname = userDto.Nickname.ToUpperInvariant(),
                        RegistrationDate = userDto.RegistrationDate,
                    };

                    await _mediator.Send(userCommand, new CancellationToken());

                    return await _accountServiceCQS.SetPasswordAsync(userDto.Id, userDto.PasswordHash);
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

        public async Task<int?> UpdateAsync(CreateOrEditUserDto userDto)
        {
            try
            {
                if (userDto != null)
                {
                    var command = _mapper.Map<UpdateUserCommand>(userDto);

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
                if (GetUserByIdAsync(id) != null)
                {
                    return await _mediator.Send(new DeleteUserCommand(id), 
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
