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
                    RegistrationDate = DateTime.Now,
                    NormalizedEmail = "123@MAIL.RU",
                    Nickname = "Tom",
                    NormalizedNickname = "TOM"
                });
        }
    }
}
