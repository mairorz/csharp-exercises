using System.ComponentModel.DataAnnotations.Schema;

namespace ServicesManagement.Api.Models;

[Table("work_orders")]
public class WorkOrder
{
    public int Id { get; set; }

    [Column("client_id")]
    public int ClientId { get; set; }

    [Column("service_id")]
    public int ServiceId { get; set; }

    public string Notes { get; set; } = null!;

    [Column("scheduled_date")]
    public DateTime ScheduledDate { get; set; }

    public string Status { get; set; } = "scheduled";

    public Client? Client { get; set; }

    public Service? Service { get; set; }

    public ICollection<Consumption> Consumptions { get; set; } = new List<Consumption>();
}
