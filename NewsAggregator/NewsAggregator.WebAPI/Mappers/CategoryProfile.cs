using AutoMapper;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Data.Entities;

namespace NewsAggregator.WebAPI.Mappers
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();

            CreateMap<Category, CategoryWithArticlesDto>();
        }
    }
}
