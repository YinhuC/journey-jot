using System;

namespace JourneyJot.Models

{
    public class Comment
    {
        public Guid Id { get; set; }

        public User Author { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string Content { get; set; }

        public Post Post { get; set; }

    }
}

