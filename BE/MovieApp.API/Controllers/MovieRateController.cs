using Microsoft.AspNetCore.Mvc;

namespace MovieApp.API.Controllers
{
    public class MovieRateController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
