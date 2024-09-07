using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using auctionapp.Models; // 假设 Models 命名空间包含 Chat 和 AuctionDbContext

namespace auctionapp.Controllers
{
    [Route("api/chat")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly AuctionDbContext _context;

        public ChatController(AuctionDbContext context)
        {
            _context = context;
        }

        // GET: /api/chat
        // 可选参数 count 用于指定返回的聊天记录条数
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Chat>>> GetChats([FromQuery] int? count)
        {
            // 获取所有聊天记录，按时间顺序排序
            var chats = _context.Chats
                .OrderBy(c => c.Chattime); // 按时间排序

            // 如果指定了条数，限制返回条数
            if (count.HasValue)
            {
                chats = (IOrderedQueryable<Chat>)chats.OrderByDescending(c => c.Chattime).Take(count.Value);
            }

            var chatList = await chats.Select(c => new ChatDto
            {
                Userid = c.Userid ?? 0,
                Chattime = c.Chattime,
                Content = c.Content
            })
                .ToListAsync();

            // 返回聊天记录
            return Ok(chatList);
        }

        // POST: /api/chat
        // 用于上传新的聊天记录
        [HttpPost]
        public async Task<ActionResult> PostChat([FromBody] ChatDto chatDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid input data." });
            }

            // 检查用户是否存在
            var user = await _context.Users.FindAsync(chatDto.Userid);
            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }
            var maxChatID = await _context.Chats.MaxAsync(c => (decimal?)c.Chatid) ?? 0;
            // 创建新的聊天记录
            var chat = new Chat
            {
                Chatid = maxChatID + 1,
                Userid = chatDto.Userid,
                Chattime = chatDto.Chattime,
                Content = chatDto.Content,
                // User = user // 设置关联的用户
            };

            // 保存到数据库
            _context.Chats.Add(chat);
            await _context.SaveChangesAsync();
            return Ok();
            //return CreatedAtAction(nameof(PostChat), new { id = chat.Chatid }, chat);
        }
    }

    // DTO 类用于接收聊天数据
    public class ChatDto
    {
        public decimal Userid { get; set; }
        public DateTime? Chattime { get; set; }
        public string? Content { get; set; }
    }
}
