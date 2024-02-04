using System;
using JourneyJot.Models;

namespace JourneyJot.Intefaces

{
    public interface IUserRepository : IRepository<User>
    {
        User GetByUsername(string username);

        User GetByEmail(string email);

        IEnumerable<Post> GetPostsByUser(Guid id);

        IEnumerable<Comment> GetCommentsByUser(Guid id);

    }
}

