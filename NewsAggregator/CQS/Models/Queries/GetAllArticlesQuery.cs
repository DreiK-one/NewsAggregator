﻿using MediatR;
using NewsAggregator.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQS.Models.Queries
{
    public class GetAllArticlesQuery : IRequest<IEnumerable<ArticleDto>>
    {
    }
}
