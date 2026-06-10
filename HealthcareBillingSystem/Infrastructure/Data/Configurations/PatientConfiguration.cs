using HealthcareBillingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthcareBillingSystem.Infrastructure.Data.Configurations;

public class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.HasKey(patient => patient.Id);

        builder.Property(patient => patient.FullName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(patient => patient.BirthDate)
            .IsRequired();
    }
}
