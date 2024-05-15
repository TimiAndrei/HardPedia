namespace HardPedia.Models.Domain;

public class Category
{

    public Guid Id { get; set; }

    public string Name { get; set; }

    public string DisplayName { get; set; }

    public string? Description { get; set; }

    public ICollection<Subject> Subjects { get; set; }
}
