using HealthcareBillingSystem.Application.DTOs;
using HealthcareBillingSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareBillingSystem.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class PatientsController : ControllerBase
{
    private readonly IPatientService _patientService;

    public PatientsController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PatientReadDto>>> GetAll()
    {
        return Ok(await _patientService.GetAllPatientsAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PatientReadDto>> GetById(int id)
    {
        return Ok(await _patientService.GetPatientByIdAsync(id));
    }

    [HttpPost]
    public async Task<ActionResult<PatientReadDto>> Create(PatientCreateDto dto)
    {
        var patient = await _patientService.CreatePatientAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = patient.Id }, patient);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, PatientUpdateDto dto)
    {
        await _patientService.UpdatePatientAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _patientService.DeletePatientAsync(id);
        return NoContent();
    }
}
