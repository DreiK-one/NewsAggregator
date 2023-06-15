using AutoMapper;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Data.Entities;


namespace NewsAggregator.WebAPI.Mappers
{
    public class SourceProfile : Profile
    {
        public SourceProfile()
        {
            CreateMap<Source, SourceDto>().ReverseMap();

            CreateMap<Source, RssUrlsFromSourceDto>()
                .ForMember(dest => dest.SourceId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.RssUrl, opt => opt.MapFrom(src => src.RssUrl));
        }
    }
}