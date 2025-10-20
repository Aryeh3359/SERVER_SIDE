using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly AppDbContext context;

        public MembersController(AppDbContext context)
        {
            this.context = context ?? throw new System.ArgumentNullException(nameof(context));
        }
        [HttpGet] // GET: api/members
        public async Task<ActionResult<IReadOnlyList<AppUser>>> GetMembers()
        {
            var res = context.Users == null ? new List<AppUser>() : await context.Users.ToListAsync();
            return Ok(res);
        }

        [HttpGet("{id}")] // GET: api/members/3
        public async Task<ActionResult<AppUser>> GetMember(string id)
        {
            if (context.Users == null)
            {
                return NotFound();
            }

            var res = await context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (res == null)
            {
               return NotFound();
           }
            return  Ok(res);
        }

        [HttpGet("by-name/{displayName}")] // GET: api/members/by-name/sagiv
        public ActionResult<AppUser> GetMemberByDisplayName(string displayName)
        {
            if (context.Users == null)
            {
                return NotFound();
            }

            var res = context.Users.FirstOrDefault(u => u.DisplayName == displayName);
            return res == null ? NotFound() : Ok(res);
        }
    }
}