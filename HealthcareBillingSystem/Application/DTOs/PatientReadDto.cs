namespace HealthcareBillingSystem.Application.DTOs;

public class PatientReadDto
{
    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public DateTime BirthDate { get; set; }
}
