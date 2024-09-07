using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using auctionapp.Models;

namespace auctionapp.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly AuctionDbContext _context;

        public ShoppingCartController(AuctionDbContext context)
        {
            _context = context;
        }

        // GET: /user/users/{userId}/cart
        [HttpGet("users/{userId}/cart")]
        public async Task<ActionResult<IEnumerable<object>>> GetCartItems(decimal userId)
        {
            // 查找指定 ID 的用户及其购物车中的物品
            var user = await _context.Users
                .Include(u => u.ItemsNavigation) // 加载购物车中的物品
                .FirstOrDefaultAsync(u => u.Userid == userId);

            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            // 返回购物车物品的名称和价格
            var cartItems = user.ItemsNavigation.Select(item => new
            {
                itemName = item.Itemname,
                price = item.Startingprice,
                itemId = item.Itemid
            }).ToList();

            return Ok(cartItems);
        }

        // DELETE: /user/users/{userId}/cart/{itemId}
        [HttpDelete("users/{userId}/cart/{itemId}")]
        public async Task<ActionResult> RemoveCartItem(decimal userId, decimal itemId)
        {
            // 查找指定 ID 的用户及其购物车中的物品
            var user = await _context.Users
                .Include(u => u.ItemsNavigation) // 加载购物车中的物品
                .FirstOrDefaultAsync(u => u.Userid == userId);

            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            // 查找指定的物品
            var item = user.ItemsNavigation.FirstOrDefault(i => i.Itemid == itemId);

            if (item == null)
            {
                return NotFound(new { message = "Item not found in cart." });
            }

            // 从购物车中移除物品
            user.ItemsNavigation.Remove(item);

            // 保存更改
            await _context.SaveChangesAsync();

            return Ok(new { success = true });
        }
    }
}
