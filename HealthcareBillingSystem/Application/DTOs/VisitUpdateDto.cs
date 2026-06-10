namespace HealthcareBillingSystem.Application.DTOs;

public class VisitUpdateDto
{
    public int PatientId { get; set; }

    public int DoctorId { get; set; }

    public DateTime VisitDate { get; set; }

    public decimal Fee { get; set; }
}
