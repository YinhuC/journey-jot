using System;
using JourneyJot.Data;
using JourneyJot.Intefaces;
using JourneyJot.Models;

namespace JourneyJot.Repository
{
    public class TagRepository : ITagRepository
    {
        private readonly DataContext _context;

        public TagRepository(DataContext context)
        {
            _context = context;
        }

        public void Add(Tag entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Tag entity)
        {
            throw new NotImplementedException();
        }

        public bool Exists(Guid id)
        {
            return _context.Tags.Any(t => t.Id == id);
        }

        public IEnumerable<Tag> GetAll()
        {
            return _context.Tags.ToList();
        }

        public Tag GetById(Guid id)
        {
            return _context.Tags.Where(u => u.Id == id).FirstOrDefault();
        }

        public void Update(Tag entity)
        {
            throw new NotImplementedException();
        }
    }
}

