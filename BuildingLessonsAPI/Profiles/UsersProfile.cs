using AutoMapper;
using BuildingLessonsAPI.DTOs.UserDTOs;
using BuildingLessonsAPI.Models;

namespace BuildingLessonsAPI.Profiles
{
    public class UsersProfile : Profile
    {
        public UsersProfile()
        {
            CreateMap<User, UserReadDTO>();
            CreateMap<UserCreateDTO, User>();
            CreateMap<User, UserWithTokenDTO>();
            CreateMap<UserEditDTO, User>();
        }
    }
}
