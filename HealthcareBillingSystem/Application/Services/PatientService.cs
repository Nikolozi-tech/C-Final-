using AutoMapper;
using HealthcareBillingSystem.Application.DTOs;
using HealthcareBillingSystem.Application.Exceptions;
using HealthcareBillingSystem.Application.Interfaces;
using HealthcareBillingSystem.Domain.Entities;
using HealthcareBillingSystem.Domain.Interfaces;

namespace HealthcareBillingSystem.Application.Services;

public class PatientService : IPatientService
{
    private readonly IRepository<Patient> _patientRepository;
    private readonly IMapper _mapper;

    public PatientService(IRepository<Patient> patientRepository, IMapper mapper)
    {
        _patientRepository = patientRepository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<PatientReadDto>> GetAllPatientsAsync()
    {
        var patients = await _patientRepository.GetAllAsync();
        return _mapper.Map<IReadOnlyList<PatientReadDto>>(patients);
    }

    public async Task<PatientReadDto> GetPatientByIdAsync(int id)
    {
        var patient = await GetPatientEntityAsync(id);
        return _mapper.Map<PatientReadDto>(patient);
    }

    public async Task<PatientReadDto> CreatePatientAsync(PatientCreateDto dto)
    {
        ValidatePatient(dto.FullName, dto.BirthDate);

        var patient = _mapper.Map<Patient>(dto);
        var createdPatient = await _patientRepository.AddAsync(patient);
        return _mapper.Map<PatientReadDto>(createdPatient);
    }

    public async Task UpdatePatientAsync(int id, PatientUpdateDto dto)
    {
        ValidatePatient(dto.FullName, dto.BirthDate);

        var patient = await GetPatientEntityAsync(id);
        _mapper.Map(dto, patient);
        await _patientRepository.UpdateAsync(patient);
    }

    public async Task DeletePatientAsync(int id)
    {
        var patient = await GetPatientEntityAsync(id);
        await _patientRepository.DeleteAsync(patient);
    }

    private async Task<Patient> GetPatientEntityAsync(int id)
    {
        var patient = await _patientRepository.GetByIdAsync(id);
        return patient ?? throw new NotFoundException($"Patient with id {id} was not found.");
    }

    private static void ValidatePatient(string fullName, DateTime birthDate)
    {
        if (string.IsNullOrWhiteSpace(fullName))
        {
            throw new BusinessRuleValidationException("Patient full name is required.");
        }

        if (birthDate.Date > DateTime.UtcNow.Date)
        {
            throw new BusinessRuleValidationException("Patient birth date cannot be in the future.");
        }
    }
}
