namespace HealthcareBillingSystem.Application.DTOs;

public class VisitCreateDto
{
    public int PatientId { get; set; }

    public int DoctorId { get; set; }

    public DateTime VisitDate { get; set; }

    public decimal Fee { get; set; }
}
