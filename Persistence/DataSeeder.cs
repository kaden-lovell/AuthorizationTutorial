using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using BloggerApi.Models;
using BloggerApi.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BloggerApi.Persistence {
  public class DataSeeder {
    public static void Initialize(DataContext context, IServiceProvider services) {
      var logger = services.GetRequiredService<ILogger<DataSeeder>>();

      // initial connection test
      if (context.Database.CanConnect()) {
        logger.LogInformation("Database already exists.");

        return;
      }

      // ensure database creation
      context.Database.EnsureCreated();

      // START TRANSACTIONS
      logger.LogInformation("Start seeding the database.");

      // SERVER
      logger.LogInformation("Seeding server started...");
      byte[] key;
      byte[] iv;

      // generate hashes
      using (var aes = Aes.Create()) {
        key = aes.Key;
        iv = aes.IV;
      }

      var server = new Server {
        Key = key,
        IV = iv,
        CreatedDate = DateTime.UtcNow
      };

      context.Server.Add(server);
      context.SaveChanges();
      logger.LogInformation("Seeding server finished...");

      // USER
      logger.LogInformation("Seeding users started...");
      var users = new List<User> {
        new() {
          Email = "adminlogin@domain.com",
          FirstName = "Admin",
          LastName = "Access",
          Password = CryptoUtility.EncryptString(Role.ADMINISTRATOR, server.Key, server.IV),
          Role = Role.ADMINISTRATOR,
          Active = true,
          CreatedDate = DateTime.UtcNow
        },

        new() {
          Email = "clientlogin@domain.com",
          FirstName = "Client",
          LastName = "Access",
          Password = CryptoUtility.EncryptString(Role.CLIENT, server.Key, server.IV),
          Role = Role.CLIENT,
          Active = true,
          CreatedDate = DateTime.UtcNow
        }
      };

      context.User.AddRange(users);
      context.SaveChanges();
      logger.LogInformation("Seeding users finished...");

      // END OF TRANSACTIONS
      logger.LogInformation("Finished seeding the database.");
    }
  }
}