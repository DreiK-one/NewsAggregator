﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Data.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public virtual IEnumerable<Article> Articles { get; set; }
    }
}
