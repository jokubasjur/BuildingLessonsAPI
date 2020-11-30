using AutoMapper;
using BuildingLessonsAPI.DTOs.LessonDTOs;
using BuildingLessonsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingLessonsAPI.Profiles
{
    public class LessonsProfile : Profile
    {
        public LessonsProfile()
        {
            CreateMap<Lesson, LessonReadDTO>();
            CreateMap<LessonCreateDTO, Lesson>();
            CreateMap<LessonEditDTO, Lesson>();
        }
    }
}
