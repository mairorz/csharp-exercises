namespace WebApplicationServices.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly AppDbContext _db;

    public TestController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet("ping")]
    public IActionResult Ping()
    {
        var totalUsers = _db.Users.Count();
        return Ok(
            new
            {
                message = "Conexión correcta",
                usuarios = totalUsers
            });
    }
}
