using AutoMapper;
using JourneyJot.Dto;
using JourneyJot.Models;

namespace JourneyJot.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // Users
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();

            CreateMap<UserDtoCreate, User>();
            CreateMap<User, UserDtoCreate>();

            CreateMap<User, UserDtoUpdate>();
            CreateMap<UserDtoUpdate, User>();

            // Posts
            CreateMap<Post, PostDto>();
            CreateMap<PostDto, Post>();

            CreateMap<Post, PostDtoCreate>();
            CreateMap<PostDtoCreate, Post>();

            CreateMap<Post, PostDtoUpdate>();
            CreateMap<PostDtoUpdate, Post>();

            // Comments
            CreateMap<Comment, CommentDto>();
            CreateMap<CommentDto, Comment>();

            CreateMap<Comment, CommentDtoCreate>();
            CreateMap<CommentDtoCreate, Comment>();

            CreateMap<Comment, CommentDtoUpdate>();
            CreateMap<CommentDtoUpdate, Comment>();

            // Categories
            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();

            CreateMap<Category, CategoryDtoCreate>();
            CreateMap<CategoryDtoCreate, Category>();

            CreateMap<Category, CategoryDtoUpdate>();
            CreateMap<CategoryDtoUpdate, Category>();

            // Tags
            CreateMap<Tag, TagDto>();
            CreateMap<TagDto, Tag>();

            CreateMap<Tag, TagDtoCreate>();
            CreateMap<TagDtoCreate, Tag>();

            CreateMap<Tag, TagDtoUpdate>();
            CreateMap<TagDtoUpdate, Tag>();

        }
    }
}
