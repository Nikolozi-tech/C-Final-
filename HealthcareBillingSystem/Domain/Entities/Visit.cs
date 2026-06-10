namespace HealthcareBillingSystem.Domain.Entities;

public class Visit
{
    public int Id { get; set; }

    public int PatientId { get; set; }

    public Patient Patient { get; set; } = null!;

    public int DoctorId { get; set; }

    public Doctor Doctor { get; set; } = null!;

    public DateTime VisitDate { get; set; }

    public decimal Fee { get; set; }
}
