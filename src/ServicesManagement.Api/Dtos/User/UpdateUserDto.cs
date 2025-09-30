namespace ServicesManagement.Api.Dtos;

public class UpdateUserDto
{
    public string? Username { get; set; }

    public string? Name { get; set; }

    public string? Surname { get; set; }

    public string? PasswordHash { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }
}