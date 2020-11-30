using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildingLessonsAPI.Models
{
    public class Report
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreateDate { get; set; }

        [Required]
        public string Text { get; set; }
        public bool IsReaded { get; set; }
        public bool IsApproved { get; set; }
        public int ReportingUserId { get; set; }
        public int ReportedLessonId { get; set; }

        [ForeignKey("ReportingUserId")]
        public User User { get; set; }

        [ForeignKey("ReportedLessonId")]
        public Lesson Lesson { get; set; }
    }
}
