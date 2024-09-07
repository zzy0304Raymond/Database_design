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
    [Route("api/bid")]
    [ApiController]

    public class BidController : Controller
    {



        private readonly AuctionDbContext _context;

        public BidController(AuctionDbContext context)
        {
            _context = context;
        }


        //用户出价

        [HttpPost]

        public async Task<ActionResult<string>> UserBid([FromBody] bidDataDto bidData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid input data." });
            }
            try
            {
                var auction = await _context.Auctions
                    .Include(auction => auction.Bidrecords)
                    .FirstOrDefaultAsync(auction => auction.Itemid == bidData.itemId);

                auction.Currenthighestbiduserid = bidData.userId;

                _context.Auctions.Update(auction);

                await _context.SaveChangesAsync();

                if (auction == null)
                {
                    return BadRequest(new { message = "Invaild Auction!" });
                }

                var userBid = new Bidrecord
                {
                    Bidid = _context.Bidrecords.Any() ? _context.Bidrecords.Max(a => a.Bidid) + 1 : 1 ,
                    Auctionid = auction.Auctionid,
                    Userid = bidData.userId,
                    Bidamount = bidData.bidAmount,
                    Bidtime = DateTime.Now,
                    Auction = auction,
                    User = await _context.Users.FindAsync(bidData.userId)
                };


                _context.Bidrecords.AddAsync(userBid);
                await _context.SaveChangesAsync();

                return Ok();

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }






        public class bidDataDto
        {
            public decimal itemId { get; set; }

            public decimal userId { get; set; }

            public decimal bidAmount { get; set; }

            public string bidTime { get; set; }
        }




    }
}
