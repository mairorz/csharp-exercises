namespace ServicesManagement.Api.Dtos;

public class UpdatePurchasesDto
{
    public string? Name { get; set; }

    public string? Description { get; set; }

    public decimal? Quantity { get; set; }

    public decimal? UnitCost { get; set; }
}