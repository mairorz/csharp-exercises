using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServicesManagement.Api.Data;
using ServicesManagement.Api.Dtos;
using ServicesManagement.Api.Models;

namespace ServicesManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WorkOrderController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public WorkOrderController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet("getAll")]
    public async Task<ActionResult<IEnumerable<GetWorkOrderDto>>> GetAll()
    {
        var result = await _context.WorkOrders
            .Include(wo => wo.Client)
            .Include(wo => wo.Service)
            .ToListAsync();
            
        var resultDto = _mapper.Map<IEnumerable<GetWorkOrderDto>>(result);
        return Ok(resultDto);
    }
    
    [HttpGet("getAll/byStatus/{status}")]
    public async Task<ActionResult<IEnumerable<GetWorkOrderDto>>> GetAllByStatus(string status)
    {
        if (string.IsNullOrWhiteSpace(status))
        {
            return BadRequest("El estado de la orden de trabajo no puede estar vacío");
        }

        var result = await _context.WorkOrders
            .Include(wo => wo.Client)
            .Include(wo => wo.Service)
            .Where(wo => wo.Status.ToLower() == status.ToLower())
            .ToListAsync();
            
        var resultDto = _mapper.Map<IEnumerable<GetWorkOrderDto>>(result);
        return Ok(resultDto);
    }

    [HttpGet("getById/{id:int}")]
    public async Task<ActionResult<GetWorkOrderDto>> GetById(int id)
    {
        var result = await _context.WorkOrders
            .Include(wo => wo.Client)
            .Include(wo => wo.Service)
            .Where(wo => wo.Id == id)
            .FirstOrDefaultAsync();

        if (result == null)
        {
            return NotFound();
        }

        var resultDto = _mapper.Map<GetWorkOrderDto>(result);
        return Ok(resultDto);
    }

    [HttpPost("add")]
    public async Task<ActionResult<WorkOrder>> Add(AddWorkOrderDto workOrderDto)
    {
        var client = await _context.Clients.FindAsync(workOrderDto.ClientId);
        if (client == null || client.Status == "inactive")
        {
            return NotFound
            (
                new
                {
                    field = "clientId",
                    msg = "El cliente asociado no existe o está inactivo"
                }
            );
        }
        
        var service = await _context.Services.FindAsync(workOrderDto.ServiceId);
        if (service == null || service.Status == "inactive")
        {
            return NotFound
            (
                new
                {
                    field = "serviceId",
                    msg = "El servicio asociado no existe o está inactivo"
                }
            );
        }

        var newWorkOrder = _mapper.Map<WorkOrder>(workOrderDto);
        newWorkOrder.Status = "scheduled"; // Valor por defecto
        
        _context.WorkOrders.Add(newWorkOrder);
        await _context.SaveChangesAsync();
        
        var workOrderResponse = await _context.WorkOrders
            .Include(wo => wo.Client)
            .Include(wo => wo.Service)
            .Where(wo => wo.Id == newWorkOrder.Id)
            .FirstOrDefaultAsync();

        var workOrderResponseDto = _mapper.Map<GetWorkOrderDto>(workOrderResponse);

        return CreatedAtAction
        (
            nameof(Add),
            new { id = newWorkOrder.Id },
            workOrderResponseDto
        );
    }

    [HttpPut("update/{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateWorkOrderDto updatedWorkOrderDto)
    {
        var workOrder = await _context.WorkOrders.FindAsync(id);

        if (workOrder == null)
        { 
            return NotFound();   
        }

        if (workOrder.Status == "completed" || workOrder.Status == "canceled")
        {
            return Problem
            (
                statusCode: 409,
                title: "Orden de trabajo cerrada",
                detail: "No se puede modificar una orden de trabajo con estado 'completed' o 'canceled'"
            );
        }
        
        if (updatedWorkOrderDto.Notes != null) 
        {
            workOrder.Notes = updatedWorkOrderDto.Notes;
        }
        if (updatedWorkOrderDto.ScheduledDate.HasValue) 
        {
            workOrder.ScheduledDate = updatedWorkOrderDto.ScheduledDate.Value;
        }

        await _context.SaveChangesAsync();

        return NoContent();
    }
    
    [HttpPatch("changeStatus/{id:int}")]
    public async Task<IActionResult> ChangeStatus(int id, [FromQuery] string newStatus)
    {
        var workOrder = await _context.WorkOrders.FindAsync(id);

        if (workOrder == null)
        {
            return NotFound();
        }

        var validStatuses = new[] { "scheduled", "in_progress", "completed", "canceled" };
        if (!validStatuses.Contains(newStatus.ToLower()))
        {
            return BadRequest("El estado de la orden de trabajo es inválido.");
        }

        workOrder.Status = newStatus.ToLower();
        await _context.SaveChangesAsync();

        return NoContent();
    }
    
    [HttpDelete("delete/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var workOrder = await _context.WorkOrders.FindAsync(id);

        if (workOrder == null)
        {
            return NotFound();
        }
        
        if (workOrder.Status != "scheduled")
        {
            return Conflict(
                new
                {
                    msg = "Solo se pueden eliminar órdenes de trabajo con estado 'scheduled'"
                }
            );
        }

        _context.WorkOrders.Remove(workOrder);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}