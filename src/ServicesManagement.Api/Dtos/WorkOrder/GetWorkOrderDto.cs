namespace ServicesManagement.Api.Dtos;

public class GetWorkOrderDto
{
    public int Id { get; init; }

    public int ClientId { get; init; }

    public int ServiceId { get; init; }

    public string Notes { get; init; } = string.Empty;

    public DateTime ScheduledDate { get; init; }

    public string Status { get; init; } = string.Empty;

    public GetClientDto? Client { get; init; }
    
    public GetServiceDto? Service { get; init; }
}