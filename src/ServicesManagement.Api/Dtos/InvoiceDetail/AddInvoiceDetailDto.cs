namespace ServicesManagement.Api.Dtos;

public class AddInvoiceDetailDto
{
    public int InvoiceId { get; set; }

    public int ServiceId { get; set; }

    public string? Description { get; set; }

    public decimal Quantity { get; set; }

    public decimal UnitPrice { get; set; }
}