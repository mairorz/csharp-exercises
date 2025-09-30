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
public class InvoiceDetailController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public InvoiceDetailController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet("getAll/byInvoice/{invoiceId:int}")]
    public async Task<ActionResult<IEnumerable<GetInvoiceDetailDto>>> GetAllByInvoice(int invoiceId)
    {
        var result = await _context.InvoiceDetails
            .Include(id => id.Service)
            .Where(id => id.InvoiceId == invoiceId)
            .ToListAsync();
            
        if (result.Count == 0)
        {
            return NotFound("No se encontraron detalles para la factura especificada.");
        }

        var resultDto = _mapper.Map<IEnumerable<GetInvoiceDetailDto>>(result);
        return Ok(resultDto);
    }
    
    [HttpGet("getById/{id:int}")]
    public async Task<ActionResult<GetInvoiceDetailDto>> GetById(int id)
    {
        var result = await _context.InvoiceDetails
            .Include(d => d.Service)
            .Where(d => d.Id == id)
            .FirstOrDefaultAsync();

        if (result == null)
        {
            return NotFound();
        }

        var resultDto = _mapper.Map<GetInvoiceDetailDto>(result);
        return Ok(resultDto);
    }

    [HttpPost("add")]
    public async Task<ActionResult<InvoiceDetail>> Add(AddInvoiceDetailDto detailDto)
    {
        var invoice = await _context.Invoices.FindAsync(detailDto.InvoiceId);
        if (invoice == null)
        {
            return NotFound(
                new
                {
                    field = "invoiceId",
                    msg = "La factura asociada no existe"
                }
            );
        }

        if (invoice.Status != "draft")
        {
            return Conflict(
                new
                {
                    field = "invoiceId",
                    msg = "No se puede agregar un detalle a una factura que no esté en estado 'draft'"
                }
            );
        }
        
        var service = await _context.Services.FindAsync(detailDto.ServiceId);
        if (service == null || service.Status == "inactive")
        {
            return NotFound(
                new
                {
                    field = "serviceId",
                    msg = "El servicio asociado no existe o está inactivo"
                }
            );
        }

        var newDetail = _mapper.Map<InvoiceDetail>(detailDto);
        
        _context.InvoiceDetails.Add(newDetail);
        await _context.SaveChangesAsync();
        
        var detailResponse = await _context.InvoiceDetails
            .Include(id => id.Service)
            .Where(id => id.Id == newDetail.Id)
            .FirstOrDefaultAsync();

        var detailResponseDto = _mapper.Map<GetInvoiceDetailDto>(detailResponse);

        return CreatedAtAction
        (
            nameof(Add),
            new { id = newDetail.Id },
            detailResponseDto
        );
    }

    [HttpPut("update/{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateInvoiceDetailDto updatedDetailDto)
    {
        var detail = await _context.InvoiceDetails.Include(d => d.Invoice).FirstOrDefaultAsync(d => d.Id == id);

        if (detail == null)
        { 
            return NotFound();   
        }
        
        if (detail.Invoice != null && detail.Invoice.Status != "draft")
        {
            return Conflict(
                new
                {
                    msg = "No se puede modificar un detalle de una factura que no esté en estado 'draft'"
                }
            );
        }
        
        if (updatedDetailDto.Quantity.HasValue) 
        {
            detail.Quantity = updatedDetailDto.Quantity.Value;
        }
        if (updatedDetailDto.UnitPrice.HasValue) 
        {
            detail.UnitPrice = updatedDetailDto.UnitPrice.Value;
        }
        if (updatedDetailDto.Description != null) 
        {
            detail.Description = updatedDetailDto.Description;
        }
        
        await _context.SaveChangesAsync();

        return NoContent();
    }
    
    [HttpDelete("delete/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var detail = await _context.InvoiceDetails.Include(d => d.Invoice).FirstOrDefaultAsync(d => d.Id == id);

        if (detail == null)
        {
            return NotFound();
        }

        if (detail.Invoice != null && detail.Invoice.Status != "draft")
        {
            return Conflict(
                new
                {
                    msg = "No se puede eliminar un detalle de una factura que no esté en estado 'draft'"
                }
            );
        }

        _context.InvoiceDetails.Remove(detail);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}