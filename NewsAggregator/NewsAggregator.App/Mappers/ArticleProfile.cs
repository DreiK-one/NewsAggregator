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
            CreateMap<Article, ArticleViewModel>()
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Coefficient));

            CreateMap<Article, ArticleDto>();

            CreateMap<ArticleDto, ArticleViewModel>()
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Coefficient));

            CreateMap<ArticleDto, ReadViewModel>();
        }
    }
}
