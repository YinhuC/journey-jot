using JourneyJot.Models;

namespace JourneyJot.Dto
{
    public class CommentDtoUpdate
    {
        public Guid AuthorId { get; set; }

        public string Content { get; set; }
    }
}
