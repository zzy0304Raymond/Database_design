using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using auctionapp.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;


namespace auctionapp.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AuctionDbContext _context;

        public UserController(AuctionDbContext context)
        {
            _context = context;
        }


        //POST api/login
        [HttpPost]
        [HttpPost("login")]
        public async Task<ActionResult<UserLoginDto>> Login([FromBody] LoginDto loginDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
            if (user == null || !VerifyPassword(loginDto.Password, user.Password))
            {
                return BadRequest(new { message = "Invalid credentials." });
            }

            var token = GenerateJwtToken(user);
            return Ok(new UserLoginDto { Token = token, User = new UserDto { Id = user.Userid, Username = user.Username, Email = user.Email } });
        }


        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register([FromBody] RegisterDto registerDto)
        {
            var user = new User
            {
                Username = registerDto.Username,
                Email = registerDto.Email,
                Password = HashPassword(registerDto.Password),
                Registrationdate = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Register), new { id = user.Userid }, new UserDto { Id = user.Userid, Username = user.Username, Email = user.Email });
        }

        [HttpGet("users/{userId}")]
        public async Task<ActionResult<UserDetailsDto>> GetUser(string userId)
        {
            var user = await _context.Users.Include(u => u.Items).Include(u => u.Auctions).Include(u => u.Feedbackpublishes).Include(u => u.TransactionBuyerusers).Include(u => u.TransactionSellerusers).Include(u => u.Notifications).FirstOrDefaultAsync(u => u.Userid == userId);
            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            return Ok(new UserDetailsDto
            {
                Id = user.Userid,
                Username = user.Username,
                Email = user.Email,
                Name = user.Address, // Assuming 'Address' is used as 'Name' for simplicity
                ProfileImage = "url_to_profile_image", // Placeholder for actual profile image URL
                OtherFields = new { Items = user.Items, Auctions = user.Auctions, Feedbacks = user.Feedbackpublishes, Transactions = user.TransactionBuyerusers.Concat(user.TransactionSellerusers), Notifications = user.Notifications }
            });
        }


        [HttpGet("users/{userId}/bids")]
        public async Task<ActionResult<IEnumerable<BidHistoryDto>>> GetUserBids(string userId)
        {
            var bids = await _context.Bidrecords
                .Where(br => br.Userid == userId)
                .Select(br => new BidHistoryDto
                {
                    Id = br.Bidid,
                    ItemId = br.Auction.Item.Itemid , // 如果 Auction 或 Item 为 null，则ItemId为0
                    Amount = (decimal)br.Bidamount,
                    Timestamp = br.Bidtime.HasValue ? br.Bidtime.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") : null // 格式化为 ISO 8601 格式
                })
                .ToListAsync();

            return Ok(bids);
        }


        [HttpPut("users/{userId}")]
        public async Task<ActionResult<UserDto>> UpdateUser(string userId, [FromBody] UpdateUserDto updateUserDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Userid == userId);
            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            user.Username = updateUserDto.Username;
            user.Email = updateUserDto.Email;
            user.Address = updateUserDto.Name; // Assuming 'Address' is used as 'Name' for simplicity
                                               // Update other fields as needed

            await _context.SaveChangesAsync();

            return Ok(new UserDto { Id = user.Userid, Username = user.Username, Email = user.Email });
        }





        private bool VerifyPassword(string password, string storedPassword)
        {
            return password == storedPassword;
        }


        public static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var saltBytes = new byte[128 / 8];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(saltBytes);
                }

                var salt = Convert.ToBase64String(saltBytes);
                var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < hash.Length; i++)
                {
                    hash[i] ^= saltBytes[i % saltBytes.Length];
                }

                var hashedPassword = Convert.ToBase64String(hash);
                return hashedPassword;
            }
        }


        public static string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("j9b8v7x6u5t4s3r2n1m0k0l8j6f4d2b0a9s8d7c6v5b4n3m2");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.NameIdentifier, user.Userid),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(1), // 设置令牌过期时间
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }




        public class LoginDto
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class UserDto
        {
            public string Id { get; set; }
            public string Username { get; set; }
            public string Email { get; set; }
        }

        public class UserLoginDto
        {
            public string Token { get; set; }
            public UserDto User { get; set; }
        }

        public class RegisterDto
        {
            public string Username { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }
        public class UserDetailsDto
        {
            public string Id { get; set; }
            public string Username { get; set; }
            public string Email { get; set; }
            public string Name { get; set; }
            public string ProfileImage { get; set; }
            public dynamic OtherFields { get; set; }
        }

        public class BidHistoryDto
        {
            public string Id { get; set; }
            public string ItemId { get; set; }
            public decimal Amount { get; set; }
            public string Timestamp { get; set; }
        }

        public class UpdateUserDto
        {
            public string Username { get; set; }
            public string Email { get; set; }
            public string Name { get; set; }
            public string ProfileImage { get; set; }
        }

    }
}
