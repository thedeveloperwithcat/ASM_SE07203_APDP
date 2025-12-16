using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SE07203_F1.Data;
using System.Security.Cryptography;
using SE07203_F1.Services;
using System.Text;

namespace SE07203_F1.Controllers
{
    public class LoginController : Controller
    {
        readonly ApplicationDbContext _context;
        private readonly LoginCounter _loginCounter;

        public LoginController(ApplicationDbContext context, LoginCounter loginCounter)
        {
            _context = context;
            _loginCounter = loginCounter;
        }
        public IActionResult Index()
        {
            // Nếu đã đăng nhập rồi thì chuyển về Home
            if (HttpContext.Session.GetString("username") != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            try
            {
                var account = await _context.Accounts.FirstOrDefaultAsync(u => u.Username == username);

                if (account == null)
                {
                    ViewBag.Error = "Tên đăng nhập không tồn tại.";
                    return View("Index");
                }

                string hashedPassword = HashPassword(password);

                if (account.Password != hashedPassword)
                {
                    ViewBag.Error = "Mật khẩu không đúng.";
                    return View("Index");
                }
                else
                {
                    // ---- ĐĂNG NHẬP THÀNH CÔNG ----
                    HttpContext.Session.SetString("username", username);
                    HttpContext.Session.SetString("fullname", account.Fullname);
                    HttpContext.Session.SetInt32("id", account.Id);
                    HttpContext.Session.SetString("role", account.Role);

                    // -----------------------------
                    // Tăng số lần đăng nhập (Singleton)
                    // -----------------------------
                    _loginCounter.Increase();

                    // Log ra console (tùy chọn)
                    Console.WriteLine("Số lần login: " + _loginCounter.GetCount());

                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Đã xảy ra lỗi hệ thống: " + ex.Message;
                return View("Index");
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}

//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using SE07203_F1.Data;
//using System.Security.Cryptography; 
//using System.Text; 

//namespace SE07203_F1.Controllers
//{
//    public class LoginController : Controller
//    {
//        readonly ApplicationDbContext _context;

//        public LoginController(ApplicationDbContext context)
//        {
//            _context = context;
//        }
//        public IActionResult Index()
//        {
//            return View();
//        }

//        private string HashPassword(string password)
//        {
//            using (var sha256 = SHA256.Create())
//            {
//                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
//                return Convert.ToBase64String(bytes);
//            }
//        }

//        public async Task<IActionResult> Login(string username, string password)
//        {
//            try
//            {
//                var account = await _context.Accounts.FirstOrDefaultAsync(u => u.Username == username);

//                if (account == null)
//                {
//                    ViewBag.Error = "Tên đăng nhập hoặc mật khẩu không đúng.";
//                    return View("Error");
//                }
//                string hashedPassword = HashPassword(password);

//                if (account.Password != hashedPassword)
//                {
//                    ViewBag.Error = "Tên đăng nhập hoặc mật khẩu không đúng.";
//                    return View("Error");
//                }
//                else
//                {
//                    // Đăng nhập thành công
//                    HttpContext.Session.SetString("username", username);
//                    HttpContext.Session.SetString("fullname", account.Fullname);
//                    HttpContext.Session.SetInt32("id", account.Id);
//                    return View("Success");
//                }

//            }
//            catch (Exception ex)
//            {
//                ViewBag.Error = "Đã xảy ra lỗi hệ thống trong quá trình đăng nhập.";
//                return View("Error");
//            }

//        }
//    }
//}
