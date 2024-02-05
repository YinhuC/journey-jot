using JourneyJot.Models;

namespace JourneyJot.Dto
{
    public class PostDtoUpdate
    {
        public string AuthorId { get; set; }

        public string? Title { get; set; }

        public string? Content { get; set; }
    }
}
