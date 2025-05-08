using Microsoft.AspNetCore.Mvc;

namespace economia.Controllers
{
    public class PruebaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
