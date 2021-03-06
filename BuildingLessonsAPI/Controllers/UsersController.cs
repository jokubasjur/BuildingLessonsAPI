﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BuildingLessonsAPI.Models;
using BuildingLessonsAPI.DTOs.UserDTOs;
using AutoMapper;
using BuildingLessonsAPI.Entities;
using BuildingLessonsAPI.Services;
using Microsoft.AspNetCore.Authorization;
using BuildingLessonsAPI.DTOs.LessonDTOs;

namespace BuildingLessonsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly LessonsContext _context;
        private readonly IMapper _mapper;
        private IUserService _userService;

        public UsersController(LessonsContext context, IMapper mapper, IUserService userService)
        {
            _context = context;
            _mapper = mapper;
            _userService = userService;
        }

        // GET: api/Users/1/Lessons
        [HttpGet("{id}/Lessons")]
        public async Task<ActionResult<IEnumerable<LessonReadDTO>>> GetUserLessons(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var lessonList = await _context.Lessons.Where(l => l.AuthorId == id).ToListAsync();

            return _mapper.Map<IEnumerable<LessonReadDTO>>(lessonList).ToList();
        }

        // GET: api/Users
        [Authorize(Roles = Role.Admin)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserReadDTO>>> GetUsers()
        {
            var userList = await _context.Users.ToListAsync();

            return _mapper.Map<IEnumerable<UserReadDTO>>(userList).ToList();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserReadDTO>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return _mapper.Map<UserReadDTO>(user);
        }

        // PUT: api/Users/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditUser(int id, UserEditDTO userEditDTO)
        {
            var currentUserId = int.Parse(User.Identity.Name);
            if (id != currentUserId)
                return Forbid();

            if (id != userEditDTO.Id)
            {
                return BadRequest();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _mapper.Map(userEditDTO, user);
            if (!string.IsNullOrWhiteSpace(userEditDTO.Password))
            {
                _userService.CreateHashedPassword(user, userEditDTO.Password);
            }
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<UserReadDTO>> CreateUser(UserCreateDTO userCreateDTO)
        {
            var existingUser = await _context.Users.Where(u => u.Username.Equals(userCreateDTO.Username)).FirstOrDefaultAsync();
            if (existingUser != null)
            {
                return BadRequest(new { message = "User with username \"" + userCreateDTO.Username  + "\" already exist." });
            }

            var user = _mapper.Map<User>(userCreateDTO);

            user.Role = Role.User;
            _userService.CreateHashedPassword(user, userCreateDTO.Password);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var returningUser = _mapper.Map<UserReadDTO>(user);

            return CreatedAtAction("GetUser", new { id = user.Id }, returningUser);
        }

        // DELETE: api/Users/5
        [Authorize(Roles = Role.Admin)]
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserReadDTO>> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            bool isUserCanBeDeleted = true;
            String errorMessage = "User can't be deleted. Reasons:";
            var lesson = await _context.Lessons.Where(l => l.AuthorId == id).FirstOrDefaultAsync();
            if (lesson != null)
            {
                isUserCanBeDeleted = false;
                errorMessage += "\n - User have lessons associated with him. Delete user lessons first.";
            }

            var comment = await _context.Comments.Where(c => c.UserId == id).FirstOrDefaultAsync();
            if (comment != null)
            {
                isUserCanBeDeleted = false;
                errorMessage += "\n - User have comments associated with him. Delete user comments first.";
            }

            var report = await _context.Reports.Where(r => r.ReportingUserId == id).FirstOrDefaultAsync();
            if (report != null)
            {
                isUserCanBeDeleted = false;
                errorMessage += "\n - User have reports associated with him. Delete user reports first.";
            }
            if (!isUserCanBeDeleted)
            {
                return BadRequest(new { message = errorMessage });
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return _mapper.Map<UserReadDTO>(user);
        }

        [HttpPost("authenticate")]
        public async Task<ActionResult<UserWithTokenDTO>> AuthenticateUser(UserAuthenticate userAuthenticate)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username.Equals(userAuthenticate.Username));
            if (user == null)
            {
                return BadRequest(new { message = "Username is incorrect" });
            }
            if (!_userService.VerifyPassword(user, userAuthenticate.Password))
            {
                return BadRequest(new { message = "Password is incorrect" });
            }

            UserWithTokenDTO userWithToken = _mapper.Map<UserWithTokenDTO>(user);
            _userService.Authenticate(userWithToken);

            return userWithToken;
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
