﻿using System;
using JourneyJot.Data;
using JourneyJot.Intefaces;
using JourneyJot.Models;

namespace JourneyJot.Repository
{
    public class UserRepository: IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }


        public IEnumerable<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public User GetByEmail(string email)
        {
            return _context.Users.Where(u => u.Email == email).FirstOrDefault();
        }

        public User GetById(Guid id)
        {
            return _context.Users.Where(u => u.Id == id).FirstOrDefault();
        }

        public User GetByUsername(string username)
        {
            return _context.Users.Where(u => u.Username == username).FirstOrDefault();
        }

        public IEnumerable<Comment> GetCommentsByUser(Guid id)
        {
            return _context.Users.Where(u => u.Id == id).SelectMany(u => u.Comments).ToList();
        }

        public IEnumerable<Post> GetPostsByUser(Guid id)
        {
            return _context.Users.Where(u => u.Id == id).SelectMany(u => u.Posts).ToList();
        }

        public bool Exists(Guid id)
        {
            return _context.Users.Any(u => u.Id == id);
        }

        public bool Create(User entity)
        {
            _context.Add(entity);
            return Save();
        }

        public bool Update(User entity)
        {
            _context.Update(entity);
            return Save();
        }

        public bool Delete(User entity)
        {
            _context.Remove(entity);
            return Save();
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}

