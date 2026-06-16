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
        //[Authorize]
        public async Task<IActionResult> LogState([FromBody] StateLogDto dto)
        {
            var username = User.FindFirstValue(ClaimTypes.Email) ?? "unknown";
            var log = new StateLog
            {
                Timestamp = DateTime.UtcNow,
                ChangedByUser = username,
                StateJson = System.Text.Json.JsonSerializer.Serialize(dto.State),
                ActionDescription = dto.ActionDescription,
                AlarmActive = dto.AlarmActive
            };

            _db.StateLogs.Add(log);
            await _db.SaveChangesAsync();

            return Ok("State logged succesfully!");
        }

        //GET api/statelog - get history of all state shanges
        [HttpGet]
        //[Authorize]
        public async Task<IActionResult> GetHistory()
        {
            var logs = await _db.StateLogs
                .OrderByDescending(s => s.Timestamp)
                .Take(50)
                .ToListAsync();

            return Ok(logs);
        }

        [HttpGet("alarms")]
        //[Authorize]
        public async Task<IActionResult> GetAlarms()
        {
            var alarms = await _db.StateLogs
                .Where(s => s.AlarmActive == true)
                .OrderByDescending(s => s.Timestamp)
                .Take(50)
                .ToListAsync();

            return Ok(alarms);
                
        }
    }

    public class StateLogDto
    {
        public ConveyorSystemState State { get; set; } = new();
        public string ActionDescription { get; set; } = string.Empty;
        public bool AlarmActive { get; set; } = false;
    }
}