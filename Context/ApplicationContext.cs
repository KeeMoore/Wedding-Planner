using Wedding_Planner.Models;
using Microsoft.EntityFrameworkCore;

namespace Wedding_Planner.Context;

public class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Wedding> Weddings { get; set; }
    public DbSet<RSVP> RSVPs{ get; set; }

    public ApplicationContext(DbContextOptions options) : base(options) { }
}