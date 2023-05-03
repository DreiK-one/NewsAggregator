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
            var id = await (await _unitOfWork.Roles
                .FindBy(role => role.Name.Equals(name)))
                .Select(role => role.Id)
                .FirstOrDefaultAsync();
            return id;
        }

        public async Task<string> GetRoleNameByIdAsync(Guid id)
        {
            return (await _unitOfWork.Roles.GetById(id)).Name;
        }

        public async Task<Guid> CreateRole(string name)
        {
            var id = Guid.NewGuid();
            await _unitOfWork.Roles.Add(new Role()
            {
                Id = id,
                Name = name
            });
            await _unitOfWork.Save();
            return id;
        }

        public async Task<int?> ChangeUserRole(UserRoleDto dto)
        {
            try
            {
                if (dto != null)
                {
                    var userRoleId = FindUserRoleIdByUserId(dto.UserId);
                    dto.Id = userRoleId;

                    await _unitOfWork.UserRoles.PatchAsync(dto.Id, new List<PatchModel>
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

        public Guid FindUserRoleIdByUserId(Guid id)
        {
            try
            {
                return (_unitOfWork.UserRoles.Get().Result
                          .Where(userId => userId.UserId.Equals(id))
                          .FirstOrDefault()).Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }
    }
}
