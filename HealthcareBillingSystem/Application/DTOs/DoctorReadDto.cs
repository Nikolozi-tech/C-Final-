namespace HealthcareBillingSystem.Application.DTOs;

public class DoctorReadDto
{
    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Specialization { get; set; } = string.Empty;

    public string ApplicationUserId { get; set; } = string.Empty;
}
