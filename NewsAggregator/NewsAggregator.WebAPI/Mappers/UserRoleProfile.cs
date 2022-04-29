using AutoMapper;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Data.Entities;
using NewsAggregator.WebAPI.Models.Responses;

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
