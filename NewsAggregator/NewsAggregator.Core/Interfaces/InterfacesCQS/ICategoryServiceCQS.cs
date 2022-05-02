using NewsAggregator.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Core.Interfaces.InterfacesCQS
{
    public interface ICategoryServiceCQS
    {
        Task<CategoryWithArticlesDto> GetCategoryById(Guid id);
        Task<CategoryWithArticlesDto> GetCategoryByName(string name);
        Task<IEnumerable<CategoryWithArticlesDto>> GetAllCategories();
    }
}
