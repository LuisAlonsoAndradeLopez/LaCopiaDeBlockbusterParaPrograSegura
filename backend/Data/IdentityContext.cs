using backendnet.Data.Seed;
using backendnet.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace backendnet.Data;

public class IdentityContext(DbContextOptions<IdentityContext> options) : IdentityDbContext<CustomIdentityUser>(options)
{
    public DbSet<Movie> Movie { get; set; }
    public DbSet<Category> Category { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Initialize the database
        //modelBuilder.ApplyConfiguration(new SeedCategory());
        //modelBuilder.ApplyConfiguration(new SeedMovie());
        modelBuilder.SeedUserIdentityData();

        base.OnModelCreating(modelBuilder);
    }
}