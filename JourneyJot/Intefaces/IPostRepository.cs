using System;
using JourneyJot.Models;

namespace JourneyJot.Intefaces
{
    public interface IPostRepository: IRepository<Post>
    {
        IEnumerable<Comment> GetPostComments(Guid postId);

        IEnumerable<Category> GetPostCategories(Guid postId);

        IEnumerable<Tag> GetPostTags(Guid postId);

    }
}

