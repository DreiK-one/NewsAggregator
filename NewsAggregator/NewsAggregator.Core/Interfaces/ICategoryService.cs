﻿using NewsAggregator.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Core.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
        Task<int?> CreateAsync(CategoryDto categoryDto);
        Task<int?> UpdateAsync(CategoryDto categoryDto);
        Task<int?> DeleteAsync(Guid id);
        Task<Guid> GetCategoryByUrl(string url);
        Task<CategoryDto> GetCategoryAsync(Guid id);
        Task<CategoryWithArticlesDto> GetCategoryByNameWithArticlesAsync(string name);
        Task<CategoryWithArticlesDto> GetCategoryByNameWithArticlesForAdminAsync(string name);
        Task<CategoryWithArticlesDto> GetCategoryByIdWithArticlesAsync(Guid id);
    }
}
