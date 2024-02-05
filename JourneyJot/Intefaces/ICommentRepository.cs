using System;
using JourneyJot.Models;

namespace JourneyJot.Intefaces
{
    public interface ICommentRepository: IRepository<Comment>
    {
        bool DeleteComments(IEnumerable<Comment> posts);

    }
}

