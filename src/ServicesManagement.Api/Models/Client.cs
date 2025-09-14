using System.ComponentModel.DataAnnotations.Schema;

namespace ServicesManagement.Api.Models;

[Table("clients")]
public class Client
{
    public int Id { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    [Column("contact_phone")]
    public string ContactPhone { get; set; } = null!;

    [Column("contact_email")]
    public string ContactEmail { get; set; } = null!;

    public string Status { get; set; } = "active";

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }

    public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public ICollection<WorkOrder> WorkOrders { get; set; } = new List<WorkOrder>();

    public User? User { get; set; }
}

