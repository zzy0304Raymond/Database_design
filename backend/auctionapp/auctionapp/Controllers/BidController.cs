//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using auctionapp.Models;
//using System.IdentityModel.Tokens.Jwt;
//using Microsoft.IdentityModel.Tokens;
//using System.Security.Claims;
//using System.Text;
//using System.Security.Cryptography;
//using Microsoft.EntityFrameworkCore.Storage;

//namespace auctionapp.Controllers
//{
//    [Route("api/bid")]
//    [ApiController]

//    public class BidController : Controller
//    {
        

       
//        private readonly AuctionDbContext _context;

//        public BidController(AuctionDbContext context)
//        {
//            _context = context;
//        }


//        //用户出价

//        [HttpPost]

//        public async Task<ActionResult<string>> UserBid([FromBody] bidDataDto bidData)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(new { message = "Invalid input data." });
//            }
//            try
//            {
//                var auction = await _context.Auctions
//                    .Include(auction => auction.Bidrecords)
//                    .FirstOrDefluatAsync(bidData.auctionId)

//            }

//        }






//        public class bidDataDto
//        {
//            public decimal auctionId { get; set; }

//            public decimal userId { get; set; }

//            public decimal bidAmount { get; set; }

//            public string bidTime { get; set; }
//        }




//    }
