using JourneyJot.Models;

namespace JourneyJot.Dto
{
    public class CommentDtoCreate
    {
        public Guid AuthorId { get; set; }

        public string Content { get; set; }
    }
}
