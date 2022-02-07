using AutoMapper;
using NewsAggregator.App.Models;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Data.Entities;

namespace NewsAggregator.App.Mappers
{
    public class SourceProfile : Profile
    {
        public SourceProfile()
        {
            CreateMap<Source, SourceDto>();

            CreateMap<SourceDto, SourceModel>();

            CreateMap<SourceModel, ReadArticleViewModel>();
        }
    }
}