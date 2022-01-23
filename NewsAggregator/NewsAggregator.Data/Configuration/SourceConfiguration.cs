using Microsoft.EntityFrameworkCore;
using NewsAggregator.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Data.Configuration
{
    public class SourceConfiguration : IEntityTypeConfiguration<Source>
    {
        public void Configure(EntityTypeBuilder<Source> builder)
        {
            builder.HasData(
                new Source
                {
                    Id = Guid.NewGuid(),
                    Name = "Onliner",
                    BaseUrl = "https://www.onliner.by/",
                    RssUrl = "https://www.onliner.by/feed"
                },
                new Source
                {
                    Id = Guid.NewGuid(),
                    Name = "Lenta",
                    BaseUrl = "https://lenta.ru",
                    RssUrl = "lenta.ru/rss/news"

                });
        }
    }
}
