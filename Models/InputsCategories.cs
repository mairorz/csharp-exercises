namespace WebApplicationServices.Models;

[Table("inputs_categories")]
public class InputsCategories
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public ICollection<Inputs> Inputs { get; set; } = new List<Inputs>();
}
