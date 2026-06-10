using HealthcareBillingSystem.Application.DTOs;

namespace HealthcareBillingSystem.Application.Interfaces;

public interface IPatientService
{
    Task<IReadOnlyList<PatientReadDto>> GetAllPatientsAsync();

    Task<PatientReadDto> GetPatientByIdAsync(int id);

    Task<PatientReadDto> CreatePatientAsync(PatientCreateDto dto);

    Task UpdatePatientAsync(int id, PatientUpdateDto dto);

    Task DeletePatientAsync(int id);
}
