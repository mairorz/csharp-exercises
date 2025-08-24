using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationUsers.Data;
using WebApplicationUsers.Models;

namespace WebApplicationUsers.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly AppDbContext _db;
    public UserController(AppDbContext db) => _db = db;

    [HttpGet("list")]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        var users = await _db.users.AsNoTracking().ToListAsync();
        return Ok(users);
    }

    [HttpGet("search/{id:int}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        var u = await _db.users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return u is null ? NotFound("No existe.") : Ok(u);
    }

    [HttpPost("create")]
    public async Task<ActionResult<User>> Create([FromBody] User dto)
    {
        _db.users.Add(dto);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetUser), new { id = dto.Id }, dto);
    }

    [HttpPut("update/{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] User dto)
    {
        var u = await _db.users.FindAsync(id);
        if (u is null) return NotFound();

        u.Nombres  = dto.Nombres;
        u.Apellidos= dto.Apellidos;
        u.Correo   = dto.Correo;
        u.Telefono = dto.Telefono;
        u.Username = dto.Username;
        u.Password = dto.Password;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("delete/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var u = await _db.users.FindAsync(id);
        if (u is null) return NotFound();
        _db.users.Remove(u);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
