namespace ServicesManagement.Api.Dtos;

public class GetInvoiceDto
{
    public int Id { get; init; }

    public int ClientId { get; init; }

    public int UserId { get; init; }

    public DateTime IssueDate { get; init; }

    public DateTime? PaidDate { get; init; }

    public DateTime CreatedAt { get; init; }

    public string Status { get; init; } = string.Empty;

    public GetUserDto? User { get; init; }
    
    public GetClientDto? Client { get; init; }
}
