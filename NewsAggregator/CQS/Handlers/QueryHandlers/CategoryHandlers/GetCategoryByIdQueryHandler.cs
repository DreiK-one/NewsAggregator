using AutoMapper;
using CQS.Models.Queries.CategoryQueries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQS.Handlers.QueryHandlers.CategoryHandlers
{
    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryWithArticlesDto>
    {
        private readonly IMapper _mapper;
        private readonly NewsAggregatorContext _database;

        public GetCategoryByIdQueryHandler(NewsAggregatorContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public async Task<CategoryWithArticlesDto> Handle(GetCategoryByIdQuery query, CancellationToken token)
        {
            var category = await _database.Categories
                .Where(c => c.Id.Equals(query.Id))
                .Include(a => a.Articles)
                    .ThenInclude(c => c.Comments)
                .Select(c => _mapper.Map<CategoryWithArticlesDto>(c))
                .FirstOrDefaultAsync(cancellationToken: token);

            return category;
        }
    }
}
