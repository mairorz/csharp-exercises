namespace ServicesManagement.Api.Dtos;

public class GetPurchasesDto
{
    public int Id { get; init; }

    public int SupplierId { get; init; }

    public int InputId { get; init; }

    public string Name { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public decimal Quantity { get; init; }

    public decimal UnitCost { get; init; }

    public DateTime PurchasedDate { get; init; }

    public GetSupplierDto? Supplier { get; init; }
    
    public GetInputsDto? Input { get; init; }
}
