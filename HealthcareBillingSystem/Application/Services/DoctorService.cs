using AutoMapper;
using HealthcareBillingSystem.Application.DTOs;
using HealthcareBillingSystem.Application.Exceptions;
using HealthcareBillingSystem.Application.Interfaces;
using HealthcareBillingSystem.Domain.Entities;
using HealthcareBillingSystem.Domain.Interfaces;

namespace HealthcareBillingSystem.Application.Services;

public class DoctorService : IDoctorService
{
    private readonly IRepository<Doctor> _doctorRepository;
    private readonly IMapper _mapper;

    public DoctorService(IRepository<Doctor> doctorRepository, IMapper mapper)
    {
        _doctorRepository = doctorRepository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<DoctorReadDto>> GetAllDoctorsAsync()
    {
        var doctors = await _doctorRepository.GetAllAsync();
        return _mapper.Map<IReadOnlyList<DoctorReadDto>>(doctors);
    }

    public async Task<DoctorReadDto> GetDoctorByIdAsync(int id)
    {
        var doctor = await GetDoctorEntityAsync(id);
        return _mapper.Map<DoctorReadDto>(doctor);
    }

    public async Task<DoctorReadDto> CreateDoctorAsync(DoctorCreateDto dto)
    {
        ValidateDoctor(dto);

        var doctor = _mapper.Map<Doctor>(dto);
        var createdDoctor = await _doctorRepository.AddAsync(doctor);
        return _mapper.Map<DoctorReadDto>(createdDoctor);
    }

    private async Task<Doctor> GetDoctorEntityAsync(int id)
    {
        var doctor = await _doctorRepository.GetByIdAsync(id);
        return doctor ?? throw new NotFoundException($"Doctor with id {id} was not found.");
    }

    private static void ValidateDoctor(DoctorCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.FullName))
        {
            throw new BusinessRuleValidationException("Doctor full name is required.");
        }

        if (string.IsNullOrWhiteSpace(dto.Specialization))
        {
            throw new BusinessRuleValidationException("Doctor specialization must not be empty.");
        }

        if (string.IsNullOrWhiteSpace(dto.ApplicationUserId))
        {
            throw new BusinessRuleValidationException("Doctor ApplicationUserId is required.");
        }
    }
}
