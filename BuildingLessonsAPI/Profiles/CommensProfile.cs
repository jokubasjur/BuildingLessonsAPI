using AutoMapper;
using BuildingLessonsAPI.DTOs.CommentDTOs;
using BuildingLessonsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingLessonsAPI.Profiles
{
    public class CommensProfile : Profile
    {
        public CommensProfile()
        {
            CreateMap<Comment, CommentReadDTO>();
            CreateMap<CommentCreateDTO, Comment>();
            CreateMap<CommentEditDTO, Comment>();
        }
    }
}
