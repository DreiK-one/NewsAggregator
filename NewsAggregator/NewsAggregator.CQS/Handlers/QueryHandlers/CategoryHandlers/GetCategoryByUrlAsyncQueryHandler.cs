using NewsAggregetor.CQS.Models.Queries.CategoryQueries;
using MediatR;
using NewsAggregator.Data;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Core.Helpers;


namespace NewsAggregetor.CQS.Handlers.QueryHandlers.CategoryHandlers
{
    public class GetCategoryByUrlAsyncQueryHandler : IRequestHandler<GetCategoryByUrlAsyncQuery, Guid>
    {
        private readonly NewsAggregatorContext _database;

        public GetCategoryByUrlAsyncQueryHandler(NewsAggregatorContext database)
        {
            _database = database;
        }

        public async Task<Guid> Handle(GetCategoryByUrlAsyncQuery query, CancellationToken token)
        {
            var str = query.Url.Substring(8);
            var str2 = str.Remove(str.IndexOf('.'), str.Length - str.IndexOf('.'));
            var res = str2.Substring(0, 1).ToUpper() + (str2.Length > 1 ? str2.Substring(1) : "");

            var categories = await _database.Categories
                .Select(category => category.Name).ToListAsync();

            if (categories.Contains(res))
            {
                return (await _database.Categories
                .FirstOrDefaultAsync(category => category.Name.Equals(res)))?.Id ?? Guid.Empty;
            }
            else
            {
                return (await _database.Categories
                .FirstOrDefaultAsync(category => category.Name.Equals(Variables.Categories.Games)))?.Id ?? Guid.Empty;
            }
        }
    }
}
