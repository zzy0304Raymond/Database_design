using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using auctionapp.Models;

namespace auctionapp.Controllers
{


    [Route("api/auction-items")]
    [ApiController]
    public class AuctionItemsController : ControllerBase
    {
        private readonly AuctionDbContext _context;

        public AuctionItemsController(AuctionDbContext context)
        {
            _context = context;
        }

        //GET: api/auction-items/{itemId} 获取物品详细

        [HttpGet("{itemId}")]
        public async Task<IActionResult> GetAuctionItem(string itemId)
        {
            try
            {
                // 从数据库中获取拍卖物品的详细信息
                var item = await _context.Items
                    .Include(item => item.Auctions) // 包含拍卖信息
                    .Include(item => item.Feedbackpublishes) // 包含反馈发布信息
                    .Include(item => item.Users) // 包含用户信息
                    .FirstOrDefaultAsync(item => item.Itemid == itemId);

                if (item == null)
                {
                    // 如果没有找到物品，返回404状态码
                    return NotFound($"Item with ID {itemId} not found.");
                }

                // 将拍卖物品的详细信息转换为JSON格式
                var itemDetails = new
                {
                    id = item.Itemid,
                    images = item.Image != null ? Convert.ToBase64String(item.Image) : null, // 假设需要将图片转换为Base64字符串
                    name = item.Itemname,
                    currentBid = item.Auctions.Any() ? item.Auctions.Max(a => a.Currenthighestbid) : (decimal?)null, // 获取当前最高出价
                    startingBid = item.Startingprice,
                    description = item.Description,
                    condition = "New", // 
                    details = item.Feedbackpublishes.FirstOrDefault().Feedback.Content,
                    stock = item.Users.Count, //
                    category = "General", // 
                    recommendedItems = item.Auctions.Select(a => new
                    {
                        auctionId = a.Auctionid,
                        name = item.Itemname,
                        images = item.Image != null ? Convert.ToBase64String(item.Image) : null,
                        currentBid = a.Currenthighestbid,
                        startingBid = item.Startingprice,
                        description = item.Description,
                        condition = "New",
                        details = " ",
                        stock = item.Users.Count
                    })
                };

                // 返回200状态码和拍卖物品的详细信息
                return Ok(itemDetails);
            }
            catch (Exception ex)
            {
                // 如果发生异常，返回500状态码和错误信息
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        // GET: api/auction-items
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuctionItemDto>>> GetAuctionItems()
        {
            try
            {
                var items = await _context.Items
                    .Select(item => new AuctionItemDto
                    {
                        Id = item.Itemid,
                        Name = item.Itemname,
                        StartingBid = item.Startingprice ?? 0,
                        Category = "", // Assuming no category column, adjust if necessary
                        PostTime = item.Postdate.ToString(), // Adjust based on the actual data type
                        ImageUrl = item.Image != null ? $"data:image/png;base64,{Convert.ToBase64String(item.Image)}" : ""
                    })
                    .ToListAsync();

                return Ok(items);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching auction items." });
            }
        }

        // POST: api/auction-items
        [HttpPost]
        public async Task<ActionResult<AuctionItemDto>> AddAuctionItem([FromBody] CreateAuctionItemDto newItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid input data." });
            }

            try
            {
                var item = new Item
                {
                    Itemid = "8",
                    Itemname = newItem.Name,
                    Description = "", // Adjust this if a description is needed
                    Startingprice = newItem.StartingBid,
                    Postdate = DateTime.Parse(newItem.PostTime),
                    Image = newItem.ImageUrl != "" ? Convert.FromBase64String(newItem.ImageUrl.Replace("data:image/png;base64,", "")) : null
                };

                _context.Items.Add(item);
                await _context.SaveChangesAsync();

                var createdItem = new AuctionItemDto
                {
                    Id = item.Itemid,
                    Name = item.Itemname,
                    StartingBid = item.Startingprice ?? 0,
                    Category = newItem.Category,
                    PostTime = item.Postdate.ToString(),
                    ImageUrl = newItem.ImageUrl
                };

                return CreatedAtAction(nameof(GetAuctionItems), new { id = item.Itemid }, createdItem);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while adding the auction item. " + ex });
            }
        }

        // PUT: api/auction-items/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<AuctionItemDto>> UpdateAuctionItem(string id, [FromBody] CreateAuctionItemDto updatedItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid input data." });
            }

            try
            {
                var item = await _context.Items.FindAsync(id);
                if (item == null)
                {
                    return NotFound(new { message = "Auction item not found." });
                }

                item.Itemname = updatedItem.Name;
                item.Startingprice = updatedItem.StartingBid;
                item.Postdate = DateTime.Parse(updatedItem.PostTime);
                item.Image = updatedItem.ImageUrl != null ? Convert.FromBase64String(updatedItem.ImageUrl.Replace("data:image/png;base64,", "")) : null;

                _context.Entry(item).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                var updatedResponse = new AuctionItemDto
                {
                    Id = item.Itemid,
                    Name = item.Itemname,
                    StartingBid = item.Startingprice ?? 0,
                    Category = updatedItem.Category,
                    PostTime = item.Postdate.ToString(),
                    ImageUrl = updatedItem.ImageUrl
                };

                return Ok(updatedResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the auction item." });
            }
        }

        // DELETE: api/auction-items/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuctionItem(string id)
        {
            try
            {
                var item = await _context.Items.FindAsync(id);
                if (item == null)
                {
                    return NotFound(new { message = "Auction item not found." });
                }

                _context.Items.Remove(item);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the auction item." });
            }
        }

        //获取推荐物品列表
        [HttpGet("recommendations")]
        public async Task<IActionResult> GetRecommendedItems(string category)
        {
            if (string.IsNullOrEmpty(category))
            {
                return BadRequest("Category parameter is required.");
            }

            try
            {
                var recommendedItems = await _context.Items
                    .Where(item => item.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
                    .Select(item => new
                    {
                        id = item.Itemid,
                        name = item.Itemname,
                        currentBid = item.Auctions.Any() ? item.Auctions.Max(a => a.Currenthighestbid) : (decimal?)null,
                        startingBid = item.Startingprice,
                        description = item.Description,
                        imageUrl = item.Image != null ? Convert.ToBase64String(item.Image) : null
                    })
                    .ToListAsync();

                return Ok(recommendedItems);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        [HttpGet("{itemId}/bids")]
        public async Task<IActionResult> GetBidHistory(string itemId)
        {
            if (string.IsNullOrEmpty(itemId))
            {
                return BadRequest("Item ID parameter is required.");
            }

            try
            {
                // 确保 itemId 与 Bidrecord 中的字段类型匹配，这里假设是 string 类型
                var bidHistory = await _context.Bidrecords.
                    .Where(record => record.Itemid == itemId)
                    .Select(record => new
                    {
                        id = record.Id,
                        bidderId = record.BidderId,
                        amount = record.Amount,
                        timestamp = record.Timestamp.ToString("o") // ISO 8601 格式
                    })
                    .ToListAsync();

                return Ok(bidHistory);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }

    // DTO class to match the API response format
    public class AuctionItemDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal StartingBid { get; set; }
        public string Category { get; set; }
        public string PostTime { get; set; }
        public string ImageUrl { get; set; }
    }

    // DTO class for input
    public class CreateAuctionItemDto
    {
        public string Name { get; set; }
        public decimal StartingBid { get; set; }
        public string Category { get; set; }
        public string PostTime { get; set; }
        public string ImageUrl { get; set; }
    }
}
