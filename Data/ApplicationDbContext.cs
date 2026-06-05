using Barber.Models;
using Microsoft.EntityFrameworkCore;

namespace Barber.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    
    
    public DbSet<User>  Users { get; set; }
    public DbSet<Appointment>  Appointments { get; set; }
    public DbSet<Hairdresser>  Hairdressers { get; set; }
    
    public DbSet<Haircut> Haircuts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Appointment>()
            .HasIndex(a => new { a.HairdresserId, a.Date })
            .IsUnique();
        
        modelBuilder.Entity<Haircut>().ToTable("Haircut");
    }
}