using System;

namespace JourneyJot.Models

{
    public class Tag
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public ICollection<Post> Posts { get; set; }

    }
}

