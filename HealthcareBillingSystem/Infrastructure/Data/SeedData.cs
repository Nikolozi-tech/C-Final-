using HealthcareBillingSystem.Infrastructure.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HealthcareBillingSystem.Infrastructure.Data;

public static class SeedData
{
    public const string AdminRoleId = "a5dbbf92-6e2a-4c92-a6a5-20a3d4e53501";
    public const string DoctorRoleId = "b6e64d3f-0510-4555-9154-99b0cf3af201";
    public const string AdminUserId = "2f33103b-1d50-4c1e-ae9f-4fb2f9fe8001";

    public static void Seed(ModelBuilder builder)
    {
        var adminRole = new IdentityRole
        {
            Id = AdminRoleId,
            Name = "Admin",
            NormalizedName = "ADMIN",
            ConcurrencyStamp = AdminRoleId
        };

        var doctorRole = new IdentityRole
        {
            Id = DoctorRoleId,
            Name = "Doctor",
            NormalizedName = "DOCTOR",
            ConcurrencyStamp = DoctorRoleId
        };

        var adminUser = new ApplicationUser
        {
            Id = AdminUserId,
            FullName = "System Administrator",
            UserName = "admin@healthcare.local",
            NormalizedUserName = "ADMIN@HEALTHCARE.LOCAL",
            Email = "admin@healthcare.local",
            NormalizedEmail = "ADMIN@HEALTHCARE.LOCAL",
            EmailConfirmed = true,
            SecurityStamp = "85b75a2d-0990-4adc-a9f7-4807c3be6d57",
            ConcurrencyStamp = "11bc2db4-5156-4e44-970f-50948210a094"
        };

        adminUser.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(adminUser, "Admin@12345");

        builder.Entity<IdentityRole>().HasData(adminRole, doctorRole);
        builder.Entity<ApplicationUser>().HasData(adminUser);
        builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
        {
            RoleId = AdminRoleId,
            UserId = AdminUserId
        });
    }
}
