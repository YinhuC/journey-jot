using System;
using JourneyJot.Models;

namespace JourneyJot.Intefaces
{
    public interface ICategoryRepository: IRepository<Category>
    {
        IEnumerable<Post> GetPostsByCategory(Guid categoryId);

        Category GetCategoryByName(string name);
    }
}

