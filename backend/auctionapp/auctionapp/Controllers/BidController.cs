using Microsoft.AspNetCore.Mvc;

namespace auctionapp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
