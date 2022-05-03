﻿using Microsoft.EntityFrameworkCore;
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
using NewsAggregator.Core.DTOs;
using NewsAggregator.Domain.WebApiServices;
using NewsAggregator.Core.Interfaces.WebApiInterfaces;
using NewsAggregator.Core.Interfaces.InterfacesCQS;
using NewsAggregator.Domain.ServicesCQS;
using NewsAggregetor.CQS.Models.Queries.ArticleQueries;
using NewsAggregetor.CQS.Handlers.QueryHandlers.ArticleHandlers;
using NewsAggregetor.CQS.Models.Queries.CategoryQueries;
using NewsAggregetor.CQS.Handlers.QueryHandlers.CategoryHandlers;
using NewsAggregetor.CQS.Models.Commands.CommentCommands;
using NewsAggregetor.CQS.Handlers.CommandHandlers.CommentHandlers;
using NewsAggregetor.CQS.Models.Queries.AccountQueries;
using NewsAggregetor.CQS.Handlers.QueryHandlers.AccountHandlers;
using NewsAggregetor.CQS.Models.Commands.AccountCommands;
using NewsAggregetor.CQS.Handlers.CommandHandlers.AccountCommands;
using System.Reflection;

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

            services.AddControllers().AddFluentValidation(fv =>
            {
                fv.RegisterValidatorsFromAssemblyContaining<CommentValidator>();
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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            var rssService = serviceProvider.GetRequiredService<IRssService>();
            RecurringJob.AddOrUpdate("Aggregate news",
                () => rssService.GetNewsFromSourcesAsync(),
                "*/10 * * * *");

            var rateService = serviceProvider.GetRequiredService<IRateService>();
            RecurringJob.AddOrUpdate("Rate news",
                () => rateService.RateArticle(),
                "*/1 * * * *");
        }
    }
}
