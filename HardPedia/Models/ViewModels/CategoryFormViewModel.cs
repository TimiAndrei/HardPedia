namespace HardPedia.Models.ViewModels;

public class CategoryFormViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }

    public CategoryFormViewModel()
    {
        Name = string.Empty;
    }

    public CategoryFormViewModel(Guid id, string name, string? description)
    {
        Id = id;
        Name = name;
        Description = description;
    }
}
