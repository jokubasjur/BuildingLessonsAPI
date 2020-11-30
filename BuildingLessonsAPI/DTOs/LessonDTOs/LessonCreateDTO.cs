using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildingLessonsAPI.DTOs.LessonDTOs
{
    public class LessonCreateDTO
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Text { get; set; }

        public int AuthorId { get; set; }
    }
}
