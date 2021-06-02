using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SecandAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BC = BCrypt.Net.BCrypt;

namespace SecandAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly MahmoudContext _context;

        public RegisterController(MahmoudContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<ActionResult<UserInform>> PostUserInform(UserInform userInform)
        {
            userInform.Password = BC.HashPassword(userInform.Password);

            _context.UserInform.Add(userInform);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserInform", new { id = userInform.UserId }, userInform);
        }
    }
}
