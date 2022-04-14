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
            CreateMap<UserRole, UserRoleDto>().ReverseMap();

            CreateMap<UserRoleDto, UserRoleModel>().ReverseMap();

            CreateMap<UserRoleModel, UserViewModel>().ReverseMap();

            CreateMap<EditUserViewModel, UserRoleDto>()
                .ForPath(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));
        }
    }
}
