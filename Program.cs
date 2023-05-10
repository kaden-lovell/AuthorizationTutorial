using BloggerApi.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BloggerApi {
  public class Program {
    public static void Main(string[] args) {
      var host = CreateHostBuilder(args).Build();
      SeedDatabase(host);
      host.Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
      Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });

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