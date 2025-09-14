using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServicesManagement.Api.Data;
using ServicesManagement.Api.Models;

namespace ServicesManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public UserController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("getAll")]
    public async Task<ActionResult<IEnumerable<User>>> GetAll()
    {
        var result = await _context.Users
                    .Where(u => u.Status == "active")
                    .Select(u => new
                    {
                        UserId = u.Id,
                        UserName = u.Name,
                        UserSurname = u.Surname,
                        UserPhone = u.Phone,
                        UserEmail = u.Email,
                        UserRole = u.Role,
                        UserStatus = u.Status,
                        UserCreatedAt = u.CreatedAt,
                        UserUpdatedAt = u.UpdatedAt
                    })
                    .ToListAsync();

        return Ok(result);
    }

    [HttpGet("getAll/inactive")]
    public async Task<ActionResult<IEnumerable<User>>> GetAllInactive()
    {
        var result = await _context.Users
                    .Where(u => u.Status == "inactive")
                    .Select(u => new
                    {
                        UserId = u.Id,
                        UserName = u.Name,
                        UserSurname = u.Surname,
                        UserPhone = u.Phone,
                        UserEmail = u.Email,
                        UserRole = u.Role,
                        UserStatus = u.Status,
                        UserCreatedAt = u.CreatedAt,
                        UserUpdatedAt = u.UpdatedAt
                    })
                    .ToListAsync();

        return Ok(result);
    }


    [HttpGet("getById/{id:int}")]
    public async Task<ActionResult<User>> GetById(int id)
    {
        var result = await _context.Users
                    .Where(u => u.Id == id)
                    .Select(u => new User
                    {
                        Id = u.Id,
                        Name = u.Name,
                        Surname = u.Surname,
                        Phone = u.Phone,
                        Email = u.Email,
                        Role = u.Role,
                        Status = u.Status,
                        CreatedAt = u.CreatedAt,
                        UpdatedAt = u.UpdatedAt
                    })
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
                    msg = "Usuario inactivo"
                }
            );
        }

        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<ActionResult<User>> Add(User newUser)
    {
        var emailExist = await _context.Users.AnyAsync(u => u.Email == newUser.Email);
        if (emailExist)
        {
            return Conflict
            (
                new
                {
                    field = "email",
                    msg = "Ya existe el correo ingresado"
                }
            );    
        }

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        return CreatedAtAction
        (
            nameof(Add),
            new { id = newUser.Id },
            newUser
        );
    }

    [HttpPut("update/{id:int}")]
    public async Task<IActionResult> Update(int id, User updatedUser)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
        { 
            return NotFound();   
        }

        var emailExist = await _context.Users.AnyAsync(u => u.Email == updatedUser.Email);
        if (emailExist)
        {
            return Conflict
            (
                new
                {
                    field = "email",
                    msg = "Ya existe el correo ingresado"
                }
            );    
        }
        
        if (user.Status == "inactive")
        {
            return Problem
            (
                statusCode: 409,
                title: "Usuario inactivo",
                detail: "No se puede modificar mientras esté inactivo"
            );
        }

        user.Name = updatedUser.Name;
        user.Surname = updatedUser.Surname;
        user.PasswordHash = updatedUser.PasswordHash;
        user.Phone = updatedUser.Phone;
        user.Email = updatedUser.Email;
        user.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPatch("disable/{id:int}")]
    public async Task<IActionResult> Disable(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        if (user.Status == "inactive")
        {
            return NoContent();
        }

        user.Status = "inactive";
        user.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }
}
