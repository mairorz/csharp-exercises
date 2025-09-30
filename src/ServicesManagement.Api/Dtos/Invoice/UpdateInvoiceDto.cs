namespace ServicesManagement.Api.Dtos;

public class UpdateInvoiceDto
{
    public int? ClientId { get; set; }

    public int? UserId { get; set; }

    public DateTime? IssueDate { get; set; }

    public DateTime? PaidDate { get; set; }
}