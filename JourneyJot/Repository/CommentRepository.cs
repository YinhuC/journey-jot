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

        public bool Add(Comment entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Comment entity)
        {
            throw new NotImplementedException();
        }

        public bool Exists(Guid id)
        {
            return _context.Comments.Any(c => c.Id == id);
        }

        public IEnumerable<Comment> GetAll()
        {
            return _context.Comments.ToList();
        }

        public Comment GetById(Guid id)
        {
            return _context.Comments.Where(u => u.Id == id).FirstOrDefault();
        }

        public bool Update(Comment entity)
        {
            throw new NotImplementedException();
        }
    }
}

