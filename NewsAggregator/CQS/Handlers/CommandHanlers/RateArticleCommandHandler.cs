using AutoMapper;
using CQS.Models.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQS.Handlers.CommandHanlers
{
    public class RateArticleCommandHandler : IRequestHandler<RateArticleCommand, bool>
    {
        private readonly IMapper _mapper;
        private readonly NewsAggregatorContext _database;
        
        public RateArticleCommandHandler(NewsAggregatorContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public async Task<bool> Handle(RateArticleCommand command, CancellationToken token)
        {
            var article = await _database.Articles.FirstOrDefaultAsync(a => a.Id.Equals(command.Id), cancellationToken: token);

            article.Coefficient = command.Rate;

            var result = await _database.SaveChangesAsync(token);

            return true;
        }
    }
}
