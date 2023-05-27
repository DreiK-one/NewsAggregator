using NewsAggregator.Core.DTOs;


namespace NewsAggregator.Core.Interfaces.InterfacesCQS
{
    public interface ICategoryServiceCQS
    {
        Task<IEnumerable<CategoryWithArticlesDto>> GetAllCategories();
        Task<CategoryDto> GetCategoryAsync(Guid id);
        Task<Guid> GetCategoryByUrlAsync(string url);
        Task<CategoryWithArticlesDto> GetCategoryByIdWithArticlesAsync(Guid id);
        Task<CategoryWithArticlesDto> GetCategoryByNameWithArticlesAsync(string name);
        Task<CategoryWithArticlesDto> GetCategoryByNameWithArticlesForAdminAsync(string name);
        Task<int?> CreateAsync(CategoryDto categoryDto);
        Task<int?> UpdateAsync(CategoryDto categoryDto);
        Task<int?> DeleteAsync(Guid id);
    }
}
