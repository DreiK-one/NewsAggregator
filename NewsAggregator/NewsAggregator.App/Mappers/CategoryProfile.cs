using AutoMapper;
using NewsAggregator.App.Models;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Data.Entities;

namespace NewsAggregator.App.Mappers
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();

            CreateMap<CategoryDto, CategoryModel>().ReverseMap();

            CreateMap<CategoryDto, DeleteCategoryViewModel>().ReverseMap();

            CreateMap<CategoryModel, CreateOrEditArticleViewModel>();

            CreateMap<Category, CategoryWithArticlesDto>();

            CreateMap<CategoryWithArticlesDto, CategoryWithArticlesViewModel>();   
        }
    }
}
