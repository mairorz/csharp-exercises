namespace ServicesManagement.Api.Dtos;

public class UpdateServiceDto
{
    public string? Name { get; set; }

    public string? Description { get; set; }

    public decimal? Price { get; set; }
}