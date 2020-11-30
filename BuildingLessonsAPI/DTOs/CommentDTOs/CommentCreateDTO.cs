using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BuildingLessonsAPI.DTOs.CommentDTOs
{
    public class CommentCreateDTO
    {
        [Required]
        public string Text { get; set; }
        public int UserId { get; set; }
    }
}
