using Microsoft.EntityFrameworkCore;
using MessageExchanger.Server.Data.Entities;

namespace MessageExchanger.Server.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}