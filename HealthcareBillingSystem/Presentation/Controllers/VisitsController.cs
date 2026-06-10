using HealthcareBillingSystem.Application.DTOs;
using HealthcareBillingSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareBillingSystem.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,Doctor")]
public class VisitsController : ControllerBase
{
    private readonly IVisitService _visitService;

    public VisitsController(IVisitService visitService)
    {
        _visitService = visitService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResponseDto<VisitReadDto>>> GetAll([FromQuery] VisitFilterDto filter)
    {
        return Ok(await _visitService.GetAllVisitsAsync(filter));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<VisitReadDto>> GetById(int id)
    {
        return Ok(await _visitService.GetVisitByIdAsync(id));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<VisitReadDto>> Create(VisitCreateDto dto)
    {
        var visit = await _visitService.CreateVisitAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = visit.Id }, visit);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, VisitUpdateDto dto)
    {
        await _visitService.UpdateVisitAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        await _visitService.DeleteVisitAsync(id);
        return NoContent();
    }

    [HttpGet("billing/{patientId:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<BillingSummaryDto>> GetBilling(int patientId)
    {
        return Ok(await _visitService.CalculateTotalBillingForPatient(patientId));
    }

    [HttpGet("doctor-statistics/{doctorId:int}")]
    public async Task<ActionResult<DoctorStatisticsDto>> GetDoctorStatistics(int doctorId)
    {
        return Ok(await _visitService.CountDoctorVisits(doctorId));
    }
}
