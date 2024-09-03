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
