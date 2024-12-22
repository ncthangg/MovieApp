using Microsoft.AspNetCore.Mvc;

namespace MovieApp.API.Controllers
{
    public class UserWatchHistoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
