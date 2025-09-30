using System.ComponentModel.DataAnnotations;

namespace ServicesManagement.Api.Dtos;

public class LoginDto
{
    [EmailAddress]
    public string? Email { get; set; } = null!;

    public string? Username { get; set; }

    [Required]
    public string Password { get; set; } = null!;
}