using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecandAPI.Models;
using BC = BCrypt.Net.BCrypt;

namespace SecandAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserInfoController : ControllerBase
    {
        private readonly MahmoudContext _context;

        public UserInfoController(MahmoudContext context)
        {
            _context = context;
        }

        // GET: api/UserInfo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserInform>>> GetUserInform()
        {
            return await _context.UserInform.ToListAsync();
        }

        // GET: api/UserInfo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserInform>> GetUserInform(int id)
        {
            var userInform = await _context.UserInform.FindAsync(id);

            if (userInform == null)
            {
                return NotFound();
            }

            return userInform;
        }

        // PUT: api/UserInfo/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserInform(int id, UserInform userInform)
        {
            if (id != userInform.UserId)
            {
                return BadRequest();
            }

            _context.Entry(userInform).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserInformExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/UserInfo
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [AllowAnonymous] // that to make this Method don't need Authorization
        [HttpPost("register")]
        public async Task<ActionResult<UserInform>> PostUserInform(UserInform userInform)
        {
            userInform.Password = BC.HashPassword(userInform.Password);

            _context.UserInform.Add(userInform);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserInform", new { id = userInform.UserId }, userInform);
        }

        // DELETE: api/UserInfo/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserInform>> DeleteUserInform(int id)
        {
            var userInform = await _context.UserInform.FindAsync(id);
            if (userInform == null)
            {
                return NotFound();
            }

            _context.UserInform.Remove(userInform);
            await _context.SaveChangesAsync();

            return userInform;
        }

        private bool UserInformExists(int id)
        {
            return _context.UserInform.Any(e => e.UserId == id);
        }
    }
}
