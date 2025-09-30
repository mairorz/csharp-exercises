namespace ServicesManagement.Api.Dtos;

public class AddServiceDto
{
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }
}
