using Microsoft.AspNetCore.Mvc;

namespace SE07203_F1.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
