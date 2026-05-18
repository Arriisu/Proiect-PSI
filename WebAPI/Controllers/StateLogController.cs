using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebAPI.Data;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StateLogController : ControllerBase
    {
        private readonly AppDbContext _db;
        public StateLogController(AppDbContext db)
        {
            _db = db;
        }

        //POST api/statelog - save a new state change
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> LogState([FromBody] StateLogDto dto)
        {
            var username = User.FindFirstValue(ClaimTypes.Email) ?? "unknown";
            var log = new StateLog
            {
                Timestamp = DateTime.UtcNow,
                ChangedByUser = username,
                StateJson = dto.StateJson,
                ActionDescription = dto.ActionDescription
            };

            _db.StateLogs.Add(log);
            await _db.SaveChangesAsync();

            return Ok("State logged succesfully!");
        }

        //GET api/statelog - get history of all state shanges
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetHistory()
        {
            var logs = await _db.StateLogs
                .OrderByDescending(s => s.Timestamp)
                .Take(50)
                .ToListAsync();

            return Ok(logs);
        }
    }

    public class StateLogDto
    {
        public string StateJson { get; set; } = "{}";
        public string ActionDescription { get; set; } = string.Empty;
    }
}