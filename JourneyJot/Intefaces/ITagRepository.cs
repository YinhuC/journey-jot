using System;
using JourneyJot.Models;

namespace JourneyJot.Intefaces
{
    public interface ITagRepository: IRepository<Tag>
    {
        IEnumerable<Post> GetPostByTag(Guid tagId);
    }
}

