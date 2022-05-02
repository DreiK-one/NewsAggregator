using NewsAggregator.Core.DTOs;


namespace NewsAggregator.Core.Interfaces.InterfacesCQS
{
    public interface ICategoryServiceCQS
    {
        Task<CategoryWithArticlesDto> GetCategoryById(Guid id);
        Task<CategoryWithArticlesDto> GetCategoryByName(string name);
        Task<IEnumerable<CategoryWithArticlesDto>> GetAllCategories();
    }
}
