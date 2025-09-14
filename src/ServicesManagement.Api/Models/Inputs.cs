using System.ComponentModel.DataAnnotations.Schema;

namespace ServicesManagement.Api.Models;

[Table("inputs")]
public class Inputs
{
    public int Id { get; set; }

    [Column("category_id")]
    public int CategoryId { get; set; }

    public string Name { get; set; } = null!;

    public string Unit { get; set; } = null!;

    public ICollection<Purchases> Purchases { get; set; } = new List<Purchases>();

    public ICollection<Consumption> Consumptions { get; set; } = new List<Consumption>();

    public InputsCategories? InputsCategories { get; set; }
}
