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

        [HttpGet("health")]
        public ActionResult HealthCheck()
        {
            return Ok(new { status = "Auth service is running" });
        }

        [HttpPost("register")]
        public async Task<ActionResult<AppUser>> Register(AppUserDTO user)
        {
            
            if (_context.Users == null)
            {
                return StatusCode(500, new { message = "Database not available" });
            }

            var exists = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (exists != null)
            {
                return Conflict(new { message = "User already exists" });
            }

            var newUser = new AppUser
            {
                DisplayName = user.DisplayName,
                Email = user.Email
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return Ok(newUser);

        }

    [HttpPost("login")]
public async Task<ActionResult<AppUser>> Login([FromBody] AppUser credentials)
{
    // 1️⃣ Validate input
    if (credentials == null || string.IsNullOrWhiteSpace(credentials.Email) )
        return BadRequest(new { message = "Email and password are required." });

    // 2️⃣ Check database availability
    if (_context.Users == null)
        return StatusCode(500, new { message = "Database not available." });

    // 3️⃣ Find user by email
    var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == credentials.Email);
    if (user == null)
        return Unauthorized(new { message = "Invalid email or password." });

   

    // 5️⃣ Return success
    return Ok(new
    {
        message = "Login successful!",
        user = new
        {
            
            user.Email,
            user.DisplayName
        }
    });
}

        }
    }

