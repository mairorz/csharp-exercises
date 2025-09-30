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
public class ConsumptionController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ConsumptionController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet("getAll")]
    public async Task<ActionResult<IEnumerable<GetConsumptionDto>>> GetAll()
    {
        var result = await _context.Consumptions
            .Include(c => c.WorkOrder)
                .ThenInclude(w => w.Client)
            .Include(c => c.WorkOrder)
                .ThenInclude(w => w.Service)
            .Include(c => c.Input)
                .ThenInclude(i => i.Category)
            .ToListAsync();

        var resultDto = _mapper.Map<IEnumerable<GetConsumptionDto>>(result);
        return Ok(resultDto);
    }
    
    [HttpGet("getAll/byWorkOrder/{workOrderId:int}")]
    public async Task<ActionResult<IEnumerable<GetConsumptionDto>>> GetAllByWorkOrder(int workOrderId)
    {
        var workOrderExist = await _context.WorkOrders.AnyAsync(wo => wo.Id == workOrderId);
        if (!workOrderExist)
        {
            return NotFound("La orden de trabajo especificada no existe.");
        }

        var result = await _context.Consumptions
            .Include(c => c.WorkOrder)
                .ThenInclude(w => w.Client)
            .Include(c => c.WorkOrder)
                .ThenInclude(w => w.Service)
            .Include(c => c.Input)
                .ThenInclude(i => i.Category)
            .Where(c => c.WorkOrderId == workOrderId)
            .ToListAsync();
            
        var resultDto = _mapper.Map<IEnumerable<GetConsumptionDto>>(result);
        return Ok(resultDto);
    }

    [HttpGet("getById/{id:int}")]
    public async Task<ActionResult<GetConsumptionDto>> GetById(int id)
    {
        var result = await _context.Consumptions
            .Include(c => c.WorkOrder)
                .ThenInclude(w => w.Client)
            .Include(c => c.WorkOrder)
                .ThenInclude(w => w.Service)
            .Include(c => c.Input)
                .ThenInclude(i => i.Category)
            .Where(c => c.Id == id)
            .FirstOrDefaultAsync();

        if (result == null)
        {
            return NotFound();
        }

        var resultDto = _mapper.Map<GetConsumptionDto>(result);
        return Ok(resultDto);
    }

    [HttpPost("add")]
    public async Task<ActionResult<Consumption>> Add(AddConsumptionDto consumptionDto)
    {
        var workOrder = await _context.WorkOrders.FindAsync(consumptionDto.WorkOrderId);
        if (workOrder == null)
        {
            return NotFound
            (
                new
                {
                    field = "workOrderId",
                    msg = "La orden de trabajo asociada no existe"
                }
            );
        }
        
        if (workOrder.Status != "in_progress")
        {
            return Conflict(
                new
                {
                    field = "workOrderId",
                    msg = "Solo se pueden agregar consumos a órdenes de trabajo en estado 'in_progress'"
                }
            );
        }
        
        var inputExist = await _context.Inputs.AnyAsync(i => i.Id == consumptionDto.InputId);
        if (!inputExist)
        {
            return NotFound
            (
                new
                {
                    field = "inputId",
                    msg = "El insumo asociado no existe"
                }
            );
        }
        
        var newConsumption = _mapper.Map<Consumption>(consumptionDto);
        newConsumption.ConsumedDate = DateTime.UtcNow;
        
        _context.Consumptions.Add(newConsumption);
        await _context.SaveChangesAsync();
        
        var consumptionResponse = await _context.Consumptions
            .Include(c => c.WorkOrder)
            .Include(c => c.Input)
            .Where(c => c.Id == newConsumption.Id)
            .FirstOrDefaultAsync();

        var consumptionResponseDto = _mapper.Map<GetConsumptionDto>(consumptionResponse);

        return CreatedAtAction
        (
            nameof(Add),
            new { id = newConsumption.Id },
            consumptionResponseDto
        );
    }

    [HttpPut("update/{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateConsumptionDto updatedConsumptionDto)
    {
        var consumption = await _context.Consumptions.Include(c => c.WorkOrder).FirstOrDefaultAsync(c => c.Id == id);

        if (consumption == null)
        { 
            return NotFound();   
        }
        
        if (consumption.WorkOrder != null && consumption.WorkOrder.Status != "in_progress")
        {
            return Conflict(
                new
                {
                    msg = "Solo se pueden modificar consumos de órdenes de trabajo en estado 'in_progress'"
                }
            );
        }
        
        if (updatedConsumptionDto.Quantity.HasValue) 
        {
            consumption.Quantity = updatedConsumptionDto.Quantity.Value;
        }

        await _context.SaveChangesAsync();

        return NoContent();
    }
    
    [HttpDelete("delete/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var consumption = await _context.Consumptions.Include(c => c.WorkOrder).FirstOrDefaultAsync(c => c.Id == id);

        if (consumption == null)
        {
            return NotFound();
        }

        if (consumption.WorkOrder != null && consumption.WorkOrder.Status != "in_progress")
        {
            return Conflict(
                new
                {
                    msg = "Solo se pueden eliminar consumos de órdenes de trabajo en estado 'in_progress'"
                }
            );
        }

        _context.Consumptions.Remove(consumption);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}