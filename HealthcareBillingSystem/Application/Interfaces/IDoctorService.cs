using HealthcareBillingSystem.Application.DTOs;

namespace HealthcareBillingSystem.Application.Interfaces;

public interface IDoctorService
{
    Task<IReadOnlyList<DoctorReadDto>> GetAllDoctorsAsync();

    Task<DoctorReadDto> GetDoctorByIdAsync(int id);

    Task<DoctorReadDto> CreateDoctorAsync(DoctorCreateDto dto);
}
