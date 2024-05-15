using HardPedia.Models.Domain;

namespace HardPedia.Models.ViewModels;

public class CategorySubjectsViewModel
{
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; }
    public string? CategoryDescription { get; set; }
    public List<Subject> Subjects { get; set; }
    public int TotalPages { get; set; }
    public int CurrentPage { get; set; }

    public CategorySubjectsViewModel()
    {
        Subjects = new List<Subject>();
        CategoryName = string.Empty;
    }

    public CategorySubjectsViewModel(Guid categoryId, string categoryName, string? categoryDescription, List<Subject> subjects, int totalPages, int currentPage)
    {
        CategoryId = categoryId;
        CategoryName = categoryName;
        CategoryDescription = categoryDescription;
        Subjects = subjects;
        TotalPages = totalPages;
        CurrentPage = currentPage;
    }
}
