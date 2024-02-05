using JourneyJot.Models;

namespace JourneyJot.Dto
{
    public class PostDtoCreate
    {
        public string AuthorId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string[]? Categories { get; set; }

        public string[]? Tags { get; set; }
    }
}
