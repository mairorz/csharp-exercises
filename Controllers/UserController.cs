namespace WebApplicationServices.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly AppDbContext _db;

    public UserController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet("listar")]
    public async Task<ActionResult<IEnumerable<User>>> Listar()
    {
        var resultado = await _db.Users
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

        return Ok(resultado);
    }

    [HttpGet("listar/inactivos")]
    public async Task<ActionResult<IEnumerable<User>>> ListarInactivos()
    {
        var users = await _db.Users
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

        return Ok(users);
    }


    [HttpGet("listar/{id:int}")]
    public async Task<ActionResult<User>> Obtener(int id)
    {
        var user = await _db.Users
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

        if (user == null)
        {
            return NotFound();
        }

        if (user.Status == "inactive")
        {
            return NotFound(
                new
                {
                    id,
                    msg = "Usuario inactivo"
                }
            );
        }

        return Ok(user);
    }

    [HttpPost("agregar")]
    public async Task<ActionResult<User>> Crear([FromBody] User nuevo)
    {
        var emailExist = await _db.Users.AnyAsync(u => u.Email == nuevo.Email);
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

        _db.Users.Add(nuevo);
        await _db.SaveChangesAsync();

        return CreatedAtAction
        (
            nameof(Obtener),
            new { id = nuevo.Id },
            nuevo
        );
    }

    [HttpPut("editar/{id:int}")]
    public async Task<IActionResult> Actualizar(int id, [FromBody] User cambios)
    {
        var user = await _db.Users.FindAsync(id);

        if (user == null)
        { 
            return NotFound();   
        }

        var emailExist = await _db.Users.AnyAsync(u => u.Email == cambios.Email);
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

        user.Name = cambios.Name;
        user.Surname = cambios.Surname;
        user.PasswordHash   = cambios.PasswordHash;
        user.Phone   = cambios.Phone;
        user.Email   = cambios.Email;
        user.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return NoContent();
    }

    [HttpPatch("desactivar/{id:int}")]
    public async Task<IActionResult> Desactivar(int id)
    {
        var user = await _db.Users.FindAsync(id);

        if (user == null) return NotFound();

        if (user.Status == "inactive") return NoContent();

        user.Status = "inactive";
        user.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return NoContent();
    }
}
