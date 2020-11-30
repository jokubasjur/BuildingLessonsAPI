using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BuildingLessonsAPI.DTOs.ReportDTOs
{
    public class ReportCreateDTO
    {
        [Required]
        public string Text { get; set; }
        public int ReportingUserId { get; set; }
    }
}
