using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BuildingLessonsAPI.Models;
using AutoMapper;
using BuildingLessonsAPI.DTOs.CommentDTOs;
using Microsoft.AspNetCore.Authorization;
using BuildingLessonsAPI.Entities;

namespace BuildingLessonsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly LessonsContext _context;
        private readonly IMapper _mapper;

        public CommentsController(LessonsContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Comments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CommentReadDTO>> GetComment(int id)
        {
            var comment = await _context.Comments.FindAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            return _mapper.Map<CommentReadDTO>(comment);
        }

        // PUT: api/Comments/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditComment(int id, CommentEditDTO commentEditDTO)
        {
            var comment = await _context.Comments.FindAsync(id);
            var currentUserId = int.Parse(User.Identity.Name);
            if (comment == null || comment.UserId != currentUserId)
            {
                return Forbid();
            }
            if (id != comment.Id)
            {
                return BadRequest();
            }

            _mapper.Map(commentEditDTO, comment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Comments/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<CommentReadDTO>> DeleteComment(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            var currentUserId = int.Parse(User.Identity.Name);
            if (comment.UserId != currentUserId && !User.IsInRole(Role.Admin))
            {
                return Forbid();
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return _mapper.Map<CommentReadDTO>(comment);
        }

        private bool CommentExists(int id)
        {
            return _context.Comments.Any(e => e.Id == id);
        }
    }
}
