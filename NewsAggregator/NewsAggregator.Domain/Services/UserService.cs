using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Interfaces;
using NewsAggregator.Core.Interfaces.Data;
using NewsAggregator.Data;
using NewsAggregator.Data.Entities;


namespace NewsAggregator.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccountService _accountService;

        public UserService(IMapper mapper,
            ILogger<UserService> logger,
            IUnitOfWork unitOfWork, 
            IAccountService accountService)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _accountService = accountService;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersWithAllInfoAsync()
        {
            try
            {
                return await _unitOfWork.Users.Get()
                .Include(userRoles => userRoles.UserRoles)
                .ThenInclude(role => role.Role)
                .Include(comments => comments.Comments)
                .ThenInclude(articles => articles.Article)
                .Select(users => _mapper.Map<UserDto>(users))
                .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<UserDto> GetUserByIdAsync(Guid id)
        {
            try
            {
                var user = await _unitOfWork.Users.Get()
                .Where(u => u.Id.Equals(id))
                .FirstOrDefaultAsync();

                return _mapper.Map<UserDto>(user);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<int?> UpdateAsync(CreateOrEditUserDto userDto)
        {
            try
            {
                if (userDto != null)
                {
                    await _unitOfWork.Users.PatchAsync(userDto.Id, new List<PatchModel>
                    {
                        new PatchModel
                        {
                            PropertyName = "Email",
                            PropertyValue = userDto.Email
                        },
                        new PatchModel
                        {
                            PropertyName = "NormalizedEmail",
                            PropertyValue = userDto.Email.ToUpperInvariant()
                        },
                        new PatchModel
                        {
                            PropertyName = "Nickname",
                            PropertyValue = userDto.Nickname
                        },
                        new PatchModel
                        {
                            PropertyName = "NormalizedNickname",
                            PropertyValue = userDto.Nickname.ToUpperInvariant()
                        },
                    });
                    return await _unitOfWork.Save();
                }
                else
                {
                    throw new NullReferenceException();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<int?> DeleteAsync(Guid id)
        {
            try
            {
                if (await _unitOfWork.Users.GetById(id) != null)
                {
                    await _unitOfWork.Users.Remove(id);
                    return await _unitOfWork.Save();
                }
                else
                {
                    throw new NullReferenceException();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<int?> CreateAsync(CreateOrEditUserDto userDto)
        {
            try
            {
                if (userDto != null)
                {
                    await _unitOfWork.UserRoles.Add(new UserRole
                    {
                        Id = Guid.NewGuid(),
                        UserId = userDto.Id,
                        RoleId = userDto.RoleId
                    });

                    await _unitOfWork.Users.Add(new User
                    {
                        Id = userDto.Id,
                        Email = userDto.Email,
                        NormalizedEmail = userDto.Email.ToUpperInvariant(),
                        Nickname = userDto.Nickname,
                        NormalizedNickname = userDto.Nickname.ToUpperInvariant(),
                        RegistrationDate = userDto.RegistrationDate,
                    });
                    await _unitOfWork.Save();

                   return await _accountService.SetPasswordAsync(userDto.Id, userDto.PasswordHash);
                }
                else
                {
                    throw new NullReferenceException();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                throw;
            }
        }
    }
}
