using HealthcareBillingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthcareBillingSystem.Infrastructure.Data.Configurations;

public class VisitConfiguration : IEntityTypeConfiguration<Visit>
{
    public void Configure(EntityTypeBuilder<Visit> builder)
    {
        builder.HasKey(visit => visit.Id);

        builder.Property(visit => visit.VisitDate)
            .IsRequired();

        builder.Property(visit => visit.Fee)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.HasOne(visit => visit.Patient)
            .WithMany(patient => patient.Visits)
            .HasForeignKey(visit => visit.PatientId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(visit => visit.Doctor)
            .WithMany(doctor => doctor.Visits)
            .HasForeignKey(visit => visit.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(visit => new { visit.PatientId, visit.VisitDate });
    }
}
