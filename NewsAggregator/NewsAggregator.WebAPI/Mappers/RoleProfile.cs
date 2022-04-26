using AutoMapper;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Data.Entities;

namespace NewsAggregator.WebAPI.Mappers
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<Role, RoleDto>().ReverseMap();

            CreateMap<RoleDto, UserRoleDto>();
        }
    }
}
