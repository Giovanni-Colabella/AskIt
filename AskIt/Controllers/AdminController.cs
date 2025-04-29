using Microsoft.AspNetCore.Mvc;

namespace AskIt.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
