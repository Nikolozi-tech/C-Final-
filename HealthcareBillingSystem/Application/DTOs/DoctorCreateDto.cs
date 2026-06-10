namespace HealthcareBillingSystem.Application.DTOs;

public class DoctorCreateDto
{
    public string FullName { get; set; } = string.Empty;

    public string Specialization { get; set; } = string.Empty;

    public string ApplicationUserId { get; set; } = string.Empty;
}
