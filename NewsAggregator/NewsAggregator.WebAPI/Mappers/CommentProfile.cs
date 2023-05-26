using AutoMapper;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Data.Entities;
using NewsAggregator.WebAPI.Models.Requests;
using NewsAggregetor.CQS.Models.Commands.CommentCommands;

namespace NewsAggregator.WebAPI.Mappers
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateMap<Comment, CommentDto>().ReverseMap();

            CreateMap<CommentDto, UserDto>().ReverseMap();

            CreateMap<Comment, CreateOrEditCommentDto>()
                .ReverseMap();

            CreateMap<CreateOrEditCommentDto, CreateCommentRequest>()
                .ReverseMap();

            CreateMap<CreateOrEditCommentDto, EditCommentRequest>()
                .ReverseMap();

            CreateMap<CreateArticleCommand, Comment>();

            CreateMap<EditCommentCommand, Comment>();

            CreateMap<CreateOrEditCommentDto, CreateArticleCommand>();

            CreateMap<CreateOrEditCommentDto, EditCommentCommand>();
        }
    }
}
