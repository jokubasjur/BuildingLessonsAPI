using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingLessonsAPI.DTOs.ReportDTOs
{
    public class ReportReadDTO
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public string Text { get; set; }
        public bool IsReaded { get; set; }
        public bool IsApproved { get; set; }
        public int ReportingUserId { get; set; }
        public int ReportedLessonId { get; set; }
    }
}
