using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServicesManagement.Api.Data;
using ServicesManagement.Api.Dtos;
using ServicesManagement.Api.Models;

namespace ServicesManagement.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class ServiceController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ServiceController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet("getAll")]
    public async Task<ActionResult<IEnumerable<GetServiceDto>>> GetAll()
    {
        var result = await _context.Services
            .Where(s => s.Status == "active")
            .ToListAsync();
            
        var resultDto = _mapper.Map<IEnumerable<GetServiceDto>>(result);
        return Ok(resultDto);
    }

    [HttpGet("getAll/inactive")]
    public async Task<ActionResult<IEnumerable<GetServiceDto>>> GetAllInactive()
    {
        var result = await _context.Services
            .Where(s => s.Status == "inactive")
            .ToListAsync();

        var resultDto = _mapper.Map<IEnumerable<GetServiceDto>>(result);
        return Ok(resultDto);
    }

    [HttpGet("getById/{id:int}")]
    public async Task<ActionResult<GetServiceDto>> GetById(int id)
    {
        var result = await _context.Services
            .Where(s => s.Id == id)
            .FirstOrDefaultAsync();

        if (result == null)
        {
            return NotFound();
        }

        if (result.Status == "inactive")
        {
            return NotFound(
                new
                {
                    id,
                    msg = "Servicio inactivo"
                }
            );
        }

        var resultDto = _mapper.Map<GetServiceDto>(result);
        return Ok(resultDto);
    }

    [HttpPost("add")]
    public async Task<ActionResult<Service>> Add(AddServiceDto serviceDto)
    {
        var nameExist = await _context.Services.AnyAsync(s => s.Name == serviceDto.Name);
        if (nameExist)
        {
            return Conflict
            (
                new
                {
                    field = "name",
                    msg = "Ya existe un servicio con este nombre"
                }
            );    
        }

        var newService = _mapper.Map<Service>(serviceDto);
        
        newService.CreatedAt = DateTime.UtcNow;

        _context.Services.Add(newService);
        await _context.SaveChangesAsync();
        
        var serviceResponseDto = _mapper.Map<GetServiceDto>(newService);

        return CreatedAtAction
        (
            nameof(Add),
            new { id = newService.Id },
            serviceResponseDto
        );
    }

    [HttpPut("update/{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateServiceDto updatedServiceDto)
    {
        var service = await _context.Services.FindAsync(id);

        if (service == null)
        { 
            return NotFound();   
        }

        if (service.Status == "inactive")
        {
            return Problem
            (
                statusCode: 409,
                title: "Servicio inactivo",
                detail: "No se puede modificar mientras esté inactivo"
            );
        }

        if (updatedServiceDto.Name != null && updatedServiceDto.Name != service.Name)
        {
            var nameExist = await _context.Services.AnyAsync(s => s.Name == updatedServiceDto.Name && s.Id != id);
            if (nameExist)
            {
                return Conflict
                (
                    new
                    {
                        field = "name",
                        msg = "Ya existe un servicio con este nombre"
                    }
                );    
            }
        }
        
        if (updatedServiceDto.Name != null) 
        {
            service.Name = updatedServiceDto.Name;
        }
        if (updatedServiceDto.Description != null) 
        {
            service.Description = updatedServiceDto.Description;
        }
        if (updatedServiceDto.Price.HasValue) 
        {
            service.Price = updatedServiceDto.Price.Value;
        }

        service.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }
    
    [HttpPatch("disable/{id:int}")]
    public async Task<IActionResult> Disable(int id)
    {
        var service = await _context.Services.FindAsync(id);

        if (service == null)
        {
            return NotFound();
        }

        if (service.Status == "inactive")
        {
            return NoContent();
        }

        service.Status = "inactive";
        service.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }
}