using System;
using System.Runtime.Intrinsics.Arm;
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

        public bool Add(Post entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Post entity)
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
            return _context.Posts.Where(p => p.Id == id).FirstOrDefault();
        }

        public IEnumerable<Comment> GetPostComments(Guid postId)
        {
            return _context.Posts.Where(p => p.Id == postId).SelectMany(p => p.Comments).ToList();
        }

        public IEnumerable<Category> GetPostCategories(Guid postId)
        {
            return _context.PostCategories.Where(pc => pc.Post.Id == postId).Select(pc => pc.Category).ToList();
        }

        public IEnumerable<Tag> GetPostTags(Guid postId)
        {
            return _context.PostTags.Where(pt => pt.Post.Id == postId).Select(pt => pt.Tag).ToList();
        }

        public bool Update(Post entity)
        {
            throw new NotImplementedException();
        }

        bool IRepository<Post>.Add(Post entity)
        {
            throw new NotImplementedException();
        }

        bool IRepository<Post>.Delete(Post entity)
        {
            throw new NotImplementedException();
        }

        bool IRepository<Post>.Update(Post entity)
        {
            throw new NotImplementedException();
        }
    }
}

