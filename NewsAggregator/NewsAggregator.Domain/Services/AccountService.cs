using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Interfaces;
using NewsAggregator.Core.Interfaces.Data;
using NewsAggregator.Data;
using NewsAggregator.Data.Entities;
using NewsAggregator.Core.Helpers;
using System.Security.Cryptography;
using System.Text;


namespace NewsAggregator.Domain.Services
{
    public class AccountService : IAccountService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<AccountService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IRoleService _roleService;

        public AccountService(IMapper mapper,
            ILogger<AccountService> logger,
            IUnitOfWork unitOfWork,
            IRoleService roleService, 
            IConfiguration configuration)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _roleService = roleService;
            _configuration = configuration;
        }

        public async Task<bool> CheckUserWithThatEmailIsExistAsync(string email)
        {
            try
            {
                var normalizedEmail = email.ToUpperInvariant();

                return await _unitOfWork.Users.Get().Result
                    .AnyAsync(user =>
                        user.NormalizedEmail
                           .Equals(normalizedEmail));
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public async Task<Guid?> GetUserIdByEmailAsync(string email)
        {
            try
            {
                string normalizedEmail = email.ToUpperInvariant();

                return (await (await _unitOfWork.Users.FindBy(user =>
                    user.NormalizedEmail != null && user.NormalizedEmail
                        .Equals(normalizedEmail))).FirstOrDefaultAsync())?.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                throw;
            }
        }
        public async Task<UserDto> GetUserById(Guid id)
        {
            try
            {
                var user = await _unitOfWork.Users.GetById(id);
                return _mapper.Map<UserDto>(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public async Task<UserDto> GetUserByEmailAsync(string email)
        {
            try
            {
                string normalizedEmail = email.ToUpperInvariant();

                var user = await (await _unitOfWork.Users.FindBy(user =>
                    user.NormalizedEmail != null && user.NormalizedEmail
                        .Equals(normalizedEmail)))
                    .Include(user => user.UserRoles)
                        .ThenInclude(role => role.Role)
                    .FirstOrDefaultAsync();
                return _mapper.Map<UserDto>(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public async Task<Guid> GetUserIdByNicknameAsync(string nickname)
        {
            try
            {
                string normalizedNickname = nickname.ToUpperInvariant();

                return (await (await _unitOfWork.Users.FindBy(user =>
                    user.NormalizedNickname != null && user.NormalizedNickname
                        .Equals(normalizedNickname))).FirstOrDefaultAsync()).Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public async Task<string?> GetUserNicknameByIdAsync(Guid id)
        {
            try
            {
                return (await _unitOfWork.Users.GetById(id)).Nickname;
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public async Task<Guid> CreateUserAsync(string email, string nickname)
        {
            try
            {
                var id = Guid.NewGuid();
                await _unitOfWork.Users.Add(new User
                {
                    Id = id,
                    Email = email,
                    NormalizedEmail = email.ToUpperInvariant(),
                    Nickname = nickname,
                    NormalizedNickname = nickname.ToUpperInvariant(),
                    RegistrationDate = DateTime.Now
                });
                await _unitOfWork.Save();
                return id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }           
        }

        public async Task<int> SetRoleAsync(Guid userId, string roleName)
        {
            try
            {
                var roleId = await _roleService.GetRoleIdByNameAsync(roleName);
                if (roleId == Guid.Empty)
                {
                    roleId = await _roleService.CreateRole(roleName);
                }

                await _unitOfWork.UserRoles.Add(new UserRole
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    RoleId = roleId
                });
                return await _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public async Task<IEnumerable<string>> GetRolesAsync(Guid userId)
        {
            try
            {
                var userRoleIds = (await _unitOfWork.Users
                .GetByIdWithIncludes(userId, user => user.UserRoles))
                .UserRoles.Select(role => role.RoleId);

                var names = new List<string>();
                foreach (var userRoleId in userRoleIds)
                {
                    names.Add(await _roleService.GetRoleNameByIdAsync(userRoleId));
                }

                return names;
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public async Task<int> SetPasswordAsync(Guid userId, string password)
        {
            try
            {
                await _unitOfWork.Users.PatchAsync(userId, new List<PatchModel>
                {
                    new PatchModel()
                    {
                        PropertyName = "PasswordHash",
                        PropertyValue = GetPasswordHash(password, _configuration["ApplicationVariables:Salt"])
                    }
                });
                return await _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public async Task<bool> CheckPasswordByEmailAsync(string email, string password)
        {
            try
            {
                var user = await GetUserByEmailAsync(email);
                if (user.Id != Guid.Empty)
                {
                    if (!string.IsNullOrEmpty(user.PasswordHash))
                    {
                        var enteredPasswordHash = GetPasswordHash(password, _configuration["ApplicationVariables:Salt"]);

                        if (user.PasswordHash.Equals(enteredPasswordHash))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            } 
        }

        public async Task<bool> CheckPasswordByIdAsync(Guid id, string password)
        {
            try
            {
                var userPasswordHash = (await _unitOfWork.Users.GetById(id)).PasswordHash;
                if (!string.IsNullOrEmpty(userPasswordHash))
                {
                    var enteredPasswordHash = GetPasswordHash(password, _configuration["ApplicationVariables:Salt"]);

                    if (userPasswordHash.Equals(enteredPasswordHash))
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        private string GetPasswordHash(string password, string salt)
        {
            try
            {
                var sha1 = new SHA1CryptoServiceProvider();
                var sha1Data = sha1.ComputeHash(Encoding.UTF8.GetBytes($"{salt}_{password}"));
                var hashedPassword = Encoding.UTF8.GetString(sha1Data);
                return hashedPassword;
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public async Task<int> UpdateEmail(Guid userId, string email)
        {
            try
            {
                await _unitOfWork.Users.PatchAsync(userId, new List<PatchModel>
            {
                new PatchModel
                {
                    PropertyName = "Email",
                    PropertyValue = email
                },

                new PatchModel
                {
                    PropertyName = "NormalizedEmail",
                    PropertyValue = email.ToUpperInvariant()
                }
            });
                return await _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public async Task<int> UpdateNickname(Guid userId, string nickname)
        {
            try
            {
                await _unitOfWork.Users.PatchAsync(userId, new List<PatchModel>
            {
                new PatchModel
                {
                    PropertyName = "Nickname",
                    PropertyValue = nickname
                },

                new PatchModel
                {
                    PropertyName = "NormalizedNickname",
                    PropertyValue = nickname.ToUpperInvariant()
                }
            });
                return await _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public bool ValidateIsEmailExists(string email)
        {
            try
            {
                var normalizedEmail = email.ToUpperInvariant();

                return _unitOfWork.Users.Get().Result
                    .Any(user => user.NormalizedEmail.Equals(normalizedEmail));
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public bool ValidateIsNicknameExists(string nickname)
        {
            try
            {
                var normalizedNickname = nickname.ToUpperInvariant();

                return _unitOfWork.Users.Get().Result
                    .Any(user => user.NormalizedNickname.Equals(normalizedNickname));
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }
    }
}
