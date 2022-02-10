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
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;

        private readonly IUnitOfWork _unitOfWork;

        public UserService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<UserDto>> GetAllUsersWithAllInfoAsync()
        {
            return await _unitOfWork.Users.Get()
                .Include(userRoles => userRoles.UserRoles)
                .ThenInclude(role => role.Role)
                .Include(userActivity => userActivity.UserActivities)
                .Include(comments => comments.Comments)
                .ThenInclude(articles => articles.Article)
                .Select(users => _mapper.Map<UserDto>(users))
                .ToListAsync();
        }

        public async Task<int?> UpdateAsync(UserDto userDto)
        {
            try
            {
                if (userDto != null)
                {
                    await _unitOfWork.Users.Update(_mapper.Map<User>(userDto));
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
                if (await _unitOfWork.Users.GetById(id) != null)
                {
                    await _unitOfWork.Users.Remove(id);
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
