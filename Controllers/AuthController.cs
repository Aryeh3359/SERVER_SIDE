using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context ?? throw new System.ArgumentNullException(nameof(context));
        }

        [HttpPost("register")]
        public async Task<ActionResult<AppUser>> Register(AppUser user)
        {
            if (user == null || string.IsNullOrWhiteSpace(user.Email))
            {
                return BadRequest();
            }

            if (_context.Users == null)
            {
                return StatusCode(500, new { message = "Database not available" });
            }

            var exists = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (exists != null)
            {
                return Conflict(new { message = "User already exists" });
            }

            if (string.IsNullOrWhiteSpace(user.Id)) user.Id = System.Guid.NewGuid().ToString();

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(null, new { id = user.Id }, user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AppUser>> Login([FromBody] AppUser credentials)
        {
            if (credentials == null || string.IsNullOrWhiteSpace(credentials.Email))
                return BadRequest();

            if (_context.Users == null)
                return StatusCode(500, new { message = "Database not available" });

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == credentials.Email);
            if (user == null)
                return Unauthorized(new { message = "Invalid credentials" });

            // NOTE: This is a minimal placeholder. Do NOT use plain-text checks in production.
            return Ok(user);
        }
    }
}
