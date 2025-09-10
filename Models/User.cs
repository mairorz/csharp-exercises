namespace WebApplicationServices.Models;

[Table("users")]
public class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    [Column("password_hash")]
    public string PasswordHash { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Role { get; set; } = "owner";

    public string Status { get; set; } = "active";

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }

    public ICollection<Client> Clients { get; set; } = new List<Client>();

    public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
}

