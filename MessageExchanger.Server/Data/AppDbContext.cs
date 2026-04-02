using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MessageExchanger.Server.Data.Entities;

namespace MessageExchanger.Server.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false)
                    .Build();

                string connectionString = configuration.GetConnectionString("DefaultConnection")
                    ?? throw new Exception("Connection string not found.");

                optionsBuilder.UseSqlServer(connectionString);
            }
        }
    }
}