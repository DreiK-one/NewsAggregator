using Microsoft.EntityFrameworkCore;
using NewsAggregator.Core.Interfaces;
using NewsAggregator.Core.Interfaces.Data;
using NewsAggregator.Data;
using NewsAggregator.Data.Entities;
using NewsAggregator.DataAccess;
using NewsAggregator.Domain.Services;
using Microsoft.OpenApi.Models;
using FluentValidation.AspNetCore;
using NewsAggregator.WebAPI.Validators;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MediatR;
using NewsAggregator.Domain.WebApiServices;
using NewsAggregator.Core.Interfaces.WebApiInterfaces;
using NewsAggregator.Core.Interfaces.InterfacesCQS;
using NewsAggregator.Domain.ServicesCQS;
using System.Reflection;
using Microsoft.ApplicationInsights;


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

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IArticleRepository, ArticleRepository>();
            services.AddScoped<IBaseRepository<Category>, CategoryRepository>();
            services.AddScoped<IBaseRepository<Comment>, CommentRepository>();
            services.AddScoped<IBaseRepository<Role>, RoleRepository>();
            services.AddScoped<IBaseRepository<Source>, SourceRepository>();
            services.AddScoped<IBaseRepository<User>, UserRepository>();
            services.AddScoped<IBaseRepository<UserActivity>, UserActivityRepository>();
            services.AddScoped<IBaseRepository<UserRole>, UserRoleRepository>();
            services.AddScoped<IBaseRepository<RefreshToken>, RefreshTokenRepository>();

            services.AddScoped<IArticleService, ArticleService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<ISourceService, SourceService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRssService, RssService>();
            services.AddScoped<IHtmlParserService, HtmlParserService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IRateService, RateService>();

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IJwtService, JwtService>();

            services.AddScoped<IArticleServiceCQS, ArticleServiceCQS>();
            services.AddScoped<ICommentServiceCQS, CommentServiceCQS>();
            services.AddScoped<ICategoryServiceCQS, CategoryServiceCQS>();
            services.AddScoped<IAccountServiceCQS, AccountServiceCQS>();
            services.AddScoped<IUserServiceCQS, UserServiceCQS>();
            services.AddScoped<ISourceServiceCQS, SourceServiceCQS>();
            services.AddScoped<IRoleServiceCQS, RoleServiceCQS>();
            services.AddScoped<IRssServiceCQS, RssServiceCQS>();
            services.AddScoped<IRateServiceCQS, RateServiceCQS>();
            services.AddScoped<IHtmlParserServiceCQS, HtmlParserServiceCQS>();


            services.AddScoped<IValidationMethodsCQS, ValidationMethodsCQS>();

            services.AddScoped<TelemetryClient>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(Configuration.GetConnectionString("HangfireConnection"),
                    new SqlServerStorageOptions
                    {
                        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                        QueuePollInterval = TimeSpan.Zero,
                        UseRecommendedIsolationLevel = true,
                        DisableGlobalLocks = true
                    }));
            services.AddHangfireServer();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = true;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["AppSettings:Secret"]))
                    };
                });

            Assembly.Load("NewsAggregator.CQS");
            services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

            services.AddCors(options =>
            {
                options.AddPolicy(name: "Enable",
                    policy =>
                    {
                        policy.AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowAnyOrigin();
                    });
            });

            services.AddControllers().AddFluentValidation(fv =>
            {
                fv.RegisterValidatorsFromAssemblyContaining<Startup>();
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "NewsAggregator.WebAPI", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NewsAggregator.WebAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("Enable");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            var aggregateNews = serviceProvider.GetRequiredService<IRssService>();
            RecurringJob.AddOrUpdate("Aggregate",
                () => aggregateNews.GetNewsFromSourcesAsync(),
                "*/10 * * * *");

            var parseNews = serviceProvider.GetRequiredService<IHtmlParserService>();
            RecurringJob.AddOrUpdate("Parse",
                () => parseNews.GetArticleContentFromUrlAsync(),
                "*/20 * * * *");

            var rateNews = serviceProvider.GetRequiredService<IRateService>();
            RecurringJob.AddOrUpdate("Rate",
                () => rateNews.RateArticle(),
                "*/1 * * * *");
        }
    }
}
