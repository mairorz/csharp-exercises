namespace ServicesManagement.Api.Dtos;

public class AddSupplierDto
{
    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Email { get; set; } = null!;
}