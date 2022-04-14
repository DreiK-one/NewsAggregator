using AutoMapper;
using NewsAggregator.App.Models;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Data.Entities;

namespace NewsAggregator.App.Mappers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();

            CreateMap<UserDto, UserModel>().ReverseMap();

            CreateMap<UserDto, UserViewModel>();

            CreateMap<UserDto, EditUserViewModel>();

            CreateMap<CreateOrEditUserDto, User>();

            CreateMap<DeleteUserViewModel, UserDto>().ReverseMap();

            CreateMap<EditUserViewModel, CreateOrEditUserDto>();

            CreateMap<CreateUserViewModel, CreateOrEditUserDto>();
        }
    }
}