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

            CreateMap<CommentDto, CommentModel>().ReverseMap();

            CreateMap<CommentModel, ReadArticleViewModel>();

            CreateMap<CommentModel, ArticleEditViewModel>();

            CreateMap<CommentModel, UserViewModel>();
        }
    }
}
