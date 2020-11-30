using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BuildingLessonsAPI.DTOs.ReportDTOs
{
    public class ReportEditDTO
    {
        [Key]
        public int Id { get; set; }
        public bool IsReaded { get; set; }
        public bool IsApproved { get; set; }
    }
}
