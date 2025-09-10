namespace WebApplicationServices.Models;

[Table("invoice_detail")]
public class InvoiceDetail
{
    public int Id { get; set; }

    [Column("invoice_id")]
    public int InvoiceId { get; set; }

    [Column("service_id")]
    public int ServiceId { get; set; }

    public string? Description { get; set; }

    public decimal Quantity { get; set; }

    [Column("unit_price")]
    public decimal UnitPrice { get; set; }

    public Invoice? Invoice { get; set; }

    public Service? Service { get; set; }
}
