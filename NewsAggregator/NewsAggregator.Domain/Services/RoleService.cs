using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Helpers;
using NewsAggregator.Core.Interfaces;
using NewsAggregator.Core.Interfaces.Data;
using NewsAggregator.Data;
using NewsAggregator.Data.Entities;


namespace NewsAggregator.Domain.Services
{
    public class RoleService : IRoleService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<RoleService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public RoleService(IMapper mapper, 
            ILogger<RoleService> logger, 
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
            
        }

        public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
        {
            try
            {
                return await _unitOfWork.Roles.Get().Result
                .Select(role => _mapper.Map<RoleDto>(role))
                .ToListAsync();
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
                return await _unitOfWork.UserRoles.Get().Result
                    .Where(userId => userId.UserId.Equals(id))
                    .Select(roleId => roleId.RoleId)
                    .FirstOrDefaultAsync();
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
                    var existRole = await _unitOfWork.Roles.Get().Result
                        .FirstOrDefaultAsync(r => r.Name.ToLower() == roleDto.Name.ToLower());

                    if (existRole == null)
                    {
                        await _unitOfWork.Roles.Add(_mapper.Map<Role>(roleDto));
                        return await _unitOfWork.Save();
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

        public async Task<int?> UpdateAsync(RoleDto roleDto)
        {
            try
            {
                if (roleDto != null)
                {
                    await _unitOfWork.Roles.Update(_mapper.Map<Role>(roleDto));
                    return await _unitOfWork.Save();
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
                if (await _unitOfWork.Roles.GetById(id) != null)
                {
                    await _unitOfWork.Roles.Remove(id);
                    return await _unitOfWork.Save();
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

        public async Task<RoleDto> GetRoleAsync(Guid id)
        {
            try
            {
                var role = await _unitOfWork.Roles.GetById(id);
                return _mapper.Map<RoleDto>(role);
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
                var role = await _unitOfWork.Roles.Get().Result
                    .FirstOrDefaultAsync(role => role.Name.Equals(name));

                return role.Id;
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
                var role = await _unitOfWork.Roles.GetById(id);

                return role.Name;
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

                var id = Guid.NewGuid();
                var role = new Role()
                {
                    Id = id,
                    Name = name
                };

                await _unitOfWork.Roles.Add(role);
                await _unitOfWork.Save();

                return id;
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
                    var userRole = await _unitOfWork.UserRoles.Get().Result
                        .FirstOrDefaultAsync(u => u.UserId == dto.UserId);

                    if (userRole == null)
                    {
                        return null;
                    }

                    await _unitOfWork.UserRoles.PatchAsync(userRole.UserId, new List<PatchModel>
                    {
                        new PatchModel
                        {
                            PropertyName = Variables.RoleFields.RoleId,
                            PropertyValue = dto.RoleId
                        }
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
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }
    }
}
