using Microsoft.EntityFrameworkCore;
using WebApplicationUsers.Models;

namespace WebApplicationUsers.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<User> users => Set<User>();
}
