using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using second_api.Data;
using second_api.Models;

namespace second_api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class IssueController : ControllerBase
    {
        private readonly AppDbContext _context;
        public IssueController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<IEnumerable<Issue>> Get()
        => await _context.Issues.ToListAsync();


        [HttpGet("id")]
        [ProducesResponseType(typeof(Issue), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var issue = await _context.Issues.FindAsync(id);
            if (issue == null) return Ok("ID Not Found");
            return Ok(issue);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create(Issue issue )
        {
            if (issue.Id == Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"))
            {
                issue.Id = Guid.NewGuid();
            }
            var titleIsEx = await _context.Issues.FirstOrDefaultAsync(x => x.Title == issue.Title) ;
            if (titleIsEx != null)
            {
                return Ok("Title is already exist");
            }

            if (string.IsNullOrEmpty(issue.Title))
            {
                return Ok("Title is required");
            }

            if (string.IsNullOrEmpty(issue.Description))
            {
                return Ok("Description is required");
            }

            await _context.Issues.AddAsync(issue);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = issue.Id }, issue);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(Guid id, Issue issue)
        {
            if (id != issue.Id) return Ok("check form ID");
            _context.Entry(issue).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return  Ok("Successfully updated");
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var issueToDelete = await _context.Issues.FindAsync(id);
            if (issueToDelete == null) return Ok("ID Not Found");

            _context.Issues.Remove(issueToDelete);
            await _context.SaveChangesAsync();

            return Ok("Successfully Deleted");
        }
    }
}
