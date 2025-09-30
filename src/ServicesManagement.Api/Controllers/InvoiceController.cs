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
public class InvoiceController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public InvoiceController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet("getAll")]
    public async Task<ActionResult<IEnumerable<GetInvoiceDto>>> GetAll()
    {
        var result = await _context.Invoices
            .Include(i => i.User)
            .Include(i => i.Client)
            .ToListAsync();
            
        var resultDto = _mapper.Map<IEnumerable<GetInvoiceDto>>(result);
        return Ok(resultDto);
    }
    
    [HttpGet("getAll/byStatus/{status}")]
    public async Task<ActionResult<IEnumerable<GetInvoiceDto>>> GetAllByStatus(string status)
    {
        if (string.IsNullOrWhiteSpace(status))
        {
            return BadRequest("El estado de la factura no puede estar vacío");
        }

        var result = await _context.Invoices
            .Include(i => i.User)
            .Include(i => i.Client)
            .Where(i => i.Status.ToLower() == status.ToLower())
            .ToListAsync();
            
        var resultDto = _mapper.Map<IEnumerable<GetInvoiceDto>>(result);
        return Ok(resultDto);
    }

    [HttpGet("getById/{id:int}")]
    public async Task<ActionResult<GetInvoiceDto>> GetById(int id)
    {
        var result = await _context.Invoices
            .Include(i => i.User)
            .Include(i => i.Client)
            .Where(i => i.Id == id)
            .FirstOrDefaultAsync();

        if (result == null)
        {
            return NotFound();
        }

        var resultDto = _mapper.Map<GetInvoiceDto>(result);
        return Ok(resultDto);
    }

    [HttpPost("add")]
    public async Task<ActionResult<Invoice>> Add(AddInvoiceDto invoiceDto)
    {
        var client = await _context.Clients.FindAsync(invoiceDto.ClientId);
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

        var user = await _context.Users.FindAsync(invoiceDto.UserId);
        if (user == null || user.Status == "inactive")
        {
            return NotFound
            (
                new
                {
                    field = "userId",
                    msg = "El usuario asociado no existe o está inactivo"
                }
            );
        }

        var newInvoice = _mapper.Map<Invoice>(invoiceDto);
        newInvoice.CreatedAt = DateTime.UtcNow;
        newInvoice.IssueDate = DateTime.UtcNow;
        newInvoice.Status = "draft";

        _context.Invoices.Add(newInvoice);
        await _context.SaveChangesAsync();
        
        var invoiceResponse = await _context.Invoices
            .Include(i => i.User)
            .Include(i => i.Client)
            .Where(i => i.Id == newInvoice.Id)
            .FirstOrDefaultAsync();

        var invoiceResponseDto = _mapper.Map<GetInvoiceDto>(invoiceResponse);

        return CreatedAtAction
        (
            nameof(Add),
            new { id = newInvoice.Id },
            invoiceResponseDto
        );
    }

    [HttpPut("update/{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateInvoiceDto updatedInvoiceDto)
    {
        var invoice = await _context.Invoices.FindAsync(id);

        if (invoice == null)
        { 
            return NotFound();   
        }

        if (invoice.Status == "paid" || invoice.Status == "canceled")
        {
            return Problem
            (
                statusCode: 409,
                title: "Factura cerrada",
                detail: "No se puede modificar una factura con estado 'paid' o 'canceled'"
            );
        }

        if (updatedInvoiceDto.ClientId.HasValue && updatedInvoiceDto.ClientId.Value != invoice.ClientId)
        {
            var client = await _context.Clients.FindAsync(updatedInvoiceDto.ClientId.Value);
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
            invoice.ClientId = updatedInvoiceDto.ClientId.Value;
        }

        if (updatedInvoiceDto.UserId.HasValue && updatedInvoiceDto.UserId.Value != invoice.UserId)
        {
            var user = await _context.Users.FindAsync(updatedInvoiceDto.UserId.Value);
            if (user == null || user.Status == "inactive")
            {
                return NotFound
                (
                    new
                    {
                        field = "userId",
                        msg = "El usuario asociado no existe o está inactivo"
                    }
                );
            }
            invoice.UserId = updatedInvoiceDto.UserId.Value;
        }
        
        if (updatedInvoiceDto.IssueDate.HasValue) 
        {
            invoice.IssueDate = updatedInvoiceDto.IssueDate.Value;
        }
        if (updatedInvoiceDto.PaidDate.HasValue) 
        {
            invoice.PaidDate = updatedInvoiceDto.PaidDate.Value;
        }

        await _context.SaveChangesAsync();

        return NoContent();
    }
    
    [HttpPatch("changeStatus/{id:int}")]
    public async Task<IActionResult> ChangeStatus(int id, [FromQuery] string newStatus)
    {
        var invoice = await _context.Invoices.FindAsync(id);

        if (invoice == null)
        {
            return NotFound();
        }

        var validStatuses = new[] { "draft", "paid", "canceled" };
        if (!validStatuses.Contains(newStatus.ToLower()))
        {
            return BadRequest("El estado de la factura es inválido.");
        }

        invoice.Status = newStatus.ToLower();
        await _context.SaveChangesAsync();

        return NoContent();
    }
}