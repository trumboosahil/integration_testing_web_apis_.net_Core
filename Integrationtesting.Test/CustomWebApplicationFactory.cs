using System;
using System.Linq;
using IntegrationTesting.Web.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Integrationtesting.Test
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        private SqliteConnection Connection;
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            Connection = new SqliteConnection("DataSource=:memory:");
            Connection.Open();
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<CustomerDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Add ApplicationDbContext using an in-memory database for testing.
              //  services.AddDbContext<CustomerDbContext>((_, context) => context.UseInMemoryDatabase("InMemoryDbForTesting"));

                // Register new database service (SQLite In-Memory)
                services.AddDbContext<CustomerDbContext>(options => options.UseSqlite(Connection));

                // Build the service provider.
                var serviceProvider = services.BuildServiceProvider();

                // Create a scope to obtain a reference to the database
                // context (ApplicationDbContext).
                using var scope = serviceProvider.CreateScope();

                var db = scope.ServiceProvider.GetRequiredService<CustomerDbContext>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                // Ensure the database is created.
                db.Database.EnsureCreated();

              
               
            });
        }
    }
}