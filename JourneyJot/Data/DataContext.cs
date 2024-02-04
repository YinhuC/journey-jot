using System;
using JourneyJot.Models;
using Microsoft.EntityFrameworkCore;

namespace JourneyJot.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PostCategory>()
                .HasKey(pc => new { pc.PostId, pc.CategoryId });
            modelBuilder.Entity<PostCategory>()
                .HasOne(p => p.Post)
                .WithMany(pc => pc.PostCategories)
                .HasForeignKey(p => p.PostId);
            modelBuilder.Entity<PostCategory>()
                .HasOne(p => p.Category)
                .WithMany(pc => pc.PostCategories)
                .HasForeignKey(c => c.CategoryId);

            modelBuilder.Entity<PostTag>()
                .HasKey(pt => new { pt.PostId, pt.TagId });
            modelBuilder.Entity<PostTag>()
                .HasOne(p => p.Post)
                .WithMany(pt => pt.PostTags)
                .HasForeignKey(p => p.PostId);
            modelBuilder.Entity<PostTag>()
                .HasOne(p => p.Tag)
                .WithMany(pt => pt.PostTags)
                .HasForeignKey(t => t.TagId);
        }

    }
}

