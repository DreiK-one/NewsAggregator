﻿using AutoMapper;
using CQS.Models.Queries.CommentQueries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQS.Handlers.QueryHandlers.CommentHandlers
{
    public class GetCommentByIdQueryHandler : IRequestHandler<GetCommentByIdQuery, CreateOrEditCommentDto>
    {
        private readonly IMapper _mapper;
        private readonly NewsAggregatorContext _database;

        public GetCommentByIdQueryHandler(NewsAggregatorContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public async Task<CreateOrEditCommentDto> Handle(GetCommentByIdQuery query, CancellationToken token)
        {
            var comment = await _database.Comments
                .AsNoTracking()
                .Where(c => c.Id.Equals(query.Id))
                .Select(c => _mapper.Map<CreateOrEditCommentDto>(c))
                .FirstOrDefaultAsync(cancellationToken: token);

            return comment;
        }
    }
}
