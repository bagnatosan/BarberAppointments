using Barber.Models;
using Microsoft.EntityFrameworkCore;

namespace Barber.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    
    
    public DbSet<User>  Users { get; set; }
    public DbSet<Appointment>  Appointments { get; set; }
    public DbSet<Hairdresser>  Hairdressers { get; set; }
    
}