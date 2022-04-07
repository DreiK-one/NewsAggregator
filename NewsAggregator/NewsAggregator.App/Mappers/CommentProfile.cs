using AutoMapper;
using NewsAggregator.App.Models;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Data.Entities;

namespace NewsAggregator.App.Mappers
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateMap<Comment, CommentDto>().ReverseMap();

            CreateMap<CommentDto, CommentViewModel>().ReverseMap();

            CreateMap<CommentViewModel, ReadArticleViewModel>();

            CreateMap<CommentViewModel, UserViewModel>();


            CreateMap<Comment, CreateOrEditCommentDto>().ReverseMap();
            CreateMap<CreateOrEditCommentDto, CommentModel>().ReverseMap();
        }
    }
}
