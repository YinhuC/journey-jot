using System;

namespace JourneyJot.Models

{
    public class Post
    {
        public Guid Id { get; set; }

        public User Author { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public ICollection<Category> Categories { get; set; }

        public ICollection<Tag> Tags { get; set; }

        public ICollection<PostCategory> PostCategories { get; set; }

        public ICollection<PostTag> PostTags { get; set; }

    }
}

