using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using auctionapp.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;

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
        public async Task<ActionResult<AdminLoginDto>> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid input data." });
            }
            try
            {
                var user = await _context.Users
                    .Include(user => user.Admins)
                    .FirstOrDefaultAsync(u => u.Email==loginDto.email);
                if (user == null)
                {
                    return BadRequest(new { message = "Illegal email!" });
                }
                else if (user.Admins.Count == 0)
                {
                    return BadRequest(new { message = " Not Admin Permission!" });
                }
                else
                {
                    if (!VerifyPassword(loginDto.password, user.Password))
                    {
                        return BadRequest(new { message = "Incorrect Password!" });
                    }
                    else
                    {
                        var adlogindto = new AdminLoginDto { message = "Login successfully" };
                        return Ok(adlogindto);
                    }
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
            //var admin = await _context.Admins.Include(a => a.Users).FirstOrDefaultAsync(a => a.Users.Any(u => u.Email == loginDto.Email));
            //if (admin == null)
            //{
            //    return BadRequest(new { message = "No admin found with the provided email." });
            //}

            //var user = admin.Users.FirstOrDefault(u => u.Email == loginDto.Email);
            //if (user == null || !VerifyPassword(loginDto.Password, user.Password))
            //{
            //    return BadRequest(new { message = "Invalid credentials." });
            //}

            //var token = GenerateJwtToken(user);
            //return Ok(new AdminLoginDto { Token = token });
        }


        private bool VerifyPassword(string password, string storedPassword)
        {
            return password == storedPassword;
        }

        


    }
}




public class LoginDto
{
    public string email { get; set; }
    public string password { get; set; }
}

public class AdminLoginDto
{
    public string message;
}