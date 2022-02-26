using Microsoft.EntityFrameworkCore;
using NewsAggregator.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace NewsAggregator.Data.Configuration
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasData(
                new Role
                {
                    Id = Guid.NewGuid(),
                    Name = "Admin"
                },
                new Role
                {
                    Id = Guid.NewGuid(),
                    Name = "User"
                },
                new Role
                {
                    Id= Guid.NewGuid(),
                    Name = "Moderator"
                });
        }
    }
}
