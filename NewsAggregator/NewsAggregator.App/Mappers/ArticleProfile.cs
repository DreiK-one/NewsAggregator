using AutoMapper;
using NewsAggregator.App.Models;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Data.Entities;

namespace NewsAggregator.App.Mappers
{
    public class ArticleProfile : Profile
    {
        public ArticleProfile()
        {
            CreateMap<Article, ArticleDto>()
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments))
                .ForMember(dest => dest.SourceName, opt => opt.MapFrom(src => src.Source));

            CreateMap<ArticleDto, ReadArticleViewModel>();

            CreateMap<ArticleDto, AllNewsOnHomeScreenViewModel>();
        }
    }
}
