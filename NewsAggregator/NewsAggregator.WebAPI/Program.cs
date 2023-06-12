using NewsAggregator.Data;
using Serilog;
using Serilog.Events;


namespace NewsAggregator.WebAPI
{
    public class Program
    {
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseSerilog((context, services, configuration) => configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext())
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });

        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .WriteTo.File(@"D:\Games\Projects\Web\NewsAggregator\testLogs\Seq\LogAPI.log")
                .Enrich.FromLogContext()
                .CreateBootstrapLogger();

            Log.Information("Starting web host");
            CreateHostBuilder(args).Build().MigrateDatabase().Run();
        }
    }
}
