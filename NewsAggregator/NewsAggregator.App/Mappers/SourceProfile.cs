﻿using AutoMapper;
using NewsAggregator.App.Models;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Data.Entities;

namespace NewsAggregator.App.Mappers
{
    public class SourceProfile : Profile
    {
        public SourceProfile()
        {
            CreateMap<Source, SourceDto>().ReverseMap();

            CreateMap<SourceDto, SourceModel>().ReverseMap();

            CreateMap<SourceDto, DeleteSourceViewModel>().ReverseMap();

            CreateMap<SourceModel, CreateOrEditArticleViewModel>();

            CreateMap<SourceModel, ReadArticleViewModel>();

            CreateMap<Source, RssUrlsFromSourceDto>()
                .ForMember(dest => dest.SourceId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.RssUrl, opt => opt.MapFrom(src => src.RssUrl));
        }
    }
}