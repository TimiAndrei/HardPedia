namespace HardPedia.Models.Domain;

public class Category
{

    public Guid Id { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    public ICollection<Subject> Subjects { get; set; } = [];

    public Category()
    {
    }

    public Category(string name, string? description)
    {
        Name = name;
        Description = description;
    }
}
