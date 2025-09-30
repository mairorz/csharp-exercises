namespace ServicesManagement.Api.Dtos;

public class AddClientDto
{
    public int UserId { get; set; }
    
    public string Name { get; set; } = null!;
    
    public string ContactPhone { get; set; } = null!;
    
    public string ContactEmail { get; set; } = null!;
}
