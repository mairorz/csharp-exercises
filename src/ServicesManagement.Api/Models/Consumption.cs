using System.ComponentModel.DataAnnotations.Schema;

namespace ServicesManagement.Api.Models;

[Table("consumptions")]
public class Consumption
{
    public int Id { get; set; }

    [Column("work_order_id")]
    public int WorkOrderId { get; set; }

    [Column("input_id")]
    public int InputId { get; set; }

    public decimal Quantity { get; set; }

    [Column("consumed_date")]
    public DateTime ConsumedDate { get; set; }

    public WorkOrder? WorkOrder { get; set; }

    public Inputs? Input { get; set; }
}
