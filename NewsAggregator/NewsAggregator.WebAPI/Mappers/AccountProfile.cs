using AutoMapper;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Data.Entities;
using NewsAggregator.WebAPI.Models.Requests;
using NewsAggregator.WebAPI.Models.Responses;

namespace NewsAggregator.WebAPI.Mappers
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<AuthenticateRequest, LoginDto>();

            CreateMap<RegisterRequest, RegisterDto>();

            CreateMap<UserDto, RegisterResponse>();


            CreateMap<RefreshToken, RefreshTokenDto>().ReverseMap();

            CreateMap<JwtAuthDto, AuthenticateResponse>();
        }
    }
}
