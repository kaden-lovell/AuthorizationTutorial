using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ClientPortalApi.Models;
using ClientPortalApi.Services;

namespace ClientPortalApi.Persistence {
    public class DataSeeder {
        public static void Initialize(DataContext context, IServiceProvider services) {
            var _logger = services.GetRequiredService<ILogger<DataSeeder>>();
            // initial connection test
            if (context.Database.CanConnect()) {
                _logger.LogInformation("Database already exists.");
                return;
            }

            // ensure database creation
            context.Database.EnsureCreated();

            // START TRANSACTIONS
            _logger.LogInformation("Start seeding the database.");
  
            // SERVER
            _logger.LogInformation("Seeding server started...");
            byte[] key;
            byte[] iv;

            // generate hashes
            using (Aes aes = Aes.Create()) {
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
            _logger.LogInformation("Seeding server finished...");


            // USER
            _logger.LogInformation("Seeding users started...");
            var users = new List<User> {
                new User {
                    Email = "adminlogin@domain.com",
                    FirstName = "Admin",
                    LastName = "Access",
                    Password = CryptoUtility.EncryptString(Role.ADMINISTRATOR, server.Key, server.IV),
                    Role = Role.ADMINISTRATOR,
                    Active = true,
                    CreatedDate = DateTime.UtcNow
                },

                new User {
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
            _logger.LogInformation("Seeding users finished...");

            // END OF TRANSACTIONS
            _logger.LogInformation("Finished seeding the database.");
        }
    }
}
