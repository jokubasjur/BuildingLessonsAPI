using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BuildingLessonsAPI.Models;

namespace BuildingLessonsAPI.DTOs.LessonDTOs
{
    public class LessonReadDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreateDate { get; set; }
        public int AuthorId { get; set; }
    }
}
