using JourneyJot.Models;

namespace JourneyJot.Dto
{
    public class CommentDto
    {
        public Guid Id { get; set; }

        public Guid AuthorId { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string Content { get; set; }
    }
}
