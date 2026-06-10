using HealthcareBillingSystem.Domain.Entities;

namespace HealthcareBillingSystem.Domain.Interfaces;

public interface IVisitRepository : IRepository<Visit>
{
    Task<IReadOnlyList<Visit>> GetFilteredVisitsAsync(
        int? doctorId,
        DateTime? visitDateFrom,
        DateTime? visitDateTo,
        decimal? minFee,
        decimal? maxFee,
        string? sortBy,
        string? sortDirection,
        int pageNumber,
        int pageSize,
        string? doctorApplicationUserId = null);

    Task<int> CountFilteredVisitsAsync(
        int? doctorId,
        DateTime? visitDateFrom,
        DateTime? visitDateTo,
        decimal? minFee,
        decimal? maxFee,
        string? doctorApplicationUserId = null);

    Task<IReadOnlyList<Visit>> GetVisitsByDoctorAsync(int doctorId);

    Task<int> CountDoctorVisitsAsync(int doctorId);

    Task<decimal> CalculateTotalBillingAsync(int patientId);

    Task<bool> PatientHasVisitOnDateAsync(int patientId, DateTime visitDate, int? excludingVisitId = null);
}
