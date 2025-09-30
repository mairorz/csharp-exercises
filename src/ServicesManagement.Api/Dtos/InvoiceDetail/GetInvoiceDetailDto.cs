namespace ServicesManagement.Api.Dtos;

public class GetInvoiceDetailDto
{
    public int Id { get; init; }

    public int InvoiceId { get; init; }

    public int ServiceId { get; init; }

    public string? Description { get; init; }

    public decimal Quantity { get; init; }

    public decimal UnitPrice { get; init; }

    public GetServiceDto? Service { get; init; }
}