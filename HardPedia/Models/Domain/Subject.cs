namespace HardPedia.Models.Domain;


public class Subject
{
    public Guid Id { get; set; }

    public string Heading { get; set; }

    public string Title { get; set; }

    public string Content { get; set; }

    public string UrlHandle { get; set; }

    public DateTime CreatedOn { get; set; }

    public string Author { get; set; }

    public bool Visible { get; set; }

    public ICollection<Category> Categories { get; set; }


}
