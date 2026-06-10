using HealthcareBillingSystem.Domain.Entities;
using HealthcareBillingSystem.Infrastructure.Authentication;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HealthcareBillingSystem.Infrastructure.Data;

public class HealthDbContext : IdentityDbContext<ApplicationUser>
{
    public HealthDbContext(DbContextOptions<HealthDbContext> options)
        : base(options)
    {
    }

    public DbSet<Patient> Patients => Set<Patient>();

    public DbSet<Doctor> Doctors => Set<Doctor>();

    public DbSet<Visit> Visits => Set<Visit>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(HealthDbContext).Assembly);
        SeedData.Seed(builder);
    }
}
