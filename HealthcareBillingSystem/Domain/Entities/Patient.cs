namespace HealthcareBillingSystem.Domain.Entities;

public class Patient
{
    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public DateTime BirthDate { get; set; }

    public ICollection<Visit> Visits { get; set; } = new List<Visit>();
}
