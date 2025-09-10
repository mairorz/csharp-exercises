namespace WebApplicationServices.Models;

[Table("purchases")]
public class Purchases
{
    public int Id { get; set; }

    [Column("supplier_id")]
    public int SupplierId { get; set; }

    [Column("input_id")]
    public int InputId { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal Quantity { get; set; }

    [Column("unit_cost")]
    public decimal UnitCost { get; set; }

    [Column("purchased_date")]
    public DateTime PurchasedDate { get; set; }

    public Supplier? Supplier { get; set; }

    public Inputs? Inputs { get; set; }
}
