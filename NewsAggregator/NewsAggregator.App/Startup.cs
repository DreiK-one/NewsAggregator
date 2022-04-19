using FluentValidation.AspNetCore;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Core.Interfaces;
using NewsAggregator.Core.Interfaces.Data;
using NewsAggregator.App.Validation;
using NewsAggregator.Data;
using NewsAggregator.Data.Entities;
using NewsAggregator.DataAccess;
using NewsAggregator.Domain.Services;
using Serilog;
using Microsoft.AspNetCore.Authentication.Cookies;
using NewsAggregator.App.Filters;
using Microsoft.ApplicationInsights;

namespace NewsAggregator.App
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json");
            Configuration = configurationBuilder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<NewsAggregatorContext>(options => options.UseSqlServer(connectionString));

            // Add services to the container.
            

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IArticleRepository, ArticleRepository>();
            services.AddScoped<IBaseRepository<Category>, CategoryRepository>();
            services.AddScoped<IBaseRepository<Comment>, CommentRepository>();
            services.AddScoped<IBaseRepository<Role>, RoleRepository>();
            services.AddScoped<IBaseRepository<Source>, SourceRepository>();
            services.AddScoped<IBaseRepository<User>, UserRepository>();
            services.AddScoped<IBaseRepository<UserActivity>, UserActivityRepository>();
            services.AddScoped<IBaseRepository<UserRole>, UserRoleRepository>();

            services.AddScoped<IArticleService, ArticleService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<ISourceService, SourceService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRssService, RssService>();
            services.AddScoped<IHtmlParserService, HtmlParserService>();
            services.AddScoped<IAccountService, AccountService>();

            services.AddScoped<IValidationMethods, ValidationMethods>();

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

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(opt => 
                {
                    opt.AccessDeniedPath = "/access-denied";
                    opt.LoginPath = "/account/login";
                    opt.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                    opt.SlidingExpiration = true;
                    opt.Cookie.Name = "NewsAggregator.Session";
                });

            services.AddAuthorization();

            services.AddControllersWithViews()
                .AddFluentValidation(fv =>
                {
                    fv.RegisterValidatorsFromAssemblyContaining<ArticleValidator>();
                    fv.RegisterValidatorsFromAssemblyContaining<RoleValidator>();
                    fv.RegisterValidatorsFromAssemblyContaining<CategoryValidator>();
                    fv.RegisterValidatorsFromAssemblyContaining<SourceValidator>();
                    fv.RegisterValidatorsFromAssemblyContaining<AccountRegisterValidator>();
                    fv.RegisterValidatorsFromAssemblyContaining<AccountLoginValidator>();
                    fv.RegisterValidatorsFromAssemblyContaining<CommentValidator>();
                    fv.RegisterValidatorsFromAssemblyContaining<ChangeEmailValidator>();
                    fv.RegisterValidatorsFromAssemblyContaining<ChangeNicknameValidator>();
                    fv.RegisterValidatorsFromAssemblyContaining<ChangePasswordValidator>();
                    fv.RegisterValidatorsFromAssemblyContaining<EditUserValidator>();
                    fv.RegisterValidatorsFromAssemblyContaining<CreateUserValidator>();
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            app.UseSerilogRequestLogging(configure =>
            {
                configure.MessageTemplate = "HTTP {RequestMethod} {RequestPath} ({UserId}) responded {StatusCode} in {Elapsed:0.0000}ms";
            });
    
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseHsts();
            }
            else
            {
                app.UseExceptionHandler("/error/500");

                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.Use(async (ctx, next) =>
            {
                await next();

                if (ctx.Response.StatusCode == 404 && !ctx.Response.HasStarted)
                {
                    string originalPath = ctx.Request.Path.Value;
                    ctx.Items["originalPath"] = originalPath;
                    ctx.Request.Path = "/error/404";
                    await next();
                }
            });

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[]
                {
                    new HangfireAuthorizationFilter()
                }
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            var rssService = serviceProvider.GetRequiredService<IRssService>();
            RecurringJob.AddOrUpdate("Aggregate news",
                () => rssService.GetNewsFromSourcesAsync(),
                "*/10 * * * *");
        }
    }
}
