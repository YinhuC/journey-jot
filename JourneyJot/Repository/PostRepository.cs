﻿using System;
using System.Runtime.Intrinsics.Arm;
using System.Xml.Linq;
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

        public bool Exists(Guid id)
        {
            return _context.Posts.Any(p => p.Id == id);
        }

        public bool Create(Post entity)
        {
            entity.Author = _context.Users.Where(u => u.Id == entity.AuthorId).FirstOrDefault();

            _context.Add(entity);

            return Save();
        }

        public bool Update(Post entity)
        {
            _context.UpdateRange(entity);
            return Save();
        }

        public bool Delete(Post entity)
        {
            _context.RemoveRange(entity);
            return Save();
        }

        public bool DeletePosts(IEnumerable<Post> posts)
        {
            foreach (var post in posts)
            {
                var comments = _context.Comments.Where(c => c.PostId == post.Id);
                _context.Comments.RemoveRange(comments);
            }
            _context.Posts.RemoveRange(posts);

            return Save();
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    
    }
}

