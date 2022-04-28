using AutoMapper;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Data.Entities;
using NewsAggregator.WebAPI.Models.Requests;
using NewsAggregator.WebAPI.Models.Responses;

namespace NewsAggregator.WebAPI.Mappers
{
    public class TokenProfile : Profile
    {
        public TokenProfile()
        {
            CreateMap<AuthenticateRequest, LoginDto>();
            CreateMap<RefreshToken, RefreshTokenDto>();
            CreateMap<RefreshTokenDto, RefreshToken>();
            CreateMap<JwtAuthDto, AuthenticateResponse>();

        }
    }
}
