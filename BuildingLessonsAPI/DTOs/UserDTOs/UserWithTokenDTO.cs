using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingLessonsAPI.DTOs.UserDTOs
{
    public class UserWithTokenDTO : UserReadDTO
    {
        public string Token { get; set; }
    }
}
