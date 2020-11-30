using AutoMapper;
using BuildingLessonsAPI.DTOs.ReportDTOs;
using BuildingLessonsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingLessonsAPI.Profiles
{
    public class ReportProfile : Profile
    {
        public ReportProfile()
        {
            CreateMap<Report, ReportReadDTO>();
            CreateMap<ReportCreateDTO, Report>();
            CreateMap<ReportEditDTO, Report>();
        }
    }
}
