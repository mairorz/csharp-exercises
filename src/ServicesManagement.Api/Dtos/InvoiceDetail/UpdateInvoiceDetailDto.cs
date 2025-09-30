namespace ServicesManagement.Api.Dtos;

public class UpdateInvoiceDetailDto
{
    public string? Description { get; set; }

    public decimal? Quantity { get; set; }

    public decimal? UnitPrice { get; set; }
}