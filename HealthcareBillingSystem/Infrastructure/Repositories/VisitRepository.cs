using HealthcareBillingSystem.Domain.Entities;
using HealthcareBillingSystem.Domain.Interfaces;
using HealthcareBillingSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HealthcareBillingSystem.Infrastructure.Repositories;

public class VisitRepository : Repository<Visit>, IVisitRepository
{
    public VisitRepository(HealthDbContext context)
        : base(context)
    {
    }

    public override async Task<IReadOnlyList<Visit>> GetAllAsync()
    {
        return await BaseVisitQuery().ToListAsync();
    }

    public override async Task<Visit?> GetByIdAsync(int id)
    {
        return await BaseVisitQuery()
            .FirstOrDefaultAsync(visit => visit.Id == id);
    }

    public override async Task<Visit> AddAsync(Visit entity)
    {
        await DbSet.AddAsync(entity);
        await Context.SaveChangesAsync();

        return await GetByIdAsync(entity.Id) ?? entity;
    }

    public async Task<IReadOnlyList<Visit>> GetFilteredVisitsAsync(
        int? doctorId,
        DateTime? visitDateFrom,
        DateTime? visitDateTo,
        decimal? minFee,
        decimal? maxFee,
        string? sortBy,
        string? sortDirection,
        int pageNumber,
        int pageSize,
        string? doctorApplicationUserId = null)
    {
        var query = ApplyFilters(doctorId, visitDateFrom, visitDateTo, minFee, maxFee, doctorApplicationUserId);
        query = ApplySorting(query, sortBy, sortDirection);

        return await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<int> CountFilteredVisitsAsync(
        int? doctorId,
        DateTime? visitDateFrom,
        DateTime? visitDateTo,
        decimal? minFee,
        decimal? maxFee,
        string? doctorApplicationUserId = null)
    {
        return await ApplyFilters(doctorId, visitDateFrom, visitDateTo, minFee, maxFee, doctorApplicationUserId)
            .CountAsync();
    }

    public async Task<IReadOnlyList<Visit>> GetVisitsByDoctorAsync(int doctorId)
    {
        return await BaseVisitQuery()
            .Where(visit => visit.DoctorId == doctorId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<int> CountDoctorVisitsAsync(int doctorId)
    {
        return await DbSet.CountAsync(visit => visit.DoctorId == doctorId);
    }

    public async Task<decimal> CalculateTotalBillingAsync(int patientId)
    {
        return await DbSet
            .Where(visit => visit.PatientId == patientId)
            .SumAsync(visit => (decimal?)visit.Fee) ?? 0;
    }

    public async Task<bool> PatientHasVisitOnDateAsync(int patientId, DateTime visitDate, int? excludingVisitId = null)
    {
        var visitDay = visitDate.Date;

        return await DbSet.AnyAsync(visit =>
            visit.PatientId == patientId
            && visit.VisitDate.Date == visitDay
            && (!excludingVisitId.HasValue || visit.Id != excludingVisitId.Value));
    }

    private IQueryable<Visit> ApplyFilters(
        int? doctorId,
        DateTime? visitDateFrom,
        DateTime? visitDateTo,
        decimal? minFee,
        decimal? maxFee,
        string? doctorApplicationUserId)
    {
        var query = BaseVisitQuery();

        if (doctorId.HasValue)
        {
            query = query.Where(visit => visit.DoctorId == doctorId.Value);
        }

        if (visitDateFrom.HasValue)
        {
            query = query.Where(visit => visit.VisitDate.Date >= visitDateFrom.Value.Date);
        }

        if (visitDateTo.HasValue)
        {
            query = query.Where(visit => visit.VisitDate.Date <= visitDateTo.Value.Date);
        }

        if (minFee.HasValue)
        {
            query = query.Where(visit => visit.Fee >= minFee.Value);
        }

        if (maxFee.HasValue)
        {
            query = query.Where(visit => visit.Fee <= maxFee.Value);
        }

        if (!string.IsNullOrWhiteSpace(doctorApplicationUserId))
        {
            query = query.Where(visit => visit.Doctor.ApplicationUserId == doctorApplicationUserId);
        }

        return query;
    }

    private static IQueryable<Visit> ApplySorting(IQueryable<Visit> query, string? sortBy, string? sortDirection)
    {
        var descending = sortDirection?.Equals("desc", StringComparison.OrdinalIgnoreCase) == true;

        if (sortBy?.Equals("Fee", StringComparison.OrdinalIgnoreCase) == true)
        {
            return descending
                ? query.OrderByDescending(visit => visit.Fee)
                : query.OrderBy(visit => visit.Fee);
        }

        return descending
            ? query.OrderByDescending(visit => visit.VisitDate)
            : query.OrderBy(visit => visit.VisitDate);
    }

    private IQueryable<Visit> BaseVisitQuery()
    {
        return DbSet
            .Include(visit => visit.Patient)
            .Include(visit => visit.Doctor);
    }
}
