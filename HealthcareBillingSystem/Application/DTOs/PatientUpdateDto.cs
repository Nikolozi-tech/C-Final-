namespace HealthcareBillingSystem.Application.DTOs;

public class PatientUpdateDto
{
    public string FullName { get; set; } = string.Empty;

    public DateTime BirthDate { get; set; }
}
