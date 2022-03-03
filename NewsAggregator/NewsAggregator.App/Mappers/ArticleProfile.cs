using AutoMapper;
using NewsAggregator.App.Models;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Data.Entities;
using System.ServiceModel.Syndication;

namespace NewsAggregator.App.Mappers
{
    public class ArticleProfile : Profile
    {
        public ArticleProfile()
        {
            CreateMap<Article, ArticleDto>()
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments))
                .ForMember(dest => dest.SourceName, opt => opt.MapFrom(src => src.Source))
                .ReverseMap();

            CreateMap<ArticleDto, CreateOrEditArticleViewModel>();

            CreateMap<ArticleDto, DeleteArticleViewModel>();

            CreateMap<ArticleDto, ReadArticleViewModel>();

            CreateMap<ArticleDto, AllNewsOnHomeScreenViewModel>();

            CreateMap<SyndicationItem, RssArticleDto>()
                .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title.Text))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Summary.Text));

            CreateMap<NewArticleDto, Article>();

            CreateMap<ArticleDto, ArticleModel>();

            CreateMap<ArticleModel, CategoryWithArticlesViewModel>();

            CreateMap<CreateOrEditArticleViewModel, CreateOrEditArticleDto>()
                .ForPath(dest => dest.SourceId, opt => opt.MapFrom(src => src.Source.Id))
                .ForPath(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Category.Id))
                .ReverseMap();

            CreateMap<Article, CreateOrEditArticleDto>().ReverseMap();



            CreateMap<AllNewsOnHomeScreenViewModel, CreateOrEditArticleViewModel>()
                .ForPath(dest => dest.Source.Id, opt => opt.MapFrom(src => src.SourceId))
                .ForPath(dest => dest.Category.Id, opt => opt.MapFrom(src => src.CategoryId)); ;
        }
    }
}
