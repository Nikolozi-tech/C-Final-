using HealthcareBillingSystem.Application.DTOs;
using HealthcareBillingSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareBillingSystem.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class DoctorsController : ControllerBase
{
    private readonly IDoctorService _doctorService;

    public DoctorsController(IDoctorService doctorService)
    {
        _doctorService = doctorService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<DoctorReadDto>>> GetAll()
    {
        return Ok(await _doctorService.GetAllDoctorsAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<DoctorReadDto>> GetById(int id)
    {
        return Ok(await _doctorService.GetDoctorByIdAsync(id));
    }

    [HttpPost]
    public async Task<ActionResult<DoctorReadDto>> Create(DoctorCreateDto dto)
    {
        var doctor = await _doctorService.CreateDoctorAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = doctor.Id }, doctor);
    }
}
