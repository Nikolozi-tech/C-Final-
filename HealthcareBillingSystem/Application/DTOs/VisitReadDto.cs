namespace HealthcareBillingSystem.Application.DTOs;

public class VisitReadDto
{
    public int Id { get; set; }

    public int PatientId { get; set; }

    public string PatientName { get; set; } = string.Empty;

    public int DoctorId { get; set; }

    public string DoctorName { get; set; } = string.Empty;

    public DateTime VisitDate { get; set; }

    public decimal Fee { get; set; }
}
