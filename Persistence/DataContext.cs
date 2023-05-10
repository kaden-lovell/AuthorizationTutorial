using BloggerApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BloggerApi.Persistence {
  public class DataContext : DbContext, IPersistenceContext {
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<User> User { get; set; }

    public DbSet<Server> Server { get; set; }
  }
}