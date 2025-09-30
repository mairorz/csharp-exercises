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
public class ClientController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ClientController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet("getAll")]
    public async Task<ActionResult<IEnumerable<GetClientDto>>> GetAll()
    {
        var clients = await _context.Clients.ToListAsync();

        var clientsDto = _mapper.Map<IEnumerable<GetClientDto>>(clients);

        var active = clientsDto.Where(u => u.Status == "active").ToList();

        var inactive = clientsDto.Where(u => u.Status == "inactive").ToList();

        var response = new
        {
            Clients = clientsDto.Count(),
            Actives = active.Count,
            Inactives = inactive.Count,
            ActiveClients = active,
            InactiveClients = inactive
        };

        return Ok(response);
    }

    [HttpGet("getById/{id:int}")]
    public async Task<ActionResult<GetClientDto>> GetById(int id)
    {
        var result = await _context.Clients
            .Where(c => c.Id == id)
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
                    msg = "Cliente inactivo"
                }
            );
        }

        var resultDto = _mapper.Map<GetClientDto>(result);
        return Ok(resultDto);
    }

    [HttpPost("add")]
    public async Task<ActionResult<Client>> Add(AddClientDto clientDto)
    {
        var emailExist = await _context.Clients.AnyAsync(c => c.ContactEmail == clientDto.ContactEmail);
        if (emailExist)
        {
            return Conflict
            (
                new
                {
                    field = "contact_email",
                    msg = "Ya existe el correo ingresado para otro cliente"
                }
            );    
        }
        
        var userExist = await _context.Users.AnyAsync(u => u.Id == clientDto.UserId && u.Status == "active");
        if (!userExist)
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

        var newClient = _mapper.Map<Client>(clientDto);
        newClient.CreatedAt = DateTime.UtcNow;

        _context.Clients.Add(newClient);
        await _context.SaveChangesAsync();
        
        var clientResponseDto = _mapper.Map<GetClientDto>(newClient);

        return CreatedAtAction
        (
            nameof(Add),
            new { id = newClient.Id },
            clientResponseDto
        );
    }

    [HttpPut("update/{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateClientDto updatedClientDto)
    {
        var client = await _context.Clients.FindAsync(id);

        if (client == null)
        { 
            return NotFound();   
        }

        if (client.Status == "inactive")
        {
            return Problem
            (
                statusCode: 409,
                title: "Cliente inactivo",
                detail: "No se puede modificar mientras esté inactivo"
            );
        }

        if (updatedClientDto.ContactEmail != null && updatedClientDto.ContactEmail != client.ContactEmail)
        {
            var emailExist = await _context.Clients.AnyAsync(c => c.ContactEmail == updatedClientDto.ContactEmail && c.Id != id);
            if (emailExist)
            {
                return Conflict
                (
                    new
                    {
                        field = "contact_email",
                        msg = "Ya existe el correo ingresado"
                    }
                );    
            }
        }
        
        if (updatedClientDto.Name != null) 
        {
            client.Name = updatedClientDto.Name;
        }
        if (updatedClientDto.ContactPhone != null) 
        {
            client.ContactPhone = updatedClientDto.ContactPhone;
        }
        if (updatedClientDto.ContactEmail != null) 
        {
            client.ContactEmail = updatedClientDto.ContactEmail;
        }

        client.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }
    
    [HttpPatch("disable/{id:int}")]
    public async Task<IActionResult> Disable(int id)
    {
        var client = await _context.Clients.FindAsync(id);

        if (client == null)
        {
            return NotFound();
        }

        if (client.Status == "inactive")
        {
            return NoContent();
        }

        client.Status = "inactive";
        client.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }
}