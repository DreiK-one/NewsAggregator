﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Core.DTOs
{
    public class CategoryWithArticlesDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public List<ArticleDto> Articles { get; set; }
    }
}