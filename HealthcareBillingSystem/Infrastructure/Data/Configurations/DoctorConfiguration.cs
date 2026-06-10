using HealthcareBillingSystem.Domain.Entities;
using HealthcareBillingSystem.Infrastructure.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthcareBillingSystem.Infrastructure.Data.Configurations;

public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
{
    public void Configure(EntityTypeBuilder<Doctor> builder)
    {
        builder.HasKey(doctor => doctor.Id);

        builder.Property(doctor => doctor.FullName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(doctor => doctor.Specialization)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(doctor => doctor.ApplicationUserId)
            .IsRequired();

        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(doctor => doctor.ApplicationUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
