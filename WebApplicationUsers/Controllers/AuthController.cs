using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationUsers.Data;
using WebApplicationUsers.Models;

namespace WebApplicationUsers.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    public AuthController(AppDbContext db) => _db = db;

    public class LoginDto
    {
        public string Correo { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    [HttpPost("save")]
    public async Task<ActionResult<User>> RegisterUser([FromBody] User userInsert)
    {
        if (userInsert == null)
            return BadRequest("No se recibió información del usuario.");

        if (string.IsNullOrWhiteSpace(userInsert.Correo) || string.IsNullOrWhiteSpace(userInsert.Password))
            return BadRequest("Correo y Password son obligatorios.");

        var exists = await _db.users.AnyAsync(u =>
            u.Correo == userInsert.Correo || u.Username == userInsert.Username);

        if (exists)
            return Conflict("Ya existe un usuario con ese correo o username.");

        _db.users.Add(userInsert);
        await _db.SaveChangesAsync();

        return CreatedAtAction(
            actionName: nameof(UserController.GetUser),
            controllerName: "User",
            routeValues: new { id = userInsert.Id },
            value: userInsert
        );
    }

    [HttpPost("login")]
    public async Task<ActionResult<User>> Login([FromBody] LoginDto credentials)
    {
        if (credentials == null ||
            string.IsNullOrWhiteSpace(credentials.Correo) ||
            string.IsNullOrWhiteSpace(credentials.Password))
            return BadRequest("Debe enviar correo y password.");

        var user = await _db.users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Correo == credentials.Correo && u.Password == credentials.Password);

        if (user == null)
            return Unauthorized("Credenciales inválidas.");

        return Ok(user);
    }
}
