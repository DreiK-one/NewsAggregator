using Microsoft.EntityFrameworkCore;
using NewsAggregator.Core.Interfaces;
using NewsAggregator.Core.Interfaces.Data;
using NewsAggregator.Data;
using NewsAggregator.Data.Entities;
using NewsAggregator.DataAccess;
using NewsAggregator.Domain.Services;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .WriteTo.Console()
    .Enrich.FromLogContext()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting web host");

    var builder = WebApplication.CreateBuilder(args);

    // Full setup of serilog. We read log settings from appsettings.json
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext());

    //builder.Host.UseSerilog((ctx, lc) =>
    //{
    //    lc.MinimumLevel.Fatal().WriteTo.File(
    //        @$"D:\Games\C#\Web\NewsAggregator\testLogs\Fatal\fatal.log",
    //        fileSizeLimitBytes: 1_000_000,
    //        rollOnFileSizeLimit: true,
    //        shared: true,
    //        flushToDiskInterval: TimeSpan.FromDays(1));
    //});


    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<NewsAggregatorContext>(options => options.UseSqlServer(connectionString));

    // Add services to the container.

    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

    builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
    builder.Services.AddScoped<IBaseRepository<Category>, CategoryRepository>();
    builder.Services.AddScoped<IBaseRepository<Comment>, CommentRepository>();
    builder.Services.AddScoped<IBaseRepository<Role>, RoleRepository>();
    builder.Services.AddScoped<IBaseRepository<Source>, SourceRepository>();
    builder.Services.AddScoped<IBaseRepository<User>, UserRepository>();
    builder.Services.AddScoped<IBaseRepository<UserActivity>, UserActivityRepository>();

    builder.Services.AddScoped<IArticleService, ArticleService>();
    builder.Services.AddScoped<ICategoryService, CategoryService>();
    builder.Services.AddScoped<ICommentService, CommentService>();
    builder.Services.AddScoped<IRoleService, RoleService>();
    builder.Services.AddScoped<ISourceService, SourceService>();
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IRssService, RssService>();
    builder.Services.AddScoped<IHtmlParserService, HtmlParserService>();

    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    builder.Services.AddControllersWithViews();

    var app = builder.Build();

    // We want to log all HTTP requests
    app.UseSerilogRequestLogging(configure =>
    {
        configure.MessageTemplate = "HTTP {RequestMethod} {RequestPath} ({UserId}) responded {StatusCode} in {Elapsed:0.0000}ms";
    }); 

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.MigrateDatabase().Run();
}

catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}
return 0;