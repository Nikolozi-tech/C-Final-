namespace HealthcareBillingSystem.Domain.Entities;

public class Doctor
{
    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Specialization { get; set; } = string.Empty;

    public string ApplicationUserId { get; set; } = string.Empty;

    public ICollection<Visit> Visits { get; set; } = new List<Visit>();
}
