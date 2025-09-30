namespace ServicesManagement.Api.Dtos;

public class GetUserDto
{
    public int Id { get; init; }

    public string Username { get; init; } = string.Empty;

    public string Name { get; init; } = string.Empty;

    public string Surname { get; init; } = string.Empty;

    public string? Phone { get; init; }

    public string Email { get; init; } = string.Empty;

    public string Role { get; init; } = string.Empty;

    public string Status { get; init; } = string.Empty;

    public DateTime CreatedAt { get; init; }

    public DateTime UpdatedAt { get; init; }
}
