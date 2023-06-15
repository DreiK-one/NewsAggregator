using AutoMapper;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Data.Entities;
using NewsAggregetor.CQS.Models.Commands.AccountCommands;


namespace NewsAggregator.WebAPI.Mappers
{
    public class UserRoleProfile : Profile
    {
        public UserRoleProfile()
        {
            CreateMap<UserRole, UserRoleDto>().ReverseMap();

            CreateMap<CreateRoleCommand, UserRole>();
        }
    }
}
