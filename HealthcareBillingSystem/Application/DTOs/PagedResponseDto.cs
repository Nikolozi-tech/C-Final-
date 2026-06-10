namespace HealthcareBillingSystem.Application.DTOs;

public class PagedResponseDto<T>
{
    public IReadOnlyList<T> Data { get; set; } = Array.Empty<T>();

    public int TotalCount { get; set; }

    public int TotalPages { get; set; }

    public int CurrentPage { get; set; }
}
