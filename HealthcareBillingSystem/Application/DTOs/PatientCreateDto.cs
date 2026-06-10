namespace HealthcareBillingSystem.Application.DTOs;

public class PatientCreateDto
{
    public string FullName { get; set; } = string.Empty;

    public DateTime BirthDate { get; set; }
}
