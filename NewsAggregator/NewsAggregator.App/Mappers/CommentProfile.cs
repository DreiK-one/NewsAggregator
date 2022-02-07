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

            CreateMap<CommentModel, CommentDto>().ReverseMap();

            CreateMap<CommentModel, ReadArticleViewModel>();

        }
    }
}
