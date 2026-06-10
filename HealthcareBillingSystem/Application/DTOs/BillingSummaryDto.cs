namespace HealthcareBillingSystem.Application.DTOs;

public class BillingSummaryDto
{
    public int PatientId { get; set; }

    public string PatientName { get; set; } = string.Empty;

    public decimal TotalPaid { get; set; }
}
