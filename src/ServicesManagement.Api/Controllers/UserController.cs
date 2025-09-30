using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServicesManagement.Api.Data;
using ServicesManagement.Api.Dtos;

namespace ServicesManagement.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Roles = "admin")]
public class UserController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    private readonly IMapper _mapper;

    public UserController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet("getAll")]
    public async Task<ActionResult<IEnumerable<GetUserDto>>> GetAll()
    {
        var users = await _context.Users.ToListAsync();

        var usersDto = _mapper.Map<IEnumerable<GetUserDto>>(users);

        var active = usersDto.Where(u => u.Status == "active").ToList();

        var inactive = usersDto.Where(u => u.Status == "inactive").ToList();

        var response = new
        {
            Users = usersDto.Count(),
            Actives = active.Count,
            Inactives = inactive.Count,
            ActiveUsers = active,
            InactiveUsers = inactive
        };

        return Ok(response);
    }


    [HttpGet("getById/{id:int}")]
    public async Task<ActionResult<GetUserDto>> GetById(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        if (user.Status == "inactive")
        {
            return Conflict
            (   
                new
                { 
                    field = "status",
                    msg = "El usuario se encuentra inactivo"
                }
            );
        }

        var userDto = _mapper.Map<GetUserDto>(user);
        return Ok(userDto);
    }

    [HttpPut("update/{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateUserDto updatedUserDto)
    {
        var user = await _context.Users.FindAsync(id);

        if (user is null)
        { 
            return NotFound();   
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
        
        if (updatedUserDto.Username != null && updatedUserDto.Username != user.Username)
        {
            var usernameExist = await _context.Users.AnyAsync(u => u.Username == updatedUserDto.Username && u.Id != id);
            if (usernameExist)
            {
                return Conflict
                (
                    new
                    {
                        field = "username",
                        msg = "Ya existe el nombre de usuario ingresado"
                    }
                );    
            }
        }

        if (updatedUserDto.Email != null && updatedUserDto.Email != user.Email)
        {
            var emailExist = await _context.Users.AnyAsync(u => u.Email == updatedUserDto.Email && u.Id != id);
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
        }

        if (!string.IsNullOrEmpty(updatedUserDto.PasswordHash))
        {
            if (BCrypt.Net.BCrypt.Verify(updatedUserDto.PasswordHash, user.PasswordHash))
            {
                return Conflict
                (
                    new
                    {
                        field = "password",
                        msg = "La nueva contraseña no puede ser igual a la actual"
                    }
                );
            }
        
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(updatedUserDto.PasswordHash);
        }
        
        if (!string.IsNullOrEmpty(updatedUserDto.Username)) 
        {
            user.Username = updatedUserDto.Username;
        }

        if (!string.IsNullOrEmpty(updatedUserDto.Name))
        {
            user.Name = updatedUserDto.Name;
        }
        
        if (!string.IsNullOrEmpty(updatedUserDto.Surname))
        {
            user.Surname = updatedUserDto.Surname;
        }
        
        if (!string.IsNullOrEmpty(updatedUserDto.Phone))
        {
            user.Phone = updatedUserDto.Phone;
        }

        if (!string.IsNullOrEmpty(updatedUserDto.Email))
        {
            user.Email = updatedUserDto.Email;
        }

        if (!string.IsNullOrEmpty(updatedUserDto.PasswordHash))
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(updatedUserDto.PasswordHash); ;
        }

        user.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        var userResponseDto = _mapper.Map<GetUserDto>(updatedUserDto);

        return Ok(userResponseDto);
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
