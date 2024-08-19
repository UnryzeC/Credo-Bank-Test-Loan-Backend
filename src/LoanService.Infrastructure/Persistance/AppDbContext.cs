using System.Reflection;

using LoanService.Core.Loan;
using LoanService.Core.User;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace LoanService.Infrastructure.Persistance;

public class AppDbContext : DbContext
{
    public IConfiguration Configuration { get; set; }

    public AppDbContext( DbContextOptions<AppDbContext> options, IConfiguration configuration ) : base( options )
    {
        Configuration = configuration;
    }

    public DbSet<UserEntity> Users { get; set; }
    public DbSet<LoanRequest> LoanRequests { get; set; }

    protected override void OnConfiguring( DbContextOptionsBuilder optionsBuilder ) => optionsBuilder.EnableDetailedErrors( );

    protected override void OnModelCreating( ModelBuilder modelBuilder )
    {
        modelBuilder.ApplyConfigurationsFromAssembly( Assembly.GetExecutingAssembly( ) );

        modelBuilder.Entity<UserEntity>().HasData(
            new UserEntity
            {
                Id = Guid.NewGuid(),
                Firstname = "Sandro",
                Lastname = "Takaishvili",
                IdNumber = "62202018205",
                DateOfBirth = new DateTime( 1995, 8, 7 ),
                Email = "SandroTakaishvili@example.com",
                Password = "388F706930EF4CF0F7F4D59EE9819AA5B89FBEF8D4BB4EB7AB4D28B08CF034CC", // SomeVeryLongAndCoolPassword
                Salt = "fGWDCWTrmwnYbXJbPWHwDQ=="
            }
        );

        base.OnModelCreating( modelBuilder );
    }
}
