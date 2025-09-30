namespace ServicesManagement.Api.Dtos;

public class GetInputsDto
{
    public int Id { get; init; }
    
    public int CategoryId { get; init; }

    public string Name { get; init; } = string.Empty;

    public string Unit { get; init; } = string.Empty;

    public GetInputsCategoriesDto? Category { get; init; }
}
