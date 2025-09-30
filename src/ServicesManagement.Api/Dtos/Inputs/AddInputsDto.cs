namespace ServicesManagement.Api.Dtos;

public class AddInputsDto
{
    public int CategoryId { get; set; }

    public string Name { get; set; } = null!;

    public string Unit { get; set; } = null!;
}
