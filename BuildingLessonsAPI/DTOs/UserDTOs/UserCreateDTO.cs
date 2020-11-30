using System.ComponentModel.DataAnnotations;

namespace BuildingLessonsAPI.DTOs.UserDTOs
{
    public class UserCreateDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        [Required]
        public string Nickname { get; set; }
        public string Description { get; set; }
        public string Speciality { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Email { get; set; }
    }
}
