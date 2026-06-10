using HealthcareBillingSystem.Application.DTOs;

namespace HealthcareBillingSystem.Application.Interfaces;

public interface IVisitService
{
    Task<PagedResponseDto<VisitReadDto>> GetAllVisitsAsync(VisitFilterDto filter);

    Task<VisitReadDto> GetVisitByIdAsync(int id);

    Task<VisitReadDto> CreateVisitAsync(VisitCreateDto dto);

    Task UpdateVisitAsync(int id, VisitUpdateDto dto);

    Task DeleteVisitAsync(int id);

    Task<BillingSummaryDto> CalculateTotalBillingForPatient(int patientId);

    Task<DoctorStatisticsDto> CountDoctorVisits(int doctorId);
}
