using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Interfaces;
using NewsAggregator.Core.Interfaces.Data;
using NewsAggregator.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Domain.Services
{
    public class RoleService : IRoleService
    {
        private readonly IMapper _mapper;

        private readonly IUnitOfWork _unitOfWork;

        public RoleService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
        {
            return await _unitOfWork.Roles.Get()
                .Select(role => _mapper.Map<RoleDto>(role))
                .ToListAsync();
        }

        public async Task<int?> CreateAsync(RoleDto roleDto)
        {
            try
            {
                if (roleDto != null)
                {
                    var existRole = (await _unitOfWork.Roles.FindBy(r => r.Name.ToLower() == roleDto.Name.ToLower()));

                    if (!existRole.Any())
                    {
                        await _unitOfWork.Roles.Add(_mapper.Map<Role>(roleDto));
                        return await _unitOfWork.Save();
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                //add log
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
                    return null;
                }
            }
            catch (Exception)
            {
                //add log
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
                    return null;
                }
            }
            catch (Exception)
            {
                //add log
                throw;
            }
        }
    }
}
