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
        }
    }
}
