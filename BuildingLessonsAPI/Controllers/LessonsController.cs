using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BuildingLessonsAPI.Models;
using AutoMapper;
using BuildingLessonsAPI.DTOs.LessonDTOs;
using Microsoft.AspNetCore.Authorization;
using BuildingLessonsAPI.Entities;
using BuildingLessonsAPI.DTOs.CommentDTOs;
using BuildingLessonsAPI.DTOs.ReportDTOs;

namespace BuildingLessonsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonsController : ControllerBase
    {
        private readonly LessonsContext _context;
        private readonly IMapper _mapper;

        public LessonsController(LessonsContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Lessons
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LessonReadDTO>>> GetLessons()
        {
            var lessonList = await _context.Lessons.ToListAsync();
            return _mapper.Map<IEnumerable<LessonReadDTO>>(lessonList).ToList();
        }

        // GET: api/Lessons/1/Comments
        [HttpGet("{id}/Comments")]
        public async Task<ActionResult<IEnumerable<CommentReadDTO>>> GetLessonComments(int id)
        {
            var lesson = await _context.Lessons.FindAsync(id);

            if (lesson == null)
            {
                return NotFound();
            }

            var commentList = await _context.Comments.Where(c => c.CommentedLessonId == id).ToListAsync();
            return _mapper.Map<IEnumerable<CommentReadDTO>>(commentList).ToList();
        }

        [Authorize]
        [HttpPost("{id}/Comments")]
        public async Task<ActionResult<CommentReadDTO>> CreateLessonComment(int id, CommentCreateDTO commentCreateDTO)
        {
            var lesson = await _context.Lessons.FindAsync(id);
            if (lesson == null)
            {
                return NotFound();
            }

            var currentUserId = int.Parse(User.Identity.Name);
            if (commentCreateDTO.UserId != currentUserId)
            {
                return Forbid();
            }

            var comment = _mapper.Map<Comment>(commentCreateDTO);
            comment.CommentedLessonId = id;
            comment.CreateDate = DateTime.Now;
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            var commentReadDTO = _mapper.Map<CommentReadDTO>(comment);

            return CreatedAtAction("GetComment", "Comments", new { id = comment.Id }, commentReadDTO);
        }

        // GET: api/Lessons/1/Reports
        [Authorize(Roles = Role.Admin)]
        [HttpGet("{id}/Reports")]
        public async Task<ActionResult<IEnumerable<ReportReadDTO>>> GetLessonReports(int id)
        {
            var lesson = await _context.Lessons.FindAsync(id);

            if (lesson == null)
            {
                return NotFound();
            }

            var reportList = await _context.Reports.Where(r => r.ReportedLessonId == id).ToListAsync();
            return _mapper.Map<IEnumerable<ReportReadDTO>>(reportList).ToList();
        }

        [Authorize]
        [HttpPost("{id}/Reports")]
        public async Task<ActionResult<ReportReadDTO>> CreateLessonReport(int id, ReportCreateDTO reportCreateDTO)
        {
            var lesson = await _context.Lessons.FindAsync(id);
            if (lesson == null)
            {
                return NotFound();
            }

            var currentUserId = int.Parse(User.Identity.Name);
            if (reportCreateDTO.ReportingUserId != currentUserId)
            {
                return Forbid();
            }

            var report = _mapper.Map<Report>(reportCreateDTO);
            report.ReportedLessonId = id;
            report.CreateDate = DateTime.Now;
            report.IsApproved = false;
            report.IsReaded = false;
            _context.Reports.Add(report);
            await _context.SaveChangesAsync();

            var reportReadDTO = _mapper.Map<ReportReadDTO>(report);

            return CreatedAtAction("GetReport", "Reports", new { id = report.Id }, reportReadDTO);
        }

        // GET: api/Lessons/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LessonReadDTO>> GetLesson(int id)
        {
            var lesson = await _context.Lessons.FindAsync(id);

            if (lesson == null)
            {
                return NotFound();
            }

            return _mapper.Map<LessonReadDTO>(lesson);
        }

        // PUT: api/Lessons/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditLesson(int id, LessonEditDTO lessonEditDTO)
        {
            var lesson = await _context.Lessons.FindAsync(id);
            var currentUserId = int.Parse(User.Identity.Name);
            if (lesson == null || lesson.AuthorId != currentUserId)
            {
                return Forbid();
            }     

            if (id != lessonEditDTO.Id)
            {
                return BadRequest();
            }

            _mapper.Map(lessonEditDTO, lesson);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Lessons
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<LessonReadDTO>> CreateLesson(LessonCreateDTO lessonCreateDTO)
        {
            var currentUserId = int.Parse(User.Identity.Name);
            if (lessonCreateDTO.AuthorId != currentUserId)
            {
                return Forbid();
            }

            var lesson = _mapper.Map<Lesson>(lessonCreateDTO);
            lesson.CreateDate = DateTime.Now;

            _context.Lessons.Add(lesson);
            await _context.SaveChangesAsync();

            var lessonReadDTO = _mapper.Map<LessonReadDTO>(lesson);

            return CreatedAtAction("GetLesson", new { id = lesson.Id }, lessonReadDTO);
        }

        // DELETE: api/Lessons/5
        [Authorize(Roles = Role.Admin)]
        [HttpDelete("{id}")]
        public async Task<ActionResult<LessonReadDTO>> DeleteLesson(int id)
        {
            var lesson = await _context.Lessons.FindAsync(id);
            if (lesson == null)
            {
                return NotFound();
            }

            bool isLessonCanBeDeleted = true;
            String errorMessage = "Lesson can't be deleted. Reasons:";
            var comment = await _context.Comments.Where(c => c.CommentedLessonId == id).FirstOrDefaultAsync();
            if (comment != null)
            {
                isLessonCanBeDeleted = false;
                errorMessage += "\n - Lesson have comments associated with it. Delete lesson comments first.";
            }
            var report = await _context.Reports.Where(r => r.ReportedLessonId == id).FirstOrDefaultAsync();
            if (report != null)
            {
                isLessonCanBeDeleted = false;
                errorMessage += "\n - Lesson have reports associated with it. Delete lesson reports first.";
            }
            if (!isLessonCanBeDeleted)
            {
                return BadRequest(new { message = errorMessage });
            }

            _context.Lessons.Remove(lesson);
            await _context.SaveChangesAsync();

            return _mapper.Map<LessonReadDTO>(lesson);
        }

        private bool LessonExists(int id)
        {
            return _context.Lessons.Any(e => e.Id == id);
        }
    }
}
