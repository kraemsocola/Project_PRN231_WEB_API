using Microsoft.AspNetCore.Mvc;

namespace Project_API.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
