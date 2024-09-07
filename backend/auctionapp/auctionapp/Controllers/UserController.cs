using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using auctionapp.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore.Storage;


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
        [HttpPost("login")]
        public async Task<ActionResult<UserLoginDto>> Login([FromBody] ULoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid input data." });
            }
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(user => user.Email == loginDto.email);
                if (user == null)
                {
                    return NotFound(new { message = "Email does not exist !" });
                }

                else if (user.Password != loginDto.password)
                {
                    return BadRequest(new { message = "Password error!" });
                }

                var token = GenerateJwtToken(user);
                return Ok(new UserLoginDto { token = token, user = new UserDto { id = user.Userid, username = user.Username, email = user.Email } });


            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            //var user = await _context.Users.FindAsync(loginDto.)
            //if (user == null || !VerifyPassword(loginDto.Password, user.Password))
            //{
            //    return BadRequest(new { message = "Invalid credentials." });
            //}

            //var token = GenerateJwtToken(user);
            //return Ok(new UserLoginDto { Token = token, User = new UserDto { Id = user.Userid, Username = user.Username, Email = user.Email } });
        }


        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register([FromBody] RegisterDto newuser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid input data." });
            }

            // 检查用户名和邮箱是否已经存在
            var existingUserByUsername = await _context.Users.FirstOrDefaultAsync(u => u.Username == newuser.username);
            var existingUserByEmail = await _context.Users.FirstOrDefaultAsync(u => u.Email == newuser.email);

            if (existingUserByUsername != null)
            {
                return BadRequest(new { message = "Username already exists." });
            }

            if (existingUserByEmail != null)
            {
                return BadRequest(new { message = "Email already exists." });
            }

            try
            {
                // 获取当前最大的 Userid 并加 1
                var maxUserId = await _context.Users.MaxAsync(u => (decimal?)u.Userid) ?? 0;
                var user = new User
                {
                    Userid = maxUserId + 1,
                    Username = newuser.username,
                    Email = newuser.email,
                    Password = newuser.password,
                    Registrationdate = DateTime.UtcNow
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(Register), new { id = user.Userid }, new UserDto { id = user.Userid, username = user.Username, email = user.Email });
            }
            catch (DbUpdateException ex)
            {
                var errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                // 日志记录错误信息
                Console.WriteLine(errorMessage);
                // 返回错误信息
                return BadRequest(new { message = "An error occurred while registering the user." });
            }
        }



        //获取用户资料
        [HttpGet("users/{userId}")]
        public async Task<ActionResult<UserDetailsDto>> GetUser(decimal userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid input data." });
            }

            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    return NotFound(new { message = "User not found" });
                }
                var responseDto = new UserDetailsDto
                {
                    username = user.Username,
                    email = user.Email,
                    id = user.Userid,
                };
                return Ok(responseDto);


            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            //var user = await _context.Users.Include(u => u.Items).Include(u => u.Auctions).Include(u => u.Feedbackpublishes).Include(u => u.TransactionBuyerusers).Include(u => u.TransactionSellerusers).Include(u => u.Notifications).FirstOrDefaultAsync(u => u.Userid == userId);
            //if (user == null)
            //{
            //    return NotFound(new { message = "User not found." });
            //}

            //return Ok(new UserDetailsDto
            //{
            //    Id = user.Userid,
            //    Username = user.Username,
            //    Email = user.Email,
            //    Name = user.Address, // Assuming 'Address' is used as 'Name' for simplicity
            //    ProfileImage = "url_to_profile_image", // Placeholder for actual profile image URL
            //    OtherFields = new { Items = user.Items, Auctions = user.Auctions, Feedbacks = user.Feedbackpublishes, Transactions = user.TransactionBuyerusers.Concat(user.TransactionSellerusers), Notifications = user.Notifications }
            //});
        }

        //获取用户出价历史
        [HttpGet("users/{userId}/bids")]
        public async Task<ActionResult<IEnumerable<BidHistoryDto>>> GetUserBids(decimal userId)
        {

            var bids = await _context.Bidrecords
                .Where(br => br.Userid == userId)
                .Select(br => new BidHistoryDto
                {
                    id = br.Bidid,
                    itemId = br.Auction.Item.Itemid, // 如果 Auction 或 Item 为 null，则ItemId为0
                    amount = (decimal)br.Bidamount,
                    timestamp = br.Bidtime.HasValue ? br.Bidtime.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") : null // 格式化为 ISO 8601 格式
                })
                .ToListAsync();

            return Ok(bids);
        }


        [HttpPut("users/{userId}")]
        public async Task<ActionResult<UserDto>> UpdateUser(decimal userId, [FromBody] UpdateUserDto updateUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid input data." });
            }
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    return NotFound(new { message = "Auction item not found." });
                }

                user.Username = updateUserDto.username;
                user.Email = updateUserDto.email;


                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                var updateResponse = new UserDto
                {
                    id = user.Userid,
                    username = user.Username,
                    email = user.Email
                };
                return Ok(updateResponse);


            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
                //var user = await _context.Users.FirstOrDefaultAsync(u => u.Userid == userId);
                //if (user == null)
                //{
                //    return NotFound(new { message = "User not found." });
                //}

                //user.Username = updateUserDto.Username;
                //user.Email = updateUserDto.Email;
                //user.Address = updateUserDto.Name; // Assuming 'Address' is used as 'Name' for simplicity
                //                                   // Update other fields as needed

                //await _context.SaveChangesAsync();

                //return Ok(new UserDto { Id = user.Userid, Username = user.Username, Email = user.Email });
            }
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
                new Claim(ClaimTypes.NameIdentifier, user.Userid.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(1), // 设置令牌过期时间
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }




        public class ULoginDto
        {
            public string email { get; set; }
            public string password { get; set; }
        }

        public class UserDto
        {
            public decimal id { get; set; }
            public string username { get; set; }
            public string email { get; set; }
        }

        public class UserLoginDto
        {
            public string token { get; set; }
            public UserDto user { get; set; }
        }

        public class RegisterDto
        {
            public string username { get; set; }
            public string email { get; set; }
            public string password { get; set; }
        }
        public class UserDetailsDto
        {
            public decimal id { get; set; }
            public string username { get; set; }
            public string email { get; set; }
        }

        public class BidHistoryDto
        {
            public decimal id { get; set; }
            public decimal itemId { get; set; }
            public decimal amount { get; set; }
            public string timestamp { get; set; }
        }

        public class UpdateUserDto
        {
            public string username { get; set; }
            public string email { get; set; }

        }

    }
}
