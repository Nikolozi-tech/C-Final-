namespace HealthcareBillingSystem.Application.DTOs;

public class VisitFilterDto
{
    public int? DoctorId { get; set; }

    public DateTime? VisitDateFrom { get; set; }

    public DateTime? VisitDateTo { get; set; }

    public decimal? MinFee { get; set; }

    public decimal? MaxFee { get; set; }

    public string? SortBy { get; set; }

    public string? SortDirection { get; set; }

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}
