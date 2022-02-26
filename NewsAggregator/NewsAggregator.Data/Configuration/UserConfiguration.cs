using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewsAggregator.Data.Entities;


namespace NewsAggregator.Data.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasData(
                new User
                {
                    Id = Guid.NewGuid(),
                    Email = "123@mail.ru",
                    PasswordHash = "123",
                    FirstName = "Ted",
                    LastName = "Jackson",
                    RegistrationDate = DateTime.Now
                });
        }
    }
}
