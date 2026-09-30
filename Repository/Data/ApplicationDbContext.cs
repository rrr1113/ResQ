using Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Repository.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
{
    public DbSet<Operator> Operators { get; set; }
    public DbSet<Deployment> Deployments { get; set; }
    public DbSet<EmergencyService> EmergencyServices { get; set; }
    public DbSet<Incident> Incidents { get; set; }
    public DbSet<Location> Locations { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Location>()
            .HasIndex(x => new
            {
                x.Address,
                x.City,
                x.Country
            })
            .IsUnique();
    }
    public DbSet<ResponseTeam> ResponseTeams { get; set; }
    public DbSet<Vehicle> Vehicle { get; set; }
}