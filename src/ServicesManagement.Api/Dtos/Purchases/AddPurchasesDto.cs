namespace ServicesManagement.Api.Dtos;

public class AddPurchasesDto
{
    public int SupplierId { get; set; }

    public int InputId { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal Quantity { get; set; }

    public decimal UnitCost { get; set; }
}
