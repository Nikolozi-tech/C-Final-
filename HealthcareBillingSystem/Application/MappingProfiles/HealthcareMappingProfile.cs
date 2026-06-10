using AutoMapper;
using HealthcareBillingSystem.Application.DTOs;
using HealthcareBillingSystem.Domain.Entities;

namespace HealthcareBillingSystem.Application.MappingProfiles;

public class HealthcareMappingProfile : Profile
{
    public HealthcareMappingProfile()
    {
        CreateMap<PatientCreateDto, Patient>();
        CreateMap<PatientUpdateDto, Patient>();
        CreateMap<Patient, PatientReadDto>();

        CreateMap<DoctorCreateDto, Doctor>();
        CreateMap<Doctor, DoctorReadDto>();

        CreateMap<VisitCreateDto, Visit>();
        CreateMap<VisitUpdateDto, Visit>();
        CreateMap<Visit, VisitReadDto>()
            .ForMember(destination => destination.PatientName, options => options.MapFrom(source => source.Patient.FullName))
            .ForMember(destination => destination.DoctorName, options => options.MapFrom(source => source.Doctor.FullName));
    }
}
