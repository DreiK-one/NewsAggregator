﻿using AutoMapper;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Data.Entities;
using NewsAggregetor.CQS.Models.Commands.AccountCommands;

namespace NewsAggregator.WebAPI.Mappers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();

            CreateMap<CreateOrEditUserDto, User>();

            CreateMap<CreateUserCommand, User>();
        }
    }
}