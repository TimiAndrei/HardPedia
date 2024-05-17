namespace HardPedia.Models.Domain;


public class Subject
{
    public Guid Id { get; set; }

    public required string Heading { get; set; }

    public required string Title { get; set; }

    public required string Content { get; set; }

    public DateTime CreatedOn { get; set; }

    public required string Author { get; set; }

    public bool Visible { get; set; }

    public ICollection<Category> Categories { get; set; } = [];

    public ICollection<Comment> Comments { get; set; } = [];

    public Subject()
    {
        CreatedOn = DateTime.Now;
    }

    public Subject(string heading, string title, string content, string author, bool visible)
    {
        Heading = heading;
        Title = title;
        Content = content;
        Author = author;
        Visible = visible;
        CreatedOn = DateTime.Now;
    }


}
