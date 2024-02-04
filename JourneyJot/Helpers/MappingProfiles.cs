using AutoMapper;
using JourneyJot.Dto;
using JourneyJot.Models;

namespace JourneyJot.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<User, UserDto>();

            CreateMap<Post, PostDto>();

            CreateMap<Comment, CommentDto>();

            CreateMap<Category, CategoryDto>();

            CreateMap<Tag, TagDto>();

        }
    }
}
