using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingLessonsAPI.DTOs.UserDTOs
{
    public class UserReadDTO
    {
        public int Id { get; set; }
        public string Role { get; set; }
        public string Nickname { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }
        public string Speciality { get; set; }
    }
}
