namespace ServicesManagement.Api.Dtos;

public class AddConsumptionDto
{
    public int WorkOrderId { get; set; }

    public int InputId { get; set; }

    public decimal Quantity { get; set; }
}
