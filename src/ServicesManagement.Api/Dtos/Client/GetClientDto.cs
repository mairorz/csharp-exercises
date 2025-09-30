namespace ServicesManagement.Api.Dtos;

public class GetClientDto
{
    public int Id { get; init; }
    
    public int UserId { get; init; }
    
    public string Name { get; init; } = string.Empty;
    
    public string ContactPhone { get; init; } = string.Empty;
    
    public string ContactEmail { get; init; } = string.Empty;
    
    public string Status { get; init; } = string.Empty;
    
    public DateTime CreatedAt { get; init; }
    
    public DateTime UpdatedAt { get; init; }
}