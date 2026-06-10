using HealthcareBillingSystem.Application.Interfaces;
using HealthcareBillingSystem.Application.MappingProfiles;
using HealthcareBillingSystem.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HealthcareBillingSystem.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(HealthcareMappingProfile).Assembly);
        services.AddScoped<IPatientService, PatientService>();
        services.AddScoped<IDoctorService, DoctorService>();
        services.AddScoped<IVisitService, VisitService>();

        return services;
    }
}
