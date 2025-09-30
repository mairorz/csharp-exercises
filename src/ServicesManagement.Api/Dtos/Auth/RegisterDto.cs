using System.ComponentModel.DataAnnotations;

namespace ServicesManagement.Api.Dtos;

public class RegisterDto
{   
    [Required(ErrorMessage = "El nombre de usuario es requerido")]
    public string Username { get; set; } = null!;

    [Required(ErrorMessage = "El nombre es requerido")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "El apellido es requerido")]
    public string Surname { get; set; } = null!;
    
    [Required(ErrorMessage = "La contraseña es requerida")]
    public string PasswordHash { get; set; } = null!;

    public string? Phone { get; set; }

    [Required(ErrorMessage = "El correo es requerido")]
    [EmailAddress(ErrorMessage = "El correo no tiene un formato válido")]
    public string Email { get; set; } = null!;
}