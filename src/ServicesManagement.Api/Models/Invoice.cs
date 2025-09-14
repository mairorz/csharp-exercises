using System.ComponentModel.DataAnnotations.Schema;

namespace ServicesManagement.Api.Models;

[Table("invoices")]
public class Invoice
{
    public int Id { get; set; }

    [Column("client_id")]
    public int ClientId { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("issue_date")]
    public DateTime IssueDate { get; set; }

    [Column("paid_date")]
    public DateTime PaidDate { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    public string Status { get; set; } = "draft";

    public ICollection<InvoiceDetail> InvoiceDetails { get; set; } = new List<InvoiceDetail>();

    public User? User { get; set; }
    
    public Client? Client { get; set; }
}
