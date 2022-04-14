using AutoMapper;
using NewsAggregator.App.Models;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Data.Entities;

namespace NewsAggregator.App.Mappers
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<Role, RoleDto>().ReverseMap();

            CreateMap<RoleDto, UserRoleDto>();

            CreateMap<RoleDto, RoleModel>().ReverseMap();

            CreateMap<RoleModel, UserRoleModel>();

            CreateMap<RoleDto, DeleteRoleViewModel>().ReverseMap();
        }
    }
}
