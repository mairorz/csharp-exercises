namespace WebApplicationUsers.Models;

public class User
{ 
    public int Id { get; set; }

    public string Nombres { get; set; } = string.Empty;

    public string Apellidos { get; set; } = string.Empty;

    public string Correo { get; set; } = string.Empty;
    
    public string? Telefono { get; set; }
    public string Username { get; set; } = string.Empty;
}
