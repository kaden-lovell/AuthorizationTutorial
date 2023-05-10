using System;
using System.IO;
using BloggerApi.Configuration;
using BloggerApi.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace BloggerApi {
  public class Program {
    public static void Main(string[] args) {
      var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
      var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").AddJsonFile($"appsettings.{environment}.json", true).Build();

      var logging = new Logging();

      configuration.GetSection("Logging").Bind(logging);

      // INITIALIZE LOGGING
      if (string.IsNullOrWhiteSpace(logging.OutputTemplate)) {
        Log.Logger =
          new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", environment == "development" ? LogEventLevel.Warning : LogEventLevel.Error)
            //.MinimumLevel.Override("Microsoft.AspNetCore.SignalR", LogEventLevel.Debug)
            //.MinimumLevel.Override("Microsoft.AspNetCore.Http.Connections", LogEventLevel.Debug)
            .Enrich.FromLogContext()
            .WriteTo.File(new MessageFormatter(), logging.PathFormat, logging.LogLevel, logging.FileSizeLimit, retainedFileCountLimit: logging.FileCountLimit, rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)
            .WriteTo.Console()
            .CreateLogger();
      }
      else {
        // Example output template: {Timestamp:yyyy-MM-dd hh:mm:ss tt zzz} [{Level}] {Message}{NewLine}{Exception}
        Log.Logger =
          new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", environment == "development" ? LogEventLevel.Warning : LogEventLevel.Error)
            //.MinimumLevel.Override("Microsoft.AspNetCore.SignalR", LogEventLevel.Debug)
            //.MinimumLevel.Override("Microsoft.AspNetCore.Http.Connections", LogEventLevel.Debug)
            .Enrich.FromLogContext()
            .WriteTo.File(logging.PathFormat, logging.LogLevel, logging.OutputTemplate, null, logging.FileSizeLimit, retainedFileCountLimit: logging.FileCountLimit, rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)
            .WriteTo.Console()
            .CreateLogger();
      }

      var host = CreateHostBuilder(args).Build();
      SeedDatabase(host);

      // Start Application
      try {
        Log.Information("Starting web host");
        host.Run();
      }
      catch (Exception ex) {
        Log.Fatal(ex, "Host terminated unexpectedly");
      }
      finally {
        Log.CloseAndFlush();
      }
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
      Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
        .UseSerilog();

    private static void SeedDatabase(IHost host) {
      using var scope = host.Services.CreateScope();
      var services = scope.ServiceProvider;

      try {
        var context = services.GetRequiredService<DataContext>();
        DataSeeder.Initialize(context, services);
      }
      catch {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError("An error occurred while seeding the database");
      }
    }
  }
}