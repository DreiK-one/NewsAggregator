﻿using AutoMapper;
using NewsAggregator.App.Models;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Data.Entities;

namespace NewsAggregator.App.Mappers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>();

            CreateMap<UserDto, UserModel>();

        }
    }
}