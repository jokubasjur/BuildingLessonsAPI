using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildingLessonsAPI.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreateDate { get; set; }

        [Required]
        public string Text { get; set; }
        public int UserId { get; set; }
        public int CommentedLessonId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [ForeignKey("CommentedLessonId")]
        public Lesson Lesson { get; set; }
    }
}
