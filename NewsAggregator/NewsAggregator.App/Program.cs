using Microsoft.EntityFrameworkCore;
using NewsAggregator.Core.Interfaces;
using NewsAggregator.Core.Interfaces.Data;
using NewsAggregator.Data;
using NewsAggregator.Data.Entities;
using NewsAggregator.DataAccess;
using NewsAggregator.Domain.Services;
using Serilog;


    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((ctx, lc) => 
    {
        lc.MinimumLevel.Fatal().WriteTo.File(
            @$"D:\Games\C#\Web\NewsAggregator\testLogs\Fatal\fatal.log",
            fileSizeLimitBytes: 1_000_000,
            rollOnFileSizeLimit: true,
            flushToDiskInterval: TimeSpan.FromDays(1));
        lc.MinimumLevel.Error().WriteTo.File(
            @$"D:\Games\C#\Web\NewsAggregator\testLogs\Error\error.log",
            fileSizeLimitBytes: 1_000_000,
            rollOnFileSizeLimit: true,
            flushToDiskInterval: TimeSpan.FromDays(1));
        lc.MinimumLevel.Warning().WriteTo.File(
            @$"D:\Games\C#\Web\NewsAggregator\testLogs\Warning\warning.log",
            fileSizeLimitBytes: 1_000_000,
            rollOnFileSizeLimit: true,
            flushToDiskInterval: TimeSpan.FromDays(1));
        lc.MinimumLevel.Information().WriteTo.File(
            @$"D:\Games\C#\Web\NewsAggregator\testLogs\Info\info.log",
            fileSizeLimitBytes: 1_000_000,
            rollOnFileSizeLimit: true,
            flushToDiskInterval: TimeSpan.FromDays(1));
    });


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

    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    builder.Services.AddControllersWithViews();

    var app = builder.Build();
    Log.Information("Starting web host");

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
