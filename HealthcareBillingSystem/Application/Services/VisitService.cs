using AutoMapper;
using HealthcareBillingSystem.Application.DTOs;
using HealthcareBillingSystem.Application.Exceptions;
using HealthcareBillingSystem.Application.Interfaces;
using HealthcareBillingSystem.Domain.Entities;
using HealthcareBillingSystem.Domain.Interfaces;

namespace HealthcareBillingSystem.Application.Services;

public class VisitService : IVisitService
{
    private const int MaxPageSize = 100;

    private readonly IVisitRepository _visitRepository;
    private readonly IRepository<Patient> _patientRepository;
    private readonly IRepository<Doctor> _doctorRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public VisitService(
        IVisitRepository visitRepository,
        IRepository<Patient> patientRepository,
        IRepository<Doctor> doctorRepository,
        ICurrentUserService currentUserService,
        IMapper mapper)
    {
        _visitRepository = visitRepository;
        _patientRepository = patientRepository;
        _doctorRepository = doctorRepository;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<PagedResponseDto<VisitReadDto>> GetAllVisitsAsync(VisitFilterDto filter)
    {
        NormalizeFilter(filter);
        ValidateFilter(filter);

        var doctorApplicationUserId = GetDoctorApplicationUserScope();
        var visits = await _visitRepository.GetFilteredVisitsAsync(
            filter.DoctorId,
            filter.VisitDateFrom,
            filter.VisitDateTo,
            filter.MinFee,
            filter.MaxFee,
            filter.SortBy,
            filter.SortDirection,
            filter.PageNumber,
            filter.PageSize,
            doctorApplicationUserId);

        var totalCount = await _visitRepository.CountFilteredVisitsAsync(
            filter.DoctorId,
            filter.VisitDateFrom,
            filter.VisitDateTo,
            filter.MinFee,
            filter.MaxFee,
            doctorApplicationUserId);

        return new PagedResponseDto<VisitReadDto>
        {
            Data = _mapper.Map<IReadOnlyList<VisitReadDto>>(visits),
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)filter.PageSize),
            CurrentPage = filter.PageNumber
        };
    }

    public async Task<VisitReadDto> GetVisitByIdAsync(int id)
    {
        var visit = await GetVisitEntityAsync(id);
        EnsureDoctorCanAccess(visit.Doctor);
        return _mapper.Map<VisitReadDto>(visit);
    }

    public async Task<VisitReadDto> CreateVisitAsync(VisitCreateDto dto)
    {
        var (patient, doctor) = await ValidateVisitAsync(dto.PatientId, dto.DoctorId, dto.VisitDate, dto.Fee);

        var visit = _mapper.Map<Visit>(dto);
        visit.Patient = patient;
        visit.Doctor = doctor;

        var createdVisit = await _visitRepository.AddAsync(visit);
        return _mapper.Map<VisitReadDto>(createdVisit);
    }

    public async Task UpdateVisitAsync(int id, VisitUpdateDto dto)
    {
        var visit = await GetVisitEntityAsync(id);
        var (patient, doctor) = await ValidateVisitAsync(dto.PatientId, dto.DoctorId, dto.VisitDate, dto.Fee, id);

        _mapper.Map(dto, visit);
        visit.Patient = patient;
        visit.Doctor = doctor;

        await _visitRepository.UpdateAsync(visit);
    }

    public async Task DeleteVisitAsync(int id)
    {
        var visit = await GetVisitEntityAsync(id);
        await _visitRepository.DeleteAsync(visit);
    }

    public async Task<BillingSummaryDto> CalculateTotalBillingForPatient(int patientId)
    {
        var patient = await GetPatientEntityAsync(patientId);
        var totalPaid = await _visitRepository.CalculateTotalBillingAsync(patientId);

        return new BillingSummaryDto
        {
            PatientId = patient.Id,
            PatientName = patient.FullName,
            TotalPaid = totalPaid
        };
    }

    public async Task<DoctorStatisticsDto> CountDoctorVisits(int doctorId)
    {
        var doctor = await GetDoctorEntityAsync(doctorId);
        EnsureDoctorCanAccess(doctor);

        return new DoctorStatisticsDto
        {
            DoctorId = doctor.Id,
            DoctorName = doctor.FullName,
            TotalVisits = await _visitRepository.CountDoctorVisitsAsync(doctorId)
        };
    }

    private async Task<(Patient Patient, Doctor Doctor)> ValidateVisitAsync(
        int patientId,
        int doctorId,
        DateTime visitDate,
        decimal fee,
        int? excludingVisitId = null)
    {
        if (fee <= 0)
        {
            throw new BusinessRuleValidationException("Visit fee must be greater than zero.");
        }

        if (fee >= 1000)
        {
            throw new BusinessRuleValidationException("Visit fee must be less than 1000.");
        }

        var patient = await GetPatientEntityAsync(patientId);
        var doctor = await GetDoctorEntityAsync(doctorId);

        if (string.IsNullOrWhiteSpace(doctor.Specialization))
        {
            throw new BusinessRuleValidationException("Doctor specialization must not be empty.");
        }

        var hasVisitOnDate = await _visitRepository.PatientHasVisitOnDateAsync(patientId, visitDate, excludingVisitId);
        if (hasVisitOnDate)
        {
            throw new BusinessRuleValidationException("The same patient cannot have more than one visit on the same day.");
        }

        return (patient, doctor);
    }

    private async Task<Visit> GetVisitEntityAsync(int id)
    {
        var visit = await _visitRepository.GetByIdAsync(id);
        return visit ?? throw new NotFoundException($"Visit with id {id} was not found.");
    }

    private async Task<Patient> GetPatientEntityAsync(int id)
    {
        var patient = await _patientRepository.GetByIdAsync(id);
        return patient ?? throw new NotFoundException($"Patient with id {id} was not found.");
    }

    private async Task<Doctor> GetDoctorEntityAsync(int id)
    {
        var doctor = await _doctorRepository.GetByIdAsync(id);
        return doctor ?? throw new NotFoundException($"Doctor with id {id} was not found.");
    }

    private string? GetDoctorApplicationUserScope()
    {
        if (_currentUserService.IsInRole("Doctor") && !_currentUserService.IsInRole("Admin"))
        {
            return _currentUserService.UserId
                ?? throw new BusinessRuleValidationException("Authenticated doctor user id was not found.");
        }

        return null;
    }

    private void EnsureDoctorCanAccess(Doctor doctor)
    {
        var doctorApplicationUserId = GetDoctorApplicationUserScope();
        if (doctorApplicationUserId is not null && doctor.ApplicationUserId != doctorApplicationUserId)
        {
            throw new BusinessRuleValidationException("Doctor users can access only their own visits.");
        }
    }

    private static void NormalizeFilter(VisitFilterDto filter)
    {
        filter.PageNumber = filter.PageNumber < 1 ? 1 : filter.PageNumber;
        filter.PageSize = filter.PageSize < 1 ? 10 : Math.Min(filter.PageSize, MaxPageSize);
    }

    private static void ValidateFilter(VisitFilterDto filter)
    {
        if (filter.MinFee.HasValue && filter.MinFee.Value < 0)
        {
            throw new BusinessRuleValidationException("Minimum fee cannot be negative.");
        }

        if (filter.MaxFee.HasValue && filter.MaxFee.Value < 0)
        {
            throw new BusinessRuleValidationException("Maximum fee cannot be negative.");
        }

        if (filter.MinFee.HasValue && filter.MaxFee.HasValue && filter.MinFee.Value > filter.MaxFee.Value)
        {
            throw new BusinessRuleValidationException("Minimum fee cannot be greater than maximum fee.");
        }

        if (filter.VisitDateFrom.HasValue && filter.VisitDateTo.HasValue && filter.VisitDateFrom.Value.Date > filter.VisitDateTo.Value.Date)
        {
            throw new BusinessRuleValidationException("VisitDateFrom cannot be after VisitDateTo.");
        }

        if (!string.IsNullOrWhiteSpace(filter.SortBy)
            && !filter.SortBy.Equals("Fee", StringComparison.OrdinalIgnoreCase)
            && !filter.SortBy.Equals("VisitDate", StringComparison.OrdinalIgnoreCase))
        {
            throw new BusinessRuleValidationException("SortBy must be either Fee or VisitDate.");
        }

        if (!string.IsNullOrWhiteSpace(filter.SortDirection)
            && !filter.SortDirection.Equals("asc", StringComparison.OrdinalIgnoreCase)
            && !filter.SortDirection.Equals("desc", StringComparison.OrdinalIgnoreCase))
        {
            throw new BusinessRuleValidationException("SortDirection must be asc or desc.");
        }
    }
}
