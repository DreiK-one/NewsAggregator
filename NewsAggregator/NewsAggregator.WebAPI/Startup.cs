using Microsoft.EntityFrameworkCore;
using NewsAggregator.Core.Interfaces;
using NewsAggregator.Core.Interfaces.Data;
using NewsAggregator.Data;
using NewsAggregator.Data.Entities;
using NewsAggregator.DataAccess;
using NewsAggregator.Domain.Services;
using Microsoft.OpenApi.Models;

namespace NewsAggregator.WebAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<NewsAggregatorContext>(options =>
                options.UseSqlServer(connectionString));

            //services.AddScoped<IUnitOfWork, UnitOfWork>();

            //services.AddScoped<IArticleRepository, ArticleRepository>();
            //services.AddScoped<IBaseRepository<Category>, CategoryRepository>();
            //services.AddScoped<IBaseRepository<Comment>, CommentRepository>();
            //services.AddScoped<IBaseRepository<Role>, RoleRepository>();
            //services.AddScoped<IBaseRepository<Source>, SourceRepository>();
            //services.AddScoped<IBaseRepository<User>, UserRepository>();
            //services.AddScoped<IBaseRepository<UserActivity>, UserActivityRepository>();
            //services.AddScoped<IBaseRepository<UserRole>, UserRoleRepository>();

            //services.AddScoped<IArticleService, ArticleService>();
            //services.AddScoped<ICategoryService, CategoryService>();
            //services.AddScoped<ICommentService, CommentService>();
            //services.AddScoped<IRoleService, RoleService>();
            //services.AddScoped<ISourceService, SourceService>();
            //services.AddScoped<IUserService, UserService>();
            //services.AddScoped<IRssService, RssService>();
            //services.AddScoped<IHtmlParserService, HtmlParserService>();
            //services.AddScoped<IAccountService, AccountService>();
            //services.AddScoped<IRateService, RateService>();

            //services.AddScoped<IRequestHandler<GetArticlesByPageQuery, IEnumerable<ArticleDto>>,
            //        GetArticleByPageQueryHandler>();
            //services.AddScoped<IRequestHandler<RateArticleCommand, bool>,
            //        RateArticleCommandHandler>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //    .AddJwtBearer(options =>
            //    {
            //        options.SaveToken = true;
            //        options.RequireHttpsMetadata = true;
            //        options.TokenValidationParameters = new TokenValidationParameters()
            //        {
            //            ValidateIssuerSigningKey = true,
            //            ValidateIssuer = false,
            //            ValidateAudience = false,
            //            ClockSkew = TimeSpan.Zero,
            //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["AppSettings:Secret"]))
            //        };
            //    });

            //services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "NewsAggregator.WebAPI", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NewsAggregator.WebAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            //app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
