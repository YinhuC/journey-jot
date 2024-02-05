using JourneyJot.Models;

namespace JourneyJot.Dto
{
    public class CommentDtoCreate
    {
        public string AuthorId { get; set; }

        public string PostId { get; set; }

        public string Content { get; set; }
    }
}
