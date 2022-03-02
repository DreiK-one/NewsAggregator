using AutoMapper;
using NewsAggregator.App.Models;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Data;
using NewsAggregator.Data.Entities;

namespace NewsAggregator.App.Mappers
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();

            CreateMap<CategoryDto, CategoryModel>().ReverseMap();

            CreateMap<CategoryModel, CreateArticleViewModel>();

            CreateMap<Category, DeleteCategoryViewModel>();
            
            CreateMap<Category, CategoryWithArticlesDto>();

            CreateMap<CategoryWithArticlesDto, CategoryWithArticlesViewModel>();   
        }
    }
}
