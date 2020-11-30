using System.ComponentModel.DataAnnotations;

namespace BuildingLessonsAPI.Entities
{
    public class UserAuthenticate
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
