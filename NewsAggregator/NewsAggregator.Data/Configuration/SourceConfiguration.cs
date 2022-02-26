using Microsoft.EntityFrameworkCore;
using NewsAggregator.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


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
                    Name = "Shazoo",
                    BaseUrl = "shazoo.to",
                    RssUrl = "https://shazoo.ru/feed/rss"
                },
                new Source
                {
                    Id = Guid.NewGuid(),
                    Name = "Onliner",
                    BaseUrl = "onliner.by",
                    RssUrl = "https://www.onliner.by/feed"
                },
                new Source
                {
                    Id = Guid.NewGuid(),
                    Name = "Ria news",
                    BaseUrl = "ria.ru",
                    RssUrl = "https://ria.ru/export/rss2/archive/index.xml"
                });
        }
    }
}
