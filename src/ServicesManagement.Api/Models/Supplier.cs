using System.ComponentModel.DataAnnotations.Schema;

namespace ServicesManagement.Api.Models;

[Table("suppliers")]
public class Supplier
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Email { get; set; } = null!;

    public ICollection<Purchases> Purchases { get; set; } = new List<Purchases>();
}
