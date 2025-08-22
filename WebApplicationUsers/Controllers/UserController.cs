using Microsoft.AspNetCore.Mvc;
using WebApplicationUsers.Models;
using WebApplicationUsers.Services;

namespace WebApplicationUsers.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    [HttpGet("list")]
    public ActionResult<IEnumerable<User>> GetUsers()
    {
        return Ok(UserDataStore.Current.Users);
    }

    [HttpGet("search/{userId}")]
    public ActionResult<User> GetUser(int userId)
    {
        var user = UserDataStore.Current.Users.FirstOrDefault(x => x.Id == userId);

        if (user == null)
            return NotFound("El usuario solicitado no existe.");

        return Ok(user);
    }

    [HttpPut("update/{userId}")]
    public ActionResult PutUser([FromRoute] int userId, [FromBody] User userInsert)
    {
        var user = UserDataStore.Current.Users.FirstOrDefault(x => x.Id == userId);
        if (user == null)
            return NotFound("El usuario solicitado no existe.");

        user.Nombres  = userInsert.Nombres;
        user.Apellidos = userInsert.Apellidos;
        user.Correo    = userInsert.Correo;
        user.Telefono  = userInsert.Telefono;
        user.Username  = userInsert.Username;

        return NoContent();
    }
    
    [HttpDelete("delete/{id}")]
    public ActionResult DeleteUser(int id)
    {
        var user = UserDataStore.Current.Users.FirstOrDefault(u => u.Id == id);
        if (user == null)
            return NotFound("No existe un usuario con ese Id.");

        UserDataStore.Current.Users.Remove(user);
        return NoContent();
    }
}
