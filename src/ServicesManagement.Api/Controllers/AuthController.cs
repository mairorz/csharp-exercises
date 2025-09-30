using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ServicesManagement.Api.Data;
using ServicesManagement.Api.Dtos;
using ServicesManagement.Api.Models;

namespace ServicesManagement.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public AuthController(ApplicationDbContext context, IMapper mapper, IConfiguration configuration)
    {
        _context = context;
        _mapper = mapper;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<ActionResult<User>> Register(RegisterDto request)
    {
        if (await _context.Users.AnyAsync(u => u.Username == request.Username))
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

        if (await _context.Users.AnyAsync(u => u.Email == request.Email))
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

        var newUser = _mapper.Map<User>(request);

        newUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.PasswordHash);
        newUser.CreatedAt = DateTime.UtcNow;

        _context.Users.Add(newUser);

        await _context.SaveChangesAsync();

        var userResponseDto = _mapper.Map<GetUserDto>(newUser);

        return Ok(new
        {
            userResponseDto
        });
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(LoginDto request)
    {
        if (string.IsNullOrEmpty(request.Username) && string.IsNullOrEmpty(request.Email)) 
        {
            return BadRequest(new { error = "Tienes que ingresar el correo o el nombre del usuario" });
        }

        //Buscamos al usuario
        var user = await _context.Users.SingleOrDefaultAsync
                    (u => u.Email == request.Email || u.Username == request.Username);

        if (user is null)
        {
            return NotFound( new { success = false, msg = "No existe el usuario" });
        }

        // Verificamos credenciales
        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return Unauthorized(new { success = false, msg = "Credenciales inválidas" });
        }

        var token = CreateToken(user);

        return Ok( new { Token = token });
    }

    private string CreateToken(User user)
    {
        var claims = new List<Claim>
        {   
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration.GetValue<string>("AppSettings:Key")!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var tokenDescriptor = new JwtSecurityToken(
            issuer: _configuration.GetValue<string>("AppSettings:Issuer"),
            audience: _configuration.GetValue<string>("AppSettings:Audience"),
            claims: claims,
            expires: DateTime.UtcNow.AddHours(3),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }
}
