using AutoMapper;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Data.Entities;
using NewsAggregetor.CQS.Models.Commands.ArticleCommands;
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

            CreateMap<CreateOrEditArticleDto, CreateArticleCommand>().ReverseMap();

            CreateMap<CreateOrEditArticleDto, EditArticleCommand>().ReverseMap();
        }
    }
}
