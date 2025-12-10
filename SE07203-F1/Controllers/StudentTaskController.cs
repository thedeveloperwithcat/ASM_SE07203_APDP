using Microsoft.AspNetCore.Mvc;

namespace SE07203_F1.Controllers
{
    public class StudentTaskController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
