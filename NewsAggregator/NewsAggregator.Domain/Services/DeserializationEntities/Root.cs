﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Domain.Services.DeserializationEntities
{
    public class Root
    {
        public string Text { get; set; }
        public Annotations Annotations { get; set; }
    }
}
