using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SecandAPI.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BC = BCrypt.Net.BCrypt;

namespace SecandAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly MahmoudContext _context;

        public TokenController(IConfiguration config, MahmoudContext context)
        {
            _context = context;
            _configuration = config;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Post(UserInform userData)
        {
            if(userData!= null && userData.Email!=null && userData.Password != null)
            {
                //  var user = await GetData(userData.Email, userData.Password);
                //  var user = await GetData(userData.Email);

                var user = _context.UserInform.SingleOrDefault(x => x.Email == userData.Email);
                //Console.WriteLine(BC.Verify(userData.Password, user.Password));
                if (user != null || BC.Verify(userData.Password, user.Password))
                {
                    var claims = new[]
                    {
                       new Claim (JwtRegisteredClaimNames.Sub,_configuration["Jwt:Supject"]),
                       new Claim (JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                       new Claim (JwtRegisteredClaimNames.Iat,DateTime.UtcNow.ToString()),
                       new Claim ("id",user.UserId.ToString()),
                       new Claim ("FirstName",user.FirstName),
                       new Claim ("LastName",user.LastName),
                       new Claim ("UserName",user.UserName),
                       new Claim ("Email",user.Email),
                    };
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var sighin = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(_configuration["Jwt:Issure"], _configuration["Jwt:Audience"]
                        , claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: sighin);

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return BadRequest("Invalid email or password");
                }
            }
            else
            {
                return BadRequest();
            }
            //_context.Books.Add(books);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetBooks", new { id = books.BookId }, books);
        }

        private async Task<UserInform> GetData(string email, string password)
        {
            return await _context.UserInform.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
        }
        private async Task<UserInform> GetData(string email)
        {
            return await _context.UserInform.FirstOrDefaultAsync(u => u.Email == email );
        }
    }
}
