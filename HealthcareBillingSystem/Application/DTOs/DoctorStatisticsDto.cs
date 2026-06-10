namespace HealthcareBillingSystem.Application.DTOs;

public class DoctorStatisticsDto
{
    public int DoctorId { get; set; }

    public string DoctorName { get; set; } = string.Empty;

    public int TotalVisits { get; set; }
}
