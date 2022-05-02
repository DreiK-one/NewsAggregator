using Microsoft.EntityFrameworkCore;
using NewsAggregator.Data.Entities;


namespace NewsAggregator.Data
{
    public class NewsAggregatorContext : DbContext
    {
        public DbSet<Article> Articles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Source> Sources { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserActivity> UserActivities { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RefreshToken> RefreshTokens{ get; set; }

        public NewsAggregatorContext(DbContextOptions<NewsAggregatorContext> options) 
            : base(options)
        {
            
        }

        //Only for creation migrations

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.ApplyConfiguration(new RoleConfiguration());
        //    modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        //    modelBuilder.ApplyConfiguration(new SourceConfiguration());
        //    modelBuilder.ApplyConfiguration(new ArticleConfiguration());
        //    modelBuilder.ApplyConfiguration(new UserConfiguration());
        //    modelBuilder.ApplyConfiguration(new CommentConfiguration());
        //}
    }
}
