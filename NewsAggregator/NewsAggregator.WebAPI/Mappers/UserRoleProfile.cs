using AutoMapper;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Data.Entities;

namespace NewsAggregator.WebAPI.Mappers
{
    public class UserRoleProfile : Profile
    {
        public UserRoleProfile()
        {
            CreateMap<UserRole, UserRoleDto>().ReverseMap();
        }
    }
}
