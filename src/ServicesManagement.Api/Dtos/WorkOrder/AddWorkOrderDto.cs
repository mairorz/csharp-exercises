namespace ServicesManagement.Api.Dtos;

public class AddWorkOrderDto
{
    public int ClientId { get; set; }

    public int ServiceId { get; set; }

    public string Notes { get; set; } = null!;

    public DateTime ScheduledDate { get; set; }
}
