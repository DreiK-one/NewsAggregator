using NewsAggregator.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Core.Specifications
{
    public class ArticleSpec : BaseSpecification<Article>
    {
        public ArticleSpec(float rating) : base(art => art.Coefficient.Equals(rating))
        {
            AddOrderByDescending(article => article.CreationDate);
            //AddInclude(article => article.Category.Equals(category));
        }
    }
}
