namespace ServicesManagement.Api.Dtos;

public class GetConsumptionDto
{
    public int Id { get; init; }

    public int WorkOrderId { get; init; }

    public int InputId { get; init; }

    public decimal Quantity { get; init; }

    public DateTime ConsumedDate { get; init; }

    public GetWorkOrderDto? WorkOrder { get; init; }
    
    public GetInputsDto? Input { get; init; }
}