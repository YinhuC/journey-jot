using System;
using JourneyJot.Data;
using JourneyJot.Intefaces;
using JourneyJot.Models;
using Microsoft.EntityFrameworkCore;

namespace JourneyJot.Repository 
{
    public class PostRepository: IPostRepository
    {
        private readonly DataContext _context;

        public PostRepository(DataContext context)
        {
            _context = context;
        }

        public void Add(Post entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Post entity)
        {
            throw new NotImplementedException();
        }

        public bool Exists(Guid id)
        {
            return _context.Posts.Any(p => p.Id == id);
        }

        public IEnumerable<Post> GetAll()
        {
            return _context.Posts.ToList();
        }

        public Post GetById(Guid id)
        {
            return _context.Posts.Where(u => u.Id == id).FirstOrDefault();
        }

        public void Update(Post entity)
        {
            throw new NotImplementedException();
        }
    }
}

