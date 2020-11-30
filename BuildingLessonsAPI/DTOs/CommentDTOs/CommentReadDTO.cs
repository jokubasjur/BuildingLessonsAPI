using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingLessonsAPI.DTOs.CommentDTOs
{
    public class CommentReadDTO
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public string Text { get; set; }
        public int UserId { get; set; }
        public int CommentedLessonId { get; set; }
    }
}
