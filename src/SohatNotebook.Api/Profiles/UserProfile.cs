using AutoMapper;
using Entities.DbSet;
using Entities.Dtos;

namespace SohatNotebook.Api.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserDto, User>().ReverseMap();
            CreateMap<User, ProfileDto>().ReverseMap();
        }
    }
}
