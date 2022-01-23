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
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasData(
                new Category
                {
                    Id = Guid.NewGuid(),
                    Name = "Sport"
                },
                new Category
                {
                    Id = Guid.NewGuid(),
                    Name = "People"
                });
        }
    }
}
