using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BuildingLessonsAPI.Models;
using Microsoft.AspNetCore.Authorization;
using BuildingLessonsAPI.Entities;
using BuildingLessonsAPI.DTOs.ReportDTOs;
using AutoMapper;

namespace BuildingLessonsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly LessonsContext _context;
        private readonly IMapper _mapper;

        public ReportsController(LessonsContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Reports
        [Authorize(Roles = Role.Admin)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReportReadDTO>>> GetReports()
        {
            var reportList = await _context.Reports.ToListAsync();
            return _mapper.Map<IEnumerable<ReportReadDTO>>(reportList).ToList();
        }

        // GET: api/Reports/5
        [Authorize(Roles = Role.Admin)]
        [HttpGet("{id}")]
        public async Task<ActionResult<ReportReadDTO>> GetReport(int id)
        {
            var report = await _context.Reports.FindAsync(id);
            if (report == null)
            {
                return NotFound();
            }

            var currentUserId = int.Parse(User.Identity.Name);
            if (report.ReportingUserId != currentUserId && !User.IsInRole(Role.Admin))
            {
                return Forbid();
            }

            return _mapper.Map<ReportReadDTO>(report);
        }

        // PUT: api/Reports/5
        [Authorize(Roles = Role.Admin)]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditReport(int id, ReportEditDTO reportEditDTO)
        {
            if (id != reportEditDTO.Id)
            {
                return BadRequest();
            }

            var report = await _context.Reports.FindAsync(id);
            _mapper.Map(reportEditDTO, report);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Reports/5
        [Authorize(Roles = Role.Admin)]
        [HttpDelete("{id}")]
        public async Task<ActionResult<ReportReadDTO>> DeleteReport(int id)
        {
            var report = await _context.Reports.FindAsync(id);
            if (report == null)
            {
                return NotFound();
            }

            _context.Reports.Remove(report);
            await _context.SaveChangesAsync();

            return _mapper.Map<ReportReadDTO>(report);
        }

        private bool ReportExists(int id)
        {
            return _context.Reports.Any(e => e.Id == id);
        }
    }
}
