using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using auctionapp.Models;
using static System.Net.Mime.MediaTypeNames;


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
                    category = item.Category // 
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




        //获取拍卖列表
        // GET: api/auction-items
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuctionItemDto>>> GetAuctionItems()
        {
            try
            {
                var items = await _context.Items
                    .Where(item => item.Valid == true)
                    .Select(item => new AuctionItemDto
                    {
                        Id = item.Itemid,
                        Name = item.Itemname,
                        StartingBid = item.Startingprice ?? 0,
                        Category = item.Category,
                        PostTime = item.Postdate.ToString(), // Adjust based on the actual data type
                        ImageUrl = item.Image != null ? $"data:image/png;base64,{Convert.ToBase64String(item.Image)}" : ""
                    })
                    .ToListAsync();

                return Ok(items);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        //添加拍卖物品
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
                var max = _context.Items.Max(max => max.Itemid);
                var item = new Item
                {
                    Itemid = max + 1,
                    Itemname = newItem.Name,
                    Description = "", // Adjust this if a description is needed
                    Startingprice = newItem.StartingBid,
                    Postdate = DateTime.Now,
                    Image = Convert.FromBase64String(newItem.ImageUrl),
                    Category = newItem.Category,
                    Valid = true
                };
                _context.Items.Add(item);
                await _context.SaveChangesAsync();
                var auction = new Auction
                {
                    Auctionid = (_context.Auctions.Any() ? (_context.Auctions.Max(m => m.Auctionid) + 1) : 1),
                    Itemid = item.Itemid,
                    Starttime = DateTime.Now,
                    Endtime = DateTime.Parse(newItem.EndTime),
                    Currenthighestbid = newItem.StartingBid,
                    Currenthighestbiduserid = 4,
                    Item = item
                };


                _context.Auctions.Add(auction);
                await _context.SaveChangesAsync();

                var createdItem = new AuctionItemDto
                {
                    Id = item.Itemid,
                    Name = item.Itemname,
                    StartingBid = item.Startingprice ?? 0,
                    Category = newItem.Category,
                    PostTime = item.Postdate.ToString(),
                    ImageUrl = null
                };

                return CreatedAtAction(nameof(GetAuctionItems), new { id = item.Itemid }, createdItem);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while adding the auction item. " + ex });
            }
        }

        //更新拍卖物品
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
                item.Image = Convert.FromBase64String(updatedItem.ImageUrl);
                item.Category = updatedItem.Category;


                _context.Entry(item).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                var updatedResponse = new AuctionItemDto
                {
                    Id = item.Itemid,
                    Name = item.Itemname,
                    StartingBid = item.Startingprice ?? 0,
                    Category = updatedItem.Category,
                    PostTime = item.Postdate.ToString(),
                    ImageUrl = null
                };

                return Ok(updatedResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //轮询
        //Get; api/auction-items/confirm
        [HttpGet("confirm")]
        public async Task<ActionResult<DelDto>> comfirmDel()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Bad!" });
            }

            try
            {
                var validItems = await _context.Items
                        .Include( item =>  item.Auctions)
                        .Where(item => item.Valid == true)
                        .ToListAsync();

                var firstValidItemId = validItems
                    .Where(item => item.Auctions.Any(auction => auction.Endtime < DateTime.Now))
                    .Select(item => item.Itemid);

                if (firstValidItemId.Any())
                {
                    var Dto = new DelDto
                    {
                        itemId = firstValidItemId.FirstOrDefault()
                    };
                    return Ok(Dto);
                }
                else
                {
                    var Dto = new DelDto
                    {
                        itemId = -1
                    };
                    return Ok(Dto);
                }
                
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



        //删除拍卖物品
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

                item.Valid = false;

                _context.Items.Update(item);

                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        //获取图片
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


        // GET: /api/auction-items/recommendations?category=<your_category>
        [HttpGet("recommendations")]
        public async Task<IActionResult> GetRecommendedItems([FromQuery] string category)
        {
            if (string.IsNullOrWhiteSpace(category))
            {
                return BadRequest(new { message = "Category parameter is required." });
            }

            try
            {
                // 使用 ToLower() 或 ToUpper() 与数据库比较时忽略大小写
                var recommendedItems = await _context.Items
                    .Where(item => item.Category != null && item.Category.ToLower() == category.ToLower())
                    .Select(item => new
                    {
                        id = item.Itemid,
                        name = item.Itemname,
                        price = item.Startingprice
                    })
                    .ToListAsync();

                if (!recommendedItems.Any())
                {
                    return NotFound(new { message = "No items found in the specified category." });
                }

                return Ok(recommendedItems);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }

        //获取出价记录
        [HttpGet("{itemId}/bids")]
        public async Task<IActionResult> GetBidHistory(decimal itemId)
        {

            try
            {
                // 确保 itemId 与 Bidrecord 中的字段类型匹配，这里假设是 string 类型
                var bidHistory = await _context.Bidrecords.Include(b=>b.Auction)
                    .Where(record => record.Auction.Itemid == itemId)
                    .Select(record => new
                    {
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
        public string EndTime { get; set; }
        public string ImageUrl { get; set; }
    }

    public class DelDto
    {
        public decimal itemId { get; set; }
    }

}
