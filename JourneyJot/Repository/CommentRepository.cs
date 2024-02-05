using System;
using JourneyJot.Data;
using JourneyJot.Intefaces;
using JourneyJot.Models;

namespace JourneyJot.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly DataContext _context;

        public CommentRepository(DataContext context)
        {
            _context = context;
        }


        public IEnumerable<Comment> GetAll()
        {
            return _context.Comments.ToList();
        }

        public Comment GetById(Guid id)
        {
            return _context.Comments.Where(u => u.Id == id).FirstOrDefault();
        }

        public bool Exists(Guid id)
        {
            return _context.Comments.Any(c => c.Id == id);
        }

        public bool Create(Comment entity)
        {
            entity.Author = _context.Users.Where(u => u.Id == entity.AuthorId).FirstOrDefault();

            entity.Post = _context.Posts.Where(p => p.Id == entity.PostId).FirstOrDefault();

            _context.Add(entity);

            return Save();
        }

        public bool Update(Comment entity)
        {
            _context.Update(entity);
            return Save();
        }

        public bool Delete(Comment entity)
        {
            _context.Remove(entity);
            return Save();
        }

        public bool DeleteComments(IEnumerable<Comment> comments)
        {
            _context.RemoveRange(comments);
            return Save();
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}

