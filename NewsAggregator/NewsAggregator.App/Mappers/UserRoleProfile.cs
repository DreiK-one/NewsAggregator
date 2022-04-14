using AutoMapper;
using NewsAggregator.App.Models;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Data.Entities;

namespace NewsAggregator.App.Mappers
{
    public class UserRoleProfile : Profile
    {
        public UserRoleProfile()
        {
            CreateMap<UserRole, UserRoleDto>();

            CreateMap<UserRoleDto, UserRoleModel>();

            CreateMap<UserRoleModel, UserViewModel>();
        }
    }
}
