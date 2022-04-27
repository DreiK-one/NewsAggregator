using AutoMapper;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Data.Entities;
using NewsAggregator.WebAPI.Models.Requests;
using System.ServiceModel.Syndication;

namespace NewsAggregator.WebAPI.Mappers
{
    public class ArticleProfile : Profile
    {
        public ArticleProfile()
        {
            CreateMap<Article, ArticleDto>()
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments))
                .ForMember(dest => dest.SourceName, opt => opt.MapFrom(src => src.Source))
                .ReverseMap();

            CreateMap<ArticleDto, CommentDto>();

            CreateMap<SyndicationItem, RssArticleDto>()
                .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title.Text))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Summary.Text));

            CreateMap<NewArticleDto, Article>();

            CreateMap<Article, CreateOrEditArticleDto>().ReverseMap();

            CreateMap<GetArticlesRequest, RequestArticleDto>();

            CreateMap<RequestArticleDto, ArticleDto>()
                .ForMember(dest => dest.Coefficient, opt => opt.MapFrom(src => src.Rating))
                .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => src.EndDate));
        }
    }
}
