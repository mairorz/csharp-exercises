using Microsoft.AspNetCore.Mvc;
using WebApplicationUsers.Models;
using WebApplicationUsers.Services;

namespace WebApplicationUsers.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    public class LoginDto
    {
        public string Username { get; set; } = string.Empty;
        public string? Correo { get; set; }
    }

    [HttpPost("save")]
    public ActionResult<User> RegisterUser([FromBody] User userInsert)
    {
        if (userInsert == null)
            return BadRequest("No se recibió información del usuario.");

        if (string.IsNullOrWhiteSpace(userInsert.Username))
            return BadRequest("El Username es obligatorio.");

        var exists = UserDataStore.Current.Users
            .Any(u => u.Username.Equals(userInsert.Username, System.StringComparison.OrdinalIgnoreCase));
        if (exists)
            return Conflict("Ya existe un usuario con ese Username.");

        var maxUsersId = UserDataStore.Current.Users.Any()
            ? UserDataStore.Current.Users.Max(x => x.Id)
            : 0;

        var newUser = new User
        {
            Id = maxUsersId + 1,
            Nombres = userInsert.Nombres,
            Apellidos = userInsert.Apellidos,
            Correo = userInsert.Correo,
            Telefono = userInsert.Telefono,
            Username = userInsert.Username
        };

        UserDataStore.Current.Users.Add(newUser);

        return CreatedAtAction(
            actionName: nameof(UserController.GetUser),
            controllerName: "User",
            routeValues: new { userId = newUser.Id },
            value: newUser
        );
    }

    [HttpPost("login")]
    public ActionResult<User> Login([FromBody] LoginDto credentials)
    {
        if (credentials == null || string.IsNullOrWhiteSpace(credentials.Username))
            return BadRequest("Debe enviar el Username.");

        var query = UserDataStore.Current.Users
            .Where(u => u.Username.Equals(credentials.Username, System.StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(credentials.Correo))
            query = query.Where(u => (u.Correo ?? string.Empty)
                .Equals(credentials.Correo, System.StringComparison.OrdinalIgnoreCase));

        var user = query.FirstOrDefault();

        if (user == null)
            return Unauthorized("Credenciales inválidas.");

        return Ok(user);
    }
}
