namespace ServicesManagement.Api.Dtos;

public class GetUserDto
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Surname { get; set; }

    public string Phone { get; set; }

    public string Email { get; set; }

    public string Role { get; set; }

    public string Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
    
}