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

        public IEnumerable<Tag> GetAll()
        {
            throw new NotImplementedException();
        }

        public Tag GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Tag entity)
        {
            throw new NotImplementedException();
        }
    }
}

