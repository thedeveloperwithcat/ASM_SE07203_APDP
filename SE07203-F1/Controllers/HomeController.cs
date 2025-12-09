using Microsoft.AspNetCore.Mvc;
using SE07203_F1.Data;
using SE07203_F1.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;

namespace SE07203_F1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
            ViewBag.isConnectedDatabase = _context.Database.CanConnect();
            ViewBag.isLogin = false;
        }

        public IActionResult Index()
        {
            ViewBag.isConnectedDatabase = _context.Database.CanConnect();
            ViewBag.isLogin = false;
            Account account = new Account();
            if (HttpContext.Session.GetString("username") != null)
            {
                ViewBag.isLogin = true;
                account.Fullname = Convert.ToString(HttpContext.Session.GetString("fullname"));
                account.Id = Convert.ToInt32(HttpContext.Session.GetInt32("id"));
                account.Username = Convert.ToString(HttpContext.Session.GetString("username")) + "@fpt.edu.vn";
            }

            return View(account);
        }

        [HttpPost]
        public IActionResult Index(string email, string password)
        {
            ViewBag.isConnectedDatabase = _context.Database.CanConnect();
            ViewBag.isLogin = false;
            if (email == "linhhn13@fpt.edu.vn" && password == "123")
            {
                ViewBag.isLogin = true;
                ViewBag.email = email;
                return View();
            }
            else
            {
                ViewBag.isLogin = false;
                Account account = new Account();
                account.Fullname = "Stranger";
                account.Id = 0;
                account.Username = "Unknown";
                account.Password = "";
                return View(account);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // Set language via cookie and redirect back
        [HttpGet]
        public IActionResult SetLanguage(string culture, string returnUrl = "/")
        {
            if (string.IsNullOrEmpty(culture))
            {
                culture = "en-US";
            }

            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1), HttpOnly = false }
            );

            // ensure returnUrl is local
            if (Url.IsLocalUrl(returnUrl))
                return LocalRedirect(returnUrl);

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
