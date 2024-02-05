using JourneyJot.Models;

namespace JourneyJot.Dto
{
    public class PostDtoCreate
    {
        public Guid AuthorId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }
    }
}
