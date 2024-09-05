using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using auctionapp.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace auctionapp.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly AuctionDbContext _context;

        public AdminController(AuctionDbContext context)
        {
            _context = context;
        }

        //POST :api/admin/login
        [HttpPost("login")]

        public async Task<ActionResult<AdminLoginDto>>  Login([FromBody] LoginDto loginDto)
        {
            var admin = await _context.Admins.Include(a => a.Users).FirstOrDefaultAsync(a => a.Users.Any(u => u.Email == loginDto.Email));
            if (admin == null)
            {
                return BadRequest(new { message = "No admin found with the provided email." });
            }

            var user = admin.Users.FirstOrDefault(u => u.Email == loginDto.Email);
            if (user == null || !VerifyPassword(loginDto.Password, user.Password))
            {
                return BadRequest(new { message = "Invalid credentials." });
            }

            var token = GenerateJwtToken(user);
            return Ok(new AdminLoginDto { Token = token });
        }


        private bool VerifyPassword(string password, string storedPassword)
        {
            return password == storedPassword;
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("j9b8v7x6u5t4s3r2n1m0k0l8j6f4d2b0a9s8d7c6v5b4n3m2"); // Replace with your actual secret key
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.NameIdentifier, user.Userid),
                new Claim(ClaimTypes.Name, user.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(2), // Token expiration time
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


    }
}




public class LoginDto
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class AdminLoginDto
{
    public string Token { get; set; }
}

