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
        public async Task<IActionResult> GetAuctionItem(decimal itemId)
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
                    details = "item.Feedbackpublishes.FirstOrDefault().Feedback.Content",
                    stock = item.Users.Count, //
                    category = "General", // 
                    //recommendedItems = item.Auctions.Select(a => new
                    //{
                    //    auctionId = a.Auctionid,
                    //    name = item.Itemname,
                    //    images = item.Image != null ? Convert.ToBase64String(item.Image) : null,
                    //    currentBid = a.Currenthighestbid,
                    //    startingBid = item.Startingprice,
                    //    description = item.Description,
                    //    condition = "New",
                    //    details = " ",
                    //    stock = item.Users.Count
                    //})
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
                    Itemid = 8,
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
        public async Task<ActionResult<AuctionItemDto>> UpdateAuctionItem(decimal id, [FromBody] CreateAuctionItemDto updatedItem)
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
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE: api/auction-items/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuctionItem(decimal id)
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
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/auction-items/image/{id}
        [HttpGet("image/{id}")]
        public async Task<ActionResult<object>> GetAuctionItemImage(decimal id)
        {
            // 查找指定 ID 的拍卖物品
            var item = await _context.Items.FindAsync(id);

            if (item == null)
            {
                // 如果找不到物品，返回 404 错误
                return NotFound(new { message = "Item not found." });
            }

            // 检查图片是否存在
            if (item.Image == null || item.Image.Length == 0)
            {
                // 如果图片不存在，返回 404 错误
                return NotFound(new { message = "Image not found for the specified item." });
            }

            // 返回文件流（假设图片是 PNG 格式，可以根据实际格式调整）
            return File(item.Image, "image/png");
        }
        //获取推荐物品列表
        //[HttpGet("recommendations")]
        //public async Task<IActionResult> GetRecommendedItems(string category)
        //{
        //    if (string.IsNullOrEmpty(category))
        //    {
        //        return BadRequest("Category parameter is required.");
        //    }

        //    try
        //    {
        //        var recommendedItems = await _context.Items
        //            .Where(item => item.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
        //            .Select(item => new
        //            {
        //                id = item.Itemid,
        //                name = item.Itemname,
        //                currentBid = item.Auctions.Any() ? item.Auctions.Max(a => a.Currenthighestbid) : (decimal?)null,
        //                startingBid = item.Startingprice,
        //                description = item.Description,
        //                imageUrl = item.Image != null ? Convert.ToBase64String(item.Image) : null
        //            })
        //            .ToListAsync();

        //        return Ok(recommendedItems);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"An error occurred: {ex.Message}");
        //    }
        //}


        [HttpGet("{Bidid}/bids")]
        public async Task<IActionResult> GetBidHistory(decimal bidId)
        {
            if (bidId <= 0)
            {
                return BadRequest("Item ID parameter is required.");
            }

            try
            {
                // 确保 itemId 与 Bidrecord 中的字段类型匹配，这里假设是 string 类型
                var bidHistory = await _context.Bidrecords
                    .Where(record => record.Bidid == bidId)
                    .Select(record => new
                    {
                        id = record.Bidid,
                        bidderId = record.Userid,
                        amount = record.Bidamount,
                        timestamp = record.Bidtime.Value.ToString("o") // ISO 8601 格式
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
        public decimal Id { get; set; }
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
