using Microsoft.AspNetCore.Identity;

namespace HardPedia.Models.Domain
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedOn { get; set; }

        public Guid SubjectId { get; set; }
        public Subject Subject { get; set; }

        public Guid UserId { get; set; }

        public IdentityUser User { get; set; }

        public Comment()
        {
            CreatedOn = DateTime.Now;
        }

        public Comment(string content)
        {
            Content = content;
            CreatedOn = DateTime.Now;
        }


    }
}
