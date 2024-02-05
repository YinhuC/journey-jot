using System;
using JourneyJot.Data;
using JourneyJot.Intefaces;
using JourneyJot.Models;

namespace JourneyJot.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _context;

        public CategoryRepository(DataContext context)
        {
            _context = context;
        }

        public IEnumerable<Category> GetAll()
        {
            return _context.Categories.ToList();
        }

        public Category GetById(Guid id)
        {
            return _context.Categories.Where(c => c.Id == id).FirstOrDefault();
        }

        public Category GetCategoryByName(string name)
        {
            return _context.Categories.Where(c => c.Name == name).FirstOrDefault();
        }

        public IEnumerable<Post> GetPostsByCategory(Guid categoryId)
        {
            return _context.PostCategories.Where(pc => pc.Category.Id == categoryId).Select(pc => pc.Post).ToList();
        }

        public bool Exists(Guid id)
        {
            return _context.Categories.Any(c => c.Id == id);
        }

        public bool Create(Category entity)
        {
            _context.Add(entity);
            return Save();
        }

        public bool Update(Category entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Category entity)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}

