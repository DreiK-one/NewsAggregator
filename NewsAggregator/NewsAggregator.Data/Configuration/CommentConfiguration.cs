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
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasData(
                new Comment
                {
                    Id = Guid.NewGuid(),
                    Text = "Интересная статья!",
                    CreationDate = DateTime.Now,
                    UserId = new Guid("c46d09f1-f535-4822-9372-dc3af86672fb"),
                    ArticleId = new Guid("205E2D6A-76D2-40F1-800B-9D652146C158")
                });
        }
    }
}
